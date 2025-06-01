using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTNETAreaMap
{
    public class TypeLink
    {
        public Type Left;

        public Type Right;

        public LinkType LinkType;

        public string GetUML()
        {
            return $"{Left.Name}{LinkType.ToUMLString()}{Right.Name}";
        }

        public override bool Equals(object obj)
        {
            if(obj == null) return false;
            if(obj is TypeLink link)
            {
                return link.Right == Right && link.Left == Left && link.LinkType == LinkType;
            }
            return false;
        }

        public static bool operator ==(TypeLink left, TypeLink right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TypeLink left, TypeLink right)
        {
            return !left.Equals(right);
        }
    }

    public enum LinkType
    {
        Inheritance,
    	Composition,
        Aggregation,
        Association
    }
}
