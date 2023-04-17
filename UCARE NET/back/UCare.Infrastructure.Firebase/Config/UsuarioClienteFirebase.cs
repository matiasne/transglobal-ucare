using Google.Cloud.Firestore;

namespace UCare.Infrastructure.Firebase.Config
{
    [FirestoreData]
    public class ConfigFirebase : EntityBaseFirebase<string?>
    {
        [FirestoreProperty]
        public long UsuarioActivosMaximos { get; set; } = 5000;

        [FirestoreProperty]
        public long TiempoEnvioSMSSeconds { get; set; } = 60;
        [FirestoreProperty]
        public long? MonitorPausaTimeOut { get; set; } = 15;
        [FirestoreProperty]
        public long? ConfirmarTimeOut { get; set; } = 60;
        [FirestoreProperty]
        public long? TiempoParaReasignarAlerta { get; set; } = 600;

    }
}
