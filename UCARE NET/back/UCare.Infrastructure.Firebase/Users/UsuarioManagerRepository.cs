using AutoMapper;
using Google.Cloud.Firestore;
using UCare.Domain.Users;
using UCare.Shared.Domain.ValueObjects;
using UCare.Shared.Infrastructure;

namespace UCare.Infrastructure.Firebase.Users
{
    public class UsuarioManagerRepository : RepositoryBase, IUsuarioManagerRepository
    {
        public UsuarioManagerRepository(FirestoreDb firestoreDb, Mapper mapper) : base(firestoreDb, mapper, "User")
        {
        }

        public async Task<List<UsuarioManager>> GetAllUserManager()
        {
            return (await GetCollection().WhereNotEqualTo("Rol", Roles.Afiliado).GetSnapshotAsync()).ConvertTo<UsuarioManager, UsuarioManagerFirebase>(mapper);
        }

        public async Task<UsuarioManager> GetById(string? id)
        {
            var doc = await GetId(id!).GetSnapshotAsync();
            return mapper.Map<UsuarioManager>(doc.ConvertTo<UsuarioManagerFirebase>());
        }

        public async Task<UsuarioManager> Insert(UsuarioManager entity)
        {
            var model = mapper.Map<UsuarioManagerFirebase>(entity);
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
            _ = await GetId(id!).DeleteAsync();
        }

        public async Task<List<UsuarioManager>> GetByRol(string rol)
        {
            var query = GetCollection().WhereEqualTo("Rol", rol).WhereEqualTo("Estado", Estados.Activo);
            var result = await query.GetSnapshotAsync();
            return result.ConvertTo<UsuarioManager, UsuarioManagerFirebase>(mapper);
        }

        public async Task<UsuarioManager?> GetByUserName(string userName)
        {
            var result = await GetCollection().WhereEqualTo("UsuarioNombre", userName).WhereEqualTo("Estado", Estados.Activo).GetSnapshotAsync();
            if (result.Count > 0)
            {
                var users = result.ConvertTo<UsuarioManager, UsuarioManagerFirebase>(mapper);
                return users.FirstOrDefault(u => u.Rol != Roles.Afiliado);
            }

            return null;
        }

        public async Task<UsuarioManager?> GetByUserEmail(string userEmail)
        {
            var result = await GetCollection().WhereEqualTo("Email", userEmail).WhereEqualTo("Estado", Estados.Activo).GetSnapshotAsync();
            if (result.Count > 0)
            {
                var users = result.ConvertTo<UsuarioManager, UsuarioManagerFirebase>(mapper);
                return users.FirstOrDefault(u => u.Rol != Roles.Afiliado);
            }

            return null;
        }

        public async Task<IPaging> Get(IPaging paging)
        {
            if (paging.Order.Count > 0)
            {
                paging.Order.Insert(0, new GQ.Data.Abstractions.Paging.PagingOrder { Property = "Estado", Direction = "+" });
            }

            if (paging.Filter.Any(x => x.Property == "UsuarioId") && paging.Order.Any(x => x.Property == "UsuarioId"))
            {
                paging.Order = paging.Order.Where(x => x.Property != "UsuarioId").ToList();
            }

            if (paging.Filter.Any(x => x.Property == "Rol" && x.Condition == "in") && paging.Order.Any(x => x.Property == "Rol"))
            {
                paging.Order = paging.Order.Where(x => x.Property != "Rol").ToList();
            }

            await paging.Apply<UsuarioManagerFirebase>(GetCollection().WhereNotEqualTo("Estado", Estados.Borrado), mapper);
            return paging;
        }

