using CubeTrackerAPI.DTOs;
using CubeTrackerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CubeTrackerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReturnsController : ControllerBase
    {
        private readonly CubeService _service;

        public ReturnsController(CubeService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddReturnDto dto)
        {
            var result = await _service.AddDailyReturn(dto.Date, dto.Amount);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAll();
            return Ok(data);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> Summary()
        {
            return Ok(await _service.GetSummary());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AddReturnDto dto)
        {
            var result = await _service.UpdateReturn(id, dto.Date, dto.Amount);

            if (result == null)
                return NotFound("Return not found");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteReturn(id);

            if (!result)
                return NotFound("Return not found");

            return Ok(new { message = "Deleted successfully" });
        }
    }
}