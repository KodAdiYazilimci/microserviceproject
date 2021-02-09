using MicroserviceProject.Presentation.UI.Business.Model.Department.HR;
using MicroserviceProject.Presentation.UI.Infrastructure.Communication.Model.Basics;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroserviceProject.Presentation.UI.WindowsForm.Dialogs.HR
{
    public partial class CreateWorkerForm : Form
    {
        private readonly CredentialProvider _credentialProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly RouteNameProvider _routeNameProvider;
        private readonly ServiceCommunicator _serviceCommunicator;
        private readonly ServiceRouteRepository _serviceRouteRepository;

        public CreateWorkerForm(
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

        private void CreateWorkerForm_Load(object sender, EventArgs e)
        {
            GetPeople();
            GetTitles();
            GetDepartments();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {

        }

        private void btnVazgec_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GetPeople()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            cmbKisiler.Items.Clear();

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
                            cmbKisiler.Items.Add(person);
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

        private void GetTitles()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            cmbUnvanlar.Items.Clear();

            try
            {
                Task.Run(async delegate
                {
                    ServiceResult<List<TitleModel>> titleServiceResult =
                        await _serviceCommunicator.Call<List<TitleModel>>(
                            serviceName: _routeNameProvider.HR_GetTitles,
                            postData: null,
                            queryParameters: null,
                            cancellationToken: cancellationTokenSource.Token);

                    if (titleServiceResult.IsSuccess)
                    {
                        foreach (var title in titleServiceResult.Data)
                        {
                            cmbUnvanlar.Items.Add(title);
                        }
                    }
                    else
                    {
                        throw new Exception(titleServiceResult.Error.Description);
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

        private void GetDepartments()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            cmbDepartmanlar.Items.Clear();

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
                            cmbDepartmanlar.Items.Add(department);
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
    }
}
