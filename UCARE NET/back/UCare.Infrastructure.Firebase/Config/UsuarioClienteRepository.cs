using AutoMapper;
using Google.Cloud.Firestore;
using UCare.Domain.Config;
using UCare.Shared.Infrastructure;

namespace UCare.Infrastructure.Firebase.Config
{
    public class ConfigRepository : RepositoryBase, IConfigRepository
    {
        public ConfigRepository(FirestoreDb firestoreDb, Mapper mapper) : base(firestoreDb, mapper, "Config")
        {
        }

        public Task Delete(string? id)
        {
            throw new NotImplementedException();
        }

        public Task<IPaging> Get(IPaging paging)
        {
            throw new NotImplementedException();
        }

        public async Task<Domain.Config.Config> GetById(string? id)
        {
            var doc = await GetCollection().Limit(1).GetSnapshotAsync();
            if (doc.Count > 0)
            {
                return mapper.Map<Domain.Config.Config>(doc[0].ConvertTo<ConfigFirebase>());
            }
            return null;
        }

        public async Task<Domain.Config.Config> Insert(Domain.Config.Config entity)
        {
            var model = mapper.Map<ConfigFirebase>(entity);
            if (model == null)
            {
                throw new Exception();
            }
            var result = await GetCollection().AddAsync(model);
            entity.Id = result.Id;
            return entity;
        }

        public async Task<bool> Update(Domain.Config.Config model)
        {
            var modification = new ModificacionFirebase { Modificado = model.Modificacion.Modificado, UsuarioId = model.Modificacion.UsuarioId };
            var objectUpdate = new Dictionary<string, object> {
                        { "Estado",model.Estado },
                        { "Modificacion",modification },
               };

            if (model.UsuarioActivosMaximos.HasValue)
                objectUpdate.Add("UsuarioActivosMaximos", model.UsuarioActivosMaximos);

            if (model.TiempoEnvioSMSSeconds.HasValue)
                objectUpdate.Add("TiempoEnvioSMSSeconds", model.TiempoEnvioSMSSeconds);

            if (model.ConfirmarTimeOut.HasValue)
                objectUpdate.Add("ConfirmarTimeOut", model.ConfirmarTimeOut);

            if (model.MonitorPausaTimeOut.HasValue)
                objectUpdate.Add("MonitorPausaTimeOut", model.MonitorPausaTimeOut);

            if (model.TiempoParaReasignarAlerta.HasValue)
                objectUpdate.Add("TiempoParaReasignarAlerta", model.TiempoParaReasignarAlerta);

            await GetCollection().Document(model.Id).UpdateAsync(objectUpdate);

            return true;
        }
    }
}
