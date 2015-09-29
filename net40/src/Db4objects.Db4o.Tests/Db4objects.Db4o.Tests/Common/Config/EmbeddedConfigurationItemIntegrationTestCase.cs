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
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.Config;

namespace Db4objects.Db4o.Tests.Common.Config
{
	public class EmbeddedConfigurationItemIntegrationTestCase : ITestCase
	{
		public virtual void Test()
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.File.Storage = new MemoryStorage();
			EmbeddedConfigurationItemIntegrationTestCase.DummyConfigurationItem item = new EmbeddedConfigurationItemIntegrationTestCase.DummyConfigurationItem
				(this);
			config.AddConfigurationItem(item);
			IEmbeddedObjectContainer container = Db4oEmbedded.OpenFile(config, string.Empty);
			item.Verify(config, container);
			container.Close();
		}

		private sealed class DummyConfigurationItem : IEmbeddedConfigurationItem
		{
			private int _prepareCount = 0;

			private int _applyCount = 0;

			private IEmbeddedConfiguration _config;

			private IEmbeddedObjectContainer _container;

			public void Prepare(IEmbeddedConfiguration configuration)
			{
				this._config = configuration;
				this._prepareCount++;
			}

			public void Apply(IEmbeddedObjectContainer container)
			{
				this._container = container;
				this._applyCount++;
			}

			internal void Verify(IEmbeddedConfiguration config, IEmbeddedObjectContainer container
				)
			{
				Assert.AreSame(config, this._config);
				Assert.AreSame(container, this._container);
				Assert.AreEqual(1, this._prepareCount);
				Assert.AreEqual(1, this._applyCount);
			}

			internal DummyConfigurationItem(EmbeddedConfigurationItemIntegrationTestCase _enclosing
				)
			{
				this._enclosing = _enclosing;
			}

			private readonly EmbeddedConfigurationItemIntegrationTestCase _enclosing;
		}
	}
}
