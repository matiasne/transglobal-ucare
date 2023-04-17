namespace UCare.Shared.Domain.CodigoPaices
{
    public interface ICodigosPaicesRepositorio
    {
        CodigoPais? GetCodigoPais(string valor);
        List<CodigoPais>? GetCodigoPaisList();
    }
}
