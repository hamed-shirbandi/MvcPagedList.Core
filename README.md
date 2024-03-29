# What is MvcPagedList.Core ?

It is a light tool to create easily paging in ASP.NET Core MVC that gets data as chunks from database.
- Very simple to use
- Quick access to pages by scrolling
- Enable ajax with full support for [data-ajax attributes](https://github.com/hamed-shirbandi/MvcPagedList.Core/issues/11#issuecomment-984938612) 
- Built-in ajax loading
- Full control to customize the UI
- No effect on Controllers, Views and ViewModels
- No need to link any js or css file
- Improve performance by getting data for each pages as a new chunk from database
- [Free](https://github.com/hamed-shirbandi/MvcPagedList.Core/blob/master/LICENSE) and [open-source](https://github.com/hamed-shirbandi/MvcPagedList.Core)
- [Full documentation](https://github.com/hamed-shirbandi/MvcPagedList.Core#what-is-mvcpagedlistcore-) on GitHub
- Easy installation via [NuGet](https://www.NuGet.org/packages/MvcPagedList.Core/)
- Support all MVC projects with netcoreapp3.1 TargetFramework and after

# Install via NuGet

To install MvcPagedList.Core, run the following command in the Package Manager Console
```code
pm> Install-Package MvcPagedList.Core
```
You can also view the [package page](https://www.NuGet.org/packages/MvcPagedList.Core/) on NuGet.


# How to use ?
It is very simple to use. You just need to provide [PageCount](https://github.com/hamed-shirbandi/MvcPagedList.Core/blob/a43a70547301b0bde2f179afe92f5327de022415/src/MvcPagedList.Core.Example/Controllers/HomeController.cs#L46), [CurrentPage](https://github.com/hamed-shirbandi/MvcPagedList.Core/blob/a43a70547301b0bde2f179afe92f5327de022415/src/MvcPagedList.Core.Example/Controllers/HomeController.cs#L47) and [TotalItemCount](https://github.com/hamed-shirbandi/MvcPagedList.Core/blob/a43a70547301b0bde2f179afe92f5327de022415/src/MvcPagedList.Core.Example/Controllers/HomeController.cs#L48) in your controller and send them by [ViewBags](https://github.com/hamed-shirbandi/MvcPagedList.Core/blob/e868ad365424c474a8a7324cf4987c425bc912f6/MvcPagedList.Core.Example/Controllers/HomeController.cs#L44) to the view. Then in the view you just need to call [` PagedList.Pager() `](https://github.com/hamed-shirbandi/MvcPagedList.Core/blob/e868ad365424c474a8a7324cf4987c425bc912f6/MvcPagedList.Core.Example/Views/Home/_UsersPagedList.cshtml#L3) to show the pagination. It also has a [sample project](https://github.com/hamed-shirbandi/MvcPagedList.Core/tree/master/src/MvcPagedList.Core.Example) on GitHub.
> ` PagedList.Pager() ` has some parameters. You can use ` actionName `, ` controllerName ` and ` areaName ` to send requests created by the pagination. Another parameters are ` routeValues `, ` ajaxAttributes ` and ` pagerOptions ` that are described in the following.

```code

@PagedList.Pager(actionName: "search", controllerName: "home",
    pagerOptions: new PagerOptions
    {
        CurrentPage = (int)ViewBag.CurrentPage,
        PageCount = (int)ViewBag.PageSize,
        TotalItemCount = (int)ViewBag.TotalItemCount,
    } )
    
```

That's it! Just by adding the above lines of code, You will have the pagination.

# How to provide needed data for the pager ?
` PagerOptions ` parameter in ` PagedList.Pager() ` needs ` CurrentPage `, ` PageCount ` and ` TotalItemCount`  to make the pagination. A simple way to pass them from controller to the pager in the view is using ViewBags. For providing these items you need to run some queries on database, Here is an example to provide these data in a [controller](https://github.com/hamed-shirbandi/MvcPagedList.Core/blob/113a52b64fd6440f3cc0ec28f930d32d2a854e71/MvcPagedList.Core.Example/Controllers/HomeController.cs#L41) by using an [application service](https://github.com/hamed-shirbandi/MvcPagedList.Core/blob/113a52b64fd6440f3cc0ec28f930d32d2a854e71/MvcPagedList.Core.Example/Service/Users/UserService.cs#L85). Review the codes to learn how to do it.


# How to pass route values to the controller?
` PagedList.Pager() ` has a routeValues parameter you can use like below to pass your data to the controller:
```code

@PagedList.Pager(actionName: "search", controllerName: "home",
    routeValues: new
    {
        //Here we set term as a value and send it to the controller
        term = Context.Request.Query["term"],
    },
    pagerOptions: 
    {
      ...
    } )
    
```


# How to enable ajax pagination?
` PagedList.Pager() ` has an ` ajaxAttributes ` parameter you can use like below to enable ajax:
> Here you can use all [data-ajax attributes](https://github.com/hamed-shirbandi/MvcPagedList.Core/issues/11#issuecomment-984938612). Just replace "-" with "_"

> Don't forget to add [ajax unobtrusive](https://github.com/hamed-shirbandi/MvcPagedList.Core/blob/02cc537bc644a19a4b9f51a759c11b99f8fbff13/src/MvcPagedList.Core.Example/Views/Shared/_ValidationScriptsPartial.cshtml#L3) scripts to your page
```code

@PagedList.Pager(actionName: "search", controllerName: "home",
    ajaxAttributes: new
    {
        data_ajax = "true",
        data_ajax_update = "#ajax-show-list",
        data_ajax_method = "GET",
        data_ajax_mode = "replace"
    },
    pagerOptions: 
    {
      ...
    } )
    
```
 

# How to set a custom ajax loading?
Default ajax loading is enabled by default but if you want to have your own loading just add your loading html and css to the page and then set its id for data_ajax_loading in ajaxAttributes like below:
> Don't forget to add # at the first of the id. for example, if your loading elemnt id is my-custom-ajax-loading should set ` data_ajax_loading = "#my-custom-ajax-loading" ` 

```code

@PagedList.Pager(actionName: "search", controllerName: "home",
    ajaxAttributes: new
    {
        data_ajax = "true",
        data_ajax_loading = "#my-custom-ajax-loading",
        data_ajax_update = "#ajax-show-list",
        data_ajax_method = "GET",
        data_ajax_mode = "replace"
    },
    pagerOptions: 
    {
      ...
    } )
    
```


# How to customize the pagination UI?
` PagedList.Pager() ` has a ` pagerOptions ` parameter. It has many properties that you can use to customize the pagination. Here is a table to describe the properties:
| Prop Name     | Description  
| ------------- | ------------- 
| TotalItemCount        | It is used to show total items count and should be provided by Controller
| PageCount        | It is used to show total pages count and should be provided by Controller
| CurrentPage        | It is used to show current page and should be provided by Controller    
| DisplayMode        | It used to controll display mode for the pagiation and has 3 options: • use ` DisplayMode.Never ` when you want to hide the pagination in UI • use ` DisplayMode.IfNeeded ` when you want to show the pagination if there is atleast 2 page to show • use ` DisplayMode.Always ` when you want to show the pagination even there is 0 or 1 page 
| DisplayLinkToPreviousPage         |   Set it to ` false ` if you dont want to show the previous btn
| DisplayLinkToNextPage         | Set it to ` false ` if you dont want to show the next btn         
| DisplayInfoArea         | Set it to ` false ` if you dont want to show the information area and just need to show the pages. information area is a section below the pages to show TotalItemCount and PageCount and CurrentPage    
| DisplayPageCountAndCurrentLocation         | Set it to ` false ` if you dont want to show PageCount and CurrentPage in info area    
| CurrentLocationFormat         | It is a lable for currentPage. for example 'page' in 'page 1 of 12'   
| PageCountFormat         | It is a lable for PageCount. for example 'of' in 'page 1 of 12'    
| DisplayTotalItemCount         |  Set it to ` false ` if you dont want to show TotalItemCount in info area      
| TotalItemCountFormat         |  It is a lable for TotalItemCount. for example 'total items count' in 'total items count 400'     
| LinkToNextPageFormat         | It is a lable for next btn. for example 'next' or '>>'   
| LinkToPreviousPageFormat         | It is a lable for prev btn. for example 'prev' or '<<'       
| WrapperClasses         |  It is used to add a class from your custom css for main wrapper of the pagination    
| UlElementClasses         | It is used to add a class from your custom css for ul tage that wrapps all page numbers    
| LiElementClasses         | It is used to add a class from your custom css for li tags that wrapps each page number  
| GetStyleSheetFileFromCdn         | Set it to ` false ` if you dont want to load [css file](https://github.com/hamed-shirbandi/MvcPagedList.Core/blob/master/MvcPagedList.Core/wwwroot/css/MvcPagedList.Core.3.0.0.css) from CDN. Then you have to add it to your pages. Also you can download the [css file](https://github.com/hamed-shirbandi/MvcPagedList.Core/blob/master/MvcPagedList.Core/wwwroot/css/MvcPagedList.Core.3.0.0.css) and modify it to make your own style then add it to your pages    
| DisplayPageNumbers         | Set it to ` false ` if you dont want to show the pages and just need to show info area    
| EnableDefaultAjaxLoading         | Set it to ` false ` if you dont want to show ajax loading element    
| AjaxLoadingFormat         | Let it empty if you want to show default ajax loading or write a text to show when ajax request is sending by the pagination. for example you can write 'Please wait ... ' and it will be shown during an ajax request     


# Supporting
We work hard to make something useful for .NET community, please give a star ⭐ if this project helped you!
We need your support by giving a star or contributing or sharing this project with anyone who can benefit from it.


# Author & License
This project is developed by [Hamed Shirbandi](https://github.com/hamed-shirbandi) under [MIT](https://github.com/hamed-shirbandi/MvcPagedList.Core/blob/master/LICENSE) licensed.
Find Hamed around the web and feel free to ask your question.

<a href="https://www.linkedin.com/in/hamed-shirbandi"><img alt="LinkedIn" src="https://github.com/hamed-shirbandi/hamed-shirbandi/blob/main/docs/LinkedIn-v2.png" width="35"></a>
<a href="https://www.instagram.com/hamedshirbandi"><img alt="Instagram" src="https://github.com/hamed-shirbandi/hamed-shirbandi/blob/main/docs/Instagram-v2.png" width="35"></a>
<a href="https://github.com/hamed-shirbandi"><img alt="GitHub" src="https://github.com/hamed-shirbandi/hamed-shirbandi/blob/main/docs/GitHub-v2.png" width="35"></a>
<a href="https://medium.com/@hamed.shirbandi"><img alt="Medium" src="https://github.com/hamed-shirbandi/hamed-shirbandi/blob/main/docs/Medium-v2.png" width="35"></a>
<a href="https://www.nuget.org/profiles/hamed-shirbandi"><img alt="NuGet" src="https://github.com/hamed-shirbandi/hamed-shirbandi/raw/main/docs/Nuget-v2.png" width="35"></a>
<a href="mailto:hamed.shirbandi@gmail.com"><img alt="Email" src="https://github.com/hamed-shirbandi/hamed-shirbandi/blob/main/docs/Email-v2.png" width="35"></a>
<a href="https://t.me/hamed_shirbandi"><img alt="Telegram" src="https://github.com/hamed-shirbandi/hamed-shirbandi/blob/main/docs/Telegram-v2.png" width="35"></a>
<a href="https://twitter.com/hamed_shirbandi"><img alt="Twitter" src="https://github.com/hamed-shirbandi/hamed-shirbandi/blob/main/docs/Twitter-v2.png" width="35"></a>


# Screenshots

![alt text](https://github.com/hamed-shirbandi/MvcPagedList.Core/blob/master/src/MvcPagedList.Core.Example/wwwroot/images/Screenshot-1.png)
