using System;

namespace das.Extensions.Adapter.Annotation
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class FormattingAttribute : Attribute
    {
        public abstract object Format(object value);
    }
}
