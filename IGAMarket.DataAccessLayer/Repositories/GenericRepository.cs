using IGAMarket.DataAccessLayer.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IGAMarket.DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        

        public GenericRepository(DbContext context)
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
            // Soft delete işlemi: Entity'nin "IsDeleted" bayrağını true yapıyoruz
            var property = _context.Entry(entity).Property("IsDeleted");
            if (property != null)
            {
                property.CurrentValue = true;  // IsDeleted bayrağını true yapıyoruz
                _context.SaveChanges();
            }
            else
            {
                _dbSet.Remove(entity);  // Eğer IsDeleted özelliği yoksa normal silme işlemi yapılır
                _context.SaveChanges();
            }
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);  // Entity'yi güncelle
            _context.SaveChanges();  // Değişiklikleri kaydet
        }

        public List<T> GetList()
        {
            return _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false).ToList();  // Silinmemiş kayıtları listele
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);  // Id'ye göre bir entity döndür
        }
    }
}
