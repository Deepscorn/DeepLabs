// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using Assets.Sources.NativePlatform;

namespace Assets.Sources.Util.Data
{
    public class UserSetting<T>
    {
        private const string NoUserId = "NO_USER_ID";
        private const string Separator = "_";

        private readonly SettingImpl<T> impl = new SettingImpl<T>();
        private readonly string filePathPostfix;

        public UserSetting(string filePath)
        {
            filePathPostfix = filePath;
        }

        public void Set(T value)
        {
            impl.Set(value, GetUserPath());
        }

        public T Get()
        {
            return impl.Get(GetUserPath());
        }

        // Note, that userId may change during runtime. So, better construct file path when it is used
        protected string GetUserPath()
        {
            return GetUserId() + Separator + filePathPostfix;
        }

        private static string GetUserId()
        {
            var result = NativeApi.UserId;
            return result ?? NoUserId;
        }
    }
}
