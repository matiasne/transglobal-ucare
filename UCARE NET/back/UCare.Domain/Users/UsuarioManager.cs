using GQ.Data.Abstractions.Entity;
using System.ComponentModel.DataAnnotations;
using UCare.Domain.Alertas;
using UCare.Shared.Domain;
using UCare.Shared.Domain.ValueObjects;

namespace UCare.Domain.Users
{
    public class UsuarioManager : Usuario
    {
        public virtual List<string>? CodigoPostal { get; set; }
        public virtual string? UsuarioId { get; set; } // Usuario a quien pertenece

        public virtual MapaConfig Mapa { get; set; } = new MapaConfig();
        public void ModificarUsuario(UsuarioManager model, Modificacion modificacion)
        {
            UsuarioNombre = model.UsuarioNombre;
            Email = model.Email;
            CodigoPostal = model.CodigoPostal;
            Modificacion = modificacion;
            Estado = model.Estado;

            if (!string.IsNullOrWhiteSpace(model.Password))
                CambiarPassword(model.Password);

        }

        public static UsuarioManager CreateUsuario(UsuarioManager model, Modificacion modificacion)
        {
            var user = new UsuarioManager
            {
                UsuarioNombre = model.UsuarioNombre,
                Email = model.Email,
                Rol = model.Rol,
                Password = model.Password,
                CodigoPostal = model.CodigoPostal,
                UsuarioId = model.UsuarioId,
                Creado = modificacion,
                Modificacion = modificacion,
                Estado = Estados.Activo,

            };
            return user;
        }
        public static string[] GetRolFilter(string rol)
        {
            switch (rol)
            {
                case Roles.Propietario:
                    return new string[] { Roles.Gerente, Roles.Administrador, Roles.Monitor, Roles.Verificador };
                case Roles.Gerente:
                    return new string[] { Roles.Administrador, Roles.Monitor, Roles.Verificador };
                case Roles.Administrador:
                    return new string[] { Roles.Monitor, Roles.Verificador };
            }
            return new string[] { };
        }

        public static string[] GetRolAdd(string rol)
        {
            switch (rol)
            {
                case Roles.Propietario:
                    return new string[] { Roles.Gerente };
                case Roles.Gerente:
                    return new string[] { Roles.Administrador };
                case Roles.Administrador:
                    return new string[] { Roles.Monitor, Roles.Verificador };
            }
            return new string[] { };
        }

        public static UsuarioManager CreateUsuarioPropietario()
        {
            Modificacion modificacion = new Modificacion { UsuarioId = null, Modificado = DateTime.UtcNow };

            var user = new UsuarioManager
            {
                UsuarioNombre = "AdminUCare",
                Email = "administracion@ucare.com",
                Creado = modificacion,
                Modificacion = modificacion,
                Rol = Roles.Propietario,
                Estado = Estados.Activo,
            };
            user.CambiarPassword("Admin1234");
            return user;
        }

    }

    public class MapaConfig : IEntity
    {
        [Range(9, 16)]
        public virtual int Zoom { get; set; } = 15;
        public virtual GeoPosition Center { get; set; } = new GeoPosition() { Lat = -31.4, Lon = -64.2 };
    }
}
