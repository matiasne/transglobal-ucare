using AutoMapper;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System.Collections;
using UCare.Domain.Users;
using UCare.Shared.Domain.ValueObjects;

namespace UCare.Infrastructure.Firebase.Users
{
    public class UsuarioAfiliadoRepository : RepositoryBase, IUsuarioAfiliadoRepository
    {
        public const string ComunicacionCollection = "Comunicaciones";
        public UsuarioAfiliadoRepository(FirestoreDb firestoreDb, Mapper mapper) : base(firestoreDb, mapper, "User")
        {
        }

        public async Task<UsuarioAfiliado?> GetByUserName(string userName)
        {
            var result = await GetCollection().Limit(1).WhereEqualTo("UsuarioNombre", userName).WhereEqualTo("Rol", Roles.Afiliado).WhereNotEqualTo("Estado", Estados.Borrado).GetSnapshotAsync();

            if (result.Count > 0)
            {
                var users = result.ConvertTo<UsuarioAfiliado, UsuarioAfiliadoFirebase>(mapper);
                return users.FirstOrDefault(u => u.Rol == Roles.Afiliado);
            }

            return null;
        }

        public async Task<UsuarioAfiliado?> GetByUserEmail(string userEmail)
        {
            var result = await GetCollection().Limit(1).WhereEqualTo("Email", userEmail).WhereEqualTo("Rol", Roles.Afiliado).WhereNotEqualTo("Estado", Estados.Borrado).GetSnapshotAsync();
            if (result.Count > 0)
            {
                var users = result.ConvertTo<UsuarioAfiliado, UsuarioAfiliadoFirebase>(mapper);
                return users.FirstOrDefault(u => u.Rol == Roles.Afiliado);
            }

            return null;
        }

        public async Task<UsuarioAfiliado> GetById(string? id)
        {
            var doc = await GetId(id!).GetSnapshotAsync();
            return mapper.Map<UsuarioAfiliado>(doc.ConvertTo<UsuarioAfiliadoFirebase>());
        }

        public async Task<UsuarioAfiliado> Insert(UsuarioAfiliado entity)
        {
            var model = mapper.Map<UsuarioAfiliadoFirebase>(entity);
            model.FechaNacimiento = model.FechaNacimiento.ToUniversalTime();
            if (model == null)
            {
                throw new Exception();
            }
            var result = await GetCollection().AddAsync(model);

            entity.Id = result.Id;
            return entity;
        }

        public async Task Delete(string? id)
        {
            throw new NotImplementedException();
            //_ = await GetId(id!).DeleteAsync();
        }

        public async Task<bool> Update(UsuarioAfiliado model)
        {
            var modelFirebase = mapper.Map<UsuarioAfiliadoFirebase>(model);

            var change = new Dictionary<string, object> {
                { "Direccion",modelFirebase.Direccion },
                { "FechaNacimiento",modelFirebase.FechaNacimiento },
                { "Lenguaje",modelFirebase.Lenguaje },
                { "Nosocomio",modelFirebase.Nosocomio },
                { "NumeroIdentidad",modelFirebase.NumeroIdentidad },
                { "Sexo",modelFirebase.Sexo },
                { "UsuarioNombre",modelFirebase.UsuarioNombre },
                { "Estado",modelFirebase.Estado },
            };

            await GetCollection().Document(model.Id).UpdateAsync(change);
            return true;
        }

        public async Task<Shared.Infrastructure.IPaging> Get(Shared.Infrastructure.IPaging paging)
        {
            var query = GetCollection().WhereEqualTo("Rol", Roles.Afiliado);

            var filter = paging.Filter.Find(x => x.Property == "Estado" && x.Condition == "x");
            if (filter != null)
            {
                var list = new List<string>();
                foreach (var item in filter.GetValue() as IEnumerable)
                {
                    list.Add(item.ToString());
                }
                if (list.Count == 1)
                    paging.Filter.Add(new GQ.Data.Abstractions.Paging.PagingFilter { Condition = "=", Property = "Estado", Value = list.First() });
                else
                    paging.Filter.Add(new GQ.Data.Abstractions.Paging.PagingFilter { Condition = "in", Property = "Estado", Value = list.ToArray() });
            }

            if (paging.Filter.Any(x => x.Property == "Afiliacion.Empresa" && x.Condition == "!="))
            {
                paging.Order.Insert(0, new GQ.Data.Abstractions.Paging.PagingOrder { Property = "Afiliacion.Empresa", Direction = "+" });
            }

            var edad = GetValue(paging.Filter.FirstOrDefault(x => x.Property.ToLower() == "edad")?.Value?.ToString() ?? "") ?? new EdadMinMax();
            int edadMin = edad.Min;
            int edadMax = edad.Max;

            var now = DateTime.UtcNow;
            var hasta = now.AddYears(edadMin * -1).AddDays(-1);
            var desde = now.AddYears(edadMax * -1).AddDays(1);

            if (edadMax > 0)
                query = query.WhereGreaterThanOrEqualTo("FechaNacimiento", desde);
            if (edadMin > 0)
                query = query.WhereLessThanOrEqualTo("FechaNacimiento", hasta);


            await paging.Apply<UsuarioAfiliadoFirebase>(query, mapper);
            return paging;
        }


