using Xunit;

namespace Demo.Test
{
    public class AssertStringsTests
    {
        [Fact]
        public void StringTools_MergeName_MustReturnFullName()
        {
            //Arrange
            var stringsTools = new StringsTools();

            //Act
            var fullName = stringsTools.MergeText("Mauro", "Rodrigues");

            //Assert
            Assert.Equal("Mauro Rodrigues", fullName);
        }

        [Fact]
        public void StringsTools_MergeName_MustIgnoreCase()
        {
            //Arrange
            var stringsTools = new StringsTools();

            //Act
            var fullName = stringsTools.MergeText("Mauro", "Rodrigues");

            //Assert
            Assert.Equal("MAURO RODRIGUES", fullName, true);
        }

        [Fact]
        public void StringTools_MergeName_MustContainExcerpt()
        {
            //Arrange
            var stringsTools = new StringsTools();

            //Act
            var fullName = stringsTools.MergeText("Mauro", "Rodrigues");

            //Assert
            Assert.Contains("uro", fullName);
        }

        [Fact]
        public void StringTools_MergeName_MustStartWith()
        {
            //Arrange
            var stringsTools = new StringsTools();

            //Act
            var fullName = stringsTools.MergeText("Mauro", "Rodrigues");

            //Assert
            Assert.StartsWith("Mau", fullName);
        }

        [Fact]
        public void StringTools_MergeName_MustEndWith()
        {
            //Arrange
            var stringsTools = new StringsTools();

            //Act
            var fullName = stringsTools.MergeText("Mauro", "Rodrigues");

            //Assert
            Assert.EndsWith("gues", fullName);
        }

        [Fact]
        public void StringTools_MergeName_ValidateRegularExpression()
        {
            //Arrange
            var stringsTools = new StringsTools();

            //Act
            var fullName = stringsTools.MergeText("Mauro", "Rodrigues");

            //Assert
            Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", fullName);
        }
    }
}
