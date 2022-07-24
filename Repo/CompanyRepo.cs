using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repo
{
    public class CompanyRepo : RepoBase<Company>, ICompanyRepo
    {
        public CompanyRepo(RepoContext context) : base(context)
        {
        }

        public void CreateCompany(Company company) => Create(company);

        public void DeleteCompany(Company company) => Delete(company);

        public async Task<IEnumerable<Company>> GetAllCompanies(bool trackChanges) =>
            await FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToListAsync();

        public async Task<IEnumerable<Company>> GetByIds(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(x => ids.Contains(x.Id), trackChanges)
            .ToListAsync();

        public async Task<Company> GetCompany(Guid companyId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(companyId), trackChanges)
            .SingleOrDefaultAsync();
    }
}

