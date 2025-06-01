using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DOTNETAreaMap
{
    public static class Extension
    {
        public static string ToUMLString(this LinkType linkType)
        {
            switch(linkType)
            {
                case LinkType.Inheritance:
                    return "<|--";
                case LinkType.Aggregation:
                    return "o--";
                case LinkType.Composition:
                    return "*--";
                case LinkType.Association:
                    return "-->";
                default:
                    throw new InvalidOperationException();
            }
        }

        public static string SanitizeForUML(this string str)
        {
            Regex regex = new Regex("[`<>+=+=]");
            return regex.Replace(str, "_");
        }

        public static string ToUMLAccessor(this Type type)
        {
            if (type.IsPublic)
            {
                return "+";
            }
            if (!type.IsPublic)
            {
                return "-";
            }
            if (!type.IsVisible)
            {
                return "~";
            }
            if (type.IsNestedFamily)
            {
                return "#";
            }
            return "";
        }

        public static string ToUMLAccessor(this FieldInfo type)
        {
            if (type.IsPublic)
            {
                return "+";
            }
            if (!type.IsPublic)
            {
                return "-";
            }
            if (type.IsAssembly)
            {
                return "~";
            }
            if (type.IsFamily)
            {
                return "#";
            }
            return "";
        }

        public static string ToUMLAccessor(this MethodInfo type)
        {
            if (type.IsPublic)
            {
                return "+";
            }
            if (!type.IsPublic)
            {
                return "-";
            }
            if (type.IsAssembly)
            {
                return "~";
            }
            if (type.IsFamily)
            {
                return "#";
            }
            return "";
        }
        
    }
}
