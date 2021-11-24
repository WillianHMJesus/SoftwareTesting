using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class Product
    {
        public Product(Guid id, string name, decimal price, int quantity)
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public Guid Id { get; }
        public string Name { get; }
        public decimal Price { get; }
        public int Quantity { get; }

        public static List<Product> Products = new List<Product>
        {
            new Product(new Guid("21583148-21d7-4499-bcd8-162c079232fe"), "Tênis Nike Air Max", 250.0M, 10),
            new Product(new Guid("5e4a4bbd-99d0-4b07-87d3-49324599c2fb"), "Camiseta da Seleção Brasileira", 300.0M, 8),
        };
    }
}
