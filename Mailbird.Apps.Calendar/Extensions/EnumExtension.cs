using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Extensions
{
    public static class EnumExtension
    {

        public static string GetEnumDescription(this Enum value)
        {
            var description = "";
            if (value == null) { return description; }

            description = value.ToString();
            Type type = value.GetType();
            var field = type.GetField(description);
            var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (customAttribute.Length > 0) { description = ((DescriptionAttribute)customAttribute[0]).Description; }

            return description;
        }

    }
}
