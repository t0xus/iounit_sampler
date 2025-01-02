using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using webapi_iot_growdata5.Models;

public class CustomLoginModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var username = bindingContext.ValueProvider.GetValue("Username").FirstValue;
        var password = bindingContext.ValueProvider.GetValue("Password").FirstValue;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            bindingContext.Result = ModelBindingResult.Failed();
        }
        else
        {
            var model = new IOUnitAuthenticationLoginModel
            {
                Username = username,
                Password = password
            };

            bindingContext.Result = ModelBindingResult.Success(model);
        }

        return Task.CompletedTask;
    }
}