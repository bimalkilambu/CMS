using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CustomerManagementSystem.Repository
{
    public interface IRepository <T> where T: class
    {
        IQueryable<T> Load();

        //T LoadById(Guid guid);

        IQueryable<T> Load(Expression<Func<T, bool>> predicate);

        T Add(T entity);

        void Update(T entity);

        bool Exists(Expression<Func<T, bool>> predicate);

        IEnumerable<dynamic> ExecuteFunction(string functionName, Dictionary<string, object> parameters);

        IEnumerable<V> ExecuteFunction<V>(string functionName, Dictionary<string, object> parameters) where V : class;
    }
}