using AutoMapper;
using Google.Cloud.Firestore;
using System.Collections;
using UCare.Domain.Alertas;
using UCare.Shared.Infrastructure;

namespace UCare.Infrastructure.Firebase.Alertas
{
    public class AlertaRepository : RepositoryBase, IAlertaRepository
    {
        private const string CollectionHistorial = "AlertaHistorial";
        public AlertaRepository(FirestoreDb firestoreDb, Mapper mapper) : base(firestoreDb, mapper, "Alertas")
        {
        }

        public Task Delete(string? id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<Alerta>> GetPendientes()
        {
            var doc = await GetCollection()
                .WhereEqualTo("Cerrado", false)
                .WhereEqualTo("MonitorId", null)
                .OrderBy("Creado.Modificado").GetSnapshotAsync();
            return doc.ConvertTo<Alerta, AlertaFirebase>(mapper);
        }
        public async Task<List<Alerta>> GetPendientesAsignados()
        {
            var doc = await GetCollection()
                .WhereEqualTo("Cerrado", false)
                .WhereNotEqualTo("MonitorId", null)
                .OrderBy("MonitorId").OrderBy("Creado.Modificado").GetSnapshotAsync();
            return doc.ConvertTo<Alerta, AlertaFirebase>(mapper);
        }

        public async Task<List<Alerta>> GetPendientesAsignados(string id)
        {
            var doc = await GetCollection()
                .WhereEqualTo("Cerrado", false)
                .WhereEqualTo("MonitorId", id)
                .OrderByDescending("Creado.Modificado").Limit(10).GetSnapshotAsync();
            return doc.ConvertTo<Alerta, AlertaFirebase>(mapper);
        }

        public async Task<IPaging> Get(IPaging paging)
        {
            var filter = paging.Filter.Find(x => x.Property == "Estado" && x.Condition == "x");
            if (filter != null)
            {
                var list = new List<string>();
                foreach (var item in filter.GetValue() as IEnumerable)
                {
                    list.Add(item.ToString());
                }
                if (list.Count == 1)
                    paging.Filter.Add(new GQ.Data.Abstractions.Paging.PagingFilter { Condition = "=", Property = "Estado", Value = list.ToArray()[0] });
                if (list.Count > 1)
                    paging.Filter.Add(new GQ.Data.Abstractions.Paging.PagingFilter { Condition = "in", Property = "Estado", Value = list.ToArray() });
            }
            await paging.Apply<AlertaFirebase>(GetCollection(), mapper);
            return paging;
        }

        public async Task<Alerta> GetById(string? id)
        {
            var doc = await GetId(id!).GetSnapshotAsync();
            return mapper.Map<Alerta>(doc.ConvertTo<AlertaFirebase>());
        }

        public async Task<Alerta> Insert(Alerta entity)
        {
            var model = mapper.Map<AlertaFirebase>(entity);
            var modelHistory = mapper.Map<AlertaHistorialFirebase>(entity.GetHistorial());

            if (model == null)
            {
                throw new Exception();
            }

            var result = await GetCollection().AddAsync(model);

            entity.Id = result.Id;
            modelHistory.AlertId = entity.Id;

            result = await GetCollection().Document(result.Id).Collection(CollectionHistorial).AddAsync(modelHistory);

            return entity;
        }

        public Task<bool> Update(Alerta model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsignare(Alerta model)
        {
            var modelFirebase = mapper.Map<AlertaFirebase>(model);
            var modelHistory = mapper.Map<AlertaHistorialFirebase>(model.GetHistorial());

            var change = new Dictionary<string, object> {
                { "MonitorId",modelFirebase.MonitorId },
                { "Modificacion",modelFirebase!.Modificacion },
            };

            await GetCollection().Document(model.Id).UpdateAsync(change);
            await GetCollection().Document(model.Id).Collection(CollectionHistorial).AddAsync(modelHistory);
            return true;
        }

        public async Task<bool> UpdateBitacora(Alerta entity)
        {
            //var modelFirebase = mapper.Map<AlertaFirebase>(entity);
            //var modelHistory = mapper.Map<AlertaHistorialFirebase>(entity.GetHistorial());

            //var change = new Dictionary<string, object> {
            //    { "Bitacora",modelFirebase.Bitacora! },
            //    { "Modificacion",modelFirebase.Modificacion },
            //};

            //await GetCollection().Document(entity.Id).UpdateAsync(change);
            //await GetCollection().Document(entity.Id).Collection(CollectionHistorial).AddAsync(modelHistory);
            return true;
        }

        public async Task<bool> UpdatePosition(Alerta entity)
        {
            var modelFirebase = mapper.Map<AlertaFirebase>(entity);
            var modelHistory = mapper.Map<AlertaHistorialFirebase>(entity.GetHistorial());

            var change = new Dictionary<string, object> {
                { "Position",modelFirebase.Position! },
                { "Modificacion",modelFirebase.Modificacion },
            };

            await GetCollection().Document(entity.Id).UpdateAsync(change);
            await GetCollection().Document(entity.Id).Collection(CollectionHistorial).AddAsync(modelHistory);
            return true;
        }

        public async Task<bool> UpdateUbicacione(Alerta entity)
        {
            var modelFirebase = mapper.Map<AlertaFirebase>(entity);
            var modelHistory = mapper.Map<AlertaHistorialFirebase>(entity.GetHistorial());

            var change = new Dictionary<string, object> {
                { "AfiliadoUbicacion",modelFirebase.AfiliadoUbicacion! },
                { "Modificacion",modelFirebase.Modificacion },
            };

            await GetCollection().Document(entity.Id).UpdateAsync(change);
            await GetCollection().Document(entity.Id).Collection(CollectionHistorial).AddAsync(modelHistory);
            return true;
        }

        public async Task<bool> UpdateState(Alerta model)
        {
            var modelFirebase = mapper.Map<AlertaFirebase>(model);
            var modelHistory = mapper.Map<AlertaHistorialFirebase>(model.GetHistorial());

            var change = new Dictionary<string, object> {
                { "Estado",modelFirebase.Estado! },
                { "Modificacion",modelFirebase.Modificacion },
            };

            await GetCollection().Document(model.Id).UpdateAsync(change);
            await GetCollection().Document(model.Id).Collection(CollectionHistorial).AddAsync(modelHistory);
            return true;
        }

        public async Task<bool> UpdateConfirmarAsignacion(Alerta model)
        {
            var modelFirebase = mapper.Map<AlertaFirebase>(model);
            var modelHistory = mapper.Map<AlertaHistorialFirebase>(model.GetHistorial());

            var change = new Dictionary<string, object> {
                { "MonitorId",modelFirebase.MonitorId },
                { "ConfirmaAsignacion",modelFirebase.ConfirmaAsignacion! },
                { "Modificacion",modelFirebase.Modificacion },
            };

            await GetCollection().Document(model.Id).UpdateAsync(change);
            await GetCollection().Document(model.Id).Collection(CollectionHistorial).AddAsync(modelHistory);
            return true;
        }

        public async Task<bool> UpdateAlarma(Alerta model)
        {
            var modelFirebase = mapper.Map<AlertaFirebase>(model);
            var modelHistory = mapper.Map<AlertaHistorialFirebase>(model.GetHistorial());

            var change = new Dictionary<string, object> {
                { "AlarmaSonora",modelFirebase.AlarmaSonora! },
                { "Modificacion",modelFirebase.Modificacion },
            };

            await GetCollection().Document(model.Id).UpdateAsync(change);
            await GetCollection().Document(model.Id).Collection(CollectionHistorial).AddAsync(modelHistory);
            return true;
        }

        public async Task<bool> UpdateFinalizacion(Alerta model)
        {
            var modelFirebase = mapper.Map<AlertaFirebase>(model);
            var modelHistory = mapper.Map<AlertaHistorialFirebase>(model.GetHistorial());

            var change = new Dictionary<string, object> {
                { "Cerrado",modelFirebase.Cerrado! },
                { "AlarmaSonora",modelFirebase.AlarmaSonora! },
                { "Bitacora",modelFirebase.Bitacora! },
                { "Modificacion",modelFirebase.Modificacion },
            };

            await GetCollection().Document(model.Id).UpdateAsync(change);
            await GetCollection().Document(model.Id).Collection(CollectionHistorial).AddAsync(modelHistory);
            return true;
        }

        public async Task<Alerta?> GetAlertByUserId(string id)
        {
            var result = await GetCollection()
                .WhereEqualTo("AfiliadoId", id)
                .WhereEqualTo("Cerrado", false)
                .Limit(1)
                .GetSnapshotAsync();

            if (result.Count > 0)
            {
                return mapper.Map<Alerta>(result.Documents.First().ConvertTo<AlertaFirebase>());
            }
            return null;
        }
    }
}