        public async Task<bool> Update(UsuarioManager model)
        {
            var modelFirebase = mapper.Map<UsuarioManagerFirebase>(model);

            var change = new Dictionary<string, object> {
                { "UsuarioNombre",modelFirebase.UsuarioNombre! },
                { "Email",modelFirebase.Email! },
                { "Estado",modelFirebase.Estado },
                { "Modificacion",modelFirebase.Modificacion },
                { "CodigoPostal", modelFirebase.CodigoPostal },
            };

            if (!string.IsNullOrWhiteSpace(modelFirebase.Password))
            {
                change.Add("Password", modelFirebase.Password!);
                change.Add("Salt", modelFirebase.Salt!);
            }

            await GetCollection().Document(model.Id).UpdateAsync(change);
            return true;
        }

        public async Task<List<UsuarioManager>> GetUsersByUserId(string id)
        {
            var result = await GetCollection().WhereEqualTo("UsuarioId", id).WhereNotEqualTo("Estado", Estados.Borrado).GetSnapshotAsync();
            return result.ConvertTo<UsuarioManager, UsuarioManagerFirebase>(mapper);
        }

        public async Task<List<UsuarioManager>> GetUsersByIdRol(string id)
        {
            var entity = await GetById(id);
            var result = await GetCollection().WhereEqualTo("Rol", entity.Rol).WhereEqualTo("Estado", Estados.Activo).GetSnapshotAsync();
            return result.ConvertTo<UsuarioManager, UsuarioManagerFirebase>(mapper).Where(x => x.Id != id).ToList();
        }

        public async Task<bool> UpdateUsuarioId(UsuarioManager entity)
        {
            await GetCollection().Document(entity.Id).UpdateAsync(
                new Dictionary<string, object> {
                        { "UsuarioId",entity.UsuarioId! }
                });
            return true;
        }

        public async Task<bool> UpdateMapaConfig(string id, MapaConfig entity)
        {
            var model = mapper.Map<MapaConfigFirebase>(entity);
            model.Zoom = entity.Zoom < 9 ? 9 : (entity.Zoom > 16 ? 16 : entity.Zoom);
            await GetCollection().Document(id).UpdateAsync(
              new Dictionary<string, object> {
                        { "Mapa",model! }
              });
            return true;
        }
        public async Task<bool> InsertAu(UsuarioAuth au)
        {
            var modelFirebase = mapper.Map<UsuarioAuthFirebase>(au);
            await GetCollection().Document(au.UsuarioId).Collection("UsuarioAuth").AddAsync(modelFirebase);
            return true;
        }

        public async Task<List<UsuarioAuth>> GetAuthByUser(string id)
        {
            var result = await GetCollection().Document(id).Collection("UsuarioAuth")
                .WhereGreaterThanOrEqualTo("Expiration", DateTime.UtcNow).GetSnapshotAsync();
            return result.ConvertTo<UsuarioAuth, UsuarioAuthFirebase>(mapper);
        }

        public async Task<bool> RemoveAuthByUser(string id, string authId)
        {
            var result = await GetCollection().Document(id).Collection("UsuarioAuth").WhereEqualTo("IdKey", authId).GetSnapshotAsync();
            foreach (var item in result)
            {
                await item.Reference.DeleteAsync();
            }
            result = await GetCollection().Document(id).Collection("UsuarioAuth")
                .WhereLessThan("Expiration", DateTime.UtcNow).GetSnapshotAsync();
            foreach (var item in result)
            {
                await item.Reference.DeleteAsync();
            }
            return true;
        }

        public async Task<bool> RegistrarHistorial(string id, UsuarioManagerHistorial entity)
        {
            var modelFirebase = mapper.Map<UsuarioManagerHistorialFirebase>(entity);
            await GetCollection().Document(id).Collection("Historia").AddAsync(modelFirebase);
            return true;
        }

        public async Task<bool> UpdatePassword(UsuarioManager entity)
        {
            await GetCollection().Document(entity.Id).UpdateAsync(
                new Dictionary<string, object> {
                        { "Password",entity.Password! },
                        { "Salt",entity.Salt! }
                });

            return true;
        }
    }
}
