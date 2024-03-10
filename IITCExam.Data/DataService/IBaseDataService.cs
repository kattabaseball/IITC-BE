using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IITCExam.Data.DataService
{
    public interface IBaseDataService<TEntity> where TEntity : class
    {
        TEntity Add(TEntity item);
        void Update(TEntity item);
        void Delete(TEntity item);
        void DeleteRange(List<TEntity> item);
        IEnumerable<TEntity> GetAll();
        IDbContextTransaction InitTransaction();
    }

    public class BaseDataService<TEntity> : IBaseDataService<TEntity> where TEntity : class
    {
        protected DatabaseContext databaseContext;

        public BaseDataService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public TEntity Add(TEntity item)
        {
            databaseContext.Set<TEntity>().Add(item);
            databaseContext.SaveChanges();
            return item;
        }

        public void Update(TEntity item)
        {
            databaseContext.Set<TEntity>().Update(item);
            databaseContext.SaveChanges();
        }

        public void Delete(TEntity item)
        {
            databaseContext.Set<TEntity>().Remove(item);
            databaseContext.SaveChanges();
        }

        public void DeleteRange(List<TEntity> items)
        {
            databaseContext.Set<TEntity>().RemoveRange(items);
            databaseContext.SaveChanges();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return this.databaseContext.Set<TEntity>();
        }

        public IDbContextTransaction InitTransaction()
        {
            return databaseContext.Database.BeginTransaction();
        }
    }
}
