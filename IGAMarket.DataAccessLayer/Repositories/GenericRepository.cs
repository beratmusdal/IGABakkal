using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.DataAccessLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IGAMarket.DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        private readonly Context _context;
        private readonly DbSet<T> _dbSet;
        

        public GenericRepository(Context context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public void Insert(T entity)
        {
            _dbSet.Add(entity); 
            _context.SaveChanges();  
        }

        public void Delete(T entity)
        {           
            var property = _context.Entry(entity).Property("IsDeleted");
            if (property != null)
            {
                property.CurrentValue = true; 
                _context.SaveChanges();
            }
            else
            {
                _dbSet.Remove(entity);  
                _context.SaveChanges();
            }
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public void RemoveAll()
        {
            var allEntities = _dbSet.ToList();
            _dbSet.RemoveRange(allEntities);
            _context.SaveChanges();
        }


        public void Update(T entity)
        {
            _dbSet.Update(entity);  
            _context.SaveChanges(); 
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id); 
        }


        public T Get(Expression<Func<T, bool>> filter)
        {
            return _context.Set<T>().SingleOrDefault(filter);
        }

        public List<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            return filter == null ? _context.Set<T>().ToList() : _context.Set<T>().Where(filter).ToList();
        }
    }
}
