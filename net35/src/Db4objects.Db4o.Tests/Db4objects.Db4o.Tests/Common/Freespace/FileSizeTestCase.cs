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
using Db4objects.Db4o.Tests.Common.Freespace;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Freespace
{
	public class FileSizeTestCase : FreespaceManagerTestCaseBase
	{
		private const int Iterations = 100;

		public static void Main(string[] args)
		{
			new FileSizeTestCase().RunSolo();
		}

		public virtual void TestConsistentSizeOnDefragment()
		{
			StoreSomeItems();
			Db().Commit();
			AssertConsistentSize(new _IRunnable_20(this));
		}

		private sealed class _IRunnable_20 : IRunnable
		{
			public _IRunnable_20(FileSizeTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				try
				{
					this._enclosing.Defragment();
				}
				catch (Exception e)
				{
					Sharpen.Runtime.PrintStackTrace(e);
				}
			}

			private readonly FileSizeTestCase _enclosing;
		}

		public virtual void TestConsistentSizeOnRollback()
		{
			StoreSomeItems();
			ProduceSomeFreeSpace();
			AssertConsistentSize(new _IRunnable_34(this));
		}

		private sealed class _IRunnable_34 : IRunnable
		{
			public _IRunnable_34(FileSizeTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				this._enclosing.Store(new FreespaceManagerTestCaseBase.Item());
				this._enclosing.Db().Rollback();
			}

			private readonly FileSizeTestCase _enclosing;
		}

		public virtual void TestConsistentSizeOnCommit()
		{
			StoreSomeItems();
			Db().Commit();
			AssertConsistentSize(new _IRunnable_45(this));
		}

		private sealed class _IRunnable_45 : IRunnable
		{
			public _IRunnable_45(FileSizeTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				this._enclosing.Db().Commit();
			}

			private readonly FileSizeTestCase _enclosing;
		}

		public virtual void TestConsistentSizeOnUpdate()
		{
			StoreSomeItems();
			ProduceSomeFreeSpace();
			FreespaceManagerTestCaseBase.Item item = new FreespaceManagerTestCaseBase.Item();
			Store(item);
			Db().Commit();
			AssertConsistentSize(new _IRunnable_58(this, item));
		}

		private sealed class _IRunnable_58 : IRunnable
		{
			public _IRunnable_58(FileSizeTestCase _enclosing, FreespaceManagerTestCaseBase.Item
				 item)
			{
				this._enclosing = _enclosing;
				this.item = item;
			}

			public void Run()
			{
				this._enclosing.Store(item);
				this._enclosing.Db().Commit();
			}

			private readonly FileSizeTestCase _enclosing;

			private readonly FreespaceManagerTestCaseBase.Item item;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestConsistentSizeOnReopen()
		{
			Db().Commit();
			Reopen();
			AssertConsistentSize(new _IRunnable_69(this));
		}

		private sealed class _IRunnable_69 : IRunnable
		{
			public _IRunnable_69(FileSizeTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				try
				{
					this._enclosing.Reopen();
				}
				catch (Exception e)
				{
					Sharpen.Runtime.PrintStackTrace(e);
				}
			}

			private readonly FileSizeTestCase _enclosing;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestConsistentSizeOnUpdateAndReopen()
		{
			ProduceSomeFreeSpace();
			Store(new FreespaceManagerTestCaseBase.Item());
			Db().Commit();
			AssertConsistentSize(new _IRunnable_84(this));
		}

		private sealed class _IRunnable_84 : IRunnable
		{
			public _IRunnable_84(FileSizeTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				this._enclosing.Store(((FreespaceManagerTestCaseBase.Item)this._enclosing.RetrieveOnlyInstance
					(typeof(FreespaceManagerTestCaseBase.Item))));
				this._enclosing.Db().Commit();
				try
				{
					this._enclosing.Reopen();
				}
				catch (Exception e)
				{
					Sharpen.Runtime.PrintStackTrace(e);
				}
			}

			private readonly FileSizeTestCase _enclosing;
		}

		public virtual void AssertConsistentSize(IRunnable runnable)
		{
			Warmup(runnable);
			int originalFileSize = DatabaseFileSize();
			for (int i = 0; i < Iterations; i++)
			{
				//        	System.out.println(databaseFileSize());
				runnable.Run();
			}
			Assert.AreEqual(originalFileSize, DatabaseFileSize());
		}

		private void Warmup(IRunnable runnable)
		{
			for (int i = 0; i < 10; i++)
			{
				//        	System.out.println(databaseFileSize());
				runnable.Run();
			}
		}
	}
}
