using System.Linq.Expressions;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Dto;

namespace Service.Services
{
    public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<TEntity> _repository;

        public GenericService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<Response<TDto>> AddAsync(TDto dto)
        {
            var entity = ObjectMapper.Mapper.Map<TEntity>(dto);

            await _repository.AddAsync(entity);

            await _unitOfWork.CommitAsync();

            var newDto = ObjectMapper.Mapper.Map<TDto>(entity);
            return Response<TDto>.Success(newDto, 200);
        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync().ToListAsync();

            var items = ObjectMapper.Mapper.Map<List<TDto>>(list);

            return Response<IEnumerable<TDto>>.Success(items, 200);
        }

        public async Task<Response<IEnumerable<TDto>>> WhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var list = await _repository.Where(predicate).ToListAsync();

            var items = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(list);

            return Response<IEnumerable<TDto>>.Success(items, 200);
        }

        public async Task<Response<TDto>> GetByIdAsync(object id)
        {
            var item = await _repository.GetByIdAsync(id);

            if (item == null)
            {
                return Response<TDto>.Fail("Item not found.", 404, true);
            }

            var dto = ObjectMapper.Mapper.Map<TDto>(item);

            return Response<TDto>.Success(dto, 200);
        }

        public async Task<Response<string>> RemoveAsync(object id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return Response<string>.Fail("Entity not found.", 404, true);
            }

            _repository.Remove(entity);

            await _unitOfWork.CommitAsync();

            return Response<string>.Success(200);
        }

        public async Task<Response<string>> UpdateAsync(object id, TDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return Response<string>.Fail("Entity not found.", 404, true);
            }

            var updateEntity = ObjectMapper.Mapper.Map<TEntity>(dto);

            _repository.Update(updateEntity);

            await _unitOfWork.CommitAsync();

            return Response<string>.Success(204);
        }
    }
}