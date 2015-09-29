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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Db4oUnit;

namespace Db4objects.Db4o.Linq.Tests
{
	public class CollectionContainsOptimizationTestCase : AbstractDb4oLinqTestCase
	{
#if !CF //csc fails to find S.R.FieldInfo.GetFieldFromHandle
		public class CollectionHolder<T> where T : ICollection<string>
		{
			public T Items;
		}

        protected override void Db4oSetupAfterStore()
        {
            Container().ProduceClassMetadata(ReflectClass(typeof(ListHolder)));
        }

		public class ListHolder : CollectionHolder<List<string>>
		{
		}

		public class IListOfTHolder : CollectionHolder<IList<string>>
		{
		}

		public class ICollectionHolder : CollectionHolder<ICollection<string>>
		{
		}

		public void TestQueryOnIListContains()
		{
			AssertQuery(
				db => from IListOfTHolder p in db
					  where p.Items.Contains("foo")
					  select p);
		}

		public void TestQueryOnIListNotContains()
		{
			AssertQuery(
				db => from IListOfTHolder p in db
					  where !p.Items.Contains("foo")
					  select p);
		}

		public void TestQueryOnListContains()
		{
			AssertQuery(
				db => from ListHolder p in db
					  where p.Items.Contains("foo")
					  select p);
		}

		public void TestQueryOnListNotContains()
		{
			AssertQuery(
				db => from ListHolder p in db
					  where !p.Items.Contains("foo")
					  select p);
		}

		public void TestQueryOnICollectionContains()
		{
			AssertQuery(
				db => from ICollectionHolder p in db
					  where p.Items.Contains("foo")
					  select p);
		}

		public void TestQueryOnICollectionNotContains()
		{
			AssertQuery(
				db => from ICollectionHolder p in db
					  where !p.Items.Contains("foo")
					  select p);
		}

		private void AssertQuery<T>(Expression<Func<IObjectContainer, IEnumerable<T>>> queryExpression)
		{
			using (var recorder = new QueryStringRecorder(Db()))
			{
				var result = queryExpression.Compile().Invoke(Db());
				result.ToList();

				string expected = ExpectedRepresentationFor(queryExpression);
				Assert.AreEqual(expected, recorder.QueryString);
			}
		}

		private static string ExpectedRepresentationFor<T>(Expression<Func<IObjectContainer, IEnumerable<T>>> queryExpression)
		{
			return string.Format("({0}(Items {1} 'foo'))", ExtentTypeFrom(queryExpression), ContainmentCondition(queryExpression));
		}

		private static string ContainmentCondition<T>(Expression<Func<IObjectContainer, IEnumerable<T>>> queryExpression)
		{
			Assert.AreEqual(ExpressionType.Call, queryExpression.Body.NodeType);

			MethodCallExpression whereMethod = FindMethodCall(queryExpression, "Where");

			var whereExpression = ((LambdaExpression)((UnaryExpression)whereMethod.Arguments[1]).Operand);

			if (whereExpression.Body.NodeType == ExpressionType.Not)
			{
				return "not";
			}

			Assert.AreEqual(ExpressionType.Call, whereExpression.Body.NodeType);

			var whereCondition = (MethodCallExpression) whereExpression.Body;
			return whereCondition.Method.Name.ToLower();
		}

		private static string ExtentTypeFrom<T>(Expression<Func<IObjectContainer, IEnumerable<T>>> queryExpression)
		{
			MethodCallExpression castMethod = FindMethodCall(queryExpression, "Cast");
			Assert.IsNotNull(castMethod);
			return castMethod.Method.GetGenericArguments()[0].Name;
		}

		private static MethodCallExpression FindMethodCall<T>(Expression<Func<IObjectContainer, IEnumerable<T>>> queryExpression, string methodName)
		{
			Expression expression = queryExpression.Body;
			while (expression.NodeType == ExpressionType.Call)
			{
				var mce = (MethodCallExpression)expression;
				if (mce.Method.Name == methodName)
				{
					return mce;
				}
				expression = mce.Arguments[0];
			}

			return null;
		}
#endif
	}
}
