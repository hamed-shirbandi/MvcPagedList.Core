# What is this ?

Easily paging in  ASP.NET Core that get data as chunks from database

# Install via NuGet

To install MvcPagedList.Core, run the following command in the Package Manager Console
```code
pm> Install-Package MvcPagedList.Core
```
You can also view the [package page](https://www.nuget.org/packages/MvcPagedList.Core/) on NuGet.

# How to use ?

## Add Style if you dont want to load MvcPagedList.Core.css from cdn

The needed style load from cdn by default. if you dont want this so you must add [MvcPagedList.Core.css](https://github.com/hamed-shirbandi/MvcPagedList.Core/blob/master/MvcPagedList.Core/wwwroot/css/MvcPagedList.Core.css) to your layout.cshtml. this css copied to your project in wwwroot/css folder after installing and then you must to set GetStyleSheetFileFromCdn to false in pager defination.

## Add Scripts if you need to ajax paging
If you need to ajax paging so you need to add following scripts to your layout.cshtml
```code
jquery
jquery-validation
jquery-validation-unobtrusive
jquery-ajax-unobtrusive

```

## Implementing back-end methods

now we need to implement method that take data from database like bellow

``` c#
 public IEnumerable<UserOutput> Search(int page, int recordsPerPage, string term, SortBy sortBy, SortOrder sortOrder, out int pageSize, out int TotalItemCount)
        {
            var queryable = users.AsQueryable();

            // by term
            if (!string.IsNullOrEmpty(term))
            {
                queryable = queryable.Where(c => c.Family.Contains(term) || c.Name.Contains(term));

            }

            //sorting
            switch (sortBy)
            {
                case SortBy.AddDate:
                    queryable = sortOrder == SortOrder.Asc ? queryable.OrderBy(u => u.AddDate) : queryable.OrderByDescending(u => u.AddDate);
                    break;
                case SortBy.DisplayName:
                    queryable = sortOrder == SortOrder.Asc ? queryable.OrderBy(u => u.Name).ThenBy(u => u.Family) : queryable.OrderByDescending(u => u.Name).ThenByDescending(u => u.Family);
                    break;
                default:
                    break;
            }

            // get total and pageSize
            TotalItemCount = queryable.Count();
            pageSize = (int)Math.Ceiling((double)TotalItemCount / recordsPerPage);

            page = page > pageSize || page < 1 ? 1 : page;

            //take recordes
            var skiped = (page - 1) * recordsPerPage;
            queryable = queryable.Skip(skiped).Take(recordsPerPage);


            return queryable.Select(u => new UserOutput
            {
                Id = u.Id,
                AddDate = u.AddDate.ToShortDateString(),
                Name = u.Name,
                Family = u.Family,

            }).ToList();
        }
```
Then we need to call this method in controller and set some ViewBags for index and search action

```c#
        public ActionResult Index()
        {
  
            var page = 1;
            pageSize = 0;
            recordsPerPage = 5;
            TotalItemCount = 0;

 
            var users = _userService.Search(page: page, recordsPerPage:recordsPerPage, term:"", sortBy:SortBy.AddDate, sortOrder:SortOrder.Desc, pageSize: out pageSize, TotalItemCount:out TotalItemCount);
            
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalItemCount = TotalItemCount;

            return View(users);
        }



 public ActionResult Search(int page = 1, string term = "",SortBy sortBy = SortBy.AddDate, SortOrder sortOrder = SortOrder.Desc)
        {

            pageSize = 0;
            recordsPerPage = 5;
            TotalItemCount = 0;

            var users = _userService.Search(page: page, recordsPerPage: recordsPerPage, term: term,sortBy: sortBy, sortOrder: sortOrder, pageSize: out pageSize, TotalItemCount: out TotalItemCount);

         
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalItemCount = TotalItemCount;

            return PartialView("_UsersList", users);
        }


```

## Add Views

And now we need to add index.cshtml like this :

```code
@{
    ViewBag.Title = "Home Page";
}
@model IEnumerable<UserOutput>

<div class="panel panel-default">
    <div class="panel-heading">
        <div id="search-form" class="form-inline">
            <form class="form" asp-action="search" data-ajax="true" data-ajax-method="GET" data-ajax-update="#ajax-show-list" data-ajax-loading="#global-ajax-loading">
                <div class="form-group">
                    @Html.TextBox("term", null, new { @class = "form-control", id = "", placeholder = "search ..." })
                </div>
                <div class="form-group">
                    @Html.DropDownList("SortBy", Html.GetEnumSelectList(typeof(SortBy)), new { @class = "form-control" })
                </div>
                <div class="form-group">
                    @Html.DropDownList("SortOrder", Html.GetEnumSelectList(typeof(SortOrder)), new { @class = "form-control" })
                </div>
                <div class="form-group">
                    <input type="submit" class="btn btn-primary" value="Search" />
                </div>
            </form>

        </div>

    </div>
    <div id="ajax-show-list" class="panel-body">
        @Html.Partial("_UsersList", Model)
    </div>
</div>



```
Add partial view with _UsersList.cshtml name
```code
@model IEnumerable<MvcPagedList.Example.Service.Users.Dto.UserOutput>

@if (Model.Count() == 0)
{
    <div class="alert alert-info">
        No User found
    </div>
}
else
{
    <table class="table table-striped table-hover">
        <thead>
            <tr>
                <td> Name</td>
                <td> Family</td>
                <td>Add Date</td>
            </tr>
        </thead>
        <tbody>
            @foreach (var item in Model)
                {
                <tr>
                    <td>@item.Name</td>
                    <td>@item.Family</td>
                        <td>@item.AddDate</td>
                    </tr>
            }
        </tbody>
    </table>
}


@Html.Partial("_UsersPagedList")


```

## Add Pager to View

Add partial view with name _UsersPagedList 

```code


@using MvcPagedList.Core;

@PagedList.Pager(actionName: "search", controllerName: "home",areaName:"",
    routeValues: new
    {
        term = Context.Request.Query["term"],
        sortOrder = Context.Request.Query["sortOrder"],
        sortBy = Context.Request.Query["sortBy"],

    },
    ajaxAttributes: new //if you dont need to ajax just set ajaxAttributes to null
    {
        data_ajax = "true",
        data_ajax_loading = "#global-ajax-loading",
        data_ajax_update = "#ajax-show-list",
        data_ajax_method = "GET",
        data_ajax_mode = "replace"


    },
    pagerOptions: new PagerOptions
    {
        currentPage = (int)ViewBag.CurrentPage,
        PageCount = (int)ViewBag.PageSize,
        TotalItemCount = (int)ViewBag.TotalItemCount,
        DisplayMode = PagedListDisplayMode.IfNeeded,
        DisplayInfoArea = true,
        LinkToNextPageFormat = "next",
        LinkToPreviousPageFormat = "prev",
        CurrentLocationFormat = "page",
        PageCountFormat = "of",
        TotalItemCountFormat = "total count",
        //set it to false if you dont want to load style file from cdn then add [MvcPagedList.Core.css](https://github.com/hamed-shirbandi/MvcPagedList.Core/blob/master/MvcPagedList.Core/wwwroot/css/MvcPagedList.Core.css) to your Layout
        GetStyleSheetFileFromCdn=true,
    } )



```


# Screenshots

![alt text](https://github.com/hamed-shirbandi/MvcPagedList/blob/master/MvcPagedList.Example/Content/img/screenShots/Screenshot-1.png)
