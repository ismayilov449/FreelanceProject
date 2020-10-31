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
    public class EducationController : ControllerBase
    {
        private readonly IEduService _eduService;

        public EducationController(IEduService eduService)
        {
            _eduService = eduService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _eduService.GetAll();
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            var result = await _eduService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Education entity)
        {
            var result = await _eduService.Add(entity);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Education entity)
        {
            await _eduService.Update(entity);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            await _eduService.Delete(id);
            return Ok();
        }
    }
}
