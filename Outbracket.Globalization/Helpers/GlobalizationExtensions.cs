using System;
using System.Reflection;

namespace Outbracket.Globalization.Helpers
{
    public static class GlobalizationExtensions
    {
        public static Tuple<string, string> GetGlobalizationField(this Type sourceType, string propertyName)
        {
            var field = sourceType.GetField(propertyName, BindingFlags.Static | BindingFlags.Public);
            return !(field?.GetValue(null) is Tuple<string, string> str) ? 
                null : 
                str;
        }
    }
}