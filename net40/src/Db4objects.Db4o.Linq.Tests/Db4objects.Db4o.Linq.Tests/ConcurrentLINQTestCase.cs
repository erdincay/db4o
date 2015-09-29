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
#if !SILVERLIGHT && !CF

using System;
using System.Collections.Generic;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.IO;
using Db4oUnit;

namespace Db4objects.Db4o.Linq.Tests
{

	public class ConcurrentLinqTestCase : AbstractDb4oLinqTestCase
	{
		private const int AmoutOfTestData = 500;
		private const int QueryRuns = AmoutOfTestData / 10;
		private const int NumberOfTasks = 2;

		private IObjectServer testDB;

		protected override void Db4oSetupAfterStore()
		{
			var cfg = Db4oClientServer.NewServerConfiguration();
			cfg.File.Storage = new MemoryStorage();
			testDB = Db4oClientServer.OpenServer(cfg, "No:File:Expected", 0);
			StoreTestData(testDB);
		}

		public void TestRunLinqLinear()
		{
			var result = Queries();
			Assert.IsNull(result.Exception);
			Assert.IsTrue(result.Result);
		}

		public void TestConcurrentLinq()
		{
			Func<OperationResult<bool>> threadStart = Queries;
			var result = StartUpLinqQueries(threadStart, NumberOfTasks);

			EndAndAssert(threadStart, result);
		}

		private void StoreTestData(IObjectServer server)
		{
			using (var db = server.OpenClient())
			{
				foreach (var person in GenerateTestData())
				{
					db.Store(person);
				}
				db.Commit();
			}
		}


		private OperationResult<bool> Queries()
		{
			using (var db = testDB.OpenClient())
			{
				for (var i = 0; i < QueryRuns; i++)
				{
					try
					{
						QueriesToRun(db, i);
					}
					catch (Exception e)
					{
						return OperationResult.Failure<bool>(e);
					}
				}
				return OperationResult.Success(true);
			}
		}

		private static void QueriesToRun(IObjectContainer db, int number)
		{
			{
				var result = from SimpleObject p in db
							 where p.Number > number && p.Number < AmoutOfTestData
							 orderby p.Number ascending
							 select p;
				Assert.AreEqual(AmoutOfTestData - 1 - number, result.Count());
			}
			{
				var result = from SimpleObject p in db
							 where p.Number > number && p.Number < AmoutOfTestData
							 orderby p.Number descending
							 select p;
				Assert.AreEqual(AmoutOfTestData - 1 - number, result.Count());
			}
			var result2 = from SimpleObject p in db
						  where p.Number == number
						  select p;

			Assert.AreEqual(1, result2.Count());
		}

		private static void EndAndAssert(Func<OperationResult<bool>> threadStart, IEnumerable<IAsyncResult> results)
		{
			foreach (var result in results)
			{
				var operationResult = threadStart.EndInvoke(result);
				Assert.IsNull(operationResult.Exception);
				Assert.IsTrue(operationResult.Result);
			}
		}


		private static IEnumerable<IAsyncResult> StartUpLinqQueries(Func<OperationResult<bool>> queriesToRun, int amout)
		{
			var result = new List<IAsyncResult>();
			for (var i = 0; i < amout; i++)
			{
				result.Add(queriesToRun.BeginInvoke(null, null));
			}
			return result;
		}

		private IEnumerable<SimpleObject> GenerateTestData()
		{
			for (int i = 0; i < AmoutOfTestData; i++)
			{
				yield return new SimpleObject(i);
			}
		}
	}

	public static class OperationResult
	{
		public static OperationResult<T> Success<T>(T result)
		{
			return new OperationResult<T>(result);
		}

		public static OperationResult<T> Failure<T>(Exception exception)
		{
			return new OperationResult<T>(exception);
		}
	}

	public struct OperationResult<T>
	{
		public OperationResult(T result) : this()
		{
			Result = result;
		}

		public OperationResult(Exception execption) : this()
		{
			Exception = execption;
		}

		public T Result { get; private set; }
		public Exception Exception { get; private set; }
	}

	internal class SimpleObject
	{
		public SimpleObject(int number)
		{
			Number = number;
		}

		public int Number { get; set; }
	}
}

#endif