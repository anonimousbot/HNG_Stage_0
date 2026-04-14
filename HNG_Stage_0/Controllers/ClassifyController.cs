using HNG_Stage_0.Interfaces;
using HNG_Stage_0.Models;
using Microsoft.AspNetCore.Mvc;

namespace HNG_Stage_0.Controllers
{
    [Route("api")]
    public class ClassifyController : ControllerBase
    {
        private readonly IGenderizeService _genderizeService;
        public ClassifyController(IGenderizeService genderizeService)
        {
            _genderizeService = genderizeService;
        }

        //public IActionResult Index()
        //{
        //    return Ok();
        //}
        [HttpGet("classify")]
        public async Task<IActionResult> Classify([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new BaseResponse<string>
                {
                    Status = "error",
                    Message ="Name parameter is required"
                });
            }
            try
            {
                var result = await _genderizeService.ClassifyUser(name);
                if(result == null)
                {
                    return UnprocessableEntity(result);
                }
                return Ok(result);
            }
            catch
            {
                return StatusCode(500, new BaseResponse<string>
                {
                    Status = "error",
                    Message = "Internal Server error"
                });
            }
        }
    }
}
