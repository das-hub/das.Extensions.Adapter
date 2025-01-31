﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using das.Extensions.Adapter.Annotation;
using das.Extensions.Adapter.Extensions;

namespace das.Extensions.Adapter
{
    public class Record
    {
        protected StringBuilder _errors;

        public int Index { get; set; }

        private object[] _source;
        public object[] Source
        {
            get => _source;
            set
            {
                _source = value;
                ProcessProperties();
            }
        }

        public bool IsValid = true;

        protected void Error(string message)
        {
            if (IsValid) IsValid = false;

            if (_errors == null)
                _errors = new StringBuilder().Append($" Строка [{Index}]: {message}");
            else
                _errors.Append($"; {message}");
        }

        public string Errors => _errors?.ToString();

        protected void ProcessProperties()
        {
            BindProperties();
            if (!ValidateObject()) return;
            FormattingProperties();
        }

        protected void BindProperties()
        {
            int columnCount = Source.Length;

            IEnumerable<PropertyInfo> properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p
                .GetCustomAttributes()
                .Any(a => a is BindAttribute));

            foreach (PropertyInfo prop in properties)
            {
                if (prop.GetCustomAttribute(typeof(BindAttribute), false) is BindAttribute column)
                {
                    object value = null;
                    try
                    {
                        value = column.Column < columnCount
                            ? Source[column.Column]
                            : column.DefaultValue;

                        prop.SetValue(this, ValueOrDefault(prop.PropertyType, value, column.DefaultValue));
                    }
                    catch (Exception e)
                    {
                        Error(
                            $"Поле [{prop.Name}] ошибка преобразования [{value}] к типу [{prop.PropertyType.FullName}] ({e.Message})");
                    }
                }
            }
        }

        protected void FormattingProperties()
        {
            IEnumerable<PropertyInfo> properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p
                .GetCustomAttributes()
                .Any(a => a is FormattingAttribute));

            foreach (PropertyInfo prop in properties)
            {
                if (prop.GetCustomAttributes(typeof(FormattingAttribute), false) is FormattingAttribute[] formats)
                    foreach (FormattingAttribute format in formats)
                    {
                        object oldValue = prop.GetValue(this);
                        object newValue = format.Format(oldValue);
                        prop.SetValue(this, newValue);
                    }
            }
        }

        protected object ValueOrDefault(Type propType, object value, object defaultValue)
        {            
            if (value is null || value is DBNull)
                value = defaultValue;
                       
            return Convert.ChangeType(value, propType);
        }

        protected bool ValidateObject()
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this);

            if (!Validator.TryValidateObject(this, context, results, true))
            {
                results.Do(r => Error(r.ErrorMessage));
                return false;
            }

            return true;
        }
    }
}
