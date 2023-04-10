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
        public ActionResult Post([FromBody] User user)
        {

            return new JsonResult(user);
        }

        [HttpPost]
        public ActionResult SaveFile(IFormFile file)
        {
            var fileName = file.FileName;

            return new JsonResult($"文件名为{fileName}");
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

    public class User
    {

        public string Name { get; set; }

    }
}
