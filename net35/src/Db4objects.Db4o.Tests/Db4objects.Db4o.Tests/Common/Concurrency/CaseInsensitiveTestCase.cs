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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	/// <summary>demonstrates a case-insensitive query using an Evaluation</summary>
	public class CaseInsensitiveTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Concurrency.CaseInsensitiveTestCase().RunConcurrency
				();
		}

		public string name;

		public CaseInsensitiveTestCase()
		{
		}

		public CaseInsensitiveTestCase(string name)
		{
			this.name = name;
		}

		protected override void Store()
		{
			Store(new Db4objects.Db4o.Tests.Common.Concurrency.CaseInsensitiveTestCase("HelloWorld"
				));
		}

		public virtual void ConcQueryCaseInsenstive(IExtObjectContainer oc)
		{
			IQuery q = oc.Query();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Concurrency.CaseInsensitiveTestCase
				));
			q.Constrain(new CaseInsensitiveTestCase.CaseInsensitiveEvaluation("helloworld"));
			Assert.AreEqual(1, q.Execute().Count);
		}

		[System.Serializable]
		public class CaseInsensitiveEvaluation : IEvaluation
		{
			public string name;

			public CaseInsensitiveEvaluation(string name)
			{
				this.name = name;
			}

			public virtual void Evaluate(ICandidate candidate)
			{
				CaseInsensitiveTestCase ci = (CaseInsensitiveTestCase)candidate.GetObject();
				candidate.Include(ci.name.ToLower().Equals(name.ToLower()));
			}
		}
	}
}
#endif // !SILVERLIGHT
