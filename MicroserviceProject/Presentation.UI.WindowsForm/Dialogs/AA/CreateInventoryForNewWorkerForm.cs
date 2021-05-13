using Presentation.UI.WindowsForm.Business.Model.Department.IT;
using Presentation.UI.Infrastructure.Communication.Model.Basics;
using Presentation.UI.Infrastructure.Communication.Moderator;
using Presentation.UI.Infrastructure.Communication.Moderator.Providers;
using Presentation.UI.Infrastructure.Persistence.Repositories;
using Presentation.UI.WindowsForm.Infrastructure.Communication.Moderator;

using Microsoft.Extensions.Caching.Memory;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.UI.WindowsForm.Dialogs.AA
{
    public partial class CreateInventoryForNewWorkerForm : Form
    {
        private readonly CredentialProvider _credentialProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly RouteNameProvider _routeNameProvider;
        private readonly ServiceCommunicator _serviceCommunicator;
        private readonly ServiceRouteRepository _serviceRouteRepository;

        public CreateInventoryForNewWorkerForm(
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
                    ServiceResultModel<InventoryModel> createInventoryServiceResult =
                                await _serviceCommunicator.Call<InventoryModel>(
                                    serviceName: _routeNameProvider.AA_CreateDefaultInventoryForNewWorker,
                                    postData: new InventoryModel()
                                    {
                                        Id = (cmbEnvanter.SelectedItem as InventoryModel).Id
                                    },
                                    queryParameters: null,
                                    cancellationTokenSource: cancellationTokenSource);

                    if (createInventoryServiceResult.IsSuccess)
                    {
                        success = true;
                        MessageBox.Show("Envanter oluşturuldu");
                    }
                    else
                        throw new Exception(createInventoryServiceResult.ErrorModel.Description);
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

        private void CreateInventoryForNewWorkerForm_Load(object sender, EventArgs e)
        {
            GetInventories();
        }


        private void GetInventories()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            cmbEnvanter.Items.Clear();

            try
            {
                Task.Run(async delegate
                {
                    ServiceResultModel<List<InventoryModel>> inventoryServiceResult =
                        await _serviceCommunicator.Call<List<InventoryModel>>(
                            serviceName: _routeNameProvider.AA_GetInventories,
                            postData: null,
                            queryParameters: null,
                            cancellationTokenSource: cancellationTokenSource);

                    if (inventoryServiceResult.IsSuccess)
                    {
                        foreach (var department in inventoryServiceResult.Data)
                        {
                            cmbEnvanter.Items.Add(department);
                        }
                    }
                    else
                    {
                        throw new Exception(inventoryServiceResult.ErrorModel.Description);
                    }
                },
                cancellationToken: cancellationTokenSource.Token).Wait();
            }
            catch (Exception ex)
            {
                cancellationTokenSource.Cancel();
                MessageBox.Show(ex.ToString());
            }

            if (cmbEnvanter.Items.Count > 0)
            {
                cmbEnvanter.SelectedIndex = 0;
            }
        }
    }
}
