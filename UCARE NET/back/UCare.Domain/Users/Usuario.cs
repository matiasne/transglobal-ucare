using GQ.Architecture.DDD.ValueObject;
using GQ.Core.utils;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using UCare.Domain.Users.Events;
using UCare.Shared.Domain;

namespace UCare.Domain.Users
{
    public abstract class Usuario : EntityBase<string?>
    {
        [Required]
        public virtual string? UsuarioNombre { get; set; }
        [Required]
        [EmailAddress]
        public virtual string? Email { get; set; }
        [Required]
        public virtual string? Password { get; set; }
        public virtual string? Salt { get; set; }
        [Required]
        public virtual string Rol { get; set; } = string.Empty;


        public bool CambiarPassword(string clave)
        {
            var salt = GQ.Core.encriptacion.Encript.GetRandomSalt();
            var hashing = new GQ.Core.encriptacion.MD5Crypto.Hashing();
            var claveEncript = hashing.HashPassword(clave, salt);

            this.Password = claveEncript;
            this.Salt = salt;

            return true;
        }

        public bool ValidatePassword(string password)
        {
            var hashing = new GQ.Core.encriptacion.MD5Crypto.Hashing();
            return hashing.ValidatePassword(password, Salt, Password);
        }

        public void ValidateException()
        {
            if (!Validate())
            {
                throw new GQ.Architecture.DDD.Domain.Exceptions.ValidationException(ValidateDetails());
            }
        }

        public UsuarioAuth RecuperarPassword(string signature)
        {
            var ua = new UsuarioAuth
            {
                IdKey = $"{Id}|{Guid.NewGuid()}",
                UsuarioId = Id,
                Code = FunctionsUtils.CreateRandomCode(4, FunctionsUtils.PasswordCharaters.Numeros),
                Expiration = DateTime.UtcNow.AddMinutes(15)
            };
            this.Record(new RecuperarPasswordEvent(ua.IdKey, Rol, UsuarioNombre, ua.Code, ua.Expiration, signature));
            return ua;
        }
    }

    public class UsuarioAuth : EntityBase<string>
    {
        public string IdKey { get; set; }
        public string UsuarioId { get; set; }
        public string Code { get; set; }
        public DateTime Expiration { get; set; }
    }
}
