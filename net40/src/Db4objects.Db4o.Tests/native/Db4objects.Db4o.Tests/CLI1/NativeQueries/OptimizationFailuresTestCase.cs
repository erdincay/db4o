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
using System.IO;
using Db4objects.Db4o.Internal.Query;
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;

namespace Db4objects.Db4o.Tests.CLI1.NativeQueries
{
	public class Node
	{	
		Node _next;
		int _id;

		public Node(Node next, int id)
		{
			_next = next;
			_id = id;
		}

		public Node Next
		{
			get { return _next; }
		}

		public int Id
		{
			get { return _id; }
		}

		public bool FollowedBy3
		{
			get
			{
				return _next.Id == 3 || _next.FollowedBy3;
			}
		}
	}

	public class FollowedByPredicate : Predicate
	{
		int _id;

		public FollowedByPredicate(int id)
		{
			_id = id;
		}

		public bool Match(Node candidate)
		{
			return candidate.Next.Id == _id
				|| Match(candidate.Next);
		}
	}

	public class FollowedBy3Predicate : Predicate
	{
		public bool Match(Node candidate)
		{
			return candidate.FollowedBy3;
		}
	}

	/// <summary>
	/// </summary>
	public class OptimizationFailuresTestCase : AbstractDb4oTestCase, IOptOutSilverlight
	{
		IList<Exception> _failures = new List<Exception>();

		/*
		public void Store()
		{
			Node node = null;
			Tester.Store(node = new Node(null, 3));
			Tester.Store(node = new Node(node, 2));
			Tester.Store(node = new Node(node, 1));
		}*/

		public void TestRecursiveCandidateMethod()
		{
			ExpectFailure("this._next.get_FollowedBy3()", new FollowedBy3Predicate());
		}

		public void TestRecursivePredicateMethod()
		{
			ExpectFailure("this.Match(candidate.get_Next())", new FollowedByPredicate(2));
		}

		private void ExpectFailure(string expression, Predicate predicate)
		{
			_failures.Clear();

			Db().Configure().OptimizeNativeQueries(true);

			NativeQueryHandler handler = Stream().GetNativeQueryHandler();
			handler.QueryOptimizationFailure += new QueryOptimizationFailureHandler(OnOptimizationFailure);
			try
			{
				Db().Query(predicate);
				Assert.AreEqual(1, _failures.Count,  Join(_failures));
				Assert.AreEqual("Unsupported expression: " + expression, _failures[0].Message);
			}
			finally
			{
				handler.QueryOptimizationFailure -= new QueryOptimizationFailureHandler(OnOptimizationFailure);
			}
		}

		private string Join(IList<Exception> items)
		{
			StringWriter writer = new StringWriter();
			foreach (Exception item in items)
			{
				writer.WriteLine(item);
			}
			return writer.ToString();
		}

		private void OnOptimizationFailure(object sender, QueryOptimizationFailureEventArgs args)
		{
			_failures.Add(args.Reason);
		}
	}
}
