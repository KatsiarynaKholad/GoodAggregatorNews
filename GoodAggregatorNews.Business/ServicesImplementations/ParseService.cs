using GoodAggregatorNews.Abstractions;
using GoodAggregatorNews.Core.Abstractions;
using HtmlAgilityPack;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Business.ServicesImplementations
{
    public class ParseService:IParseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ParseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddArticleTextToArticleAsync(Guid articleid)
        {
            try
            {
                var article = await _unitOfWork.Articles.GetByIdAsync(articleid);
                if (article != null)
                {
                    var articleSourceUrl = article.SourceUrl;

                    var web = new HtmlWeb();

                    var htmldoc = web.Load(articleSourceUrl);

                    var nodes =
                       htmldoc.DocumentNode.Descendants(0)
                       .Where(n => n.HasClass("article"));

                    if (nodes.Any())
                    {
                        var articleText = nodes.FirstOrDefault()?
                           .ChildNodes
                           .Where(node => (node.Name.Equals("div")
                           || node.Name.Equals("h1")
                           || node.Name.Equals("p"))
                                       && !node.HasClass("article__reference")
                                       && !node.HasClass("article-meta")
                                       && node.Attributes["style"] == null)
                           .Select(node => node.InnerText.Trim())
                           .Aggregate((i, j) => i + Environment.NewLine + j);

                        await _unitOfWork.Articles.UpdateArticleTextAsync(articleid, articleText);
                        await _unitOfWork.Commit();

                    }


                    //var nodes =
                    //    htmldoc.DocumentNode.Descendants(0)
                    //    .Where(n => n.HasClass("article__container"));

                    //if (nodes.Any())
                    //{
                    //    var articleText = nodes.FirstOrDefault()?
                    //       .ChildNodes
                    //       .Where(node => (node.Name.Equals("p"))
                    //                   && !node.HasClass("incut")
                    //                   && !node.HasClass("article__lead")
                    //                   && !node.HasClass("global-incut")
                    //                   && node.Attributes["style"] == null)
                    //       .Select(node => node.InnerText)
                    //       .Aggregate((i, j) => i + Environment.NewLine + j);

                    //    await _unitOfWork.Articles.UpdateArticleTextAsync(articleid, articleText);
                    //    await _unitOfWork.Commit();

                    //}

                    //if (nodes.Any())
                    //{
                    //    var articleText = nodes.FirstOrDefault()?
                    //       .ChildNodes
                    //       .Where(node => (node.Name.Equals("h1")
                    //       || node.Name.Equals("p")
                    //       || node.Name.Equals("div"))
                    //                    && !node.HasClass("article__reference article__reference_header")
                    //                    && !node.HasClass("article-meta__item")
                    //                    && !node.HasClass("share")
                    //                    && !node.HasClass("global-incut")
                    //                    && !node.HasClass("custom-block rotation-1")
                    //                    && !node.HasClass("custom-block")
                    //                    && !node.HasClass("article__footer")
                    //                    && node.Attributes["style"] == null)
                    //       .Select(node => node.InnerText)
                    //       .Aggregate((i, j) => i + Environment.NewLine + j);

                    //    await _unitOfWork.Articles.UpdateArticleTextAsync(articleid, articleText);
                    //    await _unitOfWork.Commit();

                    //}

                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: AddArticleTextToArticleAsync was not successful");
                throw;
            }
        }
    }
}
