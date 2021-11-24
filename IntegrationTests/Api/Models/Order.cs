using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class Order
    {
        public Order(Guid customerId)
        {
            CustomerId = customerId;
        }

        public Guid CustomerId { get; }
        public List<Product> Items { get; set; }

        public void AddItem(Product product)
        {
            Items.Add(product);
        }

        public void DeleteItem(Product product)
        {
            Items.Remove(product);
        }
    }
}
