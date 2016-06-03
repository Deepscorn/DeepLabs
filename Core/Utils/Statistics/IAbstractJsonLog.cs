// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0

namespace Assets.Sources.Util.Statistics
{
    // NOTE: constructor is called from main thread. So if you need Application.<someInfo>
    // or other Unity things be aware that they can be accessible from main thread only
    public interface IAbstractJsonLog
    {
        // NOTE: called from background thread
        string Method { get; }
        // NOTE: called from background thread
        object GetContent();
    }
}
