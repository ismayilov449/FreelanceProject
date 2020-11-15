using Core.Models;
using Core.Models.ServiceModels;
using Dapper;
using Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Cqrs.Commands.FilterCommand
{
    public interface IFilterCommand
    {
        Task SubscribeFilter(Filter filter, string userId);
    }

    public class FilterCommand : IFilterCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string addSubscriptionSql = $@"Insert Into Subscriptions(CityId,CategoryId,EducationId,Salary,UserId,SubscribeStatus,DeleteStatus) 
Values(@CityId,
@CategoryId,
@EducationId,
@Salary,
@UserId,
1,
0)";

        public async Task SubscribeFilter(Filter filter, string userId)
        {
            try
            {
                foreach (var item in filter.Filters)
                {
                    item.UserId = userId;
                    await _unitOfWork.GetConnection().QueryAsync(addSubscriptionSql, item, _unitOfWork.GetTransaction());

                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
