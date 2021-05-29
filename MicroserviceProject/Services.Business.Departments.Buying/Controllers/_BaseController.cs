using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Mvc;

namespace Services.Business.Departments.Buying.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// İş mantığı servislerinin varsayılanlarını ayarlar
        /// </summary>
        /// <param name="services">Varsayılanları ayarlanacak iş mantığı servisleri</param>
        protected void SetServiceDefaults(params BaseService[] services)
        {
            if (Request != null)
            {
                foreach (var service in services)
                {
                    if (Request.Headers.ContainsKey("TransactionIdentity"))
                        service.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();

                    if (Request.Headers.ContainsKey("Region"))
                        service.Region = Request.Headers["Region"].ToString();
                }
            }
        }
    }
}
