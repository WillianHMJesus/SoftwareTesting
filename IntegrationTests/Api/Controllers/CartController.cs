using Api.Models;
using Api.Repositories;
using Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Api.Controllers
{
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private Guid CustomerId = new Guid("e80f8cc0-579e-4608-a0af-be934a7cdd9a");

        public CartController(IProductRepository productRepository,
            IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        [HttpPost]
        [Route("api/Cart")]
        public IActionResult AddItem(ItemViewModel model)
        {
            var product = _productRepository.GetById(model.Id);
            if (product == null) return BadRequest();

            if (product.Quantity < model.Quantity)
            {
                return BadRequest("Produto com estoque insuficiente");
            }

            var order = _orderRepository.GetByCustomerId(CustomerId);
            if (order == null) order = new Order(CustomerId);
            order.AddItem(product);

            return Ok();
        }

        [HttpDelete]
        [Route("api/Cart/{id:guid}")]
        public IActionResult RemoveItem(Guid id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return BadRequest();

            var order = _orderRepository.GetByCustomerId(CustomerId);
            if (order == null) return BadRequest();
            order.DeleteItem(product);

            return Ok();
        }
    }
}
