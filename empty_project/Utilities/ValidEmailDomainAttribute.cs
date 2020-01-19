using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace empty_project.Utilities
{
    public class ValidEmailDomainAttribute: ValidationAttribute
    {
        private readonly string[] _allowedDomains;

        public ValidEmailDomainAttribute(params string[] allowedDomains)
        {
            _allowedDomains = allowedDomains;
        }

        public override bool IsValid(object value)
        {
            var strings = value.ToString().Split('@').Last().Split('.');
            return _allowedDomains.Contains(strings.First().ToLower());
        }
    }
}
