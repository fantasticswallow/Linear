using artfulplace.Linear.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using artfulplace.Linear.Linq;
using System.Text.RegularExpressions;

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
            info.ExpressionType = OperatorKind.Not;
            info.ExpressionString1 = target;
            info.Expression1 = SearchExpression(target, brackInfo);
            return info;
        }

        private ExpressionBasicInfo ConstantExpression(string target)
        {
            var curType = ArgumentInfo.ArgumentType.Variable;
            if (Regex.IsMatch(target, "[0-9]+\\.[0-9]+"))
            {
                curType = ArgumentInfo.ArgumentType.Double;
            }
            else if (Regex.IsMatch(target, "[0-9]+"))
            {
                curType = ArgumentInfo.ArgumentType.Integer;
            }
            else if (Regex.IsMatch(target, "^(true|false)", RegexOptions.IgnoreCase))
            {
                curType = ArgumentInfo.ArgumentType.Boolean;
            }
            else if (target.Contains("."))
            {
                curType = ArgumentInfo.ArgumentType.Method;
            }
            var info = new ExpressionBasicInfo();
            info.ExpressionType = OperatorKind.Constant;
            info.ExpressionString1 = target;
            info.ConstantType = curType;
            return info;
        }

        private ExpressionBasicInfo StringExpression(string target)
        {
            var info = new ExpressionBasicInfo();
            info.ExpressionString1 = target;
            info.ExpressionType = OperatorKind.Constant;
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
            var opList = new List<OperatorKind>();
            target = replaceBracket(target, brackInfo);
            if (target.Contains("&&"))
            {
                opList.Add(OperatorKind.And);
            }
            if (target.Contains("||"))
            {
                opList.Add(OperatorKind.Or);
            }
            if (target.Contains("=="))
            {
                opList.Add(OperatorKind.Equals);
            }
            if (target.Contains("!"))
            {
                opList.Add(OperatorKind.NotEquals);
            }
            if (target.Contains(">"))
            {
                opList.Add(OperatorKind.Greater);
                opList.Add(OperatorKind.GreaterEquals);
            }
            if (target.Contains("<"))
            {
                opList.Add(OperatorKind.Less);
                opList.Add(OperatorKind.LessEquals);
            }
            if (target.Contains("+"))
            {
                opList.Add(OperatorKind.AddAssign);
                opList.Add(OperatorKind.Add);
            }
            if (target.Contains("-"))
            {
                opList.Add(OperatorKind.SubtractAssign);
                opList.Add(OperatorKind.Subtract);
            }
            if (target.Contains("*"))
            {
                opList.Add(OperatorKind.MultiplicationAssign);
                opList.Add(OperatorKind.Multiplication);
            }
            if (target.Contains("/"))
            {
                opList.Add(OperatorKind.DivisionAssign);
                opList.Add(OperatorKind.Division);
            }
            if (target.Contains("%"))
            {
                opList.Add(OperatorKind.ModuloAssign);
                opList.Add(OperatorKind.Modulo);
            }
            if (target.Contains("="))
            {
                opList.Add(OperatorKind.Equals);
            }
            opList.Add(OperatorKind.Constant);
            if (opList.Count == 1)
            {
                return ConstantExpression(target);
            }
            else
            {
                return ExpressionParse(target, brackInfo, opList, 0);
            }
        }

        private ExpressionBasicInfo ExpressionParse(string target, BracketParseInfo brackInfo,List<OperatorKind> opList,int index)
        {
            var curType = opList[index];
            
            if (curType == OperatorKind.Constant)
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

        private Dictionary<OperatorKind,string> operatorDictionary { get; set; }
        
        private void dictInit()
        {
            operatorDictionary = new Dictionary<OperatorKind, string>();
            operatorDictionary.Add(OperatorKind.And, "&&");
            operatorDictionary.Add(OperatorKind.Or, "||");
            operatorDictionary.Add(OperatorKind.Equals, "==");
            operatorDictionary.Add(OperatorKind.NotEquals, "!=");
            operatorDictionary.Add(OperatorKind.GreaterEquals, ">=");
            operatorDictionary.Add(OperatorKind.LessEquals, "<=");
            operatorDictionary.Add(OperatorKind.Greater, ">");
            operatorDictionary.Add(OperatorKind.Less, "<");

            operatorDictionary.Add(OperatorKind.AddAssign,"+=");
            operatorDictionary.Add(OperatorKind.SubtractAssign,"-=");
            operatorDictionary.Add(OperatorKind.MultiplicationAssign,"*=");
            operatorDictionary.Add(OperatorKind.DivisionAssign,"/=");
            operatorDictionary.Add(OperatorKind.ModuloAssign,"%=");
            operatorDictionary.Add(OperatorKind.Add,"+");
            operatorDictionary.Add(OperatorKind.Subtract,"-");
            operatorDictionary.Add(OperatorKind.Multiplication,"*");
            operatorDictionary.Add(OperatorKind.Division,"/");
            operatorDictionary.Add(OperatorKind.Modulo,"%");
            operatorDictionary.Add( OperatorKind.Basic,"=");
        }

        internal enum OperatorKind
        {
            // logical Operator 
            And,
            Or,
            Not,

            // equals operator
            Equals,
            NotEquals,
            Greater,
            Less,
            GreaterEquals,
            LessEquals,

            // assignment operator
            AddAssign,
            SubtractAssign,
            MultiplicationAssign,
            DivisionAssign,
            ModuloAssign,

            // arithmetic operator
            Basic,
            Add,
            Subtract,
            Multiplication,
            Division,
            Modulo,

            // others (contains not use)
            String,
            Char,
            BracketRound,
            BracketCurly,
            BracketSquare,
            Constant,

            None

        }

        #endregion

    }

    internal class ExpressionBasicInfo
    {
        internal ExpressionParser.OperatorKind ExpressionType { get; set; }
        internal ExpressionBasicInfo Expression1 { get; set; }
        internal ExpressionBasicInfo Expression2 { get; set; }
        internal string ExpressionString1 { get; set; }
        internal string ExpressionString2 { get; set; }
        internal ArgumentInfo.ArgumentType ConstantType { get; set; }
    }
}
