using BL.Casting;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace BL.Helpers
{
    public class UserLoggedAttribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            return new UserLoggedParameterBinding(parameter);
        }
    }

    public class UserLoggedParameterBinding : HttpParameterBinding
    {
        ProjectEntities db = new ProjectEntities();
        public UserLoggedParameterBinding(HttpParameterDescriptor parameter)
            : base(parameter)
        {
        }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider,
        HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var user = HttpContext.Current.User as ClaimsPrincipal;
            var identity = user.Identity as ClaimsIdentity;
            var claim = identity.Claims.Where(c => c.Type == ClaimTypes.Name).Select(s => s.Value).SingleOrDefault();
            var shop = db.Shops.Where(w => w.mailShop == claim).FirstOrDefault();
            actionContext.ActionArguments[Descriptor.ParameterName] =shop==null?null: ShopCast.GetShopDTO(shop);
            return Task.FromResult<object>(null);
        }
    }

}