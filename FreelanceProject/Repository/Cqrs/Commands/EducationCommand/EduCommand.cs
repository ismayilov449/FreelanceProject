using System;
using System.Threading.Tasks;
using Core.Models;
using Dapper;
using Repository.Infrastructure;

namespace Repository.Cqrs.Commands.EducationCommand
{
    public interface IEduCommand
    {
        public Task<Guid> Add(Education entity);
        public Task Delete(string id);
        public Task Update(Education entity);
    }
    public class EduCommand : IEduCommand
    {
        public IUnitOfWork _unitOfWork { get; set; }

        public EduCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string updateSql = $@"Update Education set Name=@{nameof(Education.Name)} where Id=@{nameof(Education.Id)}";
        private string deleteSql = $@"Update Education set DeleteStatus=1 where Id=@id";
        private string addSql = $@"Insert Into Education(Name,DeleteStatus)
                                  Output Inserted.Id
                                  Values(@{nameof(Education.Name)},0)";


        public async Task<Guid> Add(Education entity)
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

        public async Task Delete(string id)
        {
            try
            {
                var param = new { id };
                await _unitOfWork.GetConnection().QueryAsync(deleteSql, param, _unitOfWork.GetTransaction());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Update(Education entity)
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

    }
}
