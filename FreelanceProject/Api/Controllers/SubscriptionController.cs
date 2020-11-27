using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Infrastructure.Helpers;
using Api.Infrastructure.RequestModels;
using Core.Models.ServiceModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Services.Services;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly IFilterService _filterService;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public SubscriptionController(IFilterService filterService, IConfiguration configuration, IAuthService authService)
        {
            _filterService = filterService;
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost("SubscribeJob")]
        public async Task<IActionResult> Subscribe([FromBody]SubscribeRequestModel model)
        {
            var data = TokenManager.GetPrincipal(model.token, _configuration);
            var currentUser = await _authService.GetUserById(data.Claims.FirstOrDefault(i => i.Type == "UserId").Value);

           var result= await _filterService.SubscribeFilter(model.filter, currentUser.Id.ToString());
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            await _filterService.Delete(id);
            return Ok();
        }
    }
}
