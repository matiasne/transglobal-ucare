namespace UCare.Shared.Domain.Auth
{
    public interface IUniqueSession
    {
        public void AddUser(AuthUser user);
        public void RemoveUser(AuthUser user);
        public bool CheckUser(AuthUser user);
    }
}