        private EdadMinMax GetValue(string values)
        {
            return JsonConvert.DeserializeObject<EdadMinMax>(values);
        }

        public class EdadMinMax
        {
            public int Min { get; set; } = -1;
            public int Max { get; set; } = -1;
        }
        public async Task<bool> UpdateCelular(UsuarioAfiliado model)
        {
            var modelFirebase = mapper.Map<UsuarioAfiliadoFirebase>(model);

            var change = new Dictionary<string, object> {
                { "Celular",modelFirebase.Celular! },
                { "CodigoPais",modelFirebase.CodigoPais! },
                { "VerificaTelefono",modelFirebase.VerificaTelefono! },
                { "Modificacion",modelFirebase.Modificacion! },
            };

            await GetCollection().Document(model.Id).UpdateAsync(change);
            return true;
        }

        public async Task<bool> UpdateContactos(UsuarioAfiliado model)
        {
            var modelFirebase = mapper.Map<UsuarioAfiliadoFirebase>(model);

            var change = new Dictionary<string, object> {
                { "Contactos",modelFirebase.Contactos! },
                { "Modificacion",modelFirebase.Modificacion! },
            };

            await GetCollection().Document(model.Id).UpdateAsync(change);
            return true;
        }

        public async Task<bool> UpdateDatosPersonales(UsuarioAfiliado model)
        {
            var modelFirebase = mapper.Map<UsuarioAfiliadoFirebase>(model);

            var change = new Dictionary<string, object> {
                { "UsuarioNombre",modelFirebase.UsuarioNombre! },
                { "FechaNacimiento",modelFirebase.FechaNacimiento!.ToUniversalTime() },
                { "Sexo",modelFirebase.Sexo! },
                { "Email",modelFirebase.Email! },
                { "Direccion",modelFirebase.Direccion! },
                { "Nosocomio",modelFirebase.Nosocomio! },
                { "Position",modelFirebase.Position! },
                { "Modificacion",modelFirebase.Modificacion! },
            };

            await GetCollection().Document(model.Id).UpdateAsync(change);
            return true;
        }

        public async Task<bool> UpdatePatologias(UsuarioAfiliado model)
        {
            var modelFirebase = mapper.Map<UsuarioAfiliadoFirebase>(model);

            var change = new Dictionary<string, object> {
                { "Nosocomio",modelFirebase.Nosocomio! },
                { "Patologias",modelFirebase.Patologias! },
                { "Medicacion",modelFirebase.Medicacion! },
                { "Alergias",modelFirebase.Alergias! },
                { "Modificacion",modelFirebase.Modificacion! },
            };

            await GetCollection().Document(model.Id).UpdateAsync(change);
            return true;
        }

        public async Task<List<string>> GetBySexoAndEdadAndCodigoPostal(string sexo, int edadMinima, int edadMaxima, List<string>? codigoPostal)
        {
            var resultData = new List<string>();
            var query = GetCollection().WhereEqualTo("Rol", Roles.Afiliado);

            //Afiliados por código postal
            if (codigoPostal != null && codigoPostal.Count > 0)
                query = query.WhereIn("Direccion.CodigoPostal", codigoPostal.ToArray());

            //query = query.WhereNotIn("Estado", Estados.Borrado);

            if (!string.IsNullOrWhiteSpace(sexo) && sexo.ToUpper() != "T")
                query = query.WhereEqualTo("Sexo", sexo);

            var now = DateTime.UtcNow;
            var hasta = now.AddYears(edadMinima * -1).AddDays(-1);
            var desde = now.AddYears(edadMaxima * -1).AddDays(1);

            if (edadMaxima > 0)
                query = query.WhereGreaterThanOrEqualTo("FechaNacimiento", desde);
            if (edadMinima > 0)
                query = query.WhereLessThanOrEqualTo("FechaNacimiento", hasta);

            var result = await query.OrderBy("FechaNacimiento").Select("FechaNacimiento", "Estado").GetSnapshotAsync();
            foreach (var item in result)
            {
                if (item.GetValue<string>("Estado") != Estados.Borrado)
                    resultData.Add(item.Id);
            }
            return resultData;
        }

        public async Task<UsuarioAfiliado?> GetByUserNumerodeIdentidad(string numeroIdentidad)
        {
            var result = await GetCollection().Limit(1).WhereEqualTo("NumeroIdentidad", numeroIdentidad).WhereNotEqualTo("Estado", Estados.Borrado).GetSnapshotAsync();

            if (result.Count == 1)
                return mapper.Map<UsuarioAfiliado>(result[0].ConvertTo<UsuarioAfiliadoFirebase>());
            return null;
        }

