namespace Catalog.Domain.Model
{
    public class RequestsCount
    {
        public RequestsCount(int failed, int succeeded)
        {
            Failed = failed;
            Succeeded = succeeded;
        }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public int Failed { get; set; }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public int Succeeded { get; set; }

        public int Total => Failed + Succeeded;
    }
}