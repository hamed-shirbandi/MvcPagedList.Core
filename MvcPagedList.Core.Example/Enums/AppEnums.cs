using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcPagedList.Core.Example.Enums
{

    public enum SortOrder
    {
        [Display(Name = "Desc")]
        Desc = 1,
        [Display(Name = "Asc")]
        Asc = 2,



    };


    public enum SortBy
    {
        [Display(Name = "Add Date")]
        AddDate = 1,
        [Display(Name = "Display Name")]
        DisplayName = 2,

    };
}