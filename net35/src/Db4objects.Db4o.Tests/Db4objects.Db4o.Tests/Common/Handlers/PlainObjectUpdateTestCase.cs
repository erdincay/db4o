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
using Db4oUnit;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class PlainObjectUpdateTestCase : HandlerUpdateTestCaseBase
	{
		public sealed class Item
		{
			public override int GetHashCode()
			{
				int prime = 31;
				int result = 1;
				result = prime * result + ((_typed == null) ? 0 : _typed.GetHashCode());
				result = prime * result + ((_untyped == null) ? 0 : _untyped.GetHashCode());
				return result;
			}

			public override bool Equals(object obj)
			{
				if (this == obj)
				{
					return true;
				}
				if (obj == null)
				{
					return false;
				}
				if (GetType() != obj.GetType())
				{
					return false;
				}
				PlainObjectUpdateTestCase.Item other = (PlainObjectUpdateTestCase.Item)obj;
				return Check.ObjectsAreEqual(_typed, other._typed) && Check.ObjectsAreEqual(_untyped
					, other._untyped);
			}

			public object _typed;

			public object _untyped;

			public Item(object @object)
			{
				_typed = @object;
				_untyped = @object;
			}
		}

		protected override bool IsApplicableForDb4oVersion()
		{
			return Db4oMajorVersion() >= 7 && Db4oMinorVersion() >= 2;
		}

		protected override void AssertArrays(IExtObjectContainer objectContainer, object 
			obj)
		{
			object[] array = (object[])obj;
			Assert.AreEqual(2, array.Length);
			Assert.AreSame(array[0], array[1]);
		}

		protected override void AssertValues(IExtObjectContainer objectContainer, object[]
			 values)
		{
			Assert.AreEqual(1, values.Length);
			PlainObjectUpdateTestCase.Item item = (PlainObjectUpdateTestCase.Item)values[0];
			Assert.IsNotNull(item);
			Assert.IsNotNull(item._typed);
			Assert.AreSame(item._typed, item._untyped);
		}

		protected override object CreateArrays()
		{
			object @object = new object();
			return new object[] { @object, @object };
		}

		protected override object[] CreateValues()
		{
			return new object[] { new PlainObjectUpdateTestCase.Item(new object()) };
		}

		protected override string TypeName()
		{
			return typeof(object).FullName;
		}
	}
}
