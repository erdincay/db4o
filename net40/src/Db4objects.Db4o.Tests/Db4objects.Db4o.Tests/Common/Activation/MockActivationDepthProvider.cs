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
using Db4oUnit.Mocking;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Tests.Common.Activation
{
	/// <summary>
	/// An ActivationDepthProvider that records ActivationDepthProvider calls and
	/// delegates to another provider.
	/// </summary>
	/// <remarks>
	/// An ActivationDepthProvider that records ActivationDepthProvider calls and
	/// delegates to another provider.
	/// </remarks>
	public class MockActivationDepthProvider : MethodCallRecorder, IActivationDepthProvider
	{
		private readonly IActivationDepthProvider _delegate;

		public MockActivationDepthProvider()
		{
			_delegate = LegacyActivationDepthProvider.Instance;
		}

		public virtual IActivationDepth ActivationDepthFor(ClassMetadata classMetadata, ActivationMode
			 mode)
		{
			Record(new MethodCall("activationDepthFor", new object[] { classMetadata, mode })
				);
			return _delegate.ActivationDepthFor(classMetadata, mode);
		}

		public virtual IActivationDepth ActivationDepth(int depth, ActivationMode mode)
		{
			Record(new MethodCall("activationDepth", new object[] { depth, mode }));
			return _delegate.ActivationDepth(depth, mode);
		}
	}
}
