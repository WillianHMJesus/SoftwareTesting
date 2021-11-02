using Checkout.Domain;
using Core.Messages;
using FluentValidation;
using System;

namespace Checkout.Application.Commands
{
    public class AddOrderItemCommand : Command
    {
        public AddOrderItemCommand(Guid customerId, Guid productId, string name, int quantity, decimal unitValue)
        {
            CustomerId = customerId;
            ProductId = productId;
            Name = name;
            Quantity = quantity;
            UnitValue = unitValue;
        }

        public Guid CustomerId { get; }
        public Guid ProductId { get; }
        public string Name { get; }
        public int Quantity { get; }
        public decimal UnitValue { get; }

        public override bool IsValid()
        {
            ValidationResult = new AddOrderItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AddOrderItemValidation : AbstractValidator<AddOrderItemCommand>
    {
        public static string CustomerIdErrorMessage = "Id do cliente inválido";
        public static string ProductIdErrorMessage = "Id do produto inválido";
        public static string NameErrorMessage = "O nome do produto não foi informado";
        public static string MinimumQuantityErrorMessage = "A quantidade miníma de um item é 1";
        public static string MaximumQuantityErrorMessage = $"A quantidade máxima de um item é {Order.MAX_UNITS_ITEM}";
        public static string UnitValueErrorMessage = "O valor do item precisa ser maior que 0";

        public AddOrderItemValidation()
        {
            RuleFor(x => x.CustomerId).NotEqual(Guid.Empty)
                .WithMessage(CustomerIdErrorMessage);

            RuleFor(x => x.ProductId).NotEqual(Guid.Empty)
                .WithMessage(ProductIdErrorMessage);

            RuleFor(x => x.Name).NotEmpty()
                .WithMessage(NameErrorMessage);

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage(MinimumQuantityErrorMessage)
                .LessThanOrEqualTo(Order.MAX_UNITS_ITEM)
                .WithMessage(MaximumQuantityErrorMessage);

            RuleFor(x => x.UnitValue).GreaterThan(0)
                .WithMessage(UnitValueErrorMessage);
        }
    }
}
