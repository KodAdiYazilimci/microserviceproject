namespace Infrastructure.Runtime.Defintion
{
    /// <summary>
    /// Üyeye ait özel isim tanımlamasını uyarlayan arayüz
    /// </summary>
    public interface ICustomName
    {
        /// <summary>
        /// Üyenin özel ismi
        /// </summary>
        string Name { get; }
    }
}
