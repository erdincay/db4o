/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2011  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
using System;
using System.Resources;
using System.Threading;
using System.Globalization;
using OMControlLibrary.Common;

namespace OMControlLibrary
{
	public class ApplicationManager
	{

		#region Private Member

		//Resource related member variables
		private static ResourceManager m_ResourceManager;
		private static LanguageCodes m_SelectedLanguage = LanguageCodes.English;

		#endregion


		#region Properties

		public static ResourceManager ResourceManager
		{
			get
			{
				return m_ResourceManager;
			}
		}


		#endregion


		#region Public Methods

		public static bool CheckLocalAndSetLanguage()
		{
			try
			{
				if (CultureInfo.CurrentCulture.Name.Equals(Common.Constants.JAPANESE_CULTURE))
					return SetLanguage(LanguageCodes.Japanese);
				
				return SetLanguage(LanguageCodes.English);
			}
			catch (Exception)
			{
				return false;
			}
		}


		/// <summary>
		/// This function sets the current UI culture and resource manager
		/// </summary>
		/// <param name="lngCode">LanguageCode</param>
		/// <returns>True or False.</returns>
		static internal bool SetLanguage(LanguageCodes lngCode)
		{
			try
			{
				string languageCulture;
				if (lngCode == LanguageCodes.English)
				{
					m_ResourceManager = new ResourceManager(Constants.RESOURCE_NAME, typeof(ViewBase).Assembly);
					languageCulture = Common.Constants.ENGLISH_CULTURE;
				}
				else if (lngCode == LanguageCodes.Japanese)
				{
					m_ResourceManager = new ResourceManager(Constants.RESOURCE_NAME_JP, typeof(ViewBase).Assembly);
					languageCulture = Common.Constants.JAPANESE_CULTURE;
				}
				else
				{
					m_ResourceManager = new ResourceManager(Constants.RESOURCE_NAME, typeof(ViewBase).Assembly);
					languageCulture = Common.Constants.ENGLISH_CULTURE;
				}

				const bool languageStatus = true;
				m_SelectedLanguage = lngCode;
				return languageStatus;
			}
			catch (Exception)
			{
				return false;
			}
		}//End of the function SetLnaguage.

		#endregion
	}
}
