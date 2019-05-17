namespace Monitor.Domain
{
    public interface IDbContextProvider
    {
        MonitorDbContext CreateDbContext();
    }
}