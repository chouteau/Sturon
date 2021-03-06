﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sturon
{
	public interface IDependencyResolver
	{
		object GetService(Type serviceType);
		IEnumerable<object> GetServices(Type serviceType);
		IEnumerable<object> GetAllServices();
		IDisposable SturonContainer { get; }
	}
}
