using BL.Helpers;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Threading.Tasks;

namespace WebService.Controllers
{
    [RoutePrefix("WebService/User")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {
        [Route("Register")]
        [HttpPost]
        public WebResult<UserDTO> Register(UserDTO user)
        {
            return BL.Users.Register(user);
        }
    }
}