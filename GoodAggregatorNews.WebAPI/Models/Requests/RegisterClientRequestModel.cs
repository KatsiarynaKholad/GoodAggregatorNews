namespace GoodAggregatorNews.WebAPI.Models.Requests
{
    public class RegisterClientRequestModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ConfirmationPassword { get; set; }    
    }
}
