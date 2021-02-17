using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class TestController : BaseApiController
    {
        [HttpGet("{id}")]
        [Route("test-api-get")]
        public IActionResult TestApiGet(int id)
        {
            string datetimeoffset = DateTimeOffset.Now.ToString();
            string datetimeoffsetutc = DateTimeOffset.UtcNow.ToString();
            string datetime = DateTime.Now.ToString();
            string datetimeutc = DateTime.UtcNow.ToString();
            var result = datetimeoffset + "~" + datetimeoffsetutc + "~" + datetime + "~" + datetimeutc;
            return Ok(result);
        }

        [HttpPost("{id}")]
        [Route("test-api-post")]
        public IActionResult TestApiPost(string id, string returnValue)
        {
            string result = returnValue + "=" + id;
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Route("test-api-put")]
        public IActionResult TestApiPut(string id, string returnValue)
        {
            string result = returnValue + "=" + id;
            return Ok(result);
        }
    }
}
