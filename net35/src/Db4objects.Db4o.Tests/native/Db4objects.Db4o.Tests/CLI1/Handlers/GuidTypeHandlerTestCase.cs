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

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
	public class GuidTypeHandlerTestCase : ValueTypeHandlerTestCaseBase<Guid>
	{
		protected override ValueTypeHolder[] ObjectsToStore()
		{
			return Objects;
		}

		protected override ValueTypeHolder[] ObjectsToOperateOn()
		{
			return new[] { Objects[0], Objects[1] };
		}

		protected override Guid UpdateValueFor(ValueTypeHolder holder)
		{
			holder.Value = new Guid(1, 2, 3, 4, 5, 6, 7, 8, 9, 0xA, 0xB);
			return holder.Value;
		}

		private static Guid NewGuidFor(int i)
		{
			return new Guid(126 + i, 0, 0, 0, 0, 0, 0, 0, 0, 0, (byte)i);
		}

		private ValueTypeHolder[] Objects = new[]
			                            	{
			                            		new ValueTypeHolder(NewGuidFor(1), new ValueTypeHolder(NewGuidFor(10))), 
			                            		new ValueTypeHolder(NewGuidFor(2), new ValueTypeHolder(NewGuidFor(20))), 
			                            		new ValueTypeHolder(NewGuidFor(3), new ValueTypeHolder(NewGuidFor(30))), 
			                            		new ValueTypeHolder(NewGuidFor(4), new ValueTypeHolder(NewGuidFor(40))), 
			                            	};
	}
}
