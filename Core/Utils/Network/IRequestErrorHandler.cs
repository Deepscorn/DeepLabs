// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0

namespace Assets.Sources.Util.Network
{
    public interface IRequestErrorHandler
    {
        void HandleRequestError(RequestError error);
    }
}
