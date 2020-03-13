using I_Provider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace I_Provider.App_Code
{
    /// <summary>
    /// Paging Helper
    /// </summary>
    public static class PagingHelpers
    {
        /// <summary>
        /// Paging creator
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pageInfo">Page information</param>
        /// <param name="pageUrl">Create URL for each page</param>
        /// <returns></returns>
        public static MvcHtmlString PageLinks(this HtmlHelper html,
            PageInfo pageInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pageInfo.TotalPages; i++)
            {
                if (i > pageInfo.PageNumber - 3 && i < pageInfo.PageNumber + 3)
                {
                    TagBuilder tag = new TagBuilder("a");
                    tag.MergeAttribute("href", pageUrl(i));
                    tag.InnerHtml = i.ToString();
                    if (i == pageInfo.PageNumber)
                    {
                        tag.AddCssClass("selected");
                        tag.AddCssClass("btn-primary");
                    }
                    tag.AddCssClass("btn btn-default");
                    result.Append(tag.ToString());
                }
                else
                {
                    if (i == pageInfo.PageNumber - 3)
                    {
                        TagBuilder tag = new TagBuilder("a");
                        tag.MergeAttribute("href", pageUrl(1));
                        tag.InnerHtml = "1";
                        tag.AddCssClass("btn btn-default");
                        result.Append(tag.ToString());
                        result.Append("<a class=\"btn btn-default\">...</a>");
                    }
                    if (i == pageInfo.PageNumber + 3)
                    {
                        TagBuilder tag = new TagBuilder("a");
                        tag.MergeAttribute("href", pageUrl(pageInfo.TotalPages));
                        tag.InnerHtml = pageInfo.TotalPages.ToString();
                        tag.AddCssClass("btn btn-default");
                        result.Append("<a class=\"btn btn-default\">...</a>");
                        result.Append(tag.ToString());
                    }
                }
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}