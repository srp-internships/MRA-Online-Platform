namespace Application.Account.DTO
{
    public record TokenDTO
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public bool IsPasswordChanged { get; set; }
    }
}
