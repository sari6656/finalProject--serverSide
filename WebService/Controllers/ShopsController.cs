using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using BL;
using BL.Helpers;
using Entities;
namespace WebService.Controllers
{
    [RoutePrefix("WebService/Shops")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ShopsController : ApiController
    {
        [Route("Register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(ShopDTO shop)
        {
            return Ok(await Shops.Register(shop, Request.RequestUri));
        }

        [Route("Login")]
        [HttpGet]
        public async Task<IHttpActionResult> Login(string mail, string password)
        {
            return Ok(await Shops.Login(mail, password, Request.RequestUri));
        }
        //הפונקציה מחזירה מי החנות עכשיו
        [Route("getLoggedShop")]
        [HttpGet]
        public IHttpActionResult GetLoggedShop([UserLogged] ShopDTO shopDTO)
        {
            return Ok(shopDTO);
        }

        [Route("Logout")]
        [HttpGet]
        public IHttpActionResult Logout([UserLogged] ShopDTO shopDTO)
        {
            return Ok(Shops.Logout(shopDTO));
        }

        [Route("Update")]
        [HttpPost]
        public IHttpActionResult Update(ShopDTO shop)
        {
            return Ok(Shops.Update(shop));
        }

        [Route("GetSearches")]
        [HttpGet]
        public IHttpActionResult GetSearches(int codeShop)
        {
            return Ok(Shops.getSearchesForShop(codeShop));
        }

        [Route("GetAllCategories")]
        [HttpGet]
        public IHttpActionResult GetAllCategories()
        {
            return Ok(Shops.GetAllCategories());
        }

        [Route("NewCategory")]
        [HttpGet]
        public IHttpActionResult NewCategory(string newCategory, [UserLogged] ShopDTO shopDTO)
        {
            return Ok(Shops.SendEmailForNewCategory(newCategory, shopDTO));
        }
    }

}