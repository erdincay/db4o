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
using System.IO;
using OME.Crypto;

namespace OMControlLibrary.LoginToSalesForce
{
	public class CustomCookies
	{
		private readonly CryptoDES objCryptoDES = new CryptoDES();

		public CustomCookies()
		{
			objCryptoDES.Initialize();
		}

		public void SetCookies(string content)
		{
#if DEBUG

            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects" + Path.DirectorySeparatorChar + "ObjectManagerEnterprise"))
            {

                //Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects");
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects" + Path.DirectorySeparatorChar + "ObjectManagerEnterprise");

            }
#endif
			string filepath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects" + Path.DirectorySeparatorChar + "ObjectManagerEnterprise" + Path.DirectorySeparatorChar + "encyr.info";
			FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);
			string encryptSTR = objCryptoDES.DESSelfEncrypt(content);
			byte[] contents = StrToByteArray(encryptSTR);
			fs.Write(contents, 0, contents.Length);
			fs.Close();
		}

		private byte[] StrToByteArray(string str)
		{
			System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
			return encoding.GetBytes(str);
		}

		public string GetCookies()
		{
#if DEBUG

            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects" + Path.DirectorySeparatorChar + "ObjectManagerEnterprise"))
            {

                //Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects");
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects" + Path.DirectorySeparatorChar + "ObjectManagerEnterprise");

            }
#endif
			string filepath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects" + Path.DirectorySeparatorChar + "ObjectManagerEnterprise" + Path.DirectorySeparatorChar + "encyr.info";

			if (File.Exists(filepath))
			{
				byte[] contents;
				using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
				{
					contents = new byte[fs.Length];
					fs.Read(contents, 0, (int)fs.Length);
				}

				if (contents.Length > 0)
				{
					string info = ByteArrayToStr(contents);
					return objCryptoDES.DESSelfDecrypt(info);
				}
			}
			return null;
		}

		private string ByteArrayToStr(byte[] array)
		{
			System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
			return encoding.GetString(array);
		}

	}
}
