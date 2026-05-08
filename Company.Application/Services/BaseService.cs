using Company.Application.Common;
using Company.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Application.Services
{
    public abstract class BaseService<TEntity, TEntityDto, TCreateDto, TUpdateDto>
            : IBaseService<TEntityDto, TCreateDto, TUpdateDto>
            where TEntity : class
    {
        protected readonly IApplicationDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected BaseService(IApplicationDbContext context, DbSet<TEntity> dbSet)
        {
            _context = context;
            _dbSet = dbSet;
        }

        protected abstract TEntityDto MapToDto(TEntity entity);
        protected abstract TEntity MapToEntity(TCreateDto dto);
        protected abstract void UpdateEntity(TUpdateDto dto, TEntity entity);

        public virtual async Task<IEnumerable<TEntityDto>> GetAllAsync()
        {
            var entities = await _dbSet.AsNoTracking().ToListAsync();
            return entities.Select(MapToDto);
        }

        public virtual async Task<TEntityDto> GetByAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id)
                ?? throw new KeyNotFoundException($"{typeof(TEntity).Name} not found");

            return MapToDto(entity);
        }

        public virtual async Task<TEntityDto> CreateAsync(TCreateDto dto)
        {
            var entity = MapToEntity(dto);

            var idProperty = typeof(TEntity).GetProperty("Id");
            if (idProperty != null && (Guid)idProperty.GetValue(entity) == Guid.Empty)
            {
                idProperty.SetValue(entity, Guid.NewGuid());
            }

            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return MapToDto(entity);
        }

        public virtual async Task DeleteByAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id)
                ?? throw new KeyNotFoundException($"{typeof(TEntity).Name} not found");

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public abstract Task<PagedResponse<TEntityDto>> GetAllPagedAsync(PagedRequest request);

        public virtual async Task<TEntityDto> UpdateAsync(Guid id, TUpdateDto dto)
        {
            var entity = await _dbSet.FindAsync(id)
                ?? throw new KeyNotFoundException($"{typeof(TEntity).Name} not found");

            UpdateEntity(dto, entity);
            await _context.SaveChangesAsync();

            return MapToDto(entity);
        }
    }
}
