using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sturon
{
	public abstract class PluginBase
	{
		public virtual void RegisterServices()
		{
		}

		public virtual void Start()
		{
		}

		public virtual void Stop()
		{
		}
	}
}
