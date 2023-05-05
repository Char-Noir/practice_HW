namespace HW5.DAO
{
    public interface IAnalysisDao
    {
        Task<bool> CheckAnalysisAsync(int orderId);
    }
}
