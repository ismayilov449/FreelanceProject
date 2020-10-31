using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Repository.Cqrs.Commands.EducationCommand;
using Repository.Cqrs.Queries.EducationQuery;

namespace Repository
{
    public interface IEduRepository : IEduCommand, IEduQuery
    {
    }

    public class EduRepository : IEduRepository
    {
        private readonly IEduCommand _eduCommand;
        private readonly IEduQuery _eduQuery;

        public EduRepository(IEduCommand eduCommand, IEduQuery eduQuery)
        {
            _eduCommand = eduCommand;
            _eduQuery = eduQuery;
        }

        public async Task<Guid> Add(Education entity)
        {
            var result = await _eduCommand.Add(entity);
            return result;
        }
        public async Task Delete(string id)
        {
            await _eduCommand.Delete(id);
        }
        public async Task Update(Education entity)
        {
            await _eduCommand.Update(entity);
        }
        public async Task<IEnumerable<Education>> GetAll()
        {
            var result = await _eduQuery.GetAll();
            return result;
        }

        public async Task<Education> GetById(string id)
        {
            var result = await _eduQuery.GetById(id);
            return result;
        }
    }
}
