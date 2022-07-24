using Contracts;
using Entities;

namespace Repo
{
    public class RepoManager : IRepoManager
    {
        private RepoContext _context;
        private ICompanyRepo _companyRepo;
        private IEmployeeRepo _employeeRepo;
        public RepoManager(RepoContext context)
        {
            _context = context;
        }

        public ICompanyRepo Company
        {
            get
            {
                if (_companyRepo == null)
                    _companyRepo = new CompanyRepo(_context);
                return _companyRepo;
            }
        }

        public IEmployeeRepo Employee
        {
            get
            {
                if (_employeeRepo == null)
                    _employeeRepo = new EmployeeRepo(_context);
                return _employeeRepo;
            }
        }

        public Task SaveAsync() => _context.SaveChangesAsync();
    }
}

