using System;
using Core.Models.ServiceModels;

namespace Api.Infrastructure.RequestModels
{
    public class SubscribeRequestModel
    {
        public string token { get; set; }
        public Filter filter { get; set; }
    }
}
