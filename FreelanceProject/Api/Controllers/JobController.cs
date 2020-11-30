using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Infrastructure.Helpers;
using Api.Infrastructure.SignalR;
using Core.Models;
using Core.Models.SearchModels;
using Core.Models.ServiceModels;
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
        private readonly IFilterService _filterService;
        private readonly IMailService _mailService;
        private readonly INotifyService _notifyService;

        public JobController(IJobService jobService, IFilterService filterService, IMailService mailService, ISmsService smsService, INotifyService notifyService)        {
            _jobService = jobService;
            _filterService = filterService;
            _mailService = mailService;
            _notifyService = notifyService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] int offset, int limit)
        {
            var result = await _jobService.GetAll(offset, limit);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            var result = await _jobService.GetById(id);
            return Ok(result);
        }

        [HttpGet("GetByCategory")]
        public async Task<IActionResult> GetByCategory([FromQuery] string categoryId, int offset, int limit)
        {
            var result = await _jobService.GetByCategory(categoryId, offset, limit);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Job entity)
        {
            var filterRequestModel = new FilterRequestModel
            {
                CityId = entity.CityId.ToString(),
                CategoryId = entity.CategoryId.ToString(),
                EducationId = entity.EducationId.ToString(),
                Salary = entity.SalaryMin
            };

            var result = await _jobService.Add(entity);
            if (result != Guid.Empty)
            {
				var currJob = await _jobService.GetById(result.ToString());
                var users = await _filterService.GetUsers(filterRequestModel);
                if (users.Count() > 0)
                {
                    foreach (var user in users)
                    {
                        await _mailService.SendMailAsync(user.Username, user.Email, "aue");
						await _notifyService.SendNotification(currJob);
                    }
                }
            }

            


            
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
        [HttpPost("GetFullSearch")]
        public async Task<IActionResult> GetFullSearch([FromBody] JobSearchModel jobSearchModel)
        {
            var result = await _jobService.GetFullSearch(jobSearchModel);
            return Ok(result);
        }
    }
}
