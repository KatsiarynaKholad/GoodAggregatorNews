using GoodAggregatorNews.Core.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Core.Abstractions
{
    public interface ICommentService
    {
        Task<CommentDto> FindCommentByIdAsync(Guid id);
        Task<List<CommentDto>> FindCommentsByArticleIdAsync(Guid id);   
        Task<int> AddCommentAsync(CommentDto comment);
        //удаление комментария
        //изменение комментария
    }
}
