using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sturon
{
	public class PluginService
	{
		internal Lazy<List<PluginItem>> m_LazyLoader = new Lazy<List<PluginItem>>(LoadConfiguration, true);
		private static Lazy<PluginService> m_LazyPluginService = new Lazy<PluginService>(() =>
			{
				return new PluginService();
			}, true);

		private PluginService()
		{
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
			Current.Plugins = Current.m_LazyLoader.Value;
			if (Current.Plugins == null)
			{
				return;
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

		static List<PluginItem> LoadConfiguration()
		{
			var result = new List<PluginItem>();
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
