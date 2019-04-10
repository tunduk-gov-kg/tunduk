namespace Catalog.DataAccessLayer.Service
{
    public class MockUserIdProvider : IUserIdProvider<string>
    {
        public string GetCurrentUserId()
        {
            return "sample_user_id";
        }
    }
}