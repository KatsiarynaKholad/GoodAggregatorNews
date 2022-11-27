using GoodAggregatorNews.Abstractions;
using GoodAggregatorNews.Core.Abstractions;
using HtmlAgilityPack;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                       .Where(n => n.HasClass("article__body"));

                    if (nodes.Any())
                    {
                        var articleText = nodes.FirstOrDefault()?
                           .ChildNodes
                           .Where(node => (node.HasClass("article__container")
                           || node.Name.Equals("h1")
                           || node.Name.Equals("figure")
                           || node.Name.Equals("p"))
                                   && !node.HasClass("article__reference")
                                   && !node.Name.Equals("script")
                                   && node.Attributes["style"] == null)
                           .Select(node => node.InnerText.Trim())
                           .Aggregate((i, j) => i + Environment.NewLine + j);

                        var text = articleText.Replace("&nbsp;", " ");

                        await _unitOfWork.Articles.UpdateArticleTextAsync(articleid, text);
                        await _unitOfWork.Commit();

                    }

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
