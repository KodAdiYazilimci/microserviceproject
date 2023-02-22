using AutoMapper;

namespace Infrastructure.Mock.Factories
{
    /// <summary>
    /// Automapper taklidi yapan sınıf
    /// </summary>
    public class MappingFactory
    {
        /// <summary>
        /// Automapper örneğini verir
        /// </summary>
        /// <param name="profile">Automapper profili nesnesi</param>
        /// <returns></returns>
        public static IMapper GetInstance(Profile profile)
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(profile);
            });

            return mapperConfiguration.CreateMapper();
        }
    }
}
