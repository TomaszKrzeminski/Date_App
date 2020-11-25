using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DateApp.Models.Attributes
{

    public class FutureDateAttribute : Attribute, IModelValidator
    {

        public bool IsRequired => true;
        public string ErrorMessage1 { get; set; } = "Data nie może być wcześniejsza niż dziś ";
        public string ErrorMessage2 { get; set; } = "Musisz podać datę";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            DateTime? value = context.Model as DateTime?;               
                      

                if (value == null)
                {

                    return new List<ModelValidationResult> { new ModelValidationResult("", ErrorMessage2) };
                }
                else 
                {

                    DateTime date = (DateTime)value;
                    DateTime now = DateTime.Now;
                    if (date.Date < now.Date)
                    {
                        return new List<ModelValidationResult> { new ModelValidationResult("", ErrorMessage1) };
                    }
                    else
                    {
                        return Enumerable.Empty<ModelValidationResult>();
                    }


                }
           
        }
    }

    public class DateLessThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateLessThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = "Data nie może być późniejsza od poniższej";
            var currentValue = (DateTime)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                throw new ArgumentException("Błąd");

            var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);

            if (currentValue > comparisonValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }


    public class DateLaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateLaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = "Data nie może być wcześniejsza od powyższej";
            var currentValue = (DateTime)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                throw new ArgumentException("Błąd");

            var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);

            if (currentValue < comparisonValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }





}
