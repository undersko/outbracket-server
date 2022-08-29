using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApi.Binders.Decimal
{
    public class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            // Remove unnecessary commas and spaces
            value = value.Replace(",", string.Empty).Trim();

            decimal myValue = 0;
            var style = NumberStyles.Number;
            var format = new System.Globalization.NumberFormatInfo();
            format.NumberDecimalSeparator = ".";
            if (!decimal.TryParse(value, style, format, out myValue))
            {
                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName,
                    $"Could not parse {value}.");
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(myValue);
            return Task.CompletedTask;
        }
    }
}