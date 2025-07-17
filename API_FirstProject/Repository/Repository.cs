using API_FirstProject.Data;
using API_FirstProject.Models;

using API_FirstProject.Repository.IRepository;



using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace API_FirstProject.Repository
{
    public class Repository<T> : IRepositry<T> where T : class
    {


        ApplicationDbContext db;//= new AplicationDbContext();
        DbSet<T>dbset;
        public Repository(ApplicationDbContext db)
        {
            this.db = db;
           dbset= db.Set<T>();  
        }

        public IQueryable<T> GetAll(
         Func<IQueryable<T>, IQueryable<T>>[]? includes = null,
         Expression<Func<T, bool>>? filter = null,
         bool tracked = true)
        {
            IQueryable<T> query = dbset;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = include(query);
                }
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return query; 
        }




        public T? Getone(Func<IQueryable<T>, IQueryable<T>>[]? includes = null,
    Expression<Func<T, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbset;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = include(query);
                }
            }


            if (filter != null)
            {
                query = query.Where(filter);
}
            //return query.ToList();
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return query.FirstOrDefault();


        }
        public void Add(T entity)
        {

            dbset.Add(entity);

        }

        public void Edit(T entity)
        {
            dbset.Update(entity);
        }
        public void Delete(T entity)
        {
            dbset.Remove(entity);
        }
        public void Commit()
        {
            db.SaveChanges();
        }

       
    }
}
