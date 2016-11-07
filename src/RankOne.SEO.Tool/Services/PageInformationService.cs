﻿using System.Linq;
using System.Web;
using HtmlAgilityPack;
using RankOne.ExtensionMethods;
using RankOne.Helpers;
using RankOne.Models;
using Umbraco.Web;

namespace RankOne.Services
{
    public class PageInformationService
    {
        public PageInformation GetpageInformation(int id)
        {
            var pageInformation = new PageInformation();

            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var content = umbracoHelper.TypedContent(id);
            var htmlObject = umbracoHelper.RenderTemplate(id);

            var html = htmlObject.ToHtmlString();

            var htmlParser = new HtmlDocument();
            htmlParser.LoadHtml(HttpUtility.HtmlDecode(html));

            var headTag = htmlParser.DocumentNode.GetDescendingElements("head");

            if (headTag.Any())
            {
                var titleTags = headTag.First().GetDescendingElements("title");

                if (titleTags.Any())
                {
                    pageInformation.Title = titleTags.First().InnerText;
                }
            }

            var metaTags = htmlParser.DocumentNode.GetDescendingElements("meta");

            var attributeValues = from metaTag in metaTags
                                  let attribute = metaTag.GetAttribute("name")
                                  where attribute != null
                                  where attribute.Value == "description"
                                  select metaTag.GetAttribute("content");


            if (attributeValues.Any())
            {
                pageInformation.Description = attributeValues.First().Value;
            }
            pageInformation.Url = content.UrlWithDomain();

            return pageInformation;
        }
    }
}
