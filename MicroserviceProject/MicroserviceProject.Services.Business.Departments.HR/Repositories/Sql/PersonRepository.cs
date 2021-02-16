using MicroserviceProject.Services.Business.Departments.HR.Entities.Sql;
using MicroserviceProject.Services.UnitOfWork;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.HR.Repositories.Sql
{
    /// <summary>
    /// Kişi tablosu için repository sınıfı
    /// </summary>
    public class PersonRepository : BaseRepository<PersonEntity>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Kişi tablosu için repository sınıfı
        /// </summary>
        /// <param name="unitOfWork">Veritabanı işlemlerini kapsayan iş birimi nesnesi</param>
        public PersonRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// Kişilerin listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<List<PersonEntity>> GetListAsync(CancellationToken cancellationToken)
        {
            List<PersonEntity> people = new List<PersonEntity>();

            SqlCommand sqlCommand = new SqlCommand(@"SELECT 
                                                     [ID],
                                                     [NAME],
                                                     [DELETE_DATE]
                                                     FROM [HR_PEOPLE]
                                                     WHERE DELETE_DATE IS NULL",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

            if (sqlDataReader.HasRows)
            {
                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    PersonEntity person = new PersonEntity();

                    person.Id = sqlDataReader.GetInt32("ID");
                    person.Name = sqlDataReader.GetString("NAME");

                    people.Add(person);
                }
            }

            return people;
        }

        /// <summary>
        /// Yeni kişi oluşturur
        /// </summary>
        /// <param name="person">Oluşturulacak kişi nesnesi</param><
        /// <param name="unitOfWork">Oluşturma esnasında kullanılacak transaction nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<int> CreateAsync(PersonEntity person, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [HR_PEOPLE]
                                                     ([NAME])
                                                     VALUES
                                                     (@NAME);
                                                     SELECT CAST(scope_identity() AS int)",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@NAME", ((object)person.Name) ?? DBNull.Value);

            return (int)await sqlCommand.ExecuteScalarAsync(cancellationToken);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    UnitOfWork.Dispose();
                }

                disposed = true;
            }
        }
    }
}
