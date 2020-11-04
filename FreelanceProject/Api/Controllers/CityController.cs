using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _cityService.GetAll();
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            var result = await _cityService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] City entity)
        {
            var result = await _cityService.Add(entity);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] City entity)
        {
            await _cityService.Update(entity);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            await _cityService.Delete(id);
            return Ok();
        }
    }
}
