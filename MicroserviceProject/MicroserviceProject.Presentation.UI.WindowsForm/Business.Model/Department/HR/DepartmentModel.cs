namespace MicroserviceProject.Presentation.UI.Business.Model.Department.HR
{
    /// <summary>
    /// Departmanlar
    /// </summary>
    public class DepartmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Departmanın yöneticisi
        /// </summary>
        public WorkerModel Manager { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
