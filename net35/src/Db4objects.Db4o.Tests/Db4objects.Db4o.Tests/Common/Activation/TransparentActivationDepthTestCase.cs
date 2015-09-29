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
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.Activation;

namespace Db4objects.Db4o.Tests.Common.Activation
{
	public class TransparentActivationDepthTestCase : AbstractDb4oTestCase
	{
		public sealed class NonTAAware
		{
		}

		public sealed class TAAware : IActivatable
		{
			public void Activate(ActivationPurpose purpose)
			{
			}

			public void Bind(IActivator activator)
			{
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			// configured depth should be ignored by ta provider
			config.ObjectClass(typeof(TransparentActivationDepthTestCase.TAAware)).MinimumActivationDepth
				(42);
			config.ObjectClass(typeof(TransparentActivationDepthTestCase.NonTAAware)).MinimumActivationDepth
				(42);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new TransparentActivationDepthTestCase.TAAware());
			Store(new TransparentActivationDepthTestCase.NonTAAware());
		}

		public virtual void TestDescendingFromNonTAAwareToTAAware()
		{
			IActivationDepth depth = NonTAAwareDepth();
			IActivationDepth child = depth.Descend(ClassMetadataFor(typeof(TransparentActivationDepthTestCase.TAAware
				)));
			Assert.IsFalse(child.RequiresActivation());
		}

		public virtual void TestDefaultActivationNonTAAware()
		{
			IActivationDepth depth = NonTAAwareDepth();
			Assert.IsTrue(depth.RequiresActivation());
			IActivationDepth child = depth.Descend(ClassMetadataFor(typeof(TransparentActivationDepthTestCase.NonTAAware
				)));
			Assert.IsTrue(child.RequiresActivation());
		}

		public virtual void TestDefaultActivationTAAware()
		{
			IActivationDepth depth = TAAwareDepth();
			Assert.IsFalse(depth.RequiresActivation());
		}

		public virtual void TestSpecificActivationDepth()
		{
			IActivationDepth depth = Provider().ActivationDepth(3, ActivationMode.Activate);
			AssertDescendingDepth(3, depth, typeof(TransparentActivationDepthTestCase.TAAware
				));
			AssertDescendingDepth(3, depth, typeof(TransparentActivationDepthTestCase.NonTAAware
				));
		}

		public virtual void TestIntegerMaxValueMeansFull()
		{
			AssertFullActivationDepthForMaxValue(ActivationMode.Peek);
			AssertFullActivationDepthForMaxValue(ActivationMode.Activate);
		}

		private void AssertFullActivationDepthForMaxValue(ActivationMode mode)
		{
			Assert.IsInstanceOf(typeof(FullActivationDepth), Provider().ActivationDepth(int.MaxValue
				, mode));
		}

		private void AssertDescendingDepth(int expectedDepth, IActivationDepth depth, Type
			 clazz)
		{
			if (expectedDepth < 1)
			{
				Assert.IsFalse(depth.RequiresActivation());
				return;
			}
			Assert.IsTrue(depth.RequiresActivation());
			AssertDescendingDepth(expectedDepth - 1, depth.Descend(ClassMetadataFor(clazz)), 
				clazz);
		}

		private IActivationDepth NonTAAwareDepth()
		{
			return TransparentActivationDepthFor(typeof(TransparentActivationDepthTestCase.NonTAAware
				));
		}

		private IActivationDepth TransparentActivationDepthFor(Type clazz)
		{
			return Provider().ActivationDepthFor(ClassMetadataFor(clazz), ActivationMode.Activate
				);
		}

		private TransparentActivationDepthProviderImpl Provider()
		{
			return new TransparentActivationDepthProviderImpl();
		}

		private IActivationDepth TAAwareDepth()
		{
			return TransparentActivationDepthFor(typeof(TransparentActivationDepthTestCase.TAAware
				));
		}
	}
}
