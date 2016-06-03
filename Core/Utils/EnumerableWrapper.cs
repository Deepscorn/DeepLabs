// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System.Collections.Generic;

namespace Util
{
    public class EnumerableWrapper<T>: IEnumerable<T>
    {
        private readonly IEnumerator<T> enumerator;

        public EnumerableWrapper(IEnumerator<T> enumerator)
        {
            this.enumerator = enumerator;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return enumerator;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return enumerator;
        }

        public static EnumerableWrapper<T> NewInstance(IEnumerator<T> enumerator)
        {
            return new EnumerableWrapper<T>(enumerator);
        }
    }

    public class EnumerableWrapperBuilder
    {
        public static EnumerableWrapper<T> Build<T>(IEnumerator<T> enumerator)
        {
            return new EnumerableWrapper<T>(enumerator);
        }
    }
}

