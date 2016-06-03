// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System;

namespace Assets.Sources.Util.Extensions
{
    public static class ActionExt
    {
        public static void NotNullCall(this Action action)
        {
            if (action != null)
            {
                action();
            }
        }

        public static void NotNullCall<TParamType>(this Action<TParamType> actoin, TParamType param)
        {
            if (actoin != null)
            {
                actoin(param);
            }
        }

        public static TReturnType NotNullCall<TReturnType>(this Func<TReturnType> func)
        {
            if (func != null)
            {
                return func();
            }

            return default(TReturnType);
        }

        public static TReturnType NotNullCall<TReturnType, TParamType>(this Func<TParamType, TReturnType> func, TParamType param)
        {
            if (func != null)
            {
                return func(param);
            }

            return default(TReturnType);
        }
    }
}
