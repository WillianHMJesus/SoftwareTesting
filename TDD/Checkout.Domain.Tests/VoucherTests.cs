using System;
using System.Linq;
using Xunit;

namespace Checkout.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validate Voucher Value Type Valid")]
        [Trait("Category", "Checkout - Voucher")]
        public void Voucher_ValidateVoucherValueType_MustIsValid()
        {
            //Arrange
            var voucher = new Voucher("PROMO-15-REAIS", null, 15, VoucherDiscountType.Value, 1, DateTime.Now.AddDays(7), false, true);

            //Act
            var result = voucher.ValidateIfApplicable();

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validate Voucher Value Type Invalid")]
        [Trait("Category", "Checkout - Voucher")]
        public void Voucher_ValidateVoucherValueType_MustIsInvalid()
        {
            //Arrange
            var voucher = new Voucher("", null, null, VoucherDiscountType.Value, 0, DateTime.Now.AddDays(-1), true, false);

            //Act
            var result = voucher.ValidateIfApplicable();

            //Assert
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
            Assert.Contains(VoucherValidator.CodeErrorMessage, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(VoucherValidator.ActiveErrorMessage, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(VoucherValidator.ExpirationDateErrorMessage, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(VoucherValidator.QuantityErrorMessage, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(VoucherValidator.UtilizedErrorMessage, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(VoucherValidator.DiscountValueErrorMessage, result.Errors.Select(x => x.ErrorMessage));
        }

        [Fact(DisplayName = "Validate Voucher Percentage Type Valid")]
        [Trait("Category", "Checkout - Voucher")]
        public void Voucher_ValidateVoucherPercentageType_MustIsValid()
        {
            //Arrange
            var voucher = new Voucher("PROMO-15-OFF", 15, null, VoucherDiscountType.Percentage, 1, DateTime.Now.AddDays(7), false, true);

            //Act
            var result = voucher.ValidateIfApplicable();

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validate Voucher Percentage Type Invalid")]
        [Trait("Category", "Checkout - Voucher")]
        public void Voucher_ValidateVoucherPercentageType_MustIsInvalid()
        {
            //Arrange
            var voucher = new Voucher("", null, null, VoucherDiscountType.Percentage, 0, DateTime.Now.AddDays(-1), true, false);

            //Act
            var result = voucher.ValidateIfApplicable();

            //Assert
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
            Assert.Contains(VoucherValidator.CodeErrorMessage, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(VoucherValidator.ActiveErrorMessage, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(VoucherValidator.ExpirationDateErrorMessage, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(VoucherValidator.QuantityErrorMessage, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(VoucherValidator.UtilizedErrorMessage, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(VoucherValidator.DiscountPercentageErrorMessage, result.Errors.Select(x => x.ErrorMessage));
        }
    }
}
