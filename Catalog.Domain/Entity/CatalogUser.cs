using Microsoft.AspNetCore.Identity;

namespace Catalog.Domain.Entity {
    public class CatalogUser : IdentityUser {
        public string Name { get; set; }
    }
}