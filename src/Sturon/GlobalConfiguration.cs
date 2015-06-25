using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sturon
{
	public class GlobalConfiguration
	{
		static GlobalConfiguration()
		{
			Logger = new DiagnosticsLogger();
			DependencyResolver = new DefaultDependencyResolver();
		}

		public static ILogger Logger { get; set; }
		public static IDependencyResolver DependencyResolver { get; set; }

	}
}
