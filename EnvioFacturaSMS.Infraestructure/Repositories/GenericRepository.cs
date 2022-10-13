using EnvioFacturaSMS.Domain.Entities;
using EnvioFacturaSMS.Domain.Interfaces;
using EnvioFacturaSMS.Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Infraestructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DbSet<T> _entityBase;
        private readonly PrivadaContext _context;

        public GenericRepository(PrivadaContext context)
        {
            _context = context;
            _entityBase = _context.Set<T>();
        }
        public async Task<T> Get(Expression<Func<T, bool>> condition = null)
        {
            try
            {
                IQueryable<T> Query = _context.Set<T>();
                if (condition != null) Query = Query.Where(condition);
                List<T> result = await Query.ToListAsync();
                return result.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        public async Task<bool> Insert(T entity)
        {
            try
            {
                _entityBase.Add(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
            
        }

        public async Task<List<T>> List(Expression<Func<T, bool>> condition)
        {
            IQueryable<T> Query = _context.Set<T>();
            if (condition != null) Query = Query.Where(condition);
            List<T> result = await Query.ToListAsync();
            return result;
        }

        public async Task<bool> Update(T entity)
        {
            try
            {
                _entityBase.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
