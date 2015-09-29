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
using System.Text;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class StringBufferHandlerTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new StringBufferHandlerTestCase().RunAll();
		}

		public class Item
		{
			public StringBuilder buffer;

			public Item(StringBuilder contents)
			{
				buffer = contents;
			}
		}

		internal static string _bufferValue = "42";

		//$NON-NLS-1$
		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ExceptionsOnNotStorable(true);
			config.RegisterTypeHandler(new SingleClassTypeHandlerPredicate(typeof(StringBuilder
				)), new StringBufferHandler());
			config.Diagnostic().AddListener(new _IDiagnosticListener_36());
		}

		private sealed class _IDiagnosticListener_36 : IDiagnosticListener
		{
			public _IDiagnosticListener_36()
			{
			}

			public void OnDiagnostic(IDiagnostic d)
			{
				if (d is DeletionFailed)
				{
					throw new Db4oException();
				}
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new StringBufferHandlerTestCase.Item(new StringBuilder(_bufferValue)));
		}

		public virtual void TestRetrieve()
		{
			StringBufferHandlerTestCase.Item item = RetrieveItem();
			Assert.AreEqual(_bufferValue, item.buffer.ToString());
		}

		public virtual void TestTopLevelStore()
		{
			Assert.Expect(typeof(ObjectNotStorableException), new _ICodeBlock_55(this));
		}

		private sealed class _ICodeBlock_55 : ICodeBlock
		{
			public _ICodeBlock_55(StringBufferHandlerTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Store(new StringBuilder("a"));
			}

			private readonly StringBufferHandlerTestCase _enclosing;
		}

		//$NON-NLS-1$
		public virtual void TestStringBufferQuery()
		{
			IQuery query = NewItemQuery();
			query.Descend("buffer").Constrain(new StringBuilder(_bufferValue));
			Assert.AreEqual(1, query.Execute().Count);
		}

		public virtual void TestDelete()
		{
			StringBufferHandlerTestCase.Item item = RetrieveItem();
			Assert.AreEqual(_bufferValue, item.buffer.ToString());
			Db().Delete(item);
			IQuery query = NewItemQuery();
			Assert.AreEqual(0, query.Execute().Count);
		}

		private IQuery NewItemQuery()
		{
			IQuery query = NewQuery();
			query.Constrain(typeof(StringBufferHandlerTestCase.Item));
			return query;
		}

		public virtual void TestPrepareComparison()
		{
			StringBufferHandler handler = new StringBufferHandler();
			IPreparedComparison preparedComparison = handler.PrepareComparison(Trans().Context
				(), _bufferValue);
			Assert.IsGreater(preparedComparison.CompareTo("43"), 0);
		}

		//$NON-NLS-1$
		public virtual void TestStoringStringBufferDirectly()
		{
			Assert.Expect(typeof(ObjectNotStorableException), new _ICodeBlock_89(this));
		}

		private sealed class _ICodeBlock_89 : ICodeBlock
		{
			public _ICodeBlock_89(StringBufferHandlerTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				StringBuilder stringBuffer = new StringBuilder(StringBufferHandlerTestCase._bufferValue
					);
				this._enclosing.Store(stringBuffer);
			}

			private readonly StringBufferHandlerTestCase _enclosing;
		}

		private StringBufferHandlerTestCase.Item RetrieveItem()
		{
			return (StringBufferHandlerTestCase.Item)((StringBufferHandlerTestCase.Item)RetrieveOnlyInstance
				(typeof(StringBufferHandlerTestCase.Item)));
		}
	}
}
