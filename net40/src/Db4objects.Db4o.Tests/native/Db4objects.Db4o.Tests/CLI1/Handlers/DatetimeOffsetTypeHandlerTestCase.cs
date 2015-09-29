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
#if !CF && !SILVERLIGHT

using System;
using Db4objects.Db4o.Query;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
	public class DatetimeOffsetTypeHandlerTestCase : ValueTypeHandlerTestCaseBase<DateTimeOffset>
	{
		private static readonly ValueTypeHolder[] Objects = new ValueTypeHolder[]
		                                           	{
		                                           		new ValueTypeHolder(DateTimeOffset.MinValue, new ValueTypeHolder(DateTimeOffset.Now.AddDays(1))), 	
														new ValueTypeHolder(DateTimeOffset.Now, new ValueTypeHolder(DateTimeOffset.Now.AddDays(2))), 	
														new ValueTypeHolder(DateTimeOffset.MaxValue, new ValueTypeHolder(DateTimeOffset.Now.AddDays(3))), 	
													};

		protected override ValueTypeHolder[] ObjectsToStore()
		{
			return Objects;
		}

		protected override ValueTypeHolder[] ObjectsToOperateOn()
		{
			return new ValueTypeHolder[]
			       	{
						Objects[0],
						Objects[1]
					};
		}

		protected override DateTimeOffset UpdateValueFor(ValueTypeHolder holder)
		{
			holder.Value = holder.Value.AddYears(2009);
			return holder.Value;
		}

		public void TestGreater()
		{
			DateTimeOffset constraint = Objects[2].Value;
			IQuery query = NewDescendingQuery(	delegate(IQuery descendingQuery)
			                                  	{
			                                  		descendingQuery.Constrain(constraint).Greater();
			                                  	});

			IteratorAssert.AreEqual(
				Array.FindAll(Objects, delegate(ValueTypeHolder candidate) { return candidate.Value > constraint; }),
				query.Execute().GetEnumerator());
		}
	}
}

#endif