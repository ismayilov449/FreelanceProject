using System;
using System.Threading.Tasks;
using Core.Models;
using Dapper;
using Repository.Infrastructure;

namespace Repository.Cqrs.Commands.CityCommand
{
    public interface ICtyCommand
    {

        public Task<Guid> Add(City entity);
        public Task Delete(string id);
        public Task Update(City entity);
    }
    public class CtyCommand : ICtyCommand
    {
        public IUnitOfWork _unitOfWork { get; set; }

        public CtyCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

     

        private string updateSql = $@"Update Cities set Name=@{nameof(City.Name)} where Id=@{nameof(City.Id)}";

        public async Task Update(City entity)
        {
            try
            {
                await _unitOfWork.GetConnection().QueryAsync(updateSql, entity, _unitOfWork.GetTransaction());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string deleteSql = $@"Update Cities set DeleteStatus=1 where Id=@id";

        public async Task Delete( string id)
        {
            try
            {
                var param = new { id };
                 await _unitOfWork.GetConnection().QueryAsync(deleteSql,param , _unitOfWork.GetTransaction());
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string addSql = $@"Insert Into Cities(Name,DeleteStatus)
                                  Output Inserted.Id
                                  Values(@{nameof(City.Name)},0)";

        public async Task<Guid> Add(City entity)
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<Guid>(addSql, entity, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
