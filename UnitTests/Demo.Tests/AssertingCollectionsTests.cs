using Xunit;

namespace Demo.Test
{
    public class AssertingCollectionsTests
    {
        [Fact]
        public void Employee_Skills_MustNotHaveEmptySkills()
        {
            // Arrange & Act
            var employee = EmployeeFactory.Create("Mauro", 10000);

            // Assert
            Assert.All(employee.Skills, x => Assert.False(string.IsNullOrWhiteSpace(x)));
        }

        [Fact]
        public void Employee_Skills_JuniorMustHaveBasicSkill()
        {
            // Arrange & Act
            var employee = EmployeeFactory.Create("Mauro", 1000);

            // Assert
            Assert.Contains("OOP", employee.Skills);
        }


        [Fact]
        public void Employee_Skills_JuniorMustNotHaveAdvancedSkill()
        {
            // Arrange & Act
            var employee = EmployeeFactory.Create("Mauro", 1000);

            // Assert
            Assert.DoesNotContain("Microservices", employee.Skills);
        }


        [Fact]
        public void Employee_Skills_SeniorMustHaveAllSkills()
        {
            // Arrange & Act
            var employee = EmployeeFactory.Create("Mauro", 15000);

            var allSkills = new[]
            {
                "Lógica de Programação",
                "OOP",
                "Testes",
                "Microservices"
            };

            // Assert
            Assert.Equal(allSkills, employee.Skills);
        }
    }
}
