using GQ.Security.Model;

namespace UCare.Shared.Domain.Auth
{
    public class AuthUser : ISecurityUser
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? TenantId { get; set; }
        public string? CodeSession { get; set; }
        public string GetId()
        {
            return Id.ToString();
        }

        public string GetName()
        {
            return Name;
        }
    }
}
