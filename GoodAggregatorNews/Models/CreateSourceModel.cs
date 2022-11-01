using GoodAggregatorNews.Core;
using System.ComponentModel.DataAnnotations;

namespace GoodAggregatorNews.Models
{
    public class CreateSourceModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }

        public SourceType SourceType { get; set; }
    }
}
