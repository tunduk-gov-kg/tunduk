namespace XRoad.Domain
{
    public class MemberIdentifier
    {
        public string Instance { get; set; }
        public string MemberClass { get; set; }
        public string MemberCode { get; set; }
    }

    public class MemberData
    {
        public MemberIdentifier MemberIdentifier { get; set; }
        public string Name { get; set; }
    }
}