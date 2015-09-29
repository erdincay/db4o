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
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Config;
using Db4objects.Db4o.Tests.Common.Api;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Api
{
	public class EnvironmentConfigurationTestCase : AbstractInMemoryDb4oTestCase
	{
		public virtual void Test()
		{
			Container().WithEnvironment(new _IRunnable_13(this));
		}

		private sealed class _IRunnable_13 : IRunnable
		{
			public _IRunnable_13(EnvironmentConfigurationTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				Assert.AreSame(this._enclosing._service, ((EnvironmentConfigurationTestCase.IServiceInterface
					)Environments.My(typeof(EnvironmentConfigurationTestCase.IServiceInterface))));
			}

			private readonly EnvironmentConfigurationTestCase _enclosing;
		}

		public interface IServiceInterface
		{
		}

		private sealed class _IServiceInterface_23 : EnvironmentConfigurationTestCase.IServiceInterface
		{
			public _IServiceInterface_23()
			{
			}
		}

		private EnvironmentConfigurationTestCase.IServiceInterface _service = new _IServiceInterface_23
			();

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration legacy)
		{
			base.Configure(legacy);
			ICommonConfiguration common = Db4oLegacyConfigurationBridge.AsCommonConfiguration
				(legacy);
			common.Environment.Add(_service);
		}
	}
}
