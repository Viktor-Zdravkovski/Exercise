using BasicWeb.Domain;

namespace BasicWeb.Database.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<int> Create(T entity);

        Task<int> Update(T entity);

        T GetById(int id);

        void Delete(T id);

        Task<IReadOnlyList<T>> Get();
    }
}
