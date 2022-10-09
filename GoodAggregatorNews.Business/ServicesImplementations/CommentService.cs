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
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> AddCommentAsync(CommentDto commentdto)
        {
            try
            {
                var entity = _mapper.Map<Comment>(commentdto);

                if (entity!=null)
                {
                    await _unitOfWork.Comments.AddAsync(entity);
                    return await _unitOfWork.Commit();
                }
                else
                {
                    throw new ArgumentException(nameof(commentdto));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: AddCommentAsync was not successful");
                throw;
            }
        }

        public async Task<CommentDto> FindCommentByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.Comments.GetByIdAsync(id);
                if (entity!=null)
                {
                    var commentDto = _mapper.Map<CommentDto>(entity);
                    return commentDto;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: FindCommentByIdAsync was not successful");
                throw;
            }
        }

        public async Task<List<CommentDto>> FindCommentsByArticleIdAsync(Guid id)
        {
            try
            {
                var comments = await _unitOfWork.Comments
                  .FindBy(comment => comment.ArticleId.Equals(id))
                  .OrderBy(date => date.PublicationDate)
                  .ToListAsync();
                if (comments!=null)
                {
                    var dtoList = _mapper.Map<List<CommentDto>>(comments);
                    return dtoList;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: FindCommentsByArticleIdAsync was not successful");
                throw;
            }
        }
    }
}
