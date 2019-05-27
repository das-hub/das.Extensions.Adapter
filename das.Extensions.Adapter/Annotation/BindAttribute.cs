using System;

namespace das.Extensions.Adapter.Annotation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BindAttribute : Attribute
    {
        public int Column { get; protected set; }
        public string DefaultValue { get; protected set; }

        public BindAttribute(int column, string defaultValue = "")
        {
            Column = column;
            DefaultValue = defaultValue;
        }
    }
}