namespace BEIN_ServerSide_SL.IServerSideServices
{
    public interface IPublicService
    {
        Task<Dictionary<string, object>> GetAllSectorsAsync();
        Task<Dictionary<string, object>> GetAllSoftwareProductsAsync();
        Task<Dictionary<string, object>> GetSectorAsync(string sectorName);
        Task<Dictionary<string, object>> GetSoftwareProductAsync(string softwareProductName);
    }
}
