using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace SecureClient
{
    public class AuthConfig
    {
        public string InstanceId { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string BaseAddress { get; set; }
        public string Authority { 
            get 
            {
                return string.Format(CultureInfo.InvariantCulture, InstanceId, TenantId);
            } 
        }
        public string ResourceId { get; set; }

        public static AuthConfig ReadJsonFromFile(string path)
        {
            IConfiguration configuration;
            string directoryName = Path.GetDirectoryName(path);
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(path);
            configuration = builder.Build();
            return configuration.Get<AuthConfig>();
        }
    }
}
