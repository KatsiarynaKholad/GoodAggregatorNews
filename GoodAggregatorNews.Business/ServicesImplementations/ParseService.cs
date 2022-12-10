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

                    var htmlDoc = web.Load(articleSourceUrl);

                    var nodes =
                       htmlDoc.DocumentNode.Descendants(0)
                       .Where(n => n.HasClass("article__body"));

                    if (nodes.Any())
                    {
                        var articleText = (nodes.FirstOrDefault()?.ChildNodes
                           .Where(node => node.HasClass("article__container")))?
                           .FirstOrDefault()?.ChildNodes
                                .Where(node => node.Name.Equals("p")
                                    || node.Name.Equals("h2")
                                    || node.Name.Equals("h4")
                                    || node.Name.Equals("h3")
                                    || node.Name.Equals("ul")
                                    || node.Name.Equals("ol")
                                    || node.Name.Equals("li")
                                    || (node.Name.Equals("figure")
                                    && !node.HasClass("incut") 
                                    && !node.HasClass("global-incut")))
                                .Select(node => node.OuterHtml.Trim())
                                .Aggregate((i, j) => i + Environment.NewLine + j);


                        await _unitOfWork.Articles.UpdateArticleTextAsync(articleid, articleText);
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
