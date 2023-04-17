using UCare.Shared.Domain.Auth;

namespace UCare.Web.Base
{
    /// <summary>
    /// 
    /// </summary>
    public class CheckWebUniqueSession : IUniqueSession
    {
        /// <summary>
        /// Mantenemos las sesiones
        /// </summary>
        private readonly Dictionary<string, string> ValidationCodes = new Dictionary<string, string>();

        /// <summary>
        /// Agregamos al usuario
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(AuthUser user)
        {
            if (ValidationCodes.ContainsKey(user.Id))
                ValidationCodes.Remove(user.Id);
            ValidationCodes.Add(user.Id, user.CodeSession!);
        }

        /// <summary>
        /// Chequeamos el codigo del usuario si es el mismo
        /// </summary>
        /// <param name="user"></param>

        public bool CheckUser(AuthUser user)
        {
            return user != null && ValidationCodes.ContainsKey(user.Id) && ValidationCodes[user.Id] == user.CodeSession;
        }

        /// <summary>
        /// Removemos el codigo del usuario
        /// </summary>
        /// <param name="user"></param>
        public void RemoveUser(AuthUser user)
        {
            if (CheckUser(user))
                ValidationCodes.Remove(user.Id);
        }
    }
}
