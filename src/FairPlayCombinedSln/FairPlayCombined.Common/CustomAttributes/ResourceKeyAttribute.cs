using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Common.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ResourceKeyAttribute(string? defaultValue) : Attribute
    {
        public string? DefaultValue => defaultValue;
    }
}
