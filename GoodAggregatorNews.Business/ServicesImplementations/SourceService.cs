using AutoMapper;
using GoodAggregatorNews.Abstractions;
using GoodAggregatorNews.Core.Abstractions;
using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Business.ServicesImplementations
{
    public class SourceService : ISourceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SourceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> CreateSourceAsync(SourceDto dto)
        {
            try
            {
                var entity = _mapper.Map<Source>(dto);
                if (entity!=null)
                {
                    await _unitOfWork.Sources.AddAsync(entity);
                    return await _unitOfWork.Commit();
                }
                else
                {
                    throw new ArgumentException(nameof(dto));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: CreateSourceAsync was not successful");
                throw;
            }
        }

        public async Task<List<SourceDto>> GetSourceAsync()
        {
            try
            {
                return await _unitOfWork.Sources.Get()
                    .Select(entities => _mapper.Map<SourceDto>(entities))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: GetSourceAsync was not successful");
                throw;
            }
        }

        public async Task<SourceDto> GetSourceByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.Sources.GetByIdAsync(id);
                if (entity!=null)
                {
                    var dto = _mapper.Map<SourceDto>(entity);
                    return dto;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: GetSourceByIdAsync was not successful");
                throw;
            }
        }
    }
}
