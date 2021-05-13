using Presentation.UI.Business.Model.Constants;
using Presentation.UI.Infrastructure.Communication.Model.Basics;
using Presentation.UI.Infrastructure.Communication.Moderator.Providers;
using Presentation.UI.Infrastructure.Persistence.Repositories;
using Presentation.UI.WindowsForm.Business.Model.Department.Buying;
using Presentation.UI.WindowsForm.Business.Model.Department.HR;
using Presentation.UI.WindowsForm.Infrastructure.Communication.Moderator;

using Microsoft.Extensions.Caching.Memory;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.UI.WindowsForm.Dialogs.Buying
{
    public partial class CreateInventoryRequestForm : Form
    {
        private readonly CredentialProvider _credentialProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly RouteNameProvider _routeNameProvider;
        private readonly ServiceCommunicator _serviceCommunicator;
        private readonly ServiceRouteRepository _serviceRouteRepository;

        public CreateInventoryRequestForm(
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
            if (cmbEnvanter.SelectedIndex >= 0)
            {
                bool success = false;

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                try
                {
                    Task.Run(async delegate
                    {
                        ServiceResultModel<int> createInventoryRequestServiceResult =
                                    await _serviceCommunicator.Call<int>(
                                        serviceName: _routeNameProvider.Buying_CreateInventoryRequest,
                                        postData: new InventoryRequestModel()
                                        {
                                            DepartmentId = (cmbDepartman.SelectedItem as DepartmentModel).Id,

                                            InventoryId =
                                            (cmbDepartman.SelectedItem as DepartmentModel).Id == (int)Departments.AdministrativeAffairs
                                            ?
                                            (cmbEnvanter.SelectedItem as Business.Model.Department.AA.InventoryModel).Id
                                            :
                                            (cmbEnvanter.SelectedItem as Business.Model.Department.IT.InventoryModel).Id,

                                            Amount = (int)numAdet.Value
                                        },
                                        queryParameters: null,
                                        cancellationTokenSource: cancellationTokenSource);

                        if (createInventoryRequestServiceResult.IsSuccess)
                        {
                            success = true;
                            MessageBox.Show("Envanter oluşturuldu");
                        }
                        else
                            throw new Exception(createInventoryRequestServiceResult.ErrorModel.Description);
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

        private void CreateInventoryRequestForm_Load(object sender, EventArgs e)
        {
            GetDepartments();
        }

        private void GetDepartments()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            cmbDepartman.Items.Clear();

            try
            {
                Task.Run(async delegate
                {
                    ServiceResultModel<List<DepartmentModel>> departmentServiceResult =
                        await _serviceCommunicator.Call<List<DepartmentModel>>(
                            serviceName: _routeNameProvider.HR_GetDepartments,
                            postData: null,
                            queryParameters: null,
                            cancellationTokenSource: cancellationTokenSource);

                    if (departmentServiceResult.IsSuccess)
                    {
                        foreach (var department in departmentServiceResult.Data)
                        {
                            cmbDepartman.Items.Add(department);
                        }
                    }
                    else
                    {
                        throw new Exception(departmentServiceResult.ErrorModel.Description);
                    }
                },
                cancellationToken: cancellationTokenSource.Token).Wait();
            }
            catch (Exception ex)
            {
                cancellationTokenSource.Cancel();
                MessageBox.Show(ex.ToString());
            }

            if (cmbDepartman.Items.Count > 0)
            {
                cmbDepartman.SelectedIndex = 0;
            }
        }

        private void cmbDepartman_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbEnvanter.Items.Clear();

            if (cmbDepartman.SelectedIndex >= 0)
            {
                if ((cmbDepartman.SelectedItem as DepartmentModel).Id == (int)Departments.AdministrativeAffairs)
                {
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                    try
                    {
                        Task.Run(async delegate
                        {
                            ServiceResultModel<List<Business.Model.Department.AA.InventoryModel>> inventoryServiceResult =
                                await _serviceCommunicator.Call<List<Business.Model.Department.AA.InventoryModel>>(
                                    serviceName: _routeNameProvider.AA_GetInventories,
                                    postData: null,
                                    queryParameters: null,
                                    cancellationTokenSource: cancellationTokenSource);

                            if (inventoryServiceResult.IsSuccess)
                            {
                                foreach (var inventory in inventoryServiceResult.Data)
                                {
                                    cmbEnvanter.Items.Add(inventory);
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
                }
                else if ((cmbDepartman.SelectedItem as DepartmentModel).Id == (int)Departments.InformationTechnologies)
                {
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                    try
                    {
                        Task.Run(async delegate
                        {
                            ServiceResultModel<List<Business.Model.Department.IT.InventoryModel>> inventoryServiceResult =
                                await _serviceCommunicator.Call<List<Business.Model.Department.IT.InventoryModel>>(
                                    serviceName: _routeNameProvider.IT_GetInventories,
                                    postData: null,
                                    queryParameters: null,
                                    cancellationTokenSource: cancellationTokenSource);

                            if (inventoryServiceResult.IsSuccess)
                            {
                                foreach (var inventory in inventoryServiceResult.Data)
                                {
                                    cmbEnvanter.Items.Add(inventory);
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
                }
                else
                {
                    MessageBox.Show("Sadece IT ve İdari işler seçilebilir");
                    cmbDepartman.SelectedIndex = -1;
                }
            }
        }
    }
}
