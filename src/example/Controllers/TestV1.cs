using Dynamic;
using Microsoft.AspNetCore.Mvc;

namespace example.Controllers
{
    public class TestV1 : IDynamicService
    {

        public ActionResult Find()
        {

            return new JsonResult("Get");
        }

        public ActionResult GetAll()
        {

            return new JsonResult("GetAll");
        }

        [HttpGet("{id:int}")]
        public string Get(int id)
        {
            return id.ToString();
        }

        //public ActionResult AddTest()
        //{
        //    return new JsonResult("AddTest");
        //}

        [HttpPost("AddTest2")]
        public ActionResult Post()
        {
            return new JsonResult("AddTest2");
        }


        [HttpDelete("DeleteTest")]
        public ActionResult Delete()
        {
            return new JsonResult("Delete");
        }
    }
}
