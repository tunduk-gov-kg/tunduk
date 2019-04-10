using Ardalis.SmartEnum;

namespace Catalog.Domain.Enum
{
    public class MemberStatus : SmartEnum<MemberStatus>
    {
        public static MemberStatus Active = new MemberStatus("Участник", 1);
        public static MemberStatus Disabled = new MemberStatus("Ликвидирован", 2);

        private MemberStatus(string name, int value) : base(name, value) { }
    }
}