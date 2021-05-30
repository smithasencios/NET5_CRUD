using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Application.Helpers
{
	public static class TestFunctions
    {
        public static Mock<DbSet<T>> GetDbSet<T>(IQueryable<T> testData)
            where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IAsyncEnumerable<T>>().Setup(x => x.GetAsyncEnumerator(default)).Returns(new TestAsyncEnumerator<T>(testData.GetEnumerator()));
            mockSet.As<IQueryable<T>>().Setup(x => x.Provider).Returns(new TestAsyncQueryProvider<T>(testData.Provider));
            mockSet.As<IQueryable<T>>().Setup(x => x.Expression).Returns(testData.Expression);
            mockSet.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(testData.ElementType);
            mockSet.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(testData.GetEnumerator());
            return mockSet;
        }
    }
}
