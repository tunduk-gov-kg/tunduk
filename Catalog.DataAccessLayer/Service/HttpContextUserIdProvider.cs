using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Catalog.DataAccessLayer.Service {
    public class HttpContextUserIdProvider : IUserIdProvider<string> {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextUserIdProvider(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserId() {
            var identityName        = _httpContextAccessor.HttpContext.User.Identity.Name;
            var authenticatedUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return identityName ?? authenticatedUserId;
        }
    }
}