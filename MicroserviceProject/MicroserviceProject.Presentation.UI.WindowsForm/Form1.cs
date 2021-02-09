using MicroserviceProject.Presentation.UI.Infrastructure.Communication.Moderator;
using MicroserviceProject.Presentation.UI.Infrastructure.Communication.Moderator.Providers;
using MicroserviceProject.Presentation.UI.Infrastructure.Persistence.Repositories;

using Microsoft.Extensions.Caching.Memory;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroserviceProject.Presentation.UI.WindowsForm
{
    public partial class Form1 : Form
    {
        private readonly CredentialProvider _credentialProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly RouteNameProvider _routeNameProvider;
        private readonly ServiceCommunicator _serviceCommunicator;
        private readonly ServiceRouteRepository _serviceRouteRepository;

        public Form1()
        {
            InitializeComponent();

            _credentialProvider = new CredentialProvider();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _routeNameProvider = new RouteNameProvider();
            _serviceRouteRepository = new ServiceRouteRepository(
                ConfigurationManager.AppSettings["Routing.DataSource"].ToString());
            _serviceCommunicator = new ServiceCommunicator(
                _memoryCache,
                _credentialProvider,
                _routeNameProvider,
                _serviceRouteRepository);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
