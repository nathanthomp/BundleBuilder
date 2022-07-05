﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace RedBuilt.Revit.BundleBuilder
{
    // Implement INotifyPropertyChanged, IDataErrorInfo
    public abstract class ViewModel : ObservableObject, IDataErrorInfo
    {
        public string this[string propertyName]
        {
            get { return OnValidate(propertyName); }
        }

        public string Error 
        {
            get { throw new NotSupportedException(); }
        }

        protected virtual string OnValidate(string propertyName)
        {
            var context = new ValidationContext(this)
            {
                MemberName = propertyName
            };

            var results = new Collection<ValidationResult>();
            var isValid = Validator.TryValidateObject(this, context, results, true);

            if (!isValid)
            {
                ValidationResult result = results.SingleOrDefault(p => p.MemberNames.Any(memberName => memberName == propertyName));
                return result == null ? null : result.ErrorMessage;
            }
            return null;
        }
    }
}
