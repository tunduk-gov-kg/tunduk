using Ardalis.SmartEnum;

namespace Catalog.Domain.Enum {
    public class MemberType : SmartEnum<MemberType> {
        public static MemberType StateAuthority = new MemberType("Государственный орган", 1);
        public static MemberType LocalAuthority = new MemberType("Органы местного самоуправления", 2);
        public static MemberType GovernmentAgency = new MemberType("Государственное учреждение", 3);
        public static MemberType MunicipalInstitution = new MemberType("Муниципальное учреждение", 4);
        public static MemberType StateOwnedEnterprise = new MemberType("Государственное предприятие", 5);
        public static MemberType LegalEntity = new MemberType("Юридическое лицо", 6);
        public static MemberType Individual = new MemberType("Физическое лицо", 7);

        private MemberType(string name, int value) : base(name, value) {
        }
    }
}