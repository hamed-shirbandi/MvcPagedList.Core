using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcPagedList.Core
{
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
        public static IHtmlContent Pager(string actionName, string controllerName, object routeValues, object ajaxAttributes, PagerOptions pagerOptions)
        {

            if (pagerOptions.DisplayMode == PagedListDisplayMode.Never || (pagerOptions.DisplayMode == PagedListDisplayMode.IfNeeded && pagerOptions.PageCount <= 1))
                return null;


            InitialPager(pagerOptions);

            InitialTags(pagerOptions);

            GeneratePrevBtn(actionName, controllerName, routeValues, ajaxAttributes, pagerOptions);

            GeneratePageNumbers(actionName, controllerName, routeValues, ajaxAttributes, pagerOptions);

            GenerateNextBtn(actionName, controllerName, routeValues, ajaxAttributes, pagerOptions);

            wrapper.InnerHtml.AppendHtml(ul);

            GenerateInfoArea(actionName, controllerName, routeValues, ajaxAttributes, pagerOptions);

            return wrapper;
        }




        /// <summary>
        /// 
        /// </summary>
        private static void InitialTags(PagerOptions pagerOptions)
        {
            prevBtn = new TagBuilder("a");
            prevBtn.AddCssClass("btn btn-default");

            nextBtn = new TagBuilder("a");
            nextBtn.AddCssClass("btn btn-default");


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
        public static void GenerateNextBtn(string actionName, string controllerName, object routeValues, object ajaxAttributes, PagerOptions pagerOptions)
        {

            if (pagerOptions.DisplayLinkToNextPage == PagedListDisplayMode.Always || (pagerOptions.DisplayLinkToNextPage == PagedListDisplayMode.IfNeeded && !isLastPage))
            {
                var span = new TagBuilder("span");
                span.InnerHtml.AppendHtml(pagerOptions.LinkToNextPageFormat);
                var page = pagerOptions.currentPage >= pagerOptions.PageCount ? pagerOptions.PageCount : pagerOptions.currentPage + 1;


                nextBtn.MergeAjaxAttribute(ajaxAttributes);

                nextBtn.MergeUrlAttribute(actionName, controllerName, routeValues, page);

                nextBtn.InnerHtml.AppendHtml(span);
            }
        }





        /// <summary>
        /// 
        /// </summary>
        public static void GeneratePageNumbers(string actionName, string controllerName, object routeValues, object ajaxAttributes, PagerOptions pagerOptions)
        {

            for (int page = 1; page <= pagerOptions.PageCount; page++)
            {
                var li = new TagBuilder("li");
                li.AddCssClass(pagerOptions.LiElementClasses);

                if (page == 1 && pagerOptions.currentPage > pagerOptions.PageCount)
                {
                    li.AddCssClass("active");
                }
                else if (page == pagerOptions.currentPage)
                {
                    li.AddCssClass("active");
                }


                var span = new TagBuilder("span");
                span.InnerHtml.AppendHtml(page.ToString());

                var a = new TagBuilder("a");
                a.MergeAjaxAttribute(ajaxAttributes);
                a.MergeUrlAttribute(actionName, controllerName, routeValues, page);
                a.InnerHtml.AppendHtml(span);

                li.InnerHtml.AppendHtml(a);
                ul.InnerHtml.AppendHtml(li);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public static void GeneratePrevBtn(string actionName, string controllerName, object routeValues, object ajaxAttributes, PagerOptions pagerOptions)
        {
            if (pagerOptions.DisplayLinkToPreviousPage == PagedListDisplayMode.Always || (pagerOptions.DisplayLinkToPreviousPage == PagedListDisplayMode.IfNeeded && !isFirstPage))
            {

                var span = new TagBuilder("span");
                span.InnerHtml.AppendHtml(pagerOptions.LinkToPreviousPageFormat);

                var page = pagerOptions.currentPage <= 1 ? 1 : pagerOptions.currentPage - 1;


                prevBtn.MergeAjaxAttribute(ajaxAttributes);

                prevBtn.MergeUrlAttribute(actionName, controllerName, routeValues, page);

                prevBtn.InnerHtml.AppendHtml(span);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public static void GenerateInfoArea(string actionName, string controllerName, object routeValues, object ajaxAttributes, PagerOptions pagerOptions)
        {

            if (pagerOptions.DisplayInfoArea == true)
            {
                var infoDiv = new TagBuilder("div");
                infoDiv.AddCssClass("well well-sm text-primary clearfix text-center");


                if (pagerOptions.DisplayPageCountAndCurrentLocation == true)
                {

                    var infoSpan = new TagBuilder("span");
                    infoSpan.AddCssClass("pull-right");
                    infoSpan.InnerHtml.AppendHtml(pagerOptions.CurrentLocationFormat + " " + pagerOptions.currentPage + " " + pagerOptions.PageCountFormat + " " + pagerOptions.PageCount);
                    infoDiv.InnerHtml.AppendHtml(infoSpan);

                }
                if (pagerOptions.DisplayTotalItemCount == true)
                {
                    var infoSpan = new TagBuilder("span");
                    infoSpan.AddCssClass("pull-left");
                    infoSpan.InnerHtml.AppendHtml(pagerOptions.TotalItemCountFormat + " " + pagerOptions.TotalItemCount);
                    infoDiv.InnerHtml.AppendHtml(infoSpan);
                }


                if (hasPreviousPage)
                {
                    infoDiv.InnerHtml.AppendHtml(prevBtn);
                }

                if (hasNextPage)
                {
                    infoDiv.InnerHtml.AppendHtml(nextBtn);
                }

                wrapper.InnerHtml.AppendHtml(infoDiv);
            }


        }




        /// <summary>
        /// 
        /// </summary>
        private static void MergeUrlAttribute(this TagBuilder tagBuilder, string actionName, string controllerName, object routeValues, int page)
        {
            string values = string.Empty;
            if (routeValues != null)
                values = String.Join("&", routeValues.GetType().GetProperties().Select(p => p.Name + "=" + p.GetValue(routeValues, null)));
            tagBuilder.MergeAttribute("href", "/" + controllerName + "/" + actionName + "?page=" + page + "&" + values);
        }




        /// <summary>
        /// 
        /// </summary>
        public static void MergeAjaxAttribute(this TagBuilder tagBuilder, object ajaxAttributes)
        {
            if (ajaxAttributes == null)
                return;

            var attributes = ajaxAttributes.GetType().GetProperties().Select(p => new { Key = p.Name.Replace("_", "-"), Value = p.GetValue(ajaxAttributes, null) }).ToList();
            foreach (var attribute in attributes)
                tagBuilder.Attributes.Add(attribute.Key, attribute.Value.ToString());

        }
    }


}
