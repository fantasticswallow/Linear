using artfulplace.Linear.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using artfulplace.Linear.Linq;
using System.Text.RegularExpressions;
using System.Linq.Expressions;

namespace artfulplace.Linear.Lambda
{
    internal class ExpressionParser
    {

        internal ExpressionParser()
        {
            dictInit();
        }

        internal ExpressionBasicInfo ExpressionParse(string target)
        {

            var bParser = new BracketParser();
            var brInfo = bParser.ParseLambda(target);

            // bParserでかっこを分解
            // {0}なんかで置き換えながら処理してく
            // parseメソッドを分ける
            // 上のstringの分解は分離後のメソッドでやる

            return SearchExpression(target, brInfo);
        }

        private Dictionary<string, BracketParseInfo> replaceDictionary = new Dictionary<string, BracketParseInfo>();
        private int replaceId = 0;

        [System.Diagnostics.DebuggerStepThrough]
        private string replaceBracket(string target, BracketParseInfo brackInfo)
        {
            if (brackInfo == null)
            {
                return target;
            }
            brackInfo.Children.ForEach((x) =>
            {
                var idx2 = target.IndexOf(x.Result);
                if (idx2 >= 0)
                {
                    var key = "{" + replaceId.ToString() + "}";
                    target = target.Remove(idx2, x.Result.Length).Insert(idx2, key);
                    replaceDictionary.Add(key, x);
                    replaceId += 1;
                }
            });
            return target;
        }
        private string replaceBack(string target)
        {
            if (target.Contains("{"))
            {
                replaceDictionary.ForEach(_ =>
                {
                    if (target.Contains(_.Key))
                    {
                        target = target.Replace(_.Key, _.Value.Result);
                    }
                });
            }
            return target;
        }

        private ExpressionBasicInfo NotExpression(string target, BracketParseInfo brackInfo)
        {
            var info = new ExpressionBasicInfo();
            info.ExpressionType = ExpressionType.Not;
            info.ExpressionString1 = target;
            info.Expression1 = SearchExpression(target, brackInfo);
            return info;
        }

        private ExpressionBasicInfo ConstantExpression(string target)
        {
            var curType = ArgumentInfo.ArgumentType.Variable;
            target = replaceBack(target);
            if (Regex.IsMatch(target, "^[0-9]+\\.[0-9]+$"))
            {
                curType = ArgumentInfo.ArgumentType.Double;
            }
            else if (Regex.IsMatch(target, "^[0-9]+$"))
            {
                curType = ArgumentInfo.ArgumentType.Integer;
            }
            else if (Regex.IsMatch(target, "^[0-9]+L$"))
            {
                curType = ArgumentInfo.ArgumentType.Long;
            }
            else if (Regex.IsMatch(target, "^(true|false)$", RegexOptions.IgnoreCase))
            {
                curType = ArgumentInfo.ArgumentType.Boolean;
            }
            else if ((target.Contains(".")) || (target.Contains("(")))
            {
                curType = ArgumentInfo.ArgumentType.Method;
            }
            var info = new ExpressionBasicInfo();
            info.ExpressionType = ExpressionType.Constant;
            info.ExpressionString1 = target;
            info.ConstantType = curType;
            return info;
        }

        private ExpressionBasicInfo StringExpression(string target)
        {
            var info = new ExpressionBasicInfo();
            info.ExpressionString1 = target;
            info.ExpressionType = ExpressionType.Constant;
            info.ConstantType = ArgumentInfo.ArgumentType.String;
            return info;
        }

