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
using Db4oUnit.Extensions;
using Db4oUnit.Mocking;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Tests.Common.Activation;

namespace Db4objects.Db4o.Tests.Common.Activation
{
	/// <summary>
	/// Ensures the container uses the provided ActivationDepthProvider instance
	/// whenever necessary.
	/// </summary>
	/// <remarks>
	/// Ensures the container uses the provided ActivationDepthProvider instance
	/// whenever necessary.
	/// </remarks>
	public class ActivationDepthProviderConfigTestCase : AbstractDb4oTestCase, IOptOutTA
	{
		public sealed class ItemRoot
		{
			public ActivationDepthProviderConfigTestCase.Item root;

			public ItemRoot(ActivationDepthProviderConfigTestCase.Item root_)
			{
				this.root = root_;
			}

			public ItemRoot()
			{
			}
		}

		public sealed class Item
		{
			public ActivationDepthProviderConfigTestCase.Item child;

			public Item()
			{
			}

			public Item(ActivationDepthProviderConfigTestCase.Item child_)
			{
				this.child = child_;
			}
		}

		private readonly MockActivationDepthProvider _dummyProvider = new MockActivationDepthProvider
			();

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			((Config4Impl)config).ActivationDepthProvider(_dummyProvider);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new ActivationDepthProviderConfigTestCase.ItemRoot(new ActivationDepthProviderConfigTestCase.Item
				(new ActivationDepthProviderConfigTestCase.Item(new ActivationDepthProviderConfigTestCase.Item
				()))));
		}

		public virtual void TestCSActivationDepthFor()
		{
			if (!IsNetworkCS())
			{
				return;
			}
			ResetProvider();
			QueryItem();
			AssertProviderCalled(new MethodCall[] { new MethodCall("activationDepthFor", new 
				object[] { ItemRootMetadata(), ActivationMode.Activate }) });
		}

		public virtual void TestSoloActivationDepthFor()
		{
			if (IsNetworkCS())
			{
				return;
			}
			ResetProvider();
			QueryItem();
			AssertProviderCalled("activationDepthFor", ItemRootMetadata(), ActivationMode.Activate
				);
		}

		public virtual void TestSpecificActivationDepth()
		{
			ActivationDepthProviderConfigTestCase.Item item = QueryItem();
			ResetProvider();
			Db().Activate(item, 3);
			AssertProviderCalled("activationDepth", 3, ActivationMode.Activate);
		}

		private bool IsNetworkCS()
		{
			return IsMultiSession() && !IsEmbedded();
		}

		private ClassMetadata ItemRootMetadata()
		{
			return ClassMetadataFor(typeof(ActivationDepthProviderConfigTestCase.ItemRoot));
		}

		private void AssertProviderCalled(MethodCall[] expectedCalls)
		{
			_dummyProvider.Verify(expectedCalls);
		}

		private void AssertProviderCalled(string methodName, object arg1, object arg2)
		{
			_dummyProvider.Verify(new MethodCall[] { new MethodCall(methodName, new object[] 
				{ arg1, arg2 }) });
		}

		private void ResetProvider()
		{
			_dummyProvider.Reset();
		}

		private ActivationDepthProviderConfigTestCase.Item QueryItem()
		{
			return ((ActivationDepthProviderConfigTestCase.ItemRoot)((ActivationDepthProviderConfigTestCase.ItemRoot
				)RetrieveOnlyInstance(typeof(ActivationDepthProviderConfigTestCase.ItemRoot)))).
				root;
		}
	}
}
