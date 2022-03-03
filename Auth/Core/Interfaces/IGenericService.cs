using System.Linq.Expressions;
using Shared.Models.Dto;

namespace Core.Interfaces
{
    public interface IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        Task<Response<TDto>> GetByIdAsync(object id);
        Task<Response<IEnumerable<TDto>>> GetAllAsync();
        Task<Response<IEnumerable<TDto>>> WhereAsync(Expression<Func<TEntity, bool>> predicate);
        Task<Response<TDto>> AddAsync(TDto dto);
        Task<Response<string>> RemoveAsync(object id);
        Task<Response<string>> UpdateAsync(object id, TDto dto);
    }
}