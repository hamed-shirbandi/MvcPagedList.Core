using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcPagedList.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class PagedList
    {
        static bool hasNextPage;
        static bool hasPreviousPage;
        static bool isFirstPage;
        static bool isLastPage;
        static TagBuilder prevBtn;
        static TagBuilder nextBtn;
        static TagBuilder wrapper;
        static TagBuilder ul;



        /// <summary>
        /// 
        /// </summary>
        public static IHtmlContent Pager(string actionName, object routeValues, object ajaxAttributes, PagerOptions pagerOptions, string controllerName = "", string areaName = "")
        {

            if (pagerOptions.DisplayMode == PagedListDisplayMode.Never || (pagerOptions.DisplayMode == PagedListDisplayMode.IfNeeded && pagerOptions.PageCount <= 1))
                return null;


            InitialPager(pagerOptions);

            InitialTags(pagerOptions);

            GeneratePrevBtn(actionName, controllerName, areaName, routeValues, ajaxAttributes, pagerOptions);

            GeneratePageNumbers(actionName, controllerName, areaName, routeValues, ajaxAttributes, pagerOptions);

            GenerateNextBtn(actionName, controllerName, areaName, routeValues, ajaxAttributes, pagerOptions);

            wrapper.InnerHtml.AppendHtml(ul);

            GenerateInfoArea(pagerOptions);

            return wrapper;
        }




        /// <summary>
        /// 
        /// </summary>
        private static void InitialTags(PagerOptions pagerOptions)
        {
            prevBtn = new TagBuilder("a");
            prevBtn.AddCssClass("mp-btn");

            nextBtn = new TagBuilder("a");
            nextBtn.AddCssClass("mp-btn");


            wrapper = new TagBuilder("nav");
            wrapper.MergeAttribute("aria-label", "Page navigation");
            wrapper.AddCssClass(pagerOptions.WrapperClasses);



            ul = new TagBuilder("ul");
            ul.AddCssClass(pagerOptions.UlElementClasses);
        }




        /// <summary>
        /// 
        /// </summary>
        private static void InitialPager(PagerOptions pagerOptions)
        {
            hasNextPage = pagerOptions.currentPage < pagerOptions.PageCount;
            hasPreviousPage = pagerOptions.currentPage > 1;
            isFirstPage = pagerOptions.currentPage == 1;
            isLastPage = pagerOptions.currentPage == pagerOptions.PageCount;

        }




        /// <summary>
        /// 
        /// </summary>
        private static void GenerateNextBtn(string actionName, string controllerName, string areaName, object routeValues, object ajaxAttributes, PagerOptions pagerOptions)
        {

            if (pagerOptions.DisplayLinkToNextPage == PagedListDisplayMode.Always || (pagerOptions.DisplayLinkToNextPage == PagedListDisplayMode.IfNeeded && !isLastPage))
            {
                var page = pagerOptions.currentPage >= pagerOptions.PageCount ? pagerOptions.PageCount : pagerOptions.currentPage + 1;

                nextBtn.MergeAjaxAttribute(ajaxAttributes);

                nextBtn.MergeUrlAttribute(actionName, controllerName, areaName, routeValues, page);

                nextBtn.InnerHtml.AppendHtml(pagerOptions.LinkToNextPageFormat);
            }
        }





        /// <summary>
        /// 
        /// </summary>
        private static void GeneratePageNumbers(string actionName, string controllerName, string areaName, object routeValues, object ajaxAttributes, PagerOptions pagerOptions)
        {

            for (int page = 1; page <= pagerOptions.PageCount; page++)
            {
                var li = new TagBuilder("li");
                li.AddCssClass(pagerOptions.LiElementClasses);

                if (page == 1 && pagerOptions.currentPage > pagerOptions.PageCount)
                { 
                    li.AddCssClass("is-active");
                }
                else if (page == pagerOptions.currentPage)
                {
                    li.AddCssClass("is-active");
                }
                
                var a = new TagBuilder("a");
                a.MergeAjaxAttribute(ajaxAttributes);
                a.MergeUrlAttribute(actionName, controllerName, areaName, routeValues, page);
                a.InnerHtml.AppendHtml(page.ToString());

                li.InnerHtml.AppendHtml(a);
                ul.InnerHtml.AppendHtml(li);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        private static void GeneratePrevBtn(string actionName, string controllerName, string areaName, object routeValues, object ajaxAttributes, PagerOptions pagerOptions)
        {
            if (pagerOptions.DisplayLinkToPreviousPage == PagedListDisplayMode.Always || (pagerOptions.DisplayLinkToPreviousPage == PagedListDisplayMode.IfNeeded && !isFirstPage))
            {
                var page = pagerOptions.currentPage <= 1 ? 1 : pagerOptions.currentPage - 1;

                prevBtn.MergeAjaxAttribute(ajaxAttributes);

                prevBtn.MergeUrlAttribute(actionName, controllerName, areaName, routeValues, page);

                prevBtn.InnerHtml.AppendHtml(pagerOptions.LinkToPreviousPageFormat);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private static void GenerateInfoArea(PagerOptions pagerOptions)
        {

            if (pagerOptions.DisplayInfoArea == true)
            {
                var footerDiv = new TagBuilder("div");
                footerDiv.AddCssClass("mp-pagination-footer");

                var infoDiv = new TagBuilder("div");
                infoDiv.AddCssClass("mp-pagination-info");


                if (hasPreviousPage)
                {
                    infoDiv.InnerHtml.AppendHtml(prevBtn);
                }

                if (hasNextPage)
                {
                    infoDiv.InnerHtml.AppendHtml(nextBtn);
                }

              

                if (pagerOptions.DisplayPageCountAndCurrentLocation == true)
                {

                    var pageInfoDiv = new TagBuilder("div");
                    pageInfoDiv.AddCssClass("is-right");
                    pageInfoDiv.InnerHtml.AppendHtml(pagerOptions.CurrentLocationFormat + " " + pagerOptions.currentPage + " " + pagerOptions.PageCountFormat + " " + pagerOptions.PageCount);
                    infoDiv.InnerHtml.AppendHtml(pageInfoDiv);

                }


                if (pagerOptions.DisplayTotalItemCount == true)
                {
                    var totalInfoDiv = new TagBuilder("div");
                    totalInfoDiv.AddCssClass("is-left");
                    totalInfoDiv.InnerHtml.AppendHtml(pagerOptions.TotalItemCountFormat + " " + pagerOptions.TotalItemCount);
                    infoDiv.InnerHtml.AppendHtml(totalInfoDiv);
                }


                footerDiv.InnerHtml.AppendHtml(infoDiv);

                wrapper.InnerHtml.AppendHtml(footerDiv);
            }


        }




        /// <summary>
        /// 
        /// </summary>
        private static void MergeUrlAttribute(this TagBuilder tagBuilder, string actionName, string controllerName, string areaName, object routeValues, int page)
        {
            string values = string.Empty;
            if (routeValues != null)
                values = String.Join("&", routeValues.GetType().GetProperties().Select(p => p.Name + "=" + p.GetValue(routeValues, null)));

            if (!string.IsNullOrEmpty(areaName))
                areaName = "/" + areaName;

            if (!string.IsNullOrEmpty(controllerName))
                controllerName = "/" + controllerName;


            tagBuilder.MergeAttribute("href", areaName + controllerName + "/" + actionName + "?page=" + page + "&" + values);

        }




        /// <summary>
        /// 
        /// </summary>
        private static void MergeAjaxAttribute(this TagBuilder tagBuilder, object ajaxAttributes)
        {
            if (ajaxAttributes == null)
                return;

            var attributes = ajaxAttributes.GetType().GetProperties().Select(p => new { Key = p.Name.Replace("_", "-"), Value = p.GetValue(ajaxAttributes, null) }).ToList();
            foreach (var attribute in attributes)
                tagBuilder.Attributes.Add(attribute.Key, attribute.Value.ToString());

        }
    }
}
