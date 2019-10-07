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
            return Ok(Searches.GetSearchesByUserId());
        }
        [Route("GetShopsForCategory")]
        [HttpGet]
        public IHttpActionResult GetShopsForCategory(int codeCategory)
        {
            return Ok(Searches.GetShopsForCategory(codeCategory));
        }

    }
}