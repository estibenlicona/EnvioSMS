using EnvioFacturaSMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> Get(Expression<Func<T, bool>> condition);

        Task<bool> Insert(T entity);

        Task<bool> Update(T entity);

        Task<List<T>> List(Expression<Func<T, bool>> condition);
    }
}
