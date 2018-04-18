using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sturon
{
	public class PluginService
	{
		private static Lazy<PluginService> m_LazyPluginService = new Lazy<PluginService>(() =>
			{
				return new PluginService();
			}, true);

		private PluginService()
		{
			Plugins = new List<PluginItem>();
			Builders = new List<PluginBuilder>();
		}

		private static PluginService Current
		{
			get
			{
				return m_LazyPluginService.Value;
			}
		}

		internal List<PluginItem> Plugins { get; set; } 
		internal List<PluginBuilder> Builders { get; set; }

		public static void RegisterServices()
		{
			// Merge Config file with existing list
			var pluginsFromConfig = LoadConfiguration();
			if (pluginsFromConfig != null && pluginsFromConfig.Count > 0)
			{
				foreach (var plugin in pluginsFromConfig)
				{
					RegisterPlugin(plugin);
				}
			}

			Current.Builders = new List<PluginBuilder>();
			foreach (var item in Current.Plugins)
			{
				var asm = System.Reflection.Assembly.LoadFrom(item.AssemblyFileName);
				var meta = new PluginBuilder(asm);

				meta.AddServices();
				Current.Builders.Add(meta);
			}
		}

		public static void StartServices()
		{
			if (Current.Builders == null)
			{
				return;
			}
			foreach (var builder in Current.Builders)
			{
				builder.StartModuleClasses();
			}
		}

		public static void StopServices()
		{
			if (Current.Builders == null)
			{
				return;
			}
			foreach (var builder in Current.Builders)
			{
				builder.StopModuleClasses();
			}
		}

		public static void RegisterPlugin(string assemblyFileName)
		{
			if (string.IsNullOrWhiteSpace(assemblyFileName))
			{
				return;
			}
			var plugin = new PluginItem()
			{
				AssemblyFileName = assemblyFileName
			};
			RegisterPlugin(plugin);
		}

		static void RegisterPlugin(PluginItem item)
		{
			if (Current.Plugins.Any(i => i.AssemblyFileName.Equals(item.AssemblyFileName, StringComparison.InvariantCultureIgnoreCase)))
			{
				return;
			}
			Current.Plugins.Add(item);
		}

		static List<PluginItem> LoadConfiguration()
		{
			var result = new List<PluginItem>();
			var settings = Configuration.ConfigurationSettings.AppSettings;
			if (settings == null)
			{
				return result;
			}
			foreach (var key in Configuration.ConfigurationSettings.AppSettings.AllKeys)
			{
				var value = Configuration.ConfigurationSettings.AppSettings[key];
				result.Add(new PluginItem()
					{
						AssemblyFileName = value
					});
			}
			return result;
		}

	}
}
