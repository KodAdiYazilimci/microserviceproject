namespace MicroserviceProject.Presentation.UI.Business.Model.Department.HR
{
    /// <summary>
    /// Kişiler
    /// </summary>
    public class PersonModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
