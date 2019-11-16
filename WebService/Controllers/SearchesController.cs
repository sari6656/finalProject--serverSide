using BL;
using BL.Helpers;
using Entities;
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
        public IHttpActionResult RunSearch(SearchDTO searchDTO)
        {
            return Ok(Searches.Create(searchDTO));
        }
        [Route("GetHistory")]
        [HttpGet]
        public IHttpActionResult GetHistory()
        {
            string uuid = "456";
            return Ok(Searches.GetHistory(uuid));
        }
        [Route("GetHistoryFound")]
        [HttpPost]
        public IHttpActionResult GetHistoryFound(string uuid)
        {
            return Ok(Searches.GetHistoryFound(uuid));
        }
        [Route("GetHistoryNotFound")]
        [HttpPost]
        public IHttpActionResult GetHistoryNotFound(string uuid)
        {
            return Ok(Searches.GetHistoryNotFound(uuid));
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