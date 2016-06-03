// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0

namespace Assets.Sources.Util.Data
{
    public class Setting<T>
    {
        private readonly SettingImpl<T> impl = new SettingImpl<T>();
        private readonly string filePath;

        public Setting(string filePath)
        {
            this.filePath = filePath;
        }

        public void Set(T value)
        {
            impl.Set(value, filePath);
        }

        public T Get()
        {
            return impl.Get(filePath);
        }
    }
}
