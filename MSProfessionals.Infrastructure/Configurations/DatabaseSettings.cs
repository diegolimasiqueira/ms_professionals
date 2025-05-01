namespace MSProfessionals.Infrastructure.Configurations
{
    public class DatabaseSettings
    {
        public string Host { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public int MaxRetryCount { get; set; }
        public int CommandTimeout { get; set; }

        public string GetConnectionString()
        {
            return $"Host={Host};Database={Database};Username={Username};Password={Password};Port={Port};SSL Mode={(UseSsl ? "Require" : "Prefer")};";
        }
    }
} 