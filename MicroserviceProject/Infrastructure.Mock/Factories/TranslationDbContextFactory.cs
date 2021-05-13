using Infrastructure.Localization.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mock.Factories
{
    public class TranslationDbContextFactory
    {
        private static TranslationDbContext translationDbContext;

        public static TranslationDbContext GetTranslationDbContext(IConfiguration configuration)
        {
            if (translationDbContext == null)
            {
                translationDbContext = new TranslationDbContext(new TestDbContextOptions(configuration));

            }

            return translationDbContext;
        }
    }

    class TestDbContextOptions : DbContextOptionsBuilder<TranslationDbContext>
    {
        public TestDbContextOptions(IConfiguration configuration) : base()
        {
            this.UseSqlServer(
                configuration
                .GetSection("Configuration")
                .GetSection("Localization")["TranslationDbConnnectionString"]);
        }
    }
}
