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
        #region Fields

        static TagBuilder wrapper;
        static TagBuilder prevBtn;
        static TagBuilder nextBtn;
        static TagBuilder nav;
        static TagBuilder ul;
        static bool hasNextPage;
        static bool hasPreviousPage;
        static bool isFirstPage;
        static bool isLastPage;
        const string defaultAjaxLoadingElementId = "mp-ajax-loading";
        const string data_ajax_loading = "data-ajax-loading";
        static IDictionary<string, string> ajaxAttributesKeyValueList;
        #endregion

        #region Public Methods



        /// <summary>
        /// 
        /// </summary>
        public static IHtmlContent Pager(string actionName, PagerOptions pagerOptions, string controllerName = "", string areaName = "", object routeValues = null, object ajaxAttributes = null)
        {
            if (!CanShowPagination(pagerOptions))
                return null;

            InitialWrapper(pagerOptions);

            GenerateStylesheetCdnLink(pagerOptions);

            SetAjaxAttributesKeyValueList(ajaxAttributes);

            EnableDefaultAjaxLoading(pagerOptions);

            InitialPager(pagerOptions);

            InitialTags(pagerOptions);

            GeneratePrevBtn(actionName, controllerName, areaName, routeValues, pagerOptions);

            GeneratePageNumbers(actionName, controllerName, areaName, routeValues, pagerOptions);

            GenerateNextBtn(actionName, controllerName, areaName, routeValues, pagerOptions);

            nav.InnerHtml.AppendHtml(ul);

            GenerateInfoArea(pagerOptions);

            return wrapper;
        }



        #endregion

        #region Private Methods



        /// <summary>
        /// 
        /// </summary>
        private static void InitialWrapper(PagerOptions pagerOptions)
        {
            wrapper = new TagBuilder("div");
        }



        /// <summary>
        /// 
        /// </summary>
        private static void GenerateStylesheetCdnLink(PagerOptions pagerOptions)
        {
            if (pagerOptions.GetStyleSheetFileFromCdn == true)
                wrapper.InnerHtml.AppendHtml(@"<link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/gh/hamed-shirbandi/MvcPagedList.Core/MvcPagedList.Core/wwwroot/css/MvcPagedList.Core.3.0.0.css"" />");
        }



        /// <summary>
        /// 
        /// </summary>
        private static void EnableDefaultAjaxLoading(PagerOptions pagerOptions)
        {
            if (!CanShowAjaxLoading(pagerOptions))
                return;

            AddDefaultAjaxLoadingDataToAjaxAttributes();

            GenerateAjaxLoadingElements(pagerOptions);
        }



        /// <summary>
        /// 
        /// </summary>
        private static void GenerateAjaxLoadingElements(PagerOptions pagerOptions)
        {
            var loadingInnerDiv = new TagBuilder("div");
            loadingInnerDiv.AddCssClass("mp-loading-inner");
            loadingInnerDiv.MergeAttribute("id", defaultAjaxLoadingElementId);

            var loadingDiv = new TagBuilder("div");
            loadingDiv.AddCssClass("mp-loading");

            var loadingClass = string.IsNullOrWhiteSpace(pagerOptions.AjaxLoadingFormat) ? "mp-loading-figure" : "mp-loading-label";
            var contentDiv = new TagBuilder("div");
            contentDiv.AddCssClass(loadingClass);
            contentDiv.InnerHtml.AppendHtml(pagerOptions.AjaxLoadingFormat);

            loadingDiv.InnerHtml.AppendHtml(contentDiv);
            loadingInnerDiv.InnerHtml.AppendHtml(loadingDiv);
            wrapper.InnerHtml.AppendHtml(loadingInnerDiv);
        }



        /// <summary>
        /// 
        /// </summary>
        private static void InitialTags(PagerOptions pagerOptions)
        {
            if (CanShowNextBtn(pagerOptions))
            {
                nextBtn = new TagBuilder("a");
                nextBtn.AddCssClass("mp-btn");
            }

            if (CanShowPreviousBtn(pagerOptions))
            {
                prevBtn = new TagBuilder("a");
                prevBtn.AddCssClass("mp-btn");
            }

            nav = new TagBuilder("nav");
            nav.MergeAttribute("aria-label", "Page navigation");
            nav.AddCssClass(pagerOptions.WrapperClasses);

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
        private static void GenerateNextBtn(string actionName, string controllerName, string areaName, object routeValues, PagerOptions pagerOptions)
        {
            if (!CanShowNextBtn(pagerOptions))
                return;

            var page = pagerOptions.currentPage >= pagerOptions.PageCount ? pagerOptions.PageCount : pagerOptions.currentPage + 1;
            nextBtn.MergeAjaxAttribute();
            nextBtn.MergeUrlAttribute(actionName, controllerName, areaName, routeValues, page);
            nextBtn.InnerHtml.AppendHtml(pagerOptions.LinkToNextPageFormat);
        }



        /// <summary>
        /// 
        /// </summary>
        private static void GeneratePageNumbers(string actionName, string controllerName, string areaName, object routeValues, PagerOptions pagerOptions)
        {
            if (!pagerOptions.DisplayPageNumbers)
                return;

            for (int page = 1; page <= pagerOptions.PageCount; page++)
            {
                var li = new TagBuilder("li");
                li.AddCssClass(pagerOptions.LiElementClasses);

                if (page == 1 && pagerOptions.currentPage > pagerOptions.PageCount)
                    li.AddCssClass("is-active");
                else if (page == pagerOptions.currentPage)
                    li.AddCssClass("is-active");

                var a = new TagBuilder("a");
                a.MergeAjaxAttribute();
                a.MergeUrlAttribute(actionName, controllerName, areaName, routeValues, page);
                a.InnerHtml.AppendHtml(page.ToString());
                li.InnerHtml.AppendHtml(a);
                ul.InnerHtml.AppendHtml(li);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        private static void GeneratePrevBtn(string actionName, string controllerName, string areaName, object routeValues, PagerOptions pagerOptions)
        {
            if (!CanShowPreviousBtn(pagerOptions))
                return;

            var page = pagerOptions.currentPage <= 1 ? 1 : pagerOptions.currentPage - 1;
            prevBtn.MergeAjaxAttribute();
            prevBtn.MergeUrlAttribute(actionName, controllerName, areaName, routeValues, page);
            prevBtn.InnerHtml.AppendHtml(pagerOptions.LinkToPreviousPageFormat);
        }



        /// <summary>
        /// 
        /// </summary>
        private static void GenerateInfoArea(PagerOptions pagerOptions)
        {
            if (pagerOptions.DisplayInfoArea == false)
                return;

            var footerDiv = new TagBuilder("div");
            footerDiv.AddCssClass("mp-pagination-footer");

            var infoDiv = new TagBuilder("div");
            infoDiv.AddCssClass("mp-pagination-info");

            if (hasPreviousPage)
                infoDiv.InnerHtml.AppendHtml(prevBtn);

            if (hasNextPage)
                infoDiv.InnerHtml.AppendHtml(nextBtn);

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

            nav.InnerHtml.AppendHtml(footerDiv);
            wrapper.InnerHtml.AppendHtml(nav);
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
        private static void MergeAjaxAttribute(this TagBuilder tagBuilder)
        {
            foreach (var attribute in ajaxAttributesKeyValueList)
                tagBuilder.Attributes.Add(attribute.Key, attribute.Value.ToString());
        }



        /// <summary>
        /// 
        /// </summary>
        private static bool CanShowPagination(PagerOptions pagerOptions)
        {
            return pagerOptions.DisplayMode == PagedListDisplayMode.Always
                || (pagerOptions.DisplayMode == PagedListDisplayMode.IfNeeded && pagerOptions.PageCount > 1);
        }



        /// <summary>
        /// 
        /// </summary>
        private static bool CanShowNextBtn(PagerOptions pagerOptions)
        {
            return pagerOptions.DisplayLinkToNextPage == true;
        }



        /// <summary>
        /// 
        /// </summary>
        private static bool CanShowPreviousBtn(PagerOptions pagerOptions)
        {
            return pagerOptions.DisplayLinkToPreviousPage == true;
        }



        /// <summary>
        /// 
        /// </summary>
        private static bool CanShowAjaxLoading(PagerOptions pagerOptions)
        {
            return ajaxAttributesKeyValueList.Count > 0
                && pagerOptions.EnableDefaultAjaxLoading == true
                && AjaxAttributesAlreadyHasCustomAjaxLoading() == false;

        }



        /// <summary>
        /// Check if there is a data_ajax_loading already defined in ajaxAttributes and that is not empty and not the same as defaultAjaxLoadingElementId
        /// It ignore default ajax loading even EnableDefaultAjaxLoading is true
        /// </summary>
        private static bool AjaxAttributesAlreadyHasCustomAjaxLoading()
        {
            return  ajaxAttributesKeyValueList.Any(a => a.Key == data_ajax_loading && !string.IsNullOrEmpty(a.Value) && a.Value != $"#{defaultAjaxLoadingElementId}");
        }



        /// <summary>
        /// 
        /// </summary>
        private static void SetAjaxAttributesKeyValueList(object ajaxAttributes)
        {
            if (ajaxAttributes is null)
                ajaxAttributesKeyValueList = new Dictionary<string, string>();
            else
                ajaxAttributesKeyValueList = ajaxAttributes.GetType().GetProperties().Select(p => new { Key = p.Name.Replace("_", "-").ToString(), Value = p.GetValue(ajaxAttributes, null)?.ToString() }).ToDictionary(d => d.Key, d => d.Value);
        }



        /// <summary>
        /// 
        /// </summary>
        private static void AddDefaultAjaxLoadingDataToAjaxAttributes()
        {
            if (ajaxAttributesKeyValueList.ContainsKey(data_ajax_loading))
            {
                var customAjaxLoading = ajaxAttributesKeyValueList.FirstOrDefault(a => a.Key == data_ajax_loading);
                if (customAjaxLoading.Value == $"#{defaultAjaxLoadingElementId}")
                    return;
                else
                    ajaxAttributesKeyValueList.Remove(data_ajax_loading);
            }

            ajaxAttributesKeyValueList.Add(data_ajax_loading, $"#{defaultAjaxLoadingElementId}");
        }


        #endregion

    }
}
