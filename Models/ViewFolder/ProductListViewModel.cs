﻿namespace ShopQuanAo.Models.ViewFolder
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set;} = Enumerable.Empty<Product>();
        public PagingInfo PagingInfo { get; set;} = new PagingInfo();
    }
}
