using Dynamic;
using Microsoft.AspNetCore.Mvc;

namespace example.Controllers
{

    public class UserService : IDynamicService
    {

        [HttpGet]
        public ActionResult GetUser()
        {

            return new JsonResult("Get");
        }

        [HttpPost("AddUser")]
        public ActionResult Post()
        {

            return new JsonResult("Post");
        }

        [HttpPut("UpdateUser")]
        public ActionResult Put()
        {

            return new JsonResult("Post");
        }

        [HttpDelete("DeleteUser")]
        public ActionResult Delete()
        {

            return new JsonResult("Delete");
        }
    }
}
