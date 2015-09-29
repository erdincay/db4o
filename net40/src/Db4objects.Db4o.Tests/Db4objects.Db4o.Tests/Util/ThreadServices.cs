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
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Util
{
	public class ThreadServices
	{
		/// <exception cref="System.Exception"></exception>
		public static void SpawnAndJoin(string threadName, int threadCount, ICodeBlock codeBlock
			)
		{
			Thread[] threads = new Thread[threadCount];
			for (int i = 0; i < threadCount; i++)
			{
				threads[i] = new Thread(new _IRunnable_16(codeBlock), threadName);
				threads[i].Start();
			}
			for (int i = 0; i < threads.Length; i++)
			{
				threads[i].Join();
			}
		}

		private sealed class _IRunnable_16 : IRunnable
		{
			public _IRunnable_16(ICodeBlock codeBlock)
			{
				this.codeBlock = codeBlock;
			}

			public void Run()
			{
				try
				{
					codeBlock.Run();
				}
				catch (Exception t)
				{
					Sharpen.Runtime.PrintStackTrace(t);
				}
			}

			private readonly ICodeBlock codeBlock;
		}
	}
}
