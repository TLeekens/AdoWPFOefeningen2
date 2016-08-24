using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Globalization;

namespace AdoWPFOefeningen2
{
    public class KleurIngevuldRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || (String)value == String.Empty)
            {
                return new ValidationResult(false, "Kleur moet ingevuld zijn");
            }
            return ValidationResult.ValidResult;
        }
    }
}
