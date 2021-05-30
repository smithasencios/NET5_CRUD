using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests.Application.Helpers
{
	internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            this.inner = inner;
        }

        public T Current
        {
            get
            {
                return this.inner.Current;
            }
        }

        public ValueTask DisposeAsync()
        {
            this.inner.Dispose();
            return default(ValueTask);
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(this.inner.MoveNext());
        }
    }
}
