using System.Collections.Generic;

namespace MicroserviceProject.Common.Model.Communication.Validations
{
    public class Validation
    {
        public bool IsValid { get; set; }
        public List<ValidationItem> ValidationItems { get; set; }
    }
}
