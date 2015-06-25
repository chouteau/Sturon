using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sturon
{
	class PluginBuilder
	{
		Assembly assembly;
		bool loadedServices = false;
		bool initialized = false;
		bool routesRegistered = false;
		string name = null;

		List<Type> pluginTypes = new List<Type>();
		List<PluginBase> pluginClasses = new List<PluginBase>();

		public PluginBuilder(Assembly assembly)
		{
			this.assembly = assembly;

			foreach (Type type in assembly.GetExportedTypes())
			{
				if (!type.IsAbstract && typeof(PluginBase).IsAssignableFrom(type))
				{
					pluginTypes.Add(type);
				}
			}
		}

		public string Name
		{
			get
			{
				if (name == null)
				{
					name = assembly.FullName;
				}

				return name;
			}
			set { name = value; }
		}

		public void AddServices()
		{
			if (loadedServices)
			{
				return;
			}

			loadedServices = true;
			EnsurePluginClassesExist();
			foreach (var pluginClass in pluginClasses)
			{
				pluginClass.RegisterServices();
			}
		}

		public void StartModuleClasses()
		{
			if (initialized)
			{
				return;
			}

			initialized = true;
			EnsurePluginClassesExist();

			foreach (var plugin in pluginClasses)
			{
				plugin.Start();
			}
		}

		public void StopModuleClasses()
		{
			if (!initialized)
			{
				return;
			}
			foreach (var plugin in pluginClasses)
			{
				plugin.Stop();
			}
		}


		public void EnsurePluginClassesExist()
		{
			if (pluginClasses.Count == pluginTypes.Count)
			{
				return;
			}

			foreach (Type pluginType in pluginTypes)
			{
				var plugin =  GlobalConfiguration.DependencyResolver.GetService(pluginType) as PluginBase;
				pluginClasses.Add(plugin);
			}
		}

	}
}
