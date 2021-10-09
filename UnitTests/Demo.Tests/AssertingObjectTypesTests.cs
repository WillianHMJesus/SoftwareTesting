using Xunit;

namespace Demo.Test
{
    public class AssertingObjectTypesTests
    {
        [Fact]
        public void EmployeeFactory_Create_MustReturnTypeEmployee()
        {
            // Arrange & Act
            var employee = EmployeeFactory.Create("Mauro", 10000);

            // Assert
            Assert.IsType<Employee>(employee);
        }

        [Fact]
        public void EmployeeFactory_Create_MustReturnPersonDerivedType()
        {
            // Arrange & Act
            var employee = EmployeeFactory.Create("Mauro", 10000);

            // Assert
            Assert.IsAssignableFrom<Person>(employee);
        }
    }
}
