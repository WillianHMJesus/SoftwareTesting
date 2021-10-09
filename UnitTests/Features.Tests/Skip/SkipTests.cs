using Xunit;

namespace Features.Tests.Skip
{
    public class SkipTests
    {
        [Fact(DisplayName = "New Customer 2.0", Skip = "Nova versão 2.0 quebrando")]
        [Trait("Category", "Skip Tests")]
        public void Skip_Reproved_NewVersionNotCampatible()
        {
            Assert.True(false);
        }
    }
}
