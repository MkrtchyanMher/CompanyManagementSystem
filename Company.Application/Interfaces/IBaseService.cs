using Company.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Application.Interfaces
{
    public interface IBaseService<TEntityDto, TCreateDto, TUpdateDto>
    {
        Task<IEnumerable<TEntityDto>> GetAllAsync();
        Task<TEntityDto> GetByAsync(Guid id);
        Task<TEntityDto> CreateAsync(TCreateDto dto);
        Task<PagedResponse<TEntityDto>> GetAllPagedAsync(PagedRequest request);
        Task<TEntityDto> UpdateAsync(Guid id, TUpdateDto dto);
        Task DeleteByAsync(Guid id);
    }
}
