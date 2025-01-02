using Microsoft.AspNetCore.Mvc;

namespace webapi_iot_growdata5.Models
{
    //[ModelBinder(BinderType = typeof(CustomLoginModelBinder))]
    public class IOUnitAuthenticationLoginModel : Controller
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
