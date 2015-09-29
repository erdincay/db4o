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
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;
using Db4objects.Db4o.Tests.Common.Reflect.Generic;

namespace Db4objects.Db4o.Tests.Common.Reflect.Generic
{
	public class GenericObjectsTest : AbstractDb4oTestCase
	{
		private string PersonClassname = "com.acme.Person";

		public static void Main(string[] args)
		{
			new GenericObjectsTest().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestCreate()
		{
			InitGenericObjects();
			// fixture().reopen();
			IExtObjectContainer oc = Fixture().Db();
			// now check to see if person was saved
			IReflectClass rc = GetReflectClass(oc, PersonClassname);
			Assert.IsNotNull(rc);
			IQuery q = oc.Query();
			q.Constrain(rc);
			IObjectSet results = q.Execute();
			Assert.IsTrue(results.Count == 1);
		}

		//Db4oUtil.dumpResults(fixture().db(), results);
		private object InitGenericObjects()
		{
			GenericClass personClass = InitGenericClass();
			IReflectField surname = personClass.GetDeclaredField("surname");
			IReflectField birthdate = personClass.GetDeclaredField("birthdate");
			//ReflectField nArray = personClass.getDeclaredField("nArray");
			object person = personClass.NewInstance();
			surname.Set(person, "John");
			//		/int[][] arrayData = new int[2][2];
			// todo: FIXME: nArray doesn't work
			// nArray.set(person, arrayData);
			birthdate.Set(person, new DateTime());
			Fixture().Db().Store(person);
			Fixture().Db().Commit();
			return person;
		}

		/// <summary>todo: Move the GenericClass creation into a utility/factory class.</summary>
		/// <remarks>todo: Move the GenericClass creation into a utility/factory class.</remarks>
		/// <returns></returns>
		public virtual GenericClass InitGenericClass()
		{
			GenericReflector reflector = new GenericReflector(null, Platform4.ReflectorForType
				(typeof(GenericObjectsTest)));
			GenericClass _objectIClass = (GenericClass)reflector.ForClass(typeof(object));
			GenericClass result = new GenericClass(reflector, null, PersonClassname, _objectIClass
				);
			result.InitFields(Fields(result, reflector));
			return result;
		}

		private GenericField[] Fields(GenericClass personClass, GenericReflector reflector
			)
		{
			return new GenericField[] { new GenericField("surname", reflector.ForClass(typeof(
				string)), false), new GenericField("birthdate", reflector.ForClass(typeof(DateTime
				)), false), new GenericField("bestFriend", personClass, false), new GenericField
				("nArray", reflector.ForClass(typeof(int[][])), true) };
		}

		public virtual void TestUpdate()
		{
			IExtObjectContainer oc = Fixture().Db();
			InitGenericObjects();
			//Db4oUtil.dump(oc);
			IReflectClass rc = GetReflectClass(oc, PersonClassname);
			Assert.IsNotNull(rc);
			IQuery q = oc.Query();
			q.Constrain(rc);
			IObjectSet results = q.Execute();
			//Db4oUtil.dumpResults(oc, results);
			Assert.IsTrue(results.Count == 1);
		}

		public virtual void TestQuery()
		{
			IExtObjectContainer oc = Fixture().Db();
			InitGenericObjects();
			IReflectClass rc = GetReflectClass(oc, PersonClassname);
			Assert.IsNotNull(rc);
			// now query to make sure there are none left
			IQuery q = oc.Query();
			q.Constrain(rc);
			q.Descend("surname").Constrain("John");
			IObjectSet results = q.Execute();
			Assert.IsTrue(results.Count == 1);
		}

		public virtual void TestDelete()
		{
			IExtObjectContainer oc = Fixture().Db();
			InitGenericObjects();
			IReflectClass rc = GetReflectClass(oc, PersonClassname);
			Assert.IsNotNull(rc);
			IQuery q = oc.Query();
			q.Constrain(rc);
			IObjectSet results = q.Execute();
			while (results.HasNext())
			{
				object o = results.Next();
				oc.Delete(o);
			}
			oc.Commit();
			// now query to make sure there are none left
			q = oc.Query();
			q.Constrain(rc);
			q.Descend("surname").Constrain("John");
			results = q.Execute();
			Assert.IsTrue(results.Count == 0);
		}

		private IReflectClass GetReflectClass(IExtObjectContainer oc, string className)
		{
			return oc.Reflector().ForName(className);
		}

		/// <summary>
		/// This is to ensure that reflector.forObject(GenericArray) returns an instance of GenericArrayClass instead of GenericClass
		/// http://tracker.db4o.com/jira/browse/COR-376
		/// </summary>
		public virtual void TestGenericArrayClass()
		{
			IExtObjectContainer oc = Fixture().Db();
			InitGenericObjects();
			IReflectClass elementType = oc.Reflector().ForName(PersonClassname);
			object array = Reflector().Array().NewInstance(elementType, 5);
			IReflectClass arrayClass = oc.Reflector().ForObject(array);
			Assert.IsTrue(arrayClass.IsArray());
			Assert.IsTrue(arrayClass is GenericArrayClass);
			arrayClass = oc.Reflector().ForName(array.GetType().FullName);
			Assert.IsTrue(arrayClass.IsArray());
			Assert.IsTrue(arrayClass is GenericArrayClass);
			arrayClass = oc.Reflector().ForClass(array.GetType());
			Assert.IsTrue(arrayClass.IsArray());
			Assert.IsTrue(arrayClass is GenericArrayClass);
			Assert.AreEqual(arrayClass.GetName(), ReflectPlatform.FullyQualifiedName(array.GetType
				()));
			Assert.AreEqual("(GA) " + elementType.GetName(), array.ToString());
		}
	}
}
