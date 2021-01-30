using System;
using System.Collections.Generic;
using System.Text;

namespace MicroserviceProject.Infrastructure.Security.BasicTokenAuthentication.Abstracts
{
    public interface IIdentifiable
    {
        Guid Identifier { get; }
    }
}
