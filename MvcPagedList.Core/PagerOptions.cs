
namespace MvcPagedList.Core
{
  public  class PagerOptions
    {
        public PagerOptions()
        {
            DisplayMode = PagedListDisplayMode.IfNeeded;
            DisplayLinkToPreviousPage = PagedListDisplayMode.IfNeeded;
            DisplayLinkToNextPage = PagedListDisplayMode.IfNeeded;
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
        }

        public PagedListDisplayMode DisplayMode { get; set; }
        public PagedListDisplayMode DisplayLinkToPreviousPage { get; set; }
        public PagedListDisplayMode DisplayLinkToNextPage { get; set; }
        public int TotalItemCount { get; set; }
        public int PageCount { get; set; }
        public int currentPage { get; set; }
        public bool DisplayInfoArea { get; set; }
        public bool DisplayPageCountAndCurrentLocation { get; set; }
        public string CurrentLocationFormat { get; set; }
        public string PageCountFormat { get; set; }
        public bool DisplayTotalItemCount { get; set; }
        public string TotalItemCountFormat { get; set; }
        public string LinkToNextPageFormat { get; set; }
        public string LinkToPreviousPageFormat { get; set; }
        public string WrapperClasses { get; set; }
        public string UlElementClasses { get; set; }
        public string LiElementClasses { get; set; }


    }
}
