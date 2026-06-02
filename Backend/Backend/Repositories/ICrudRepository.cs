using Backend.Models;

namespace Backend.Repositories;

public interface ICrudRepository<ID, T> where T : Entity<ID>
{
   T Add(T entity);
   IEnumerable<T> GetAll();
}