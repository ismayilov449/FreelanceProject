using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _jobService.GetAll();
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            var result = await _jobService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Job entity)
        {
            var result = await _jobService.Add(entity);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Job entity)
        {
            await _jobService.Update(entity);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            await _jobService.Delete(id);
            return Ok();
        }
    }
}
