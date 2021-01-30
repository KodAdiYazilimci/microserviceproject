using System;

namespace MicroserviceProject.Model.Security
{
    public class Token
    {
        public string TokenKey { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
