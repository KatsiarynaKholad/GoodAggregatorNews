using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace GoodAggregatorNews.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        //[Remote("Checkemail", "Account",
        //HttpMethod = WebRequestMethods.Http.Post)]
        //, ErrorMessage = "Email is already exists")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
