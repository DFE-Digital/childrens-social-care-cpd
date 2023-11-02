namespace Childrens_Social_Care_CPD.Extensions
{
    public static class EnumExtension
    {
        public static T ToEnum<T>(this string enumString)
        {
            return (T)Enum.Parse(typeof(T), enumString);
        }
    }
}
