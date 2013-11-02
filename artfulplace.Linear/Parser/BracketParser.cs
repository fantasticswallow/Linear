using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using artfulplace.Linear.Linq;

namespace artfulplace.Linear.Core
{
    internal class BracketParser
    {
        private Stack<string> parseStack = new Stack<string>();
        private bool isEscape = false;
        private bool isString = false;
        private bool isChar = false;

        private readonly string[] startchs = { "(", "[", "{", "\"", "'" };

        internal BracketParseInfo Parse(string target)
        {
            
            var info = new BracketParseInfo();
            var infoStack = new Stack<BracketParseInfo>();
            info.IsRoot = true;
            var isLast = false;
            var startch = "";
            foreach (var c in target.ToCharArray().Select(_ => _.ToString()).ToArray())
            {
                parseStack.Push(c);
                
                BracketParseInfo.InfoType t = BracketParseInfo.InfoType.Round;
                if (c == "\"")
                {
                    if (isString)
                    {
                        if (parseStack.ElementAt(1) == "\\")
                        {
                            var c2 = parseStack.Pop();
                            c2 = c2.Insert(0, parseStack.Pop());
                            parseStack.Push(c2);
                        }
                        else
                        {
                            isEscape = false;
                            isString = false;

                            startch = "\"";
                            isLast = true;
                            t = BracketParseInfo.InfoType.String;
                        }
                        
                    }
                    else if (!(isChar))
                    {
                        isEscape = true;
                        isString = true;
                    }
                }
                else if (c == "'")
                {
                    if (isChar)
                    {
                        if (parseStack.ElementAt(1) == "\\")
                        {
                            var c2 = parseStack.Pop();
                            c2 = c2.Insert(0,parseStack.Pop());
                            parseStack.Push(c2);
                        }
                        else
                        {
                            isEscape = false;
                            isString = false;

                            startch = "'";
                            isLast = true;
                            t = BracketParseInfo.InfoType.Char;
                        }
                    }
                    else if (!(isString))
                    {
                        isEscape = true;
                        isChar = true;
                    }
                }
                else if (c == ")")
                {
                    if (!(isEscape))
                    {
                        startch = "(";
                        isLast = true;
                        t = BracketParseInfo.InfoType.Round;
                    }
                }
                else if (c == "}")
                {
                    if (!(isEscape))
                    {
                        startch = "{";
                        isLast = true;
                        t = BracketParseInfo.InfoType.Curly;
                    }
                }
                else if (c == "]")
                {
                    if (!(isEscape))
                    {
                        isLast = true;
                        startch = "[";
                        t = BracketParseInfo.InfoType.Square;
                    }
                }

                if (isLast)
                {
                    var curInfo = new BracketParseInfo();
                    curInfo.Type = t;
                    var cur = parseStack.Pop();
                    string lastch = "";
                    while ((lastch != startch))
                    {
                        try
                        {
                            lastch = parseStack.Pop();
                        }
                        catch (Exception)
                        {
                            throw new InvalidOperationException("Parse Error : " + startch + "の数が一致しません。");
                        }
                        cur = cur.Insert(0, lastch);
                        if ((lastch.Length > 1) && !(lastch.StartsWith("\\")))
                        {
                            if (curInfo.Children == null)
                            {
                                curInfo.Children = new List<BracketParseInfo>();
                            }
                            curInfo.Children.Add(infoStack.Pop());
                        }
                    }
                    isLast = false;
                    curInfo.Result = cur.Replace("\\\"","\"");
                    infoStack.Push(curInfo);
                    parseStack.Push(cur);
                }
                
            }
            startchs.ForEach(_ =>
            {
                if (parseStack.Contains(_))
                {
                    throw new InvalidOperationException("Parse Error : " + _ + "の数が一致しません。");
                }
            });
            if (infoStack.Count == 0)
            {
                return null;
            }
            var ci = infoStack.Pop();
            if (ci.Result == target)
            {
                ci.IsRoot = true;
                return ci;
            }
            else
            {
                info.Result = target.Replace("\\\"", "\"");
                info.Children = new List<BracketParseInfo>();
                info.Children.Add(ci);
                infoStack.ForEach(_ => info.Children.Add(_));
                return info;
            }
        }

