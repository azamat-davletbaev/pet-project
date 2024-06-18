using EasyNetQ;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using Notes.Model;
using System.Runtime.CompilerServices;

namespace Notes.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {        
        public UserController()
        {
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var guid = Guid.NewGuid().ToString();

            var request = new RequestResponse
            {
                Guid = guid,
                Head = nameof(IDataProvider.GetAllUsers),
                Body = string.Empty
            };

            var json = JsonConvert.SerializeObject(request);
                        
            using var rpcClient = new Client();                        
            var response = await rpcClient.CallAsync(json);
            
            return json;
        }
    }
}
