using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Core.DataTransferObject
{
    public class ArticleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? FullText { get; set; }
        public double? Rate { get; set; }

        public string SourceUrl { get; set; }
        public string? Category { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}",
              ApplyFormatInEditMode = true)]
        public DateTime PublicationDate { get; set; }
        public Guid SourceId { get; set; }
    }
}
