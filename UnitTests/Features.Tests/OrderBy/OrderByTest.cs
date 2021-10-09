using Xunit;

namespace Features.Tests.OrderBy
{
    [TestCaseOrderer("Features.Tests.OrderBy.PriorityOrderer", "Features.Tests")]
    public class OrderByTest
    {
        public static bool Test1Started;
        public static bool Test2Started;
        public static bool Test3Started;
        public static bool Test4Started;

        [Fact(DisplayName = "Test 04"), TestPriority(4)]
        [Trait("Category", "Order By Tests")]
        public void Test04()
        {
            Test4Started = true;

            Assert.True(Test3Started);
            Assert.True(Test1Started);
            Assert.False(Test2Started);
        }

        [Fact(DisplayName = "Test 01"), TestPriority(2)]
        [Trait("Category", "Order By Tests")]
        public void Test01()
        {
            Test1Started = true;

            Assert.True(Test3Started);
            Assert.False(Test4Started);
            Assert.False(Test2Started);
        }

        [Fact(DisplayName = "Test 03"), TestPriority(0)]
        [Trait("Category", "Order By Tests")]
        public void Test03()
        {
            Test3Started = true;

            Assert.False(Test1Started);
            Assert.False(Test4Started);
            Assert.False(Test2Started);
        }

        [Fact(DisplayName = "Test 02"), TestPriority(6)]
        [Trait("Category", "Order By Tests")]
        public void Test02()
        {
            Test2Started = true;

            Assert.True(Test3Started);
            Assert.True(Test1Started);
            Assert.True(Test4Started);
        }
    }
}
