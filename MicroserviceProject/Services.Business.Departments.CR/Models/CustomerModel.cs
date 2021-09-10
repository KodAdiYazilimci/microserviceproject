namespace Services.Business.Departments.CR.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }

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
