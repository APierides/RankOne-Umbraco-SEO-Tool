﻿using System.Linq;
using HtmlAgilityPack;
using RankOne.Business.Models;

namespace RankOne.Business.Analyzers.Html
{
    /// <summary>
    /// Analyzer for checking title tag related optimizations
    /// 
    /// https://moz.com/learn/seo/title-tag
    /// 
    /// 1. check for head tag - critical
    /// 2. check for presence of title tag - critical
    /// 3. check for multiple title tags - critical
    /// 4. check for value of title tag - critical
    /// 5. check title tag length
    ///     1. longer than 60 - major
    ///     2. shorter than 10 - major
    ///     3. shorter than 50 - minor    
    /// </summary>
    public class TitleAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(HtmlNode document, params object[] additionalValues)
        {
            var result = new AnalyzeResult
            {
                Alias = "titleanalyzer"
            };

            var headTag = HtmlHelper.GetElements(document, "head");
            if (headTag.Any())
            {
                var titleTags = HtmlHelper.GetElements(headTag.First(), "title");
                if (!titleTags.Any())
                {
                    result.AddResultRule("titleanalyzer_no_title_tag", ResultType.Error);
                }
                else if (titleTags.Count() > 1)
                {
                    result.AddResultRule("titleanalyzer_multiple_title_tags", ResultType.Error);
                }
                else
                {
                    var firstTitleTag = titleTags.FirstOrDefault();
                    if (firstTitleTag != null)
                    {
                        var titleValue = firstTitleTag.InnerText;

                        if (string.IsNullOrWhiteSpace(titleValue))
                        {
                            result.AddResultRule("titleanalyzer_no_title_value", ResultType.Error);
                        }
                        else
                        {
                            titleValue = titleValue.Trim();

                            if (titleValue.Length > 60)
                            {
                                result.AddResultRule("titleanalyzer_title_too_long", ResultType.Warning);
                            }

                            if (titleValue.Length < 10)
                            {
                                result.AddResultRule("titleanalyzer_title_too_short", ResultType.Warning);
                            }
                            else if (titleValue.Length < 50)
                            {
                                result.AddResultRule("titleanalyzer_title_less_than_50_characters", ResultType.Hint);
                            }

                            if (titleValue.Length <= 60 && titleValue.Length >= 50)
                            {
                                result.AddResultRule("titleanalyzer_title_more_than_50_less_than_60_characters",
                                    ResultType.Success);
                            }
                        }
                    }
                }
            }
            else
            {
                result.AddResultRule("titleanalyzer_no_head_tag", ResultType.Error);
            }
            return result;
        }
    }
}
