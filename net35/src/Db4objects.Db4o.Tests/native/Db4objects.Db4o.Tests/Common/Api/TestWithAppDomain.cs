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
using Db4oUnit.Extensions.Util;

namespace Db4objects.Db4o.Tests.Common.Api
{
#if !CF && !SILVERLIGHT
	public class TestWithAppDomain : TestWithTempFile
	{
		protected AppDomain _domain;

		public override void SetUp()
		{
			base.SetUp();

			string applicationBase = Path.Combine(Path.GetDirectoryName(TempFile()), GetType().Name);
			IOServices.CopyEnclosingAssemblyTo(typeof(Db4oEmbedded), applicationBase);

			AppDomainSetup setup = new AppDomainSetup();
			setup.ApplicationBase = applicationBase;
			
			_domain = AppDomain.CreateDomain(GetType().Name, null, setup);

		}

		protected string Db4oAssemblyPath()
		{
			return typeof(Db4oEmbedded).Module.FullyQualifiedName;
		}

		public override void TearDown()
		{
			AppDomain.Unload(_domain);
			base.TearDown();
		}

		protected void ExecuteAssemblyInAppDomain(string assemblyFile, params string[] args)
		{
			_domain.ExecuteAssembly(assemblyFile, null, args);
		}
	}
#endif
}
