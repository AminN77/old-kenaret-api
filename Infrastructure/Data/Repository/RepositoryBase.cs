using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Contracts.Repository;
using Infrastructure.Data.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class

    {
        protected PgSqlDbContext _repositoryContext;

        public RepositoryBase(PgSqlDbContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ?
                _repositoryContext.Set<T>()
                    .AsNoTracking()
                : _repositoryContext.Set<T>();


        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges
                ? _repositoryContext.Set<T>()
                    .Where(expression)
                    .AsNoTracking()
                : _repositoryContext.Set<T>()
                    .Where(expression);


        public void Create(T entity) => _repositoryContext.Set<T>().Add(entity);
        public async Task CreateAsync(T entity) => await _repositoryContext.Set<T>().AddAsync(entity);
        public void Update(T entity) => _repositoryContext.Set<T>().Update(entity);
        public void Delete(T entity) => _repositoryContext.Set<T>().Remove(entity);
    }
}