namespace GestaoConfeitaria.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DataExclusao { get; set; } = DateTime.UtcNow;

        // Construtor protegido para EF Core
        protected User() { }

        // Construtor para uso na aplicação
        public User(string username, string passwordHash, string role = "User")
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username é obrigatório.", nameof(username));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("PasswordHash é obrigatório.", nameof(passwordHash));

            Username = username.Trim().ToLower();
            PasswordHash = passwordHash;
            Role = role;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
