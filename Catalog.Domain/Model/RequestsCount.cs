using System;

namespace Catalog.Domain.Model
{
    public class RequestsCount : IComparable<RequestsCount>
    {
        public RequestsCount(int failed, int succeeded)
        {
            Failed = failed;
            Succeeded = succeeded;
        }

        public int Failed { get; }

        public int Succeeded { get; }

        public int Total => Failed + Succeeded;

        public int CompareTo(RequestsCount other)
        {
            return Total.CompareTo(other.Total);
        }
    }
}