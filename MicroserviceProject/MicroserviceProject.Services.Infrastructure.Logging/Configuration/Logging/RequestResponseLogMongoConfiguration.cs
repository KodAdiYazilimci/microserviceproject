﻿using MicroserviceProject.Infrastructure.Logging.MongoDb.Configuration;

using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Services.Infrastructure.Logging.Configuration.Logging
{
    /// <summary>
    /// Request-response logları için MongoDB yapılandırma ayarları
    /// </summary>
    public class RequestResponseLogMongoConfiguration : IMongoDbConfiguration
    {
        /// <summary>
        /// Request-response logları için MongoDB yapılandırma ayarları
        /// </summary>
        /// <param name="configuration"></param>
        public RequestResponseLogMongoConfiguration(IConfiguration configuration)
        {
            ConnectionString = configuration
               .GetSection("Configuration")
               .GetSection("Logging")
               .GetSection("RequestResponseLogging")
               .GetSection("MongoConfiguration")
               .GetSection("ConnectionString").Value;

            DataBase = configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RequestResponseLogging")
                .GetSection("MongoConfiguration")
                .GetSection("DataBase").Value;

            CollectionName = configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RequestResponseLogging")
                .GetSection("MongoConfiguration")
                .GetSection("CollectionName").Value;
        }

        /// <summary>
        /// MongoDB bağlantı cümlesi
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Logların yazılacaği MongoDB veritabanı adı
        /// </summary>
        public string DataBase { get; set; }

        /// <summary>
        /// Logların yazılacağı koleksiyonun adı
        /// </summary>
        public string CollectionName { get; set; }
    }
}