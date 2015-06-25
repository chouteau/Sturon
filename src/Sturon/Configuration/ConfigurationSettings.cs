using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace Sturon.Configuration
{
	public static class ConfigurationSettings
	{
		public const string SECTION_NAME = "sturon";

		private static NameValueCollection m_AppSettings = null;

		public static NameValueCollection AppSettings
		{
			get
			{
				if (m_AppSettings == null)
				{
					m_AppSettings = (NameValueCollection)System.Configuration.ConfigurationManager.GetSection(SECTION_NAME);
					if (m_AppSettings == null)
					{
						throw new System.Configuration.ConfigurationErrorsException("sturon section not found");
					}
				}
				return m_AppSettings;
			}
		}
	}
}
