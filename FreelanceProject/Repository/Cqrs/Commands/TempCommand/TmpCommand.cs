using Core.temp_folder;
using Dapper;
using Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Cqrs.Commands.TempCommand
{
    public interface ITmpCommand
    {
        public Task<int> Add(TempClass entity);
    }
    public class TmpCommand : ITmpCommand
    {
        public IUnitOfWork _unitOfWork { get; set; }

        public TmpCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string addSql = $@"Insert Into TempTable(SomeProperty)
                                  Output Inserted.Id
                                  Values(@{nameof(TempClass.SomeProperty)})";

        public async Task<int> Add(TempClass entity)
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<int>(addSql, entity, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