        /// <summary>
        /// Bracket Parse for Lambda Expressions
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal BracketParseInfo ParseLambda(string target)
        {

            var info = new BracketParseInfo();
            var infoStack = new Stack<BracketParseInfo>();
            info.IsRoot = true;
            var isLast = false;
            var startch = "";
            foreach (var c in target.ToCharArray().Select(_ => _.ToString()).ToArray())
            {
                parseStack.Push(c);

                BracketParseInfo.InfoType t = BracketParseInfo.InfoType.Round;
                if (c == "\"")
                {
                    if (isString)
                    {
                        if (parseStack.ElementAt(1) == "\\")
                        {
                            var c2 = parseStack.Pop();
                            c2 = c2.Insert(0, parseStack.Pop());
                            parseStack.Push(c2);
                        }
                        else
                        {
                            isEscape = false;
                            isString = false;

                            startch = "\"";
                            isLast = true;
                            t = BracketParseInfo.InfoType.String;
                        }

                    }
                    else if (!(isChar))
                    {
                        isEscape = true;
                        isString = true;
                    }
                }
                else if (c == "'")
                {
                    if (isChar)
                    {
                        if (parseStack.ElementAt(1) == "\\")
                        {
                            var c2 = parseStack.Pop();
                            c2 = c2.Insert(0, parseStack.Pop());
                            parseStack.Push(c2);
                        }
                        else
                        {
                            isEscape = false;
                            isString = false;

                            startch = "'";
                            isLast = true;
                            t = BracketParseInfo.InfoType.Char;
                        }
                    }
                    else if (!(isString))
                    {
                        isEscape = true;
                        isChar = true;
                    }
                }
                else if (c == ")")
                {
                    if (!(isEscape))
                    {
                        if (parseStack.ElementAt(1) == "(")
                        {
                            var c2 = parseStack.Pop();
                            c2 = c2.Insert(0, parseStack.Pop());
                            parseStack.Push(c2);
                        }
                        else
                        {
                            startch = "(";
                            isLast = true;
                            t = BracketParseInfo.InfoType.Round;
                        }
                        
                    }
                }
                
                if (isLast)
                {
                    var curInfo = new BracketParseInfo();
                    curInfo.Type = t;
                    var cur = parseStack.Pop();
                    string lastch = "";
                    while ((lastch != startch))
                    {
                        try
                        {
                            lastch = parseStack.Pop();
                        }
                        catch (Exception)
                        {
                            throw new InvalidOperationException("Parse Error : " + startch + "の数が一致しません。");
                        }
                        cur = cur.Insert(0, lastch);
                        if ((lastch.Length > 1) && (!(lastch.StartsWith("\\")) && (lastch != "()")))
                        {
                            if (curInfo.Children == null)
                            {
                                curInfo.Children = new List<BracketParseInfo>();
                            }
                            curInfo.Children.Insert(0,infoStack.Pop());
                        }
                    }
                    isLast = false;
                    if (t == BracketParseInfo.InfoType.Round)
                    {
                        if (parseStack.ElementAt(0) == "!")
                        {
                            curInfo.Type = BracketParseInfo.InfoType.NotOperator;
                            cur = cur.Insert(0, parseStack.Pop());
                        }
                    }

                    curInfo.Result = cur.Replace("\\\"", "\"");
                    infoStack.Push(curInfo);
                    parseStack.Push(cur);
                }

            }
            startchs.ForEach(_ =>
            {
                if (parseStack.Contains(_))
                {
                    throw new InvalidOperationException("Parse Error : " + _ + "の数が一致しません。");
                }
            });
            BracketParseInfo ci;
            try
            {
                ci = infoStack.Pop();
            }
            catch (Exception)
            {
                return null;
            }
            
            if (ci.Result == target)
            {
                ci.IsRoot = true;
                return ci;
            }
            else
            {
                info.Result = target.Replace("\\\"", "\"");
                info.Children = new List<BracketParseInfo>();
                info.Children.Add(ci);
                infoStack.ForEach(_ => info.Children.Add(_));
                return info;
            }
        }
    }

    internal class BracketParseInfo
    {
        internal List<BracketParseInfo> Children { get; set;}
        private bool isRoot = false;
        internal bool IsRoot
        {
            get
            {
                return isRoot;
            }
            set
            {
                isRoot = value;
            }
        }
        internal string Result { get; set; }
        internal string Capture
        {
            get
            {
                return this.Result.Remove(this.Result.Length - 1).Remove(0, 1);
            }
        }
        internal enum InfoType
        {
            Round,
            Curly,
            Square,
            String,
            Char,
            NotOperator


        }
        internal InfoType Type { get; set; }
    }
}
