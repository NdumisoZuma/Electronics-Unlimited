using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using Electronica.Models;
using System.Web.Mvc;

namespace Electronica.View_Models
{
    public class ProductIndexViewModel
    {
        public IPagedList<Product> Products { get; set; }

        public string Search { get; set; } //replace ViewBag.Search

        public IEnumerable<CategoryWithCount> CatsWithCount { get; set; }

        public string Category {get;set;} // used to select control in view
        public string sortBy { get; set; }
        public Dictionary<string, string> sorts { get; set; }

        public IEnumerable<SelectListItem> CatFilterItems // return list when html is submitted

        {
            get
            {
                var allCats = CatsWithCount.Select(cc => new SelectListItem
                {
                    Value = cc.CategoryName,
                    Text = cc.CatNameWithCount
                });

                return allCats;
            }
        
        }

        
    }


    public class CategoryWithCount
    {
        public int ProductCount { get; set; }
        public string CategoryName { get; set; }
        public string CatNameWithCount
        {
            get
            {
                return CategoryName + "(" + ProductCount.ToString() + ")";
            }
        }
    }
}