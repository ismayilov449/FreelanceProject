using Core.temp_folder;
using Dapper;
using Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Cqrs.Queries.TempQuery
{
    public interface ITmpQuery
    {
        Task<IEnumerable<TempClass>> GetAll();
        Task<TempClass> GetById(int id);
    }
    public class TmpQuery : ITmpQuery
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public TmpQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private string getAllSql = @"Select * from TempTable";
        private string getByIdSql = @"Select * from TempTable
                                      Where Id = @id";
        public async Task<IEnumerable<TempClass>> GetAll()
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryAsync<TempClass>(getAllSql, null, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TempClass> GetById(int id)
        {
            var param = new
            {
                id
            };
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<TempClass>(getByIdSql, param, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
