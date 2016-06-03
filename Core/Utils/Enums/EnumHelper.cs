// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0

namespace Assets.Sources.Util.Enums
{
    public static class EnumHelper
    {
        public static string StringValueOf<T>(T enumValue) where T : struct
        {
            return EnumDescriptionConverter<T>.Instance.ValueToDescription(enumValue);
        }

        public static T EnumValueOf<T>(string stringValue) where T : struct
        {
            return EnumDescriptionConverter<T>.Instance.DescriptionToValue(stringValue);
        }
    }
}