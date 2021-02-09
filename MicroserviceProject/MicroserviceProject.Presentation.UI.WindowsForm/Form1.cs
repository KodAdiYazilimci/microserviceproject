using MicroserviceProject.Presentation.UI.Business.Model.Department.HR;
using MicroserviceProject.Presentation.UI.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Presentation.UI.Infrastructure.Communication.Moderator;
using MicroserviceProject.Presentation.UI.Infrastructure.Communication.Moderator.Providers;
using MicroserviceProject.Presentation.UI.Infrastructure.Persistence.Repositories;
using MicroserviceProject.Presentation.UI.Infrastructure.Security.Model;

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
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            try
            {
                Task.Run(async delegate
                {
                    ServiceResult<int> createPersonServiceResult =
                                await _serviceCommunicator.Call<int>(
                                    serviceName: _routeNameProvider.HR_CreatePerson,
                                    postData: new PersonModel()
                                    {
                                        Name = "TestUser"
                                    },
                                    queryParameters: null,
                                    cancellationToken: cancellationTokenSource.Token);

                    if (createPersonServiceResult.IsSuccess)
                    {
                        MessageBox.Show("Kişi oluşturuldu");
                    }
                    else
                        throw new Exception(createPersonServiceResult.Error.Description);
                },
                cancellationToken: cancellationTokenSource.Token).Wait();
            }
            catch (Exception ex)
            {
                cancellationTokenSource.Cancel();
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
