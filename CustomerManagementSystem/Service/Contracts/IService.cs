using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CustomerManagementSystem.Service
{
    public interface IService<T> where T : class
    {
        IQueryable<T> Load();

        IQueryable<T> Load(Expression<Func<T, bool>> predicate);

        IEnumerable<V> ExecuteFunction<V>(string functionName, Dictionary<string, object> parameters) where V : class;

        T Add(T entity);

        void Update(T entity);

        bool Exists(Expression<Func<T, bool>> predicate);
    }
}