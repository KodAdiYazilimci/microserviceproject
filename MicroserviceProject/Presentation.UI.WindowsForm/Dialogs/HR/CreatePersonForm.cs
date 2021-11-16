using Communication.Http.Department.HR.Models;

using Microsoft.Extensions.Caching.Memory;

using Presentation.UI.Infrastructure.Communication.Broker.Providers;
using Presentation.UI.Infrastructure.Communication.Model.Basics;
using Presentation.UI.Infrastructure.Persistence.Repositories;
using Presentation.UI.WindowsForm.Infrastructure.Communication.Broker;

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.UI.WindowsForm.Dialogs.HR
{
    public partial class CreatePersonForm : Form
    {
        private readonly CredentialProvider _credentialProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly RouteNameProvider _routeNameProvider;
        private readonly ServiceCommunicator _serviceCommunicator;
        private readonly ServiceRouteRepository _serviceRouteRepository;

        public CreatePersonForm(
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
                    ServiceResultModel<int> createPersonServiceResult =
                                await _serviceCommunicator.Call<int>(
                                    serviceName: _routeNameProvider.HR_CreatePerson,
                                    postData: new PersonModel()
                                    {
                                        Name = txtIsim.Text
                                    },
                                    queryParameters: null,
                                    cancellationTokenSource: cancellationTokenSource);

                    if (createPersonServiceResult.IsSuccess)
                    {
                        success = true;
                        MessageBox.Show("Kişi oluşturuldu");
                    }
                    else
                        throw new Exception(createPersonServiceResult.ErrorModel.Description);
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
