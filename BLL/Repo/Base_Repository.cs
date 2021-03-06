using BLL.IRepo;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repo
{
    public abstract class Base_Repository<T> : IBaseRepo<T> where T : class
    {
        protected TripContext _Context { set; get; }

        private DbSet<T> table = null;
        private bool disposedValue;

        protected DbSet<T> DbSet
        {
            get => table ?? (table = _Context.Set<T>());
        }
        public Base_Repository(TripContext context)
        {
            _Context = context;
            table = context.Set<T>();
        }
        public void Create(T Entity)
        {
            table.Add(Entity);
        }

        public void Delete(T Entity)
        {

            table.Remove(Entity);

        }

        public  List<T> GetAll()
        {
            //CustomQueryBuilder<T>.GetList(table );

            return  table.ToList();
        }
        public  List<T> GetAllWithInc(List<string> inclde_List)
        {
            //CustomQueryBuilder<T>.GetList(table );
            IQueryable<T> res = null;
            foreach (var item in inclde_List)
            {
                 res = table.Include(item);

            }
            return res.ToList();
        }
        public  List<T> GetByConditionWithInclude(Expression<Func<T, bool>> expression, List<string> inclde_List = null)
        {
            List<T> res = null;
            foreach (var item in inclde_List)
            {
                 res =  table.Include(item).Where(expression).ToList();

            }

            return res;
        }

            public List<T> GetByCondition(Expression<Func<T, bool>> expression)
        {

            var result =  table.Where(expression).ToList();

            return result;
        }



        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        public  T GetByID(int Id)
        {
            return  table.Find(Id);
        }

        public void Update(T Entity)
        {
            table.Update(Entity);
        }



    }
}
