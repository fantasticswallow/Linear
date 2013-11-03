using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using artfulplace.Linear.Linq;
using System.Text.RegularExpressions;

namespace artfulplace.Linear.Core
{
    internal class MethodSpliter
    {
        /// <summary>
        /// ルートレベルのメソッドを分解します
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal IEnumerable<string> RootSplit(string target)
        {
            var brackParser = new BracketParser();
            var brackInfo = brackParser.Parse(target);
            target = target.Replace("\\\"", "\"");
            brackInfo.Children.ForEach((x, idx) =>
            {
                var idx2 = target.IndexOf(x.Result);
                target = target.Remove(idx2, x.Result.Length).Insert(idx2,"{" + idx.ToString() + "}");
            });
            var spliter = target.Split('.');
            brackInfo.Children.ForEach((x, idx) => {
                var repOld = "{" + idx.ToString() + "}";
                spliter = spliter.Select(_ => _.Replace(repOld, x.Result)).ToArray();
            });
            return spliter;
        }

        /// <summary>
        /// ルートレベルのメソッドを分解します
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal IEnumerable<MethodInfo> RootSplit(string target,BracketParseInfo brackInfo)
        {
            target = target.Replace("\\\"", "\"");
            if (brackInfo != null)
            {
                brackInfo.Children.ForEach((x, idx) =>
                {
                    var idx2 = target.IndexOf(x.Result);
                    target = target.Remove(idx2, x.Result.Length).Insert(idx2, "{" + idx.ToString() + "}");
                });
            }
            var spliter = target.Split('.').Select(_ => {
                var info = new MethodInfo();
                if (_.Contains("{"))
                {
                    info.Name = _.Remove(_.IndexOf('{')).Trim();
                }
                else
                {
                    info.Name = _.Trim();
                }
                info.Type = MethodInfo.MethodType.PropertyOrField;
                info.baseStr = _;
                return info;
            }).ToArray();
            
            if (brackInfo != null)
            {
                brackInfo.Children.ForEach((x, idx) =>
                {
                    var repOld = "{" + idx.ToString() + "}";
                    spliter = spliter.Select(_ =>
                    {
                        if (_.baseStr.Contains(repOld))
                        {
                            _.baseStr = _.baseStr.Replace(repOld, x.Result);
                            _.BracketInfo = x;
                            if (x.Type == BracketParseInfo.InfoType.Round)
                            {
                                _.Type = MethodInfo.MethodType.Method;
                            }
                            else if (x.Type == BracketParseInfo.InfoType.Square)
                            {
                                _.Type = MethodInfo.MethodType.Property;
                            }

                            if (!(String.IsNullOrEmpty(x.Capture)) && (x.Type != BracketParseInfo.InfoType.String))
                            {
                                _.Args = ArgSplit(x.Capture, x);
                            }
                        }
                        return _;
                    }).ToArray();
                });
            }
            return spliter;
        }

        internal IEnumerable<ArgumentInfo> ArgSplit(string target, BracketParseInfo brackInfo)
        {
            brackInfo.Children.ForEach((x, idx) =>
            {
                var idx2 = target.IndexOf(x.Result);
                target = target.Remove(idx2, x.Result.Length).Insert(idx2, "{" + idx.ToString() + "}");
            });
            var spliter = target.Split(',').Select(_ =>
            {
                var info = new ArgumentInfo();
                info.Value = _;
                if (_.Contains("=>"))
                {
                    info.Type = ArgumentInfo.ArgumentType.Lambda;
                    info.BracketInfo = brackInfo;
                }
                else if (Regex.IsMatch(_, "^[0-9]+\\.[0-9]+$"))
                {
                    info.Type = ArgumentInfo.ArgumentType.Double;
                }
                else if(Regex.IsMatch(_,"^[0-9]+$"))
                {
                    info.Type = ArgumentInfo.ArgumentType.Integer;
                }
                else if (Regex.IsMatch(_, "^(true|false)$", RegexOptions.IgnoreCase))
                {
                    info.Type = ArgumentInfo.ArgumentType.Boolean;
                }
                else if (_.Contains(".") || (_.Contains("(")))
                {
                    info.Type = ArgumentInfo.ArgumentType.Method;
                }
                else
                {
                    info.Type = ArgumentInfo.ArgumentType.Variable;
                }
                return info;
            }).ToArray();
            
            brackInfo.Children.ForEach((x, idx) =>
            {
                var repOld = "{" + idx.ToString() + "}";
                spliter = spliter.Select(_ =>
                {
                    if (_.Value.Contains(repOld))
                    {
                        if (x.Type == BracketParseInfo.InfoType.String)
                        {
                            if (_.Type == ArgumentInfo.ArgumentType.Variable)
                            {
                                _.Type = ArgumentInfo.ArgumentType.String;
                                _.Value = _.Value.Replace(repOld, x.Capture);
                            }
                            else
                            {
                                _.Value = _.Value.Replace(repOld, x.Result);
                            }
                        }
                        else if (x.Type == BracketParseInfo.InfoType.Char)
                        {
                            if (_.Type == ArgumentInfo.ArgumentType.Variable)
                            {
                                _.Type = ArgumentInfo.ArgumentType.Char;
                                _.Value = _.Value.Replace(repOld, x.Capture);
                            }
                            else
                            {
                                _.Value = _.Value.Replace(repOld, x.Result);
                            }
                        }
                        else
                        {
                            _.Value = _.Value.Replace(repOld, x.Result);
                        }
                    }
                    return _;
                }).ToArray();
            });
            return spliter;
        }
    }

}
