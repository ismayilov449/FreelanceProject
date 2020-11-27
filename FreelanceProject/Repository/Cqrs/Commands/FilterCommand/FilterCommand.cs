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
        Task<Guid> SubscribeFilter(Filter filter, string userId);
        Task Delete(string id);
    }

    public class FilterCommand : IFilterCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string addSubscriptionSql = $@"Insert Into Subscriptions(CityId,CategoryId,EducationId,Salary,UserId,SubscribeStatus,DeleteStatus)
Output Inserted.Id
Values(@CityId,
@CategoryId,
@EducationId,
@Salary,
@UserId,
1,
0)";

        private string deleteSql = $@"Update Subscriptions Set DeleteStatus = 1 Where Id = @id";

        public async Task Delete(string id)
        {
            try
            {
                await _unitOfWork.GetConnection().QueryAsync(deleteSql, new { id }, _unitOfWork.GetTransaction());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Guid> SubscribeFilter(Filter filter, string userId)
        {
            try
            {

                //  foreach (var item in filter.Filters)
                // {
                var filt = filter.Filters.AsList().ToArray()[0];
                filt.UserId = userId;
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<Guid>(addSubscriptionSql, filt, _unitOfWork.GetTransaction());

                // }
                return result;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
