using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Dynamic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace CustomerManagementSystem.Repository
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        protected IUnitOfWork unitOfWork;

        protected readonly DbContext dbContext;
        protected readonly ObjectContext objectContext;
        private readonly DbSet<T> dbSet;

        public virtual IQueryable<T> Load()
        {
            return this.dbSet.AsQueryable();
        }

        public virtual IQueryable<T> Load(Expression<Func<T, bool>> predicate)
        {
            return this.dbSet.Where(predicate).AsQueryable();
        }

        //public virtual T LoadById(Guid guid)
        //{
        //    return this.Load(x => x. == guid).SingleOrDefault();
        //}

        public virtual T Add(T entity)
        {
            return this.dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            var updatedEntity = this.dbContext.Entry(entity);

            this.dbSet.Attach(entity);
            updatedEntity.State = System.Data.Entity.EntityState.Modified;
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return this.dbSet.Count(predicate) > 0;
        }

        public virtual IEnumerable<dynamic> ExecuteQuery(string query)
        {
            return this.ExecuteQuery(query, new Dictionary<string, object>());
        }

        public virtual IEnumerable<dynamic> ExecuteQuery(string query, Dictionary<string, object> parameters)
        {
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                command.CommandTimeout = 0;
                command.CommandText = query;

                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    DbParameter dbParameter = command.CreateParameter();
                    dbParameter.ParameterName = parameter.Key;
                    dbParameter.Value = (parameter.Value == DBNull.Value || parameter.Value == null) ? parameter.Value : parameter.Value.ToString().Replace("'", "''");

                    command.Parameters.Add(dbParameter);
                }

                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var dataRow = GetDataRow(dataReader);
                        yield return dataRow;
                    }
                }

                command.Parameters.Clear();
            }
        }

        public virtual IEnumerable<V> ExecuteQuery<V>(string query, Dictionary<string, object> parameters)
        {
            SqlParameter[] sqlParameters = parameters.Select(p => new SqlParameter(p.Key, p.Value != null ? p.Value.ToString().Replace("'", "''") : p.Value)).ToArray();

            this.dbContext.Database.CommandTimeout = 0;
            return this.dbContext.Database.SqlQuery<V>(query, sqlParameters);
        }

        protected static dynamic GetDataRow(DbDataReader dataReader)
        {
            var dataRow = new ExpandoObject() as IDictionary<string, object>;

            for (var i = 0; i < dataReader.FieldCount; i++)
            {
                dataRow.Add(dataReader.GetName(i), dataReader[i]);
            }

            return dataRow;
        }

        private string GetFunctionQuery(string procedureName, Dictionary<string, object> parameters)
        {
            string execSuffix = (parameters == null) ? "" : string.Join(", ", parameters.Select(x => (x.Key.Substring(0, 1) == "@" ? "" : "@") + x.Key).ToArray());
            return "EXEC " + procedureName + " " + execSuffix;
        }

        public IEnumerable<V> ExecuteFunction<V>(string functionName, Dictionary<string, object> parameters) where V : class
        {
            return this.ExecuteQuery<V>(GetFunctionQuery(functionName, parameters), parameters);
        }

        public IEnumerable<dynamic> ExecuteFunction(string functionName, Dictionary<string, object> parameters)
        {
            return ExecuteQuery(GetFunctionQuery(functionName, parameters), parameters);
        }
    }
}