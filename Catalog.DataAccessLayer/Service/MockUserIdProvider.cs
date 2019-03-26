namespace Catalog.DataAccessLayer.Service {
    public class MockUserIdProvider : IUserIdProvider<string> {
        public string GetCurrentUserId() => "sample_user_id";
    }
}