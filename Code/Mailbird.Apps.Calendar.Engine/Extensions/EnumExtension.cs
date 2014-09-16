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
            var field = value.GetType().GetField(description);
            if (field != null)
            {
                var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (customAttribute.Length > 0) { description = ((DescriptionAttribute)customAttribute[0]).Description; }
            }

            return description;
        }

    }
}