        public async Task<UsuarioAfiliado?> GetByUserPhoneNumber(string phoneNumber)
        {
            phoneNumber = phoneNumber.StartsWith("+") ? phoneNumber : $"+{phoneNumber}";
            var result = await GetCollection().Limit(1).WhereEqualTo("Celular", phoneNumber).WhereNotEqualTo("Estado", Estados.Borrado).GetSnapshotAsync();
            //var result = await GetCollection().Limit(1).WhereEqualTo("Celular", phoneNumber).WhereNotIn("Estado", new string[] { Estados.Borrado, Estados.Desactivo }).GetSnapshotAsync();
            if (result.Count == 1)
                return mapper.Map<UsuarioAfiliado>(result[0].ConvertTo<UsuarioAfiliadoFirebase>());
            return null;
        }

        public async Task<bool> UpdateEstado(UsuarioAfiliado model)
        {
            var modelFirebase = mapper.Map<UsuarioAfiliadoFirebase>(model);

            var change = new Dictionary<string, object> {
                { "Estado",modelFirebase.Estado! },
                { "Modificacion",modelFirebase.Modificacion! },
            };

            await GetCollection().Document(model.Id).UpdateAsync(change);
            return true;
        }

        public async Task<bool> UpdateNumeroIdentidad(UsuarioAfiliado model)
        {
            var modelFirebase = mapper.Map<UsuarioAfiliadoFirebase>(model);

            var change = new Dictionary<string, object> {
                { "NumeroIdentidad",modelFirebase.NumeroIdentidad! },
                { "Modificacion",modelFirebase.Modificacion! },
            };

            await GetCollection().Document(model.Id).UpdateAsync(change);
            return true;
        }

        public async Task<bool> UpdatePassword(UsuarioAfiliado model)
        {
            var modelFirebase = mapper.Map<UsuarioAfiliadoFirebase>(model);

            var change = new Dictionary<string, object> {
                { "Password",modelFirebase.Password! },
                { "Salt",modelFirebase.Salt! },
                { "Modificacion",modelFirebase.Modificacion! },
            };

            await GetCollection().Document(model.Id).UpdateAsync(change);
            return true;
        }

        public async Task<List<ComunicacionAfiliado>> GetComunicacionesByUser(string usuarioId)
        {
            return (await GetId(usuarioId).Collection(ComunicacionCollection).WhereNotEqualTo("Estado", UCare.Shared.Domain.ValueObjects.Estados.Borrado).OrderBy("Estado").OrderBy("Fecha").GetSnapshotAsync()).ConvertTo<ComunicacionAfiliado, ComunicacionAfiliadoFirebase>(mapper);
        }

        public async Task<int> GetComunicacionesByUserCount(string usuarioId)
        {
            return (await GetId(usuarioId).Collection(ComunicacionCollection).WhereEqualTo("Estado", UCare.Shared.Domain.ValueObjects.Estados.NoLeido).GetSnapshotAsync())?.Count ?? 0;
        }

        public async Task<bool> ComunicadoLeido(string usuarioId, string comunicacionId)
        {
            await GetId(usuarioId).Collection(ComunicacionCollection).Document(comunicacionId).UpdateAsync(new Dictionary<string, object> { { "Estado", UCare.Shared.Domain.ValueObjects.Estados.Leido } });
            return true;
        }

        public async Task<bool> ComunicadoDelete(string usuarioId, string comunicacionId)
        {
            await GetId(usuarioId).Collection(ComunicacionCollection).Document(comunicacionId).UpdateAsync(new Dictionary<string, object> { { "Estado", UCare.Shared.Domain.ValueObjects.Estados.Borrado } });
            return true;
        }

        public async Task<string> ComunicadoAdd(string id, ComunicacionAfiliado comunicado)
        {
            var modelFirebase = mapper.Map<ComunicacionAfiliadoFirebase>(comunicado);
            var result = await GetId(id).Collection(ComunicacionCollection).AddAsync(modelFirebase);
            return result.Id.ToString();
        }

        public async Task<ComunicacionAfiliado> GetComunicacionesByUser(string usuarioId, string id)
        {
            var entity = (await GetId(usuarioId).Collection(ComunicacionCollection).Document(id).GetSnapshotAsync()).ConvertTo<ComunicacionAfiliadoFirebase>();
            return mapper.Map<ComunicacionAfiliado>(entity);
        }

        public async Task<long> GetCountActivos()
        {
            return (await GetCollection().WhereEqualTo("Estado", Estados.Activo).WhereEqualTo("Rol", Roles.Afiliado).GetSnapshotAsync()).Count;
        }

        public async Task<bool> UpdateToken(UsuarioAfiliado model)
        {
            var modelFirebase = mapper.Map<UsuarioAfiliadoFirebase>(model);

            var change = new Dictionary<string, object> {
                { "Token",modelFirebase.Token! },
            };
            await GetCollection().Document(model.Id).UpdateAsync(change);
            return true;
        }
    }
}
