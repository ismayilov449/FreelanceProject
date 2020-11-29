using Core.Models.ServiceModels;
using Dapper;
using Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Cqrs.Queries.FilterQuery
{
    public interface IFilterQuery
    {
        Task<IEnumerable<TempUser>> GetUsers(FilterRequestModel filterRequestModel);
    }

    public class FilterQuery : IFilterQuery
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string getUsers = @"Select S.Id Id,U.UserName Username,U.Email Email,U.PhoneNumber Number from Subscriptions S
Left Join Users U on S.UserId = U.Id
where S.DeleteStatus = 0 ";

        public async Task<IEnumerable<TempUser>> GetUsers(FilterRequestModel filterRequestModel)
        {
            if (!String.IsNullOrWhiteSpace(filterRequestModel.CityId))
            {
                getUsers += @" and S.CityId = @cityId  ";
            }
            if (!String.IsNullOrWhiteSpace(filterRequestModel.CategoryId))
            {
                getUsers += @" and S.CategoryId = @categoryId ";
            }
            if (!String.IsNullOrWhiteSpace(filterRequestModel.EducationId))
            {
                getUsers += @" and S.EducationId = @educationId ";
            }
            if (filterRequestModel.Salary != 0)
            {
                getUsers += @" and S.Salary= @salary ";
            }
            if (!String.IsNullOrWhiteSpace(filterRequestModel.UserId))
            {
                getUsers += " and U.Id = @userId ";
            }
            else
            {
                getUsers += " and U.Id = S.UserId ";
            }

            var results = await _unitOfWork.GetConnection().QueryAsync<TempUser>(getUsers, filterRequestModel, _unitOfWork.GetTransaction());

            return results;
        }
    }

    public class TempUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
    }
}
