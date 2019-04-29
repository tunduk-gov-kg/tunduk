using Ardalis.SmartEnum;

namespace Catalog.Domain.Enum
{
    public sealed class MemberRole : SmartEnum<MemberRole>
    {
        public static readonly MemberRole ServiceProvider = new MemberRole("Поставщик услуг", 1);
        public static readonly MemberRole ServiceClient = new MemberRole("Пользователь услуг", 2);
        public static readonly MemberRole ServiceDeveloper = new MemberRole("Разработчик решений", 3);

        private MemberRole(string name, int value) : base(name, value)
        {
        }
    }
}