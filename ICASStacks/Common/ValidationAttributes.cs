using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Common
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "'{0}' and '{1}' do not match.";

        private readonly object _typeId = new object();

        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(DefaultErrorMessage)
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        public string ConfirmProperty
        {
            get;
            private set;
        }

        public string OriginalProperty
        {
            get;
            private set;
        }

        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                OriginalProperty, ConfirmProperty);
        }

        public override bool IsValid(object value)
        {
            if (value == null) { return false; }
            var properties = TypeDescriptor.GetProperties(value);
            var originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            var confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
            return Equals(originalValue, confirmValue);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "'{0}' must be at least {1} character length.";

        private readonly int _minCharacters = 8;

        public ValidatePasswordLengthAttribute()
            : base(DefaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            var valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OldEnoughValidationAttribute : ValidationAttribute
    {
        public int LimitAge { get; set; }
        public OldEnoughValidationAttribute(int limitAge)
        {
            LimitAge = limitAge;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int val = (int)value;

            if (val >= LimitAge)
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessageString);
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CheckAttribute : ValidationAttribute
    {
        object[] ValidValues;

        public CheckAttribute(params object[] validValues)
        {
            ValidValues = validValues;
        }

        public override bool IsValid(object value)
        {
            return ValidValues.FirstOrDefault(v => v.Equals(value)) != null;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CheckNumberAttribute : ValidationAttribute
    {
        readonly int _compareValue;

        public CheckNumberAttribute(int compareValue)
        {
            _compareValue = compareValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int)
            {
                return (int)value > _compareValue ? ValidationResult.Success : new ValidationResult(ErrorMessageString);
            }
            if (value is long)
            {
                return (long)value > _compareValue ? ValidationResult.Success : new ValidationResult(ErrorMessageString);
            }
            return new ValidationResult(ErrorMessageString);
        }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CheckNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var name = value as string;
            if (string.IsNullOrEmpty(name))
            {
                return new ValidationResult(ErrorMessageString);
            }
            return RegExValidator.IsNameValid(name) ? ValidationResult.Success : new ValidationResult(ErrorMessageString);

        }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CheckMobileNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var name = value as string;
            if (string.IsNullOrEmpty(name))
            {
                return ValidationResult.Success;
            }
            return GSMHelper.ValidateMobileNumber(name) ? ValidationResult.Success : new ValidationResult(ErrorMessageString);

        }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CheckAmountAttribute : ValidationAttribute
    {
        readonly float _compareValue;

        public CheckAmountAttribute(float compareValue)
        {
            _compareValue = compareValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is float)
            {
                return (float)value >= _compareValue ? ValidationResult.Success : new ValidationResult(ErrorMessageString);
            }
            if (value is double)
            {
                return (double)value >= _compareValue ? ValidationResult.Success : new ValidationResult(ErrorMessageString);
            }
            return new ValidationResult(ErrorMessageString);
        }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CheckTransferCodeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var name = value as string;
            if (string.IsNullOrEmpty(name))
            {
                return ValidationResult.Success;
            }
            return RegExValidator.IsTransferCodeValid(name) ? ValidationResult.Success : new ValidationResult(ErrorMessageString);

        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CheckCustomerCodeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var name = value as string;
            if (string.IsNullOrEmpty(name))
            {
                return ValidationResult.Success;
            }
            return RegExValidator.IsCustomerCodeValid(name) ? ValidationResult.Success : new ValidationResult(ErrorMessageString);

        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CheckAccountNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var name = value as string;
            if (string.IsNullOrEmpty(name))
            {
                return ValidationResult.Success;
            }
            return RegExValidator.IsAccountNumberValid(name) ? ValidationResult.Success : new ValidationResult(ErrorMessageString);

        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CheckEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var name = value as string;
            if (string.IsNullOrEmpty(name))
            {
                return ValidationResult.Success;
            }
            return RegExValidator.IsEmailValid(name) ? ValidationResult.Success : new ValidationResult(ErrorMessageString);

        }
    }
}