        /// <summary>
        /// Search Contains Operator and Parse Expression
        /// </summary>
        /// <param name="target"></param>
        /// <param name="brackInfo"></param>
        /// <returns></returns>
        private ExpressionBasicInfo SearchExpression(string target,BracketParseInfo brackInfo)
        {
            var opList = new List<ExpressionType>();
            target = replaceBracket(target, brackInfo);
            if (target.Contains("&&"))
            {
                opList.Add(ExpressionType.And);
            }
            if (target.Contains("||"))
            {
                opList.Add(ExpressionType.Or);
            }
            if (target.Contains("=="))
            {
                opList.Add(ExpressionType.Equal);
            }
            //if (target.Contains("=="))
            //{
            //    opList.Add(ExpressionType.Equal);
            //}
            if (target.Contains("!"))
            {
                opList.Add(ExpressionType.NotEqual);
            }
            if (target.Contains(">"))
            {
                opList.Add(ExpressionType.GreaterThanOrEqual);
                opList.Add(ExpressionType.GreaterThan);
            }
            if (target.Contains("<"))
            {
                opList.Add(ExpressionType.LessThanOrEqual);
                opList.Add(ExpressionType.LessThan);
            }
            if (target.Contains("*"))
            {
                opList.Add(ExpressionType.MultiplyAssign);
                opList.Add(ExpressionType.Multiply );
            }
            if (target.Contains("/"))
            {
                opList.Add(ExpressionType.DivideAssign);
                opList.Add(ExpressionType.Divide);
            }
            if (target.Contains("%"))
            {
                opList.Add(ExpressionType.ModuloAssign);
                opList.Add(ExpressionType.Modulo);
            }
            if (target.Contains("^"))
            {
                opList.Add(ExpressionType.Power);
            }
            if (target.Contains("+"))
            {
                opList.Add(ExpressionType.AddAssign);
                opList.Add(ExpressionType.Add);
            }
            if (target.Contains("-"))
            {
                opList.Add(ExpressionType.SubtractAssign);
                opList.Add(ExpressionType.Subtract);
            }
            if (target.Contains("="))
            {
                opList.Add(ExpressionType.Assign);
            }
            opList.Add(ExpressionType.Constant);
            if (opList.Count == 1)
            {
                return ConstantExpression(target);
            }
            else
            {
                return ExpressionParse(target, brackInfo, opList, 0);
            }
        }

        private ExpressionBasicInfo ExpressionParse(string target, BracketParseInfo brackInfo,List<ExpressionType> opList,int index)
        {
            var curType = opList[index];
            
            if (curType == ExpressionType.Constant)
            {
                return ConstantExpression(target);
            }
            else
            {
                var startStr = operatorDictionary[curType];
                if (target.Contains(startStr))
                {
                    target = replaceBracket(target, brackInfo);
                    ExpressionBasicInfo info = null;
                    var sp = target.Split(new string[] { startStr }, StringSplitOptions.None);
                    var expr1 = "";
                    var expr2 = "";
                    sp.ForEach(_ =>
                    {
                        _ = _.Trim();
                        if (expr1 == "")
                        {
                            if (replaceDictionary.ContainsKey(_))
                            {
                                var x = replaceDictionary[_];
                                if (x.Type == BracketParseInfo.InfoType.Round)
                                {
                                    info = SearchExpression(x.Capture, x);
                                    expr1 = x.Capture;
                                }
                                else if (x.Type == BracketParseInfo.InfoType.NotOperator)
                                {
                                    expr1 = x.Capture.Remove(0, 1);
                                    info = NotExpression(expr1, x);

                                }
                                else if (x.Type == BracketParseInfo.InfoType.String)
                                {
                                    info = StringExpression(x.Capture);
                                    expr1 = x.Capture;
                                }
                            }
                            else
                            {
                                info = ExpressionParse(_, brackInfo,opList,index + 1);
                                expr1 = _;
                            }
                        }
                        else
                        {
                            var curInfo = new ExpressionBasicInfo();
                            curInfo.Expression1 = info;
                            curInfo.ConstantType = ArgumentInfo.ArgumentType.Variable;
                            curInfo.ExpressionString1 = replaceBack(expr1);
                            
                            if (replaceDictionary.ContainsKey(_))
                            {
                                var x = replaceDictionary[_];
                                if (x.Type == BracketParseInfo.InfoType.Round)
                                {
                                    curInfo.Expression2 = SearchExpression(x.Capture, x);
                                    expr2 = x.Capture;
                                }
                                else if (x.Type == BracketParseInfo.InfoType.NotOperator)
                                {
                                    expr2 = x.Capture.Remove(0, 1);
                                    curInfo.Expression2 = NotExpression(expr2, x);

                                }
                                else if (x.Type == BracketParseInfo.InfoType.String)
                                {
                                    curInfo.Expression2 = StringExpression(x.Capture);
                                    curInfo.ConstantType = ArgumentInfo.ArgumentType.String;
                                    expr2 = x.Capture;
                                }
                            }
                            else
                            {
                                curInfo.Expression2 = ExpressionParse(_, brackInfo, opList, index + 1);
                                expr2 = _;
                            }
                            curInfo.ExpressionString2 = replaceBack(expr2);
                            curInfo.ExpressionType = curType;
                            info = curInfo;
                            
                        }
                    });
                    return info;
                }
                else
                {
                    return ExpressionParse(target, brackInfo, opList, index + 1);
                }
            }
            
        }

