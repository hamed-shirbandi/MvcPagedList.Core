# What is MvcPagedList.Core ?

Easily paging in ASP.NET Core that get data as chunks from database

# Install via NuGet

To install MvcPagedList.Core, run the following command in the Package Manager Console
```code
pm> Install-Package MvcPagedList.Core
```
You can also view the [package page](https://www.nuget.org/packages/MvcPagedList.Core/) on NuGet.

# How to use ?
It is very simple to use. You just need to provide PageCount, currentPage and TotalItemCount in your controller and send them by [ViewBags](https://github.com/hamed-shirbandi/MvcPagedList.Core/blob/e868ad365424c474a8a7324cf4987c425bc912f6/MvcPagedList.Core.Example/Controllers/HomeController.cs#L44) to the view. Then in the view you just need to call [PagedList.Pager()](https://github.com/hamed-shirbandi/MvcPagedList.Core/blob/e868ad365424c474a8a7324cf4987c425bc912f6/MvcPagedList.Core.Example/Views/Home/_UsersPagedList.cshtml#L3) to show the pagination.


```code

@PagedList.Pager(actionName: "search", controllerName: "home",
    pagerOptions: new PagerOptions
    {
        currentPage = (int)ViewBag.CurrentPage,
        PageCount = (int)ViewBag.PageSize,
        TotalItemCount = (int)ViewBag.TotalItemCount,
    } )
    
```

# How to pass route values to the controller?
PagedList.Pager has a routeValues parameter. You can use it like bellow to pass your data to the controller:
```code

@PagedList.Pager(actionName: "search", controllerName: "home",
    routeValues: new
    {
        //Here we set term from Request.Query and set it to the controller
        term = Context.Request.Query["term"],
    },
    pagerOptions: 
    {
      ...
    } )
    
```

# How to enable ajax pagination?
PagedList.Pager has a ajaxAttributes parameter. You can use it like bellow to enable ajax:
> Here you can use all [data-ajax attributes](https://github.com/hamed-shirbandi/MvcPagedList.Core/issues/11#issuecomment-984938612). Just replace "-" with "_"
```code

@PagedList.Pager(actionName: "search", controllerName: "home",
    ajaxAttributes: new
    {
        data_ajax = "true",
        data_ajax_loading = "#global-ajax-loading",
        data_ajax_update = "#ajax-show-list",
        data_ajax_method = "GET",
        data_ajax_mode = "replace"
    },
    pagerOptions: 
    {
      ...
    } )
    
```
 
# How to customize the pagination?
PagedList.Pager has a pagerOptions parameter. It has many properties that you can use to customize the pagination. Here is a table to describe the properties:
| Prop Name     | Description  
| ------------- | ------------- 
| TotalItemCount        | ...    
| PageCount        | ...    
| currentPage        | ...    
| DisplayMode        | ...         
| DisplayLinkToPreviousPage         | ...         
| DisplayLinkToNextPage         | ...         
| DisplayInfoArea         | ...    
| DisplayPageCountAndCurrentLocation         | ...    
| CurrentLocationFormat         | ...    
| PageCountFormat         | ...    
| DisplayTotalItemCount         | ...    
| TotalItemCountFormat         | ...    
| LinkToNextPageFormat         | ...    
| LinkToPreviousPageFormat         | ...    
| WrapperClasses         | ...    
| UlElementClasses         | ...    
| LiElementClasses         | ...    
| GetStyleSheetFileFromCdn         | ...    
| DisplayPageNumbers         | ...    
| DisplayAjaxLoading         | ...    
| AjaxLoadingFormat         | ...       


# Screenshots

![alt text](https://github.com/hamed-shirbandi/MvcPagedList/blob/master/MvcPagedList.Example/Content/img/screenShots/Screenshot-1.png)
