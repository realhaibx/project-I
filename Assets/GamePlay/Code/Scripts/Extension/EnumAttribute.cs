using System;
using System.Linq;
using System.Runtime.Serialization;

namespace GamePlay.Code.Scripts.Extension
{
    public class EnumAttribute
    {
        public static string GetEnumMemberAttrValue(Type enumType, object enumVal)
        {
            var memInfo = enumType.GetMember(enumVal.ToString());
            var attr = memInfo[0].GetCustomAttributes(false).OfType<EnumMemberAttribute>().FirstOrDefault();
            if(attr != null)
            {
                return attr.Value;
            }

            return null;
        }
    }
}