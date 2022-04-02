
namespace MvcPagedList.Core
{
  public  class PagerOptions
    {
        public PagerOptions()
        {
            DisplayMode = PagedListDisplayMode.IfNeeded;
            DisplayLinkToPreviousPage = true;
            DisplayLinkToNextPage = true;
            DisplayInfoArea = true;
            DisplayPageCountAndCurrentLocation = true;
            DisplayTotalItemCount = true;
            LinkToNextPageFormat = "»";
            LinkToPreviousPageFormat = "«";
            CurrentLocationFormat = "page";
            PageCountFormat = "of";
            TotalItemCountFormat = "total item count";
            WrapperClasses = "mp-pagination-nav";
            UlElementClasses = "mp-pagination";
            LiElementClasses = "";
            GetStyleSheetFileFromCdn = true;
            DisplayPageNumbers = true;
            DisplayAjaxLoading = true;
        }

        public PagedListDisplayMode DisplayMode { get; set; }
        public bool DisplayLinkToPreviousPage { get; set; }
        public bool DisplayLinkToNextPage { get; set; }
        public int TotalItemCount { get; set; }
        public int PageCount { get; set; }
        public int currentPage { get; set; }
        /// <summary>
        /// If you dont want to show any information (PageCount,currentPage,TotalItemCount,...) bellow the pagination, set DisplayInfoArea to false
        /// </summary>
        public bool DisplayInfoArea { get; set; }
        /// <summary>
        /// If you dont want to show PageCount and CurrentLocationFormat bellow the pagination, set DisplayPageCountAndCurrentLocation to false
        /// </summary>
        public bool DisplayPageCountAndCurrentLocation { get; set; }
        /// <summary>
        /// A lable for currentPage. for example 'page' 1 of 12
        /// </summary>
        public string CurrentLocationFormat { get; set; }
        /// <summary>
        /// A lable for PageCount. for example page 1 'of' 12
        /// </summary>
        public string PageCountFormat { get; set; }
        /// <summary>
        /// If you dont want to show TotalItemCount bellow the pagination, set DisplayTotalItemCount to false
        /// </summary>
        public bool DisplayTotalItemCount { get; set; }
        /// <summary>
        ///  A lable for TotalItemCount. for example 'total item count' 245
        /// </summary>
        public string TotalItemCountFormat { get; set; }
        /// <summary>
        /// A lable for next btn
        /// </summary>
        public string LinkToNextPageFormat { get; set; }
        /// <summary>
        /// A lable for previous btn
        /// </summary>
        public string LinkToPreviousPageFormat { get; set; }
        /// <summary>
        /// If you want to perform your own styles you can use it to add a class for main wrapper of the pagination
        /// </summary>
        public string WrapperClasses { get; set; }
        /// <summary>
        /// If you want to perform your own styles you can use it to add a class for ul tage
        /// </summary>
        public string UlElementClasses { get; set; }
        /// <summary>
        /// If you want to perform your own styles you can use it to add a class for li tage
        /// </summary>
        public string LiElementClasses { get; set; }
        /// <summary>
        /// If you dont want to load MvcPagedList.Core.css from cdn, You must set this to false and add MvcPagedList.Core.css directly in to your project layout 
        /// </summary>
        public bool GetStyleSheetFileFromCdn { get; set; }
        /// <summary>
        /// if you dont want to show page numbers just set DisplayPageNumbers to false
        /// </summary>
        public bool DisplayPageNumbers { get; set; }
        /// <summary>
        /// if you dont want to show ajax loading set DisplayAjaxLoading to false
        /// </summary>
        public bool DisplayAjaxLoading { get; set; }
        /// <summary>
        /// If you want to show default animated loading let AjaxLoadingFormat empty
        /// If you want to show a message set it in AjaxLoadingFormat. For example AjaxLoadingFormat="please wait ..."
        /// </summary>
        public string AjaxLoadingFormat { get; set; }

    }
}
