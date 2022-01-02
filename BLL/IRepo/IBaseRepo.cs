using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IRepo
{
    public interface IBaseRepo<T>
    {
        List<T> GetAll();
        Task<List<T>> GetAllWithInc(List<string> inclde_List);
        Task<T> GetByID(int Id);
        Task<List<T>> GetByCondition(Expression<Func<T, bool>> expression);
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> order = null, string includeproperties = "");
        void Create(T Entity);
        void Update(T Entity);
        void Delete(T Entity);
    }
}
