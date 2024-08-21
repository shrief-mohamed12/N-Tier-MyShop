﻿using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.ViewModels
{
    public  class ProductVM
    {
        public Product? Product { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }   
    }
}
