using AutoMapper;
using GoodAggregatorNews.Abstractions;
using GoodAggregatorNews.Business.ServicesImplementations;
using GoodAggregatorNews.Core.Abstractions;
using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.Data.CQS.Queries;
using GoodAggregatorNews.Database.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Business.Tests
{
    public class CommentServiceTests
    {

        public readonly Mock<IUnitOfWork> _uowMock = new Mock<IUnitOfWork>();
        public readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();

        private CommentService GetMockedCommentService()
        {
            var commentService = new CommentService(_uowMock.Object,
                _mapperMock.Object);

            return commentService;
        }

        [Fact]
        public async Task FindCommentByIdAsync_WithCorrectId_ReturnCommentDtoAsync()
        {
            //arrange
            var id = Guid.NewGuid();

            var comment = new Comment()
            { 
                Id = id,
            };

            _uowMock.Setup(uow => uow.Comments.GetByIdAsync(id))
                .ReturnsAsync(comment);

            _mapperMock.Setup(m => m.Map<CommentDto>(comment))
                .Returns(new CommentDto { Id = id });


            //action
            var commentService = GetMockedCommentService();
            var data = await commentService.FindCommentByIdAsync(id);

            //asserts
            Assert.NotNull(data);
            Assert.Equal(comment.Id, data.Id);
        }

        [Fact]
        public async Task FindCommentByIdAsync_InCorrectId_ReturnNull()
        {
            //arrange
            var id = Guid.NewGuid();

            var comment = new Comment();

            _uowMock.Setup(uow => uow.Comments.GetByIdAsync(id))
                .ReturnsAsync(comment);

            //action
            var commentService = GetMockedCommentService();
            var data = await commentService.FindCommentByIdAsync(id);

            //asserts
            Assert.Null(data);
        }

    }
}
