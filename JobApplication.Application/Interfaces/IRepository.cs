using JobApplication.Domain;
using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface IRepository <T> where T: BaseEntity
    {
        Task InsertAsync(T entity);
        void Update(T entity);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();

        void Remove(T entity);
        Task SaveChangesAsync();
    }
}
