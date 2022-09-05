using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Contracts.Repository
{
        public interface IRepositoryBase<T>
        {
            IQueryable<T> FindAll(bool trackChanges);
            IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
                bool trackChanges);

            void Create(T entity);
            Task CreateAsync(T entity);
            void Update(T entity);
            void Delete(T entity);
        }

}