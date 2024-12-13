namespace BEIN_RL.IRepositories
{
    public interface ISector
    {
        Task<Dictionary<string, object>> GetAllAsync();
        Task<Dictionary<string, object>> GetSectorAsync(string sectorName);
    }
}
