namespace GoodAggregatorNews.Core.DataTransferObject
{
    public class ClientDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }    //???
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegistationDate { get; set; }      //???
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
    }
}