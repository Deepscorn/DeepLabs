// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Assets.Sources.Util.Pattern;

namespace Assets.Sources.Util.Enums
{
    internal sealed class EnumDescriptionConverter<T>: Singleton<EnumDescriptionConverter<T>>
    where T : struct
    {
        private readonly Dictionary<T, string> valueToStringDict =
          new Dictionary<T, string>();
        private readonly Dictionary<string, T> stringToValueDict =
          new Dictionary<string, T>();

        static EnumDescriptionConverter()
        {
            Debug.Assert(typeof(T).IsEnum,
              "The custom enum class must be used with an enum type.");
        }

        public EnumDescriptionConverter()
        {
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                var description = GetDescription(item);
                valueToStringDict[item] = description;
                stringToValueDict[description] = item;
            }
        }

        public string ValueToDescription(T value)
        {
            string result;
            valueToStringDict.TryGetValue(value, out result);
            return result;
        }

        public T DescriptionToValue(string description)
        {
            T result;
            stringToValueDict.TryGetValue(description, out result);
            return result;
        }

        private static string GetDescription(T optionValue)
        {
            var optionDescription = optionValue.ToString();
            var optionInfo = typeof(T).GetField(optionDescription);
            if (Attribute.IsDefined(optionInfo, typeof(DescriptionAttribute)))
            {
                var attribute =
                  (DescriptionAttribute)Attribute.
                     GetCustomAttribute(optionInfo, typeof(DescriptionAttribute));
                return attribute.Description;
            }
            return optionDescription;
        }
    }
}