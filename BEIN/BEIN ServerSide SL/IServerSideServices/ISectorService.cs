namespace BEIN_ServerSide_SL.IServerSideServices
{
    public interface ISectorService
    {
        Task<Dictionary<string, object>> GetAllAsync();
        Task<Dictionary<string, object>> GetSectorAsync(string sectorName);
    }
}
