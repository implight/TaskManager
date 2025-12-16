
namespace TaskManager.Infrastructure.Options
{
    public sealed class DatabaseOptions
    {
        public String ConnectionStringTemplate { get; init; }
        public string Host { get; init; } = default!;
        public int Port { get; init; }
        public string Database { get; init; } = default!;
        public string Username { get; init; } = default!;
        public string Password { get; init; } = default!;

        public String GetConnectionString()
        {
            var cn = ConnectionStringTemplate
                .Replace("{HOST}", Host)
                .Replace("{PORT}", Port.ToString())
                .Replace("{USER}", Username)
                .Replace("{PWD}", Password)
                .Replace("{DB}", Database);

            return cn;
        }
    }
}
