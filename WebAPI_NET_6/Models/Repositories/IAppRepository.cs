namespace WebAPI_NET_6.Models.Repositories;

public interface IAppRepository<TEntity>
{
    Task<IQueryable<Employee>> GetAll();
}
