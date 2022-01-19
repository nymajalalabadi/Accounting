using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.DataLayer
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        private Accounting_DB1Entities _db;


        private DbSet<TEntity> _dbset;



        public GenericRepository(Accounting_DB1Entities db)
        {
            _db = db;
            _dbset = _db.Set<TEntity>();
        }


        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity,bool>> where = null )
        {
            IQueryable<TEntity> query = _dbset;
            if (where != null)
            {
                query = query.Where(where);
            }
            return query.ToList();
        }
        


        public virtual TEntity GetById(object id)
        {
            return _dbset.Find(id);
        }


        public virtual void Insert(TEntity entity)
        {
            _dbset.Add(entity);
        }



        public virtual void Update(TEntity entity)
        {
            //if (_db.Entry(TEntity).State==EntityState.Detached)
            //{
            //    _dbset.Attach(entity);
            //}
            //_db.Entry(entity).State = EntityState.Modified;


            _dbset.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
        }


        public virtual void Delete(TEntity entity)
        {
           if( _db.Entry(entity).State == EntityState.Detached)
            {
                _dbset.Attach(entity);
            }
            _dbset.Remove(entity);
        }


        public virtual void Delete(Object id)
        {
            var entity = GetById(id);
            Delete(entity);
        }
    }
}
