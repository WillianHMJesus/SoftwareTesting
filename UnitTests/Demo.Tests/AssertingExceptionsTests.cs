using System;
using Xunit;

namespace Demo.Test
{
    public class AssertingExceptionsTests
    {
        [Fact]
        public void Calculator_Divide_MustReturnErrorDivisionByZero()
        {
            // Arrange
            var calculator = new Calculator();

            // Act & Assert
            Assert.Throws<DivideByZeroException>(() => calculator.Divide(10, 0));
        }


        [Fact]
        public void Employee_Salary_MustReturnErrorInferiorSalaryAllowed()
        {
            // Arrange & Act & Assert
            var exception =
                Assert.Throws<Exception>(() => EmployeeFactory.Create("Mauro", 250));

            Assert.Equal("Salario inferior ao permitido", exception.Message);
        }
    }
}
