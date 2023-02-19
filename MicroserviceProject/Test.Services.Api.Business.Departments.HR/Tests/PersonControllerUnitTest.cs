using Microsoft.VisualStudio.TestTools.UnitTesting;

using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Services.Api.Business.Departments.HR.Tests
{
    [TestClass]
    public class PersonControllerUnitTest
    {
        private DepartmentControllerTest departmentControllerTest;
        private PersonControllerTest personControllerTest;

        [TestInitialize]
        public void Init()
        {
            departmentControllerTest = new DepartmentControllerTest();
            personControllerTest = new PersonControllerTest();
        }

        [TestMethod]
        public async Task GetPeopleTest()
        {
            List<PersonModel> people = await GetPeopleAsync();

            Assert.IsNotNull(people != null && people.Any());
        }

        [TestMethod]
        public async Task CreatePersonTest()
        {
            var result = await personControllerTest.CreatePersonAsync(new CreatePersonCommandRequest()
            {
                Person = new PersonModel()
                {
                    Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                }
            });

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        [TestMethod]
        public async Task GetTitlesTest()
        {
            var titles = await personControllerTest.GetTitles();

            if (titles != null && !titles.Any())
            {
                await CreateTitleTest();

                titles = await personControllerTest.GetTitles();
            }

            Assert.IsTrue(titles != null && titles.Any());
        }

        [TestMethod]
        public async Task CreateTitleTest()
        {
            var result = await personControllerTest.CreateTitle(new CreateTitleCommandRequest()
            {
                Title = new TitleModel()
                {
                    Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                }
            });

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        [TestMethod]
        public async Task GetWorkersTest()
        {
            var workers = await GetWorkersAsync();

            Assert.IsTrue(workers != null && workers.Any());
        }

        private async Task<List<WorkerModel>> GetWorkersAsync()
        {
            var workers = await personControllerTest.GetWorkersAsync();

            if (workers != null && !workers.Any())
            {
                List<PersonModel> people = await GetPeopleAsync();

                var createWorkerResult = await personControllerTest.CreateWorkerAsync(new CreateWorkerCommandRequest()
                {
                    Worker = new global::Services.Communication.Http.Broker.Department.HR.Models.WorkerModel()
                    {
                        Person = people.ElementAt(new Random().Next(0, people.Count - 1))
                    }
                });

                workers = await personControllerTest.GetWorkersAsync();
            }

            return workers;
        }


        private async Task<List<PersonModel>> GetPeopleAsync()
        {
            var people = await personControllerTest.GetPeopleAsync();

            if (people != null && !people.Any())
            {
                var createPersonTask = personControllerTest.CreatePersonAsync(new CreatePersonCommandRequest()
                {
                    Person = new PersonModel()
                    {
                        Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                    }
                });

                people = await personControllerTest.GetPeopleAsync();
            }

            return people;
        }

        [TestMethod]
        public async Task CreateWorkerTest()
        {
            var departments = await GetDepartmentsAsync();

            var people = await GetPeopleAsync();

            var result = await personControllerTest.CreateWorkerAsync(new CreateWorkerCommandRequest()
            {
                Worker = new WorkerModel()
                {
                    Department = departments.ElementAt(new Random().Next(0, departments.Count - 1)),
                    Person = people.ElementAt(new Random().Next(0, people.Count - 1))
                }
            });

            Assert.IsTrue(result != null && result.IsSuccess);
        }


        private async Task<List<DepartmentModel>> GetDepartmentsAsync()
        {
            var departments = await departmentControllerTest.GetDepartmentsAsync();

            if (departments != null && !departments.Any())
            {
                var createDepartmentResult = departmentControllerTest.CreateDepartmentAsync(new CreateDepartmentCommandRequest()
                {
                    Department = new DepartmentModel()
                    {
                        Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                    }
                });

                departments = await departmentControllerTest.GetDepartmentsAsync();
            }

            return departments;
        }

        [TestCleanup]
        public void CleanUp()
        {

        }
    }
}
