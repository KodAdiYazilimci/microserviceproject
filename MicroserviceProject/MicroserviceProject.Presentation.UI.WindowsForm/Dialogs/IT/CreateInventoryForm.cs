using MicroserviceProject.Presentation.UI.Business.Model.Department.IT;
using MicroserviceProject.Presentation.UI.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Presentation.UI.Infrastructure.Communication.Moderator;
using MicroserviceProject.Presentation.UI.Infrastructure.Communication.Moderator.Providers;
using MicroserviceProject.Presentation.UI.Infrastructure.Persistence.Repositories;

using Microsoft.Extensions.Caching.Memory;

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroserviceProject.Presentation.UI.WindowsForm.Dialogs.IT
{
    public partial class CreateInventoryForm : Form
    {
        private readonly CredentialProvider _credentialProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly RouteNameProvider _routeNameProvider;
        private readonly ServiceCommunicator _serviceCommunicator;
        private readonly ServiceRouteRepository _serviceRouteRepository;

        public CreateInventoryForm(
            CredentialProvider credentialProvider,
            IMemoryCache memoryCache,
            RouteNameProvider routeNameProvider,
            ServiceCommunicator serviceCommunicator,
            ServiceRouteRepository serviceRouteRepository)
        {
            _credentialProvider = credentialProvider;
            _memoryCache = memoryCache;
            _routeNameProvider = routeNameProvider;
            _serviceCommunicator = serviceCommunicator;
            _serviceRouteRepository = serviceRouteRepository;

            CheckForIllegalCrossThreadCalls = false;

            InitializeComponent();
        }

        private void btnVazgec_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            bool success = false;

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            try
            {
                Task.Run(async delegate
                {
                    ServiceResult<int> createInventoryServiceResult =
                                await _serviceCommunicator.Call<int>(
                                    serviceName: _routeNameProvider.IT_CreateInventory,
                                    postData: new InventoryModel()
                                    {
                                        Name = txtIsim.Text,
                                        CurrentStockCount = (int)numAdet.Value
                                    },
                                    queryParameters: null,
                                    cancellationToken: cancellationTokenSource.Token);

                    if (createInventoryServiceResult.IsSuccess)
                    {
                        success = true;
                        MessageBox.Show("Envanter oluşturuldu");
                    }
                    else
                        throw new Exception(createInventoryServiceResult.Error.Description);
                },
                cancellationToken: cancellationTokenSource.Token).Wait();
            }
            catch (Exception ex)
            {
                cancellationTokenSource.Cancel();
                MessageBox.Show(ex.ToString());
            }

            if (success)
            {
                this.Close();
            }
        }
    }
}
