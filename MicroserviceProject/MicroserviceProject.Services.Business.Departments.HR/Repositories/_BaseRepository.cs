using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Services.Business.Departments.HR.Repositories
{
    public class BaseRepository
    {
        private readonly IConfiguration _configuration;

        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected string ConnectionString
        {
            get
            {
                return _configuration
                    .GetSection("Persistence")
                    .GetSection("DataSource").Value;
            }
        }
    }
}
