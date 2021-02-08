using CustomerManagementSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CustomerManagementSystem.Service
{
    public class GenericService<T> : IService<T> where T : class
    {
        protected readonly IUnitOfWork unitOfWork;

        protected IRepository<T> repository;

        public GenericService(IUnitOfWork unitOfWork, IRepository<T> repository)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
        }

        public virtual IQueryable<T> Load()
        {
            return this.repository.Load();
        }

        public virtual IQueryable<T> Load(Expression<Func<T, bool>> predicate)
        {
            return this.repository.Load(predicate);
        }

        public virtual T Add(T entity)
        {
            return this.repository.Add(entity);
        }

        public virtual void Update(T entity)
        {
            this.repository.Update(entity);
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return this.repository.Exists(predicate);
        }

        public virtual IEnumerable<dynamic> ExecuteFunction(string functionName, Dictionary<string, object> parameters)
        {
            return this.repository.ExecuteFunction(functionName, parameters);
        }

        public IEnumerable<V> ExecuteFunction<V>(string functionName, Dictionary<string, object> parameters) where V : class
        {
            return this.repository.ExecuteFunction<V>(functionName, parameters);
        }
    }
}