namespace Contracts
{
    public interface IRepoManager
    {
        ICompanyRepo Company { get; }
        IEmployeeRepo Employee { get; }
        Task SaveAsync();
    }
}

