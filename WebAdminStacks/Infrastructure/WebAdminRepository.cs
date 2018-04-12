using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using WebAdminStacks.Infrastructure.Contract;

namespace WebAdminStacks.Infrastructure
{

    #region Open Access
    public class WebAdminOpenRepository<T> : IWebAdminOpenRepository<T> where T : class
    {
        private readonly IWebAdminOpenUoWork _context;
        private readonly DbSet<T> _dbSet;
        private readonly DbContext _dbContext;

        public WebAdminOpenRepository(WebAdminOpenUoWork uoWork)
        {
            if (uoWork == null) throw new ArgumentNullException("uoWork");
            _context = uoWork;
            _dbSet = uoWork.Context.WebAdminDbContext.Set<T>();
            _dbContext = uoWork.Context.WebAdminDbContext;
        }

        public T Add(T entity)
        {
            return _dbSet.Add(entity);
        }
        
        public T Update(T entity)
        {
            var updated = _dbSet.Attach(entity);
            _context.Context.WebAdminDbContext.Entry(entity).State = EntityState.Modified;
            return updated;
        }

        public DbContext RepositoryContext()
        {
            return _dbContext;
        }
    }

    #endregion


    public class WebAdminRepository<T> : IWebAdminRepository<T> where T : class
    {

        private readonly IWebAdminUoWork _context;
        private readonly DbSet<T> _dbSet;
        private readonly DbContext _dbContext;

        public WebAdminRepository(WebAdminUoWork uoWork)
        {
            if (uoWork == null) throw new ArgumentNullException("uoWork");
            _context = uoWork;
            _dbSet = uoWork.Context.WebAdminDbContext.Set<T>();
            _dbContext = uoWork.Context.WebAdminDbContext;
        }


        public T Add(T entity)
        {
            return _dbSet.Add(entity);
        }

        public T Remove(T entity)
        {
            return _dbSet.Remove(entity);
        }

        public T Remove(object key)
        {
            var entity = _dbSet.Find(key);
            return _dbSet.Remove(entity);
        }

        public T Update(T entity)
        {
            var updated = _dbSet.Attach(entity);
            _context.Context.WebAdminDbContext.Entry(entity).State = EntityState.Modified;
            return updated;
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return GetAll().Where(predicate);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate, string includeProperties)
        {
            var query = GetAll().Where(predicate);
            if (!query.Any()) { return null; }
            if (string.IsNullOrEmpty(includeProperties)) { return query; }
            return includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public T GetById(object key)
        {
            return _dbSet.Find(key);
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        

        public IEnumerable<T> Get<TOrderBy>(Expression<Func<T, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending)
        {
            return sortOrder == SortOrder.Ascending ? GetAll().OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable() : GetAll().OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
        }

        public IEnumerable<T> Get<TOrderBy>(Expression<Func<T, bool>> criteria, Expression<Func<T, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending, string includeProperties = "")
        {
            var filtValue = GetAll(criteria, includeProperties);
            if (filtValue == null) { return null; }
            return sortOrder == SortOrder.Ascending ? filtValue.OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable() : filtValue.OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
        }

        public DbContext RepositoryContext()
        {
            return _dbContext;
        }
    }
}
