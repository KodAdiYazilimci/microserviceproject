using Presentation.UI.WindowsForm.Business.Model.Department.Buying;
using Presentation.UI.WindowsForm.Business.Model.Department.HR;
using Presentation.UI.WindowsForm.Business.Model.Department.IT;
using Presentation.UI.Infrastructure.Communication.Model.Basics;
using Presentation.UI.Infrastructure.Communication.Broker;
using Presentation.UI.Infrastructure.Communication.Broker.Providers;
using Presentation.UI.Infrastructure.Persistence.Repositories;
using Presentation.UI.WindowsForm.Dialogs.HR;
using Presentation.UI.WindowsForm.Infrastructure.Communication.Broker;

using Microsoft.Extensions.Caching.Memory;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.UI.WindowsForm
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
            CheckForIllegalCrossThreadCalls = false;

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
            //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            //ServiceResultModel<Token> serviceTokenResult =
            //    await _serviceCommunicator.Call<Token>(
            //     serviceName: _routeNameProvider.Auth_GetToken,
            //     postData: new Credential()
            //     {
            //         Email = ConfigurationManager.AppSettings["Configuration.Authorization.Credential.email"].ToString(),
            //         Password = ConfigurationManager.AppSettings["Configuration.Authorization.Credential.password"].ToString()
            //     },
            //     queryParameters: null,
            //     cancellationTokenSource: cancellationTokenSource);
        }

        private void BtnKisileriGetir_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            lstKisiler.Items.Clear();

            try
            {
                Task.Run(async delegate
                {
                    ServiceResultModel<List<PersonModel>> personServiceResult =
                        await _serviceCommunicator.Call<List<PersonModel>>(
                            serviceName: _routeNameProvider.HR_GetPeople,
                            postData: null,
                            queryParameters: null,
                            cancellationTokenSource: cancellationTokenSource);

                    if (personServiceResult.IsSuccess)
                    {
                        foreach (var person in personServiceResult.Data)
                        {
                            lstKisiler.Items.Add(person);
                        }
                    }
                    else
                    {
                        throw new Exception(personServiceResult.ErrorModel.Description);
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

        private void BnKisiOlustur_Click(object sender, EventArgs e)
        {
            CreatePersonForm createPersonForm =
                new CreatePersonForm(
                    _credentialProvider,
                    _memoryCache,
                    _routeNameProvider,
                    _serviceCommunicator,
                    _serviceRouteRepository);

            createPersonForm.ShowDialog();
        }

        private void BtnDepartmanlariGetir_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            lstDepartmanlar.Items.Clear();

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
                            lstDepartmanlar.Items.Add(department);
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
        }

        private void BtnDepartmanOlustur_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            try
            {
                Task.Run(async delegate
                {
                    ServiceResultModel<int> createDepartmentServiceResult =
                                await _serviceCommunicator.Call<int>(
                                    serviceName: _routeNameProvider.HR_CreateDepartment,
                                    postData: new DepartmentModel()
                                    {
                                        Name = "TestDepartman"
                                    },
                                    queryParameters: null,
                                    cancellationTokenSource: cancellationTokenSource);

                    if (createDepartmentServiceResult.IsSuccess)
                    {
                        MessageBox.Show("Departman oluşturuldu");
                    }
                    else
                        throw new Exception(createDepartmentServiceResult.ErrorModel.Description);
                },
                cancellationToken: cancellationTokenSource.Token).Wait();
            }
            catch (Exception ex)
            {
                cancellationTokenSource.Cancel();
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnCalisanlariGetir_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            lstCalisanlar.Items.Clear();

            try
            {
                Task.Run(async delegate
                {
                    ServiceResultModel<List<WorkerModel>> workersServiceResult =
                        await _serviceCommunicator.Call<List<WorkerModel>>(
                            serviceName: _routeNameProvider.HR_GetWorkers,
                            postData: null,
                            queryParameters: null,
                            cancellationTokenSource: cancellationTokenSource);

                    if (workersServiceResult.IsSuccess)
                    {
                        foreach (var worker in workersServiceResult.Data)
                        {
                            lstCalisanlar.Items.Add(worker);
                        }
                    }
                    else
                    {
                        throw new Exception(workersServiceResult.ErrorModel.Description);
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

        private void BtnCalisanOlustur_Click(object sender, EventArgs e)
        {
            CreateWorkerForm createWorkerForm =
                new CreateWorkerForm(
                    _credentialProvider,
                    _memoryCache,
                    _routeNameProvider,
                    _serviceCommunicator,
                    _serviceRouteRepository);

            createWorkerForm.ShowDialog();
        }

        private void BtnEnvanterleriGetir_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            lstEnvanterler.Items.Clear();

            try
            {
                Task.Run(async delegate
                {
                    ServiceResultModel<List<InventoryModel>> inventoryServiceResult =
                        await _serviceCommunicator.Call<List<InventoryModel>>(
                            serviceName: _routeNameProvider.IT_GetInventories,
                            postData: null,
                            queryParameters: null,
                            cancellationTokenSource: cancellationTokenSource);

                    if (inventoryServiceResult.IsSuccess)
                    {
                        foreach (var inventory in inventoryServiceResult.Data)
                        {
                            lstEnvanterler.Items.Add(inventory);
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

        private void BtnEnvanterOlustur_Click(object sender, EventArgs e)
        {
            Dialogs.IT.CreateInventoryForm createInventoryForm =
                new Dialogs.IT.CreateInventoryForm(
                    _credentialProvider,
                    _memoryCache,
                    _routeNameProvider,
                    _serviceCommunicator,
                    _serviceRouteRepository);

            createInventoryForm.ShowDialog();
        }

        private void BtnIdariIslerEnvanterleriGetir_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            lstIdariIslerEnvanterler.Items.Clear();

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
                        foreach (var inventory in inventoryServiceResult.Data)
                        {
                            lstIdariIslerEnvanterler.Items.Add(inventory);
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

        private void BtnIdariIslerEnventerOlustur_Click(object sender, EventArgs e)
        {
            Dialogs.AA.CreateInventoryForm createInventoryForm =
                new Dialogs.AA.CreateInventoryForm(
                    _credentialProvider,
                    _memoryCache,
                    _routeNameProvider,
                    _serviceCommunicator,
                    _serviceRouteRepository);

            createInventoryForm.ShowDialog();
        }

        private void BtnYeniBaslayanEnvanterGetir_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            lstITYeniBaslayanEnvanterleri.Items.Clear();

            try
            {
                Task.Run(async delegate
                {
                    ServiceResultModel<List<InventoryModel>> inventoryServiceResult =
                        await _serviceCommunicator.Call<List<InventoryModel>>(
                            serviceName: _routeNameProvider.IT_GetInventoriesForNewWorker,
                            postData: null,
                            queryParameters: null,
                            cancellationTokenSource: cancellationTokenSource);

                    if (inventoryServiceResult.IsSuccess)
                    {
                        foreach (var inventory in inventoryServiceResult.Data)
                        {
                            lstITYeniBaslayanEnvanterleri.Items.Add(inventory);
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

        private void BtnITYeniBaslayanEnvanterOlustur_Click(object sender, EventArgs e)
        {
            Dialogs.IT.CreateInventoryForNewWorkerForm createInventoryForm =
            new Dialogs.IT.CreateInventoryForNewWorkerForm(
                _credentialProvider,
                _memoryCache,
                _routeNameProvider,
                _serviceCommunicator,
                _serviceRouteRepository);

            createInventoryForm.ShowDialog();
        }

        private void BtnAAYeniBaslayanEnvanterleriGetir_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            lstAAYeniBaslayanEnvanterleri.Items.Clear();

            try
            {
                Task.Run(async delegate
                {
                    ServiceResultModel<List<InventoryModel>> inventoryServiceResult =
                        await _serviceCommunicator.Call<List<InventoryModel>>(
                            serviceName: _routeNameProvider.AA_GetInventoriesForNewWorker,
                            postData: null,
                            queryParameters: null,
                            cancellationTokenSource: cancellationTokenSource);

                    if (inventoryServiceResult.IsSuccess)
                    {
                        foreach (var inventory in inventoryServiceResult.Data)
                        {
                            lstAAYeniBaslayanEnvanterleri.Items.Add(inventory);
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

        private void BtnAAYeniBaslayanEnvanterOlustur_Click(object sender, EventArgs e)
        {
            Dialogs.AA.CreateInventoryForNewWorkerForm createInventoryForm =
                new Dialogs.AA.CreateInventoryForNewWorkerForm(
                    _credentialProvider,
                    _memoryCache,
                    _routeNameProvider,
                    _serviceCommunicator,
                    _serviceRouteRepository);

            createInventoryForm.ShowDialog();
        }

        private void btnSatinAlimGetir_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            lstSatinAlimlar.Items.Clear();

            try
            {
                Task.Run(async delegate
                {
                    ServiceResultModel<List<InventoryRequestModel>> inventoryRequestServiceResult =
                        await _serviceCommunicator.Call<List<InventoryRequestModel>>(
                            serviceName: _routeNameProvider.Buying_GetInventoryRequests,
                            postData: null,
                            queryParameters: null,
                            cancellationTokenSource: cancellationTokenSource);

                    if (inventoryRequestServiceResult.IsSuccess)
                    {
                        foreach (var inventoryRequest in inventoryRequestServiceResult.Data)
                        {
                            lstSatinAlimlar.Items.Add(inventoryRequest);
                        }
                    }
                    else
                    {
                        throw new Exception(inventoryRequestServiceResult.ErrorModel.Description);
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

        private void btnYeniSatinalma_Click(object sender, EventArgs e)
        {
            Dialogs.Buying.CreateInventoryRequestForm createInventoryForm =
                new Dialogs.Buying.CreateInventoryRequestForm(
                    _credentialProvider,
                    _memoryCache,
                    _routeNameProvider,
                    _serviceCommunicator,
                    _serviceRouteRepository);

            createInventoryForm.ShowDialog();
        }
    }
}
