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
            int catCounter = 0;
            int citCounter = 0;
            int eduCounter = 0;
            int allCounter = 1;
            var param = new FilterRequestModel();
            try
            {
                for (int i = 0; i < allCounter; i++)
                {

                    if (filter.Categories != null)
                    {
                        if ((filter.Categories as List<Guid>).Count > 1)
                        {
                            param.CategoryId = (filter.Categories as List<Guid>)[catCounter].ToString();
                            allCounter++;
                            catCounter++;
                        }
                        else
                        {
                            param.CategoryId = (filter.Categories as List<Guid>)[0].ToString();
                        }
                    }
                    if (filter.Cities != null)
                    {
                        if ((filter.Cities as List<Guid>).Count > 1)
                        {
                            param.CityId = (filter.Cities as List<Guid>)[citCounter].ToString();
                            allCounter++;
                            citCounter++;
                        }
                        else
                        {
                            param.CityId = (filter.Cities as List<Guid>)[0].ToString();
                        }
                    }
                    if (filter.Education != null)
                    {
                        if ((filter.Education as List<Guid>).Count > 1)
                        {
                            param.EducationId = (filter.Education as List<Guid>)[eduCounter].ToString();
                            allCounter++;
                            eduCounter++;
                        }
                        else
                        {
                            param.EducationId = (filter.Education as List<Guid>)[0].ToString();
                        }
                    }
                    param.UserId = userId;
                    param.Salary = filter.Salary;

                    await _unitOfWork.GetConnection().QueryAsync(addSubscriptionSql, param, _unitOfWork.GetTransaction());
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
