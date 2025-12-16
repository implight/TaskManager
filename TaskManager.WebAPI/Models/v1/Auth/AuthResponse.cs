namespace TaskManager.WebAPI.Models.v1.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }
}
