namespace XRoad.Domain
{
    public class SecurityServerIdentifier : MemberIdentifier
    {
        public string SecurityServerCode { get; set; }
    }

    public class SecurityServerData
    {
        public SecurityServerIdentifier SecurityServerIdentifier { get; set; }
        public string Address { get; set; }
    }
}
