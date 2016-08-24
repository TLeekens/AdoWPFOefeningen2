using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Globalization;

namespace AdoWPFOefeningen2
{
    public class VerkoopPrijsRangeRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Decimal verkoopPrijs = 0;
            NumberStyles style = NumberStyles.Currency;

            if (value == null || (String)value == String.Empty)
            {
                return new ValidationResult(false, "Prijs moet ingevuld zijn");
            }

            if (!Decimal.TryParse(value.ToString(), style, cultureInfo, out verkoopPrijs))
            {
                return new ValidationResult(false, "Prijs moet een getal zijn");
            }

            if (verkoopPrijs < 0)
            {
                return new ValidationResult(false, "Prijs mag niet negatief zijn");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }
}
