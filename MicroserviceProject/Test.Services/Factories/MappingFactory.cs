using AutoMapper;

namespace MicroserviceProject.Services.Business.Departments.HR.Test.Prepreations.Infrastructure
{
    public class MappingFactory
    {
        private static IMapper mapper = null;

        public static IMapper GetInstance(Profile profile)
        {
            if (mapper == null)
            {
                MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
                {
                    conf.AddProfile(profile);
                });

                mapper = mapperConfiguration.CreateMapper();
            }

            return mapper;
        }
    }
}
