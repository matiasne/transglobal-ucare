using AutoMapper;
using Google.Cloud.Firestore;
using UCare.Domain.Comunicaciones;
using UCare.Shared.Domain.ValueObjects;
using UCare.Shared.Infrastructure;

namespace UCare.Infrastructure.Firebase.Comunicaciones
{
    public class ComunicacionRepository : RepositoryBase, IComunicacionRepository
    {
        public const string ComunicacionesEnviadas = "ComunicacionesEnviadas";
        public ComunicacionRepository(FirestoreDb firestoreDb, Mapper mapper) : base(firestoreDb, mapper, "Comunicaciones")
        {
        }

        public async Task Delete(string? id)
        {
            _ = await GetId(id!).DeleteAsync();
        }

        public async Task<IPaging> Get(IPaging paging)
        {
            await paging.Apply<ComunicacionFirebase>(GetCollection().WhereIn("Estado", new string[] { Estados.Activo, Estados.Desactivo }), mapper);
            return paging;
        }

        public async Task<Comunicacion> GetById(string? id)
        {
            var doc = await GetId(id!).GetSnapshotAsync();
            return mapper.Map<Comunicacion>(doc.ConvertTo<ComunicacionFirebase>());
        }

        public async Task<List<Comunicacion>> GetPandientes()
        {
            var result = (await GetCollection()
                .WhereEqualTo("Estado", Estados.Activo)
                .WhereEqualTo("Enviado", false)
                .WhereLessThan("FechaEnvio", DateTime.UtcNow)
                .GetSnapshotAsync()).ConvertTo<Comunicacion, ComunicacionFirebase>(mapper);
            return result;
        }

        public async Task<Comunicacion> Insert(Comunicacion entity)
        {
            var model = mapper.Map<ComunicacionFirebase>(entity);
            if (model == null)
            {
                throw new Exception();
            }
            var result = await GetCollection().AddAsync(model);

            entity.Id = result.Id;
            return entity;
        }

        public async Task<bool> UpadeteEstado(Comunicacion comunicacion, List<ComunicacionEnvio> envios)
        {
            var entity = mapper.Map<ComunicacionFirebase>(comunicacion);

            if (entity == null)
                throw new Exception();

            await GetCollection().Document(comunicacion.Id).UpdateAsync(
                new Dictionary<string, object> {
                    {"Estado",entity.Estado },
                    {"DetalleEnvio",entity.DetalleEnvio! },
                    {"Enviado",entity.Enviado },
                });
            foreach (var enviado in envios)
            {
                var enviadoFb = mapper.Map<ComunicacionEnvioFirebase>(enviado);
                await GetCollection().Document(comunicacion.Id).Collection(ComunicacionesEnviadas).AddAsync(enviadoFb);
            }

            return true;
        }

        public async Task<bool> Update(Comunicacion model)
        {
            var entity = mapper.Map<ComunicacionFirebase>(model);

            if (entity == null)
                throw new Exception();

            await GetCollection().Document(model.Id).UpdateAsync(GetToUpdate(entity));
            return true;
        }

        public async Task<bool> UpdateEstadoComunicacion(string id, string idAfiliado, string idAfiliadoComunicado, string estado)
        {
            var result = await GetCollection().Document(id).Collection(ComunicacionesEnviadas).WhereEqualTo("AfiliadoId", idAfiliado).WhereEqualTo("ComunicadoId", idAfiliadoComunicado).GetSnapshotAsync();
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    await item.Reference.UpdateAsync(new Dictionary<string, object> { { "Estado", estado } });
                }
            }
            return true;
        }
    }
}
