using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebAdminStacks.Infrastructure.Contract
{

    public enum SortOrder { Ascending = 1, Descending }


    #region For Open Access
    public interface IWebAdminOpenRepository<T> where T : class
    {
        DbContext RepositoryContext();
        T Add(T entity);
        T Update(T entity);
    }
    #endregion



    internal interface IWebAdminRepository<T> where T : class
    {
        DbContext RepositoryContext();
        T Add(T entity);
        T Remove(T entity);
        T Remove(object key);
        T Update(T entity);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate, string includeProperties);
        T GetById(object key);
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        IEnumerable<T> Get<TOrderBy>(Expression<Func<T, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending);
        IEnumerable<T> Get<TOrderBy>(Expression<Func<T, bool>> criteria, Expression<Func<T, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending, string includeProperties = "");
    }
}
