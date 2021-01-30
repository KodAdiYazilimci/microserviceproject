using System;
using System.Collections.Generic;
using System.Text;

namespace MicroserviceProject.Common.Model.Communication.Basics
{
	public class ServiceResult<T> : ServiceResult
	{
		public T Data { get; set; }
	}
}
