using BL;
using BL.Helpers;
using Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
//using System.Web.Mvc;

namespace WebService.Controllers
{
    [RoutePrefix("WebService/Searches")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SearchesController : ApiController
    {
        [Route("GetCategories")]
        [HttpGet]
        public IHttpActionResult GetCategories()
        {
            return Ok(Searches.GetCategories());
        }
        [Route("RunSearch")]
        [HttpPost]
        public IHttpActionResult RunSearch([FromBody] JObject data)
        {
            SearchDTO searchDTO = data["search"].ToObject<SearchDTO>();
            string passwordUser = data["passwordUser"].ToObject<string>();
            return Ok(Searches.Create(searchDTO,passwordUser));
        }
        [Route("GetHistory")]
        [HttpPost]
        public IHttpActionResult GetHistory([FromBody]string passwordUser)
        {
            return Ok(Searches.GetHistory(passwordUser));
        }
        [Route("GetHistoryFound")]
        [HttpPost]
        public IHttpActionResult GetHistoryFound([FromBody]string passwordUser)
        {
            return Ok(Searches.GetHistoryFound(passwordUser));
        }
        [Route("GetHistoryNotFound")]
        [HttpPost]
        public IHttpActionResult GetHistoryNotFound([FromBody]string passwordUser)
        {
            return Ok(Searches.GetHistoryNotFound(passwordUser));
        }
        [Route("GetShopsForCategory")]
        [HttpGet]
        public IHttpActionResult GetShopsForCategory(int codeCategory)
        {
            return Ok(Searches.GetShopsForCategory(codeCategory));
        }
        [Route("CheckDistance")]
        [HttpPost]
        public IHttpActionResult CheckDistance(UserIdWithLocation userIdWithLocation)
        {
            return Ok(PlacesAndLocations.CheckDistance(userIdWithLocation));
        }

    }
}