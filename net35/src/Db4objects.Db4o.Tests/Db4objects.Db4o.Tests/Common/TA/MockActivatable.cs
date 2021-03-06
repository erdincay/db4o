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
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.Tests.Common.TA
{
	public class MockActivatable : IActivatable
	{
		[System.NonSerialized]
		private MethodCallRecorder _recorder;

		public virtual MethodCallRecorder Recorder()
		{
			if (null == _recorder)
			{
				_recorder = new MethodCallRecorder();
			}
			return _recorder;
		}

		public virtual void Bind(IActivator activator)
		{
			Record(new MethodCall("bind", new object[] { activator }));
		}

		public virtual void Activate(ActivationPurpose purpose)
		{
			Record(new MethodCall("activate", new object[] { purpose }));
		}

		private void Record(MethodCall methodCall)
		{
			Recorder().Record(methodCall);
		}
	}
}
