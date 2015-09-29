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
#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class ParameterizedEvaluationTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new ParameterizedEvaluationTestCase().RunConcurrency();
		}

		public string str;

		protected override void Store()
		{
			Store("one");
			Store("fun");
			Store("ton");
			Store("sun");
		}

		private void Store(string str)
		{
			ParameterizedEvaluationTestCase pe = new ParameterizedEvaluationTestCase();
			pe.str = str;
			Store(pe);
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			Assert.AreEqual(2, QueryContains(oc, "un").Count);
		}

		private IObjectSet QueryContains(IExtObjectContainer oc, string str)
		{
			IQuery q = oc.Query();
			q.Constrain(typeof(ParameterizedEvaluationTestCase));
			q.Constrain(new ParameterizedEvaluationTestCase.MyEvaluation(str));
			return q.Execute();
		}

		[System.Serializable]
		public class MyEvaluation : IEvaluation
		{
			public string str;

			public MyEvaluation(string str)
			{
				this.str = str;
			}

			public virtual void Evaluate(ICandidate candidate)
			{
				ParameterizedEvaluationTestCase pe = (ParameterizedEvaluationTestCase)candidate.GetObject
					();
				bool inc = pe.str.IndexOf(str) != -1;
				candidate.Include(inc);
			}
		}
	}
}
#endif // !SILVERLIGHT
