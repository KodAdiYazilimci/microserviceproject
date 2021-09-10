using Services.Business.Departments.CR.Constants;

namespace Services.Business.Departments.CR.Entities.EntityFramework
{
    /// <summary>
    /// Müşteri entity sınıfı
    /// </summary>
    public class CustomerEntity : BaseEntity
    {
        /// <summary>
        /// Müşteri tipi
        /// </summary>
        public PersonType TypeOfPerson
        {
            get
            {
                return IsLegal ? PersonType.Legal : PersonType.Natural;
            }
            set
            {
                IsLegal = value == PersonType.Legal;
            }
        }

        /// <summary>
        /// Müşterinin tüzel kişi olup olmadığı bilgisi
        /// </summary>
        public bool IsLegal { get; set; }

        /// <summary>
        /// Müşterinin adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Müşterinin ikinci adı
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Müşterinin soyadı
        /// </summary>
        public string Surname { get; set; }
    }
}
