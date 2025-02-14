namespace BEIN_RL.IRepositories
{
    public interface IPublic
    {
        Task<Dictionary<string, object>> GetAllSectorsAsync();
        Task<Dictionary<string, object>> GetAllSoftwareProductsAsync();
        Task<Dictionary<string, object>> GetSectorAsync(string sectorName);
        Task<Dictionary<string, object>> GetSoftwareProductAsync(string SoftwareProductName);
    }
}
