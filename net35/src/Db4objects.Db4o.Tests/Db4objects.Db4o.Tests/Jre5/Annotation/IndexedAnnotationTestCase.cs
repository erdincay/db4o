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
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config.Attributes;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Jre5.Annotation;

namespace Db4objects.Db4o.Tests.Jre5.Annotation
{
	public class IndexedAnnotationTestCase : AbstractDb4oTestCase
	{
		private class DataAnnotated
		{
			[Indexed]
			private int _id;

			public DataAnnotated(int id)
			{
				this._id = id;
			}

			public override string ToString()
			{
				return "DataAnnotated(" + _id + ")";
			}
		}

		private class DataNotAnnotated
		{
			private int _id;

			public DataNotAnnotated(int id)
			{
				this._id = id;
			}

			public override string ToString()
			{
				return "DataNotAnnotated(" + _id + ")";
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestIndexed()
		{
			StoreData();
			AssertIndexed();
			Reopen();
			AssertIndexed();
		}

		private void StoreData()
		{
			Db().Store(new IndexedAnnotationTestCase.DataAnnotated(42));
			Db().Store(new IndexedAnnotationTestCase.DataNotAnnotated(43));
		}

		private void AssertIndexed()
		{
			AssertIndexed(typeof(IndexedAnnotationTestCase.DataNotAnnotated), false);
			AssertIndexed(typeof(IndexedAnnotationTestCase.DataAnnotated), true);
		}

		private void AssertIndexed(Type clazz, bool expected)
		{
			IStoredClass storedClass = FileSession().StoredClass(clazz);
			IStoredField storedField = storedClass.StoredField("_id", typeof(int));
			Assert.AreEqual(expected, storedField.HasIndex());
		}

		public static void Main(string[] args)
		{
			new IndexedAnnotationTestCase().RunSoloAndClientServer();
		}
	}
}
