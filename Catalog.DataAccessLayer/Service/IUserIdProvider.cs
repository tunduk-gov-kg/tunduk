using System;

namespace Catalog.DataAccessLayer.Service
{
    public interface IUserIdProvider<out TKey> where TKey : IEquatable<TKey>
    {
        TKey GetCurrentUserId();
    }
}