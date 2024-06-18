using Microsoft.AspNetCore.Mvc;

namespace Notes.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : Controller
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "список записок";
        }
    }
}
