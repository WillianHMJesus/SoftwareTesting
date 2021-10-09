using Xunit;

namespace Demo.Test
{
    public class AssertNullBoolTests
    {
        [Fact]
        public void Employee_Name_MustNotBeNullOrEmpty()
        {
            // Arrange & Act
            var employee = new Employee("", 1000);

            // Assert
            Assert.False(string.IsNullOrEmpty(employee.Name));
        }

        [Fact]
        public void Employee_Name_MustNotHaveNickname()
        {
            // Arrange & Act
            var employee = new Employee("Mauro", 1000);

            // Assert
            Assert.Null(employee.Nickname);

            // Assert Bool
            Assert.True(string.IsNullOrEmpty(employee.Nickname));
            Assert.False(employee.Nickname?.Length > 0);
        }
    }
}
