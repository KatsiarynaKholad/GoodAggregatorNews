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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Business.Tests
{
    public class ArticleServiceTests
    {
        public readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        public readonly Mock<IUnitOfWork> _uowMock = new Mock<IUnitOfWork>();
        public readonly Mock<IParseService> _parseServiceMock = new Mock<IParseService>();
        public readonly Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();
        public readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();

        private ArticleService GetMockedArticleService()
        {
            var articleService = new ArticleService(_mapperMock.Object,
                _uowMock.Object,
                _parseServiceMock.Object,
                _configurationMock.Object,
                _mediatorMock.Object);

            return articleService;
        }

        [Fact]
        public async Task GetArticleByIdAsync_WithCorrectId_ReturnArticleAsync()
        {
            //arrange
            var id = Guid.NewGuid();

            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetArticleByIdQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Article() { Id = id });

            _mapperMock.Setup(mapper=>mapper.Map<ArticleDto>(It.IsAny<Article>()))
                .Returns(new ArticleDto { Id = id });

            //action
            var articleService = GetMockedArticleService();
            var data = await articleService.GetArticleByIdAsync(id);

            //asserts
            Assert.NotNull(data);
            Assert.Equal(id, data.Id);
        }

        [Fact]
        public async Task GetArticleByIdAsync_WithInCorrectId_ReturnNullAsync()
        {
            //arrange
            var id = Guid.NewGuid();

            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetArticleByIdQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Article());

            //action
            var articleService = GetMockedArticleService();
            var data = await articleService.GetArticleByIdAsync(id);

            //asserts
            Assert.Null(data);
        }

     
    }
}
