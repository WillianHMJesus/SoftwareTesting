using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Models
{
    public static class CartMock
    {
        public static List<Order> Cart = new List<Order>()
        {
            new Order(new Guid("e80f8cc0-579e-4608-a0af-be934a7cdd9a"))
            {
                Items = new List<Product>
                {
                    Product.Products.FirstOrDefault(x => x.Id == new Guid("21583148-21d7-4499-bcd8-162c079232fe"))
                }
            }
        };
    }
}
