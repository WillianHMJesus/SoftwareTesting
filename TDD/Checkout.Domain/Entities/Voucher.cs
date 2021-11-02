using FluentValidation;
using FluentValidation.Results;
using System;

namespace Checkout.Domain
{
    public class Voucher
    {
        public Voucher(string code, 
            decimal? discountPercentage, 
            decimal? discountValue, 
            VoucherDiscountType voucherDiscountType, 
            int quantity, 
            DateTime expirationDate, 
            bool utilized, 
            bool active)
        {
            Code = code;
            DiscountPercentage = discountPercentage;
            DiscountValue = discountValue;
            VoucherDiscountType = voucherDiscountType;
            Quantity = quantity;
            ExpirationDate = expirationDate;
            Utilized = utilized;
            Active = active;
        }

        public string Code { get; private set; }
        public decimal? DiscountPercentage { get; private set; }
        public decimal? DiscountValue { get; private set; }
        public VoucherDiscountType VoucherDiscountType { get; private set; }
        public int Quantity { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool Utilized { get; private set; }
        public bool Active { get; private set; }

        public ValidationResult ValidateIfApplicable()
        {
            return new VoucherValidator().Validate(this);
        }
    }

    public class VoucherValidator : AbstractValidator<Voucher>
    {
        public static string CodeErrorMessage => "Voucher sem código válido.";
        public static string ExpirationDateErrorMessage => "Esse voucher está expirado.";
        public static string ActiveErrorMessage => "Esse voucher não está mais ativo.";
        public static string UtilizedErrorMessage => "Esse voucher já foi utilizado.";
        public static string QuantityErrorMessage => "Esse voucher não está mais disponível.";
        public static string DiscountValueErrorMessage => "O valor do desconto precisa ser superior a 0.";
        public static string DiscountPercentageErrorMessage => "O valor da porcentagem precisa ser superior a 0.";

        public VoucherValidator()
        {
            RuleFor(x => x.Code).NotEmpty()
                .WithMessage(CodeErrorMessage);

            RuleFor(x => x.ExpirationDate).Must(ExpirationDateGreaterThanCurrent)
                .WithMessage(ExpirationDateErrorMessage);

            RuleFor(x => x.Active).Equal(true)
                .WithMessage(ActiveErrorMessage);

            RuleFor(x => x.Utilized).Equal(false)
                .WithMessage(UtilizedErrorMessage);

            RuleFor(x => x.Quantity).GreaterThan(0)
                .WithMessage(QuantityErrorMessage);

            When(x => x.VoucherDiscountType == VoucherDiscountType.Value, () =>
            {
                RuleFor(x => x.DiscountValue)
                    .NotNull()
                    .WithMessage(DiscountValueErrorMessage)
                    .GreaterThan(0)
                    .WithMessage(DiscountValueErrorMessage);
            });

            When(x => x.VoucherDiscountType == VoucherDiscountType.Percentage, () =>
            {
                RuleFor(x => x.DiscountPercentage)
                    .NotNull()
                    .WithMessage(DiscountPercentageErrorMessage)
                    .GreaterThan(0)
                    .WithMessage(DiscountPercentageErrorMessage);
            });
        }

        protected static bool ExpirationDateGreaterThanCurrent(DateTime expirationDate)
        {
            return expirationDate.Date >= DateTime.Now.Date;
        }
    }
}
