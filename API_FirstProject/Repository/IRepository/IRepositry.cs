using System.Linq.Expressions;


namespace  API_FirstProject.Repository.IRepository
{
    public interface IRepositry<T> where T : class
    {
        IQueryable<T> GetAll(

          Func<IQueryable<T>, IQueryable<T>>[]? includes = null,
          Expression<Func<T, bool>>? filter = null, bool tracked = true);
         T? Getone(Func<IQueryable<T>, IQueryable<T>>[]? includes = null,
          Expression<Func<T, bool>>? filter = null, bool tracked = true);

        void Add(T category);


        void Edit(T category);

         void Delete(T category);

        void Commit();

    }
}