        #region operator enumeration

        private Dictionary<ExpressionType,string> operatorDictionary { get; set; }
        
        private void dictInit()
        {
            operatorDictionary = new Dictionary<ExpressionType, string>();
            operatorDictionary.Add(ExpressionType.And, "&&");
            operatorDictionary.Add(ExpressionType.Or, "||");

            operatorDictionary.Add(ExpressionType.Equal, "==");
            operatorDictionary.Add(ExpressionType.NotEqual, "!=");
            operatorDictionary.Add(ExpressionType.GreaterThanOrEqual, ">=");
            operatorDictionary.Add(ExpressionType.LessThanOrEqual, "<=");
            operatorDictionary.Add(ExpressionType.GreaterThan, ">");
            operatorDictionary.Add(ExpressionType.LessThan, "<");

            operatorDictionary.Add(ExpressionType.AddAssign,"+=");
            operatorDictionary.Add(ExpressionType.SubtractAssign,"-=");
            operatorDictionary.Add(ExpressionType.MultiplyAssign,"*=");
            operatorDictionary.Add(ExpressionType.DivideAssign,"/=");
            operatorDictionary.Add(ExpressionType.ModuloAssign,"%=");
            operatorDictionary.Add(ExpressionType.PowerAssign, "^=");
            operatorDictionary.Add(ExpressionType.Add,"+");
            operatorDictionary.Add(ExpressionType.Subtract,"-");
            operatorDictionary.Add(ExpressionType.Multiply,"*");
            operatorDictionary.Add(ExpressionType.Divide,"/");
            operatorDictionary.Add(ExpressionType.Modulo,"%");
            operatorDictionary.Add(ExpressionType.Power, "^");
            operatorDictionary.Add(ExpressionType.Assign,"=");
        }

        //internal enum OperatorKind
        //{
        //    // logical Operator 
        //    And,
        //    Or,
        //    Not,
            
        //    // equals operator
        //    Equals,
        //    NotEquals,
        //    Greater,
        //    Less,
        //    GreaterEquals,
        //    LessEquals,

        //    // assignment operator
        //    AddAssign,
        //    SubtractAssign,
        //    MultiplicationAssign,
        //    DivisionAssign,
        //    ModuloAssign,
        //    PowerAssign,

        //    // arithmetic operator
        //    Basic,
        //    Add,
        //    Subtract,
        //    Multiplication,
        //    Division,
        //    Modulo,
        //    Power,

        //    // others (contains not use)
        //    String,
        //    Char,
        //    BracketRound,
        //    BracketCurly,
        //    BracketSquare,
        //    Constant,

        //    None

        //}

        #endregion

    }

    internal class ExpressionBasicInfo
    {
        internal ExpressionType ExpressionType { get; set; }
        internal ExpressionBasicInfo Expression1 { get; set; }
        internal ExpressionBasicInfo Expression2 { get; set; }
        internal string ExpressionString1 { get; set; }
        internal string ExpressionString2 { get; set; }
        internal ArgumentInfo.ArgumentType ConstantType { get; set; }
    }
}
