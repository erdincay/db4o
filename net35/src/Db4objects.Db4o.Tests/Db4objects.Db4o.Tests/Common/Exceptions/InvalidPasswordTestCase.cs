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
#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class InvalidPasswordTestCase : Db4oClientServerTestCase, IOptOutAllButNetworkingCS
	{
		public virtual void TestInvalidPassword()
		{
			int port = ClientServerFixture().ServerPort();
			Assert.Expect(typeof(InvalidPasswordException), new _ICodeBlock_20(this, port));
		}

		private sealed class _ICodeBlock_20 : ICodeBlock
		{
			public _ICodeBlock_20(InvalidPasswordTestCase _enclosing, int port)
			{
				this._enclosing = _enclosing;
				this.port = port;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.OpenClient("127.0.0.1", port, "strangeusername", "invalidPassword"
					);
			}

			private readonly InvalidPasswordTestCase _enclosing;

			private readonly int port;
		}

		protected virtual IObjectContainer OpenClient(string host, int port, string user, 
			string password)
		{
			return Db4oClientServer.OpenClient(host, port, user, password);
		}

		public virtual void TestEmptyUserPassword()
		{
			int port = ClientServerFixture().ServerPort();
			Assert.Expect(typeof(InvalidPasswordException), new _ICodeBlock_35(this, port));
		}

		private sealed class _ICodeBlock_35 : ICodeBlock
		{
			public _ICodeBlock_35(InvalidPasswordTestCase _enclosing, int port)
			{
				this._enclosing = _enclosing;
				this.port = port;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.OpenClient("127.0.0.1", port, string.Empty, string.Empty);
			}

			private readonly InvalidPasswordTestCase _enclosing;

			private readonly int port;
		}

		public virtual void TestEmptyUserNullPassword()
		{
			int port = ClientServerFixture().ServerPort();
			Assert.Expect(typeof(InvalidPasswordException), new _ICodeBlock_44(this, port));
		}

		private sealed class _ICodeBlock_44 : ICodeBlock
		{
			public _ICodeBlock_44(InvalidPasswordTestCase _enclosing, int port)
			{
				this._enclosing = _enclosing;
				this.port = port;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.OpenClient("127.0.0.1", port, string.Empty, null);
			}

			private readonly InvalidPasswordTestCase _enclosing;

			private readonly int port;
		}

		public virtual void TestNullPassword()
		{
			int port = ClientServerFixture().ServerPort();
			Assert.Expect(typeof(InvalidPasswordException), new _ICodeBlock_53(this, port));
		}

		private sealed class _ICodeBlock_53 : ICodeBlock
		{
			public _ICodeBlock_53(InvalidPasswordTestCase _enclosing, int port)
			{
				this._enclosing = _enclosing;
				this.port = port;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.OpenClient("127.0.0.1", port, null, null);
			}

			private readonly InvalidPasswordTestCase _enclosing;

			private readonly int port;
		}
	}
}
#endif // !SILVERLIGHT
