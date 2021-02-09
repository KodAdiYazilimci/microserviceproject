namespace MicroserviceProject.Presentation.UI.Business.Model.Department.HR
{
    /// <summary>
    /// Çalışanın ünvanı
    /// </summary>
    public class TitleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
