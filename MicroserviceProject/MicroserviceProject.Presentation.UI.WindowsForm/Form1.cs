using MicroserviceProject.Presentation.UI.Business.Model.Department.HR;
using MicroserviceProject.Presentation.UI.Business.Model.Department.IT;
using MicroserviceProject.Presentation.UI.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Presentation.UI.Infrastructure.Communication.Moderator;
using MicroserviceProject.Presentation.UI.Infrastructure.Communication.Moderator.Providers;
using MicroserviceProject.Presentation.UI.Infrastructure.Persistence.Repositories;
using MicroserviceProject.Presentation.UI.Infrastructure.Security.Model;
using MicroserviceProject.Presentation.UI.WindowsForm.Dialogs.HR;

using Microsoft.Extensions.Caching.Memory;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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

            //ServiceResult<Token> serviceTokenResult =
            //    await _serviceCommunicator.Call<Token>(
            //     serviceName: _routeNameProvider.Auth_GetToken,
            //     postData: new Credential()
            //     {
            //         Email = ConfigurationManager.AppSettings["Configuration.Authorization.Credential.email"].ToString(),
            //         Password = ConfigurationManager.AppSettings["Configuration.Authorization.Credential.password"].ToString()
            //     },
            //     queryParameters: null,
            //     cancellationToken: cancellationTokenSource.Token);
        }

        private void btnKisileriGetir_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            lstKisiler.Items.Clear();

            try
            {
                Task.Run(async delegate
                {
                    ServiceResult<List<PersonModel>> personServiceResult =
                        await _serviceCommunicator.Call<List<PersonModel>>(
                            serviceName: _routeNameProvider.HR_GetPeople,
                            postData: null,
                            queryParameters: null,
                            cancellationToken: cancellationTokenSource.Token);

                    if (personServiceResult.IsSuccess)
                    {
                        foreach (var person in personServiceResult.Data)
                        {
                            lstKisiler.Items.Add(person);
                        }
                    }
                    else
                    {
                        throw new Exception(personServiceResult.Error.Description);
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

        private void bnKisiOlustur_Click(object sender, EventArgs e)
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

        private void btnDepartmanlariGetir_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            lstDepartmanlar.Items.Clear();

            try
            {
                Task.Run(async delegate
                {
                    ServiceResult<List<DepartmentModel>> departmentServiceResult =
                        await _serviceCommunicator.Call<List<DepartmentModel>>(
                            serviceName: _routeNameProvider.HR_GetDepartments,
                            postData: null,
                            queryParameters: null,
                            cancellationToken: cancellationTokenSource.Token);

                    if (departmentServiceResult.IsSuccess)
                    {
                        foreach (var department in departmentServiceResult.Data)
                        {
                            lstDepartmanlar.Items.Add(department);
                        }
                    }
                    else
                    {
                        throw new Exception(departmentServiceResult.Error.Description);
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

        private void btnDepartmanOlustur_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            try
            {
                Task.Run(async delegate
                {
                    ServiceResult<int> createDepartmentServiceResult =
                                await _serviceCommunicator.Call<int>(
                                    serviceName: _routeNameProvider.HR_CreateDepartment,
                                    postData: new DepartmentModel()
                                    {
                                        Name = "TestDepartman"
                                    },
                                    queryParameters: null,
                                    cancellationToken: cancellationTokenSource.Token);

                    if (createDepartmentServiceResult.IsSuccess)
                    {
                        MessageBox.Show("Departman oluşturuldu");
                    }
                    else
                        throw new Exception(createDepartmentServiceResult.Error.Description);
                },
                cancellationToken: cancellationTokenSource.Token).Wait();
            }
            catch (Exception ex)
            {
                cancellationTokenSource.Cancel();
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCalisanlariGetir_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            lstCalisanlar.Items.Clear();

            try
            {
                Task.Run(async delegate
                {
                    ServiceResult<List<WorkerModel>> workersServiceResult =
                        await _serviceCommunicator.Call<List<WorkerModel>>(
                            serviceName: _routeNameProvider.HR_GetWorkers,
                            postData: null,
                            queryParameters: null,
                            cancellationToken: cancellationTokenSource.Token);

                    if (workersServiceResult.IsSuccess)
                    {
                        foreach (var worker in workersServiceResult.Data)
                        {
                            lstCalisanlar.Items.Add(worker);
                        }
                    }
                    else
                    {
                        throw new Exception(workersServiceResult.Error.Description);
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

        private void btnCalisanOlustur_Click(object sender, EventArgs e)
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

        private void btnEnvanterleriGetir_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            lstEnvanterler.Items.Clear();

            try
            {
                Task.Run(async delegate
                {
                    ServiceResult<List<InventoryModel>> inventoryServiceResult =
                        await _serviceCommunicator.Call<List<InventoryModel>>(
                            serviceName: _routeNameProvider.IT_GetInventories,
                            postData: null,
                            queryParameters: null,
                            cancellationToken: cancellationTokenSource.Token);

                    if (inventoryServiceResult.IsSuccess)
                    {
                        foreach (var inventory in inventoryServiceResult.Data)
                        {
                            lstEnvanterler.Items.Add(inventory);
                        }
                    }
                    else
                    {
                        throw new Exception(inventoryServiceResult.Error.Description);
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

        private void btnEnvanterOlustur_Click(object sender, EventArgs e)
        {
            CreateInventoryForm createInventoryForm =
                new CreateInventoryForm(
                    _credentialProvider,
                    _memoryCache,
                    _routeNameProvider,
                    _serviceCommunicator,
                    _serviceRouteRepository);

            createInventoryForm.ShowDialog();
        }
    }
}
