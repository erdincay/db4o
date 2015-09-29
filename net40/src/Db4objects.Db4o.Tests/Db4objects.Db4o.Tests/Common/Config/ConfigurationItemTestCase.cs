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
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.Config;

namespace Db4objects.Db4o.Tests.Common.Config
{
	public class ConfigurationItemTestCase : Db4oTestWithTempFile
	{
		internal sealed class ConfigurationItemStub : IConfigurationItem
		{
			private IInternalObjectContainer _container;

			private IConfiguration _configuration;

			public void Apply(IInternalObjectContainer container)
			{
				Assert.IsNotNull(container);
				_container = container;
			}

			public void Prepare(IConfiguration configuration)
			{
				Assert.IsNotNull(configuration);
				_configuration = configuration;
			}

			public IConfiguration PreparedConfiguration()
			{
				return _configuration;
			}

			public IInternalObjectContainer AppliedContainer()
			{
				return _container;
			}
		}

		public virtual void Test()
		{
			IEmbeddedConfiguration configuration = NewConfiguration();
			ConfigurationItemTestCase.ConfigurationItemStub item = new ConfigurationItemTestCase.ConfigurationItemStub
				();
			configuration.Common.Add(item);
			Assert.AreSame(LegacyConfigFor(configuration), item.PreparedConfiguration());
			Assert.IsNull(item.AppliedContainer());
			File4.Delete(TempFile());
			IObjectContainer container = Db4oEmbedded.OpenFile(configuration, TempFile());
			container.Close();
			Assert.AreSame(container, item.AppliedContainer());
		}

		private IConfiguration LegacyConfigFor(IEmbeddedConfiguration configuration)
		{
			EmbeddedConfigurationImpl configImpl = (EmbeddedConfigurationImpl)configuration;
			return configImpl.Legacy();
		}
	}
}
