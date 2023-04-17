using GQ.Core.utils;
using GQ.Data.Abstractions.Entity;
using GQ.Data.Abstractions.Validators;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using UCare.Domain.Alertas;
using UCare.Domain.Comunicaciones;
using UCare.Domain.Users.Events;
using UCare.Shared.Domain.ValueObjects;

namespace UCare.Domain.Users
{
    public class UsuarioAfiliado : Usuario
    {
        public virtual string Signature { get; set; }
        public virtual string? Lenguaje { get; set; }
        public virtual string NumeroIdentidad { get; set; }
        public virtual DateTime FechaNacimiento { get; set; }
        public virtual string Sexo { get; set; }
        public virtual string Token { get; set; }
        [Required]
        public virtual Direccion Direccion { get; set; }
        public virtual GeoPosition? Position { get; set; } = new GeoPosition();
        [Phone]
        [Required]
        public virtual string Celular { get; set; }
        [Required]
        public virtual string CodigoPais { get; set; }
        public virtual string Nosocomio { get; set; }
        public virtual List<string> Patologias { get; set; }
        public virtual List<string> Medicacion { get; set; }
        public virtual List<string> Alergias { get; set; }
        public virtual List<Contacto> Contactos { get; set; }
        public virtual Afiliacion Afiliacion { get; set; }
        public virtual Verificacion VerificaEmail { get; set; } = new Verificacion();
        public virtual Verificacion VerificaTelefono { get; set; } = new Verificacion();

        public void CambarEstado(UsuarioAfiliado model, string idUser)
        {
            if (new string[] { Estados.Activo, Estados.Desactivo, Estados.Revision, Estados.Borrado }.Any(x => x == model.Estado))
            {
                var EstadoOld = Estado;
                Estado = (model.Estado ?? Estado).ToUpper();
                if (EstadoOld != Estado)
                {
                    Modificacion = new Shared.Domain.Modificacion { Modificado = DateTime.UtcNow, UsuarioId = idUser };
                    this.Record(new ChangeEstadoEvent(Id, Email, UsuarioNombre, EstadoOld, Estado));
                }
            }
            else
            {
                throw new Exception("Estado incorrecto");
            }
        }

        public void CambiarContactos(UsuarioAfiliado model)
        {
            Contactos = model.Contactos ?? Contactos;
            Modificacion = new Shared.Domain.Modificacion { Modificado = DateTime.UtcNow, UsuarioId = Id };
        }

        public void CambiarDatosPersonales(UsuarioAfiliado model)
        {
            UsuarioNombre = model?.UsuarioNombre ?? UsuarioNombre;
            FechaNacimiento = model?.FechaNacimiento.ToUniversalTime() ?? FechaNacimiento;
            Sexo = model?.Sexo ?? Sexo;
            Direccion = model?.Direccion ?? Direccion;
            Position = model?.Position ?? Position;
            Nosocomio = model?.Nosocomio ?? Nosocomio;
            Modificacion = new Shared.Domain.Modificacion { Modificado = DateTime.UtcNow, UsuarioId = Id };
        }

        public void CambiarEmail(UsuarioAfiliado model)
        {
            Email = model?.Email ?? Email;
            VerificaEmail = new Verificacion { Verificado = false, CodigoVerificacion = FunctionsUtils.CreateRandomCode(4, FunctionsUtils.PasswordCharaters.Numeros) };
            Modificacion = new Shared.Domain.Modificacion { Modificado = DateTime.UtcNow, UsuarioId = Id };
            this.Record(new ChangeEmailEvent(Guid.NewGuid().ToString(), $"{UsuarioNombre}", VerificaEmail.CodigoVerificacion, $"{Email}"));
        }

        public void CambiarNumeroIdentidad(string numeroIdentidad)
        {
            NumeroIdentidad = numeroIdentidad;
            Modificacion = new Shared.Domain.Modificacion { Modificado = DateTime.UtcNow, UsuarioId = Id };
        }

        public void CambiarPatologias(UsuarioAfiliado model)
        {
            Nosocomio = model.Nosocomio ?? Nosocomio;
            Patologias = model.Patologias ?? Patologias;
            Medicacion = model.Medicacion ?? Medicacion;
            Alergias = model.Alergias ?? Alergias;
            Modificacion = new Shared.Domain.Modificacion { Modificado = DateTime.UtcNow, UsuarioId = Id };
        }

        public void CambiarTelefono(string numero, string codigoPais)
        {
            Celular = numero;
            CodigoPais = codigoPais;

            CodigoPais = CodigoPais.StartsWith("+") ? CodigoPais : $"+{CodigoPais}";
            Celular = Celular.StartsWith("+") ? Celular : $"+{Celular}";

            VerificaTelefono = new Verificacion { Verificado = false, CodigoVerificacion = FunctionsUtils.CreateRandomCode(4, FunctionsUtils.PasswordCharaters.Numeros) };
            Modificacion = new Shared.Domain.Modificacion { Modificado = DateTime.UtcNow, UsuarioId = Id };
            this.Record(new ChangeTelefonoEvent(Guid.NewGuid().ToString(), $"{UsuarioNombre}", VerificaTelefono.CodigoVerificacion, $"{Celular}", Signature));
        }

        public ComunicacionAfiliado CreateComunicado(Comunicacion comunicacion)
        {
            var comunicado = new ComunicacionAfiliado
            {
                Estado = Estados.NoLeido,
                Fecha = DateTime.UtcNow,
                IdComunicado = comunicacion.Id!,
                Mensaje = comunicacion.Mensaje,
                Titulo = comunicacion.Titulo,
            };
            this.Record(new NuevaComunicacionEvent(Id, comunicacion.Id, comunicacion.Titulo, comunicacion.Mensaje));
            return comunicado;
        }
    }

    public class Verificacion : IEntity
    {
        public bool Verificado { get; set; } = false;
        public string CodigoVerificacion { get; set; } = string.Empty;
    }

    public class Direccion : IEntity
    {
        [Required]
        public virtual string? Calle { get; set; }
        [Required]
        public virtual string? Nro { get; set; }
        public virtual string? Piso { get; set; }
        public virtual string? Barrio { get; set; }
        [Required]
        public virtual string? Ciudad { get; set; }
        public virtual string? Departamento { get; set; }
        [Required]
        public virtual string? CodigoPostal { get; set; }

        public override string ToString()
        {
            return $"Calle: {Calle} nro: {Nro} {((Barrio ?? "") != "-" ? (Barrio ?? "") : "")} {((Piso ?? "") != "-" ? (Piso ?? "") : "")} {((Departamento ?? "") != "-" ? (Departamento ?? "") : "")} ciudad :{Ciudad} c.p.: {CodigoPostal}";
        }

        public bool Validate()
        {
            return ValidateUtils.TryValidateModel(this);
        }

        public IEnumerable<ValidationResult> ValidateDetails()
        {
            List<ValidationResult> list = new List<ValidationResult>();
            ValidateUtils.TryValidateModel(this, list);
            return list;
        }
    }
    public class Contacto : IEntity
    {
        public virtual string Nombre { get; set; }
        public virtual string Telefono { get; set; }
    }

    public class Afiliacion : IEntity
    {
        public virtual string Empresa { get; set; }
        public virtual string Servicio { get; set; }
    }

    public class ComunicacionAfiliado : EntityBase<string>
    {
        public string IdComunicado { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }

    }

}
