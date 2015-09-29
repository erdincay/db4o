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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Query;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Linq.Tests.Queries
{
	internal class QueryPrettyPrinter
	{
		private readonly StringBuilder _builder = new StringBuilder();
#if SILVERLIGHT
        private readonly IDictionary<QConJoin, QConJoin> _visitedJoins = new Dictionary<QConJoin, QConJoin>();
#else
        private readonly HashSet<QConJoin> _visitedJoins = new HashSet<QConJoin>();
#endif

		public QueryPrettyPrinter(IQuery query)
		{
			VisitAll(query.Constraints().ToArray());
			foreach (SodaQueryComparator.Ordering ordering in ((QQueryBase)query).Orderings())
			{
				PrintOrderBy(ordering);
			}
		}

		private void Visit(IConstraint constraint)
		{
			var constrainType = constraint.GetType().Name;
			switch (constrainType)
			{
				case "QConClass":
					Visit(constraint as QConClass);
					break;
				case "QConObject":
					Visit(constraint as QConObject);
					break;
				case "QConJoin":
					Visit(constraint as QConJoin);
					break;
				case "QConPath":
					Visit(constraint as QConPath);
					break;
				
				default:
					throw new ArgumentException(constrainType);
			 }
		}

		protected virtual void Visit(QConClass klass)
		{
			_builder.Append("(");
			_builder.Append(GetClassName(klass.GetClassName()));

			if (klass.HasChildren())
			{
				ForEach<IConstraint>(klass.IterateChildren(), cons => Visit(cons));
			}
			_builder.Append(")");
		}

		private static void ForEach<TElement>(IEnumerator list, Action<TElement> action)
		{
			while (list.MoveNext())
			{
				action((TElement)list.Current);
			}
		}

		protected virtual void Visit(QConPath path)
		{
			PrintDescend(path);
		}

		private void PrintDescend(QConPath path)
		{
			_builder.AppendFormat("({0}", path.GetField().Name());

			VisitAllChildrenOf(path);

			_builder.Append(")");
		}

		private void VisitAllChildrenOf(QCon path)
		{
			VisitAll(Iterators.Iterable(path.IterateChildren()).Cast<IConstraint>());
		}

		private void VisitAll(IEnumerable<IConstraint> constraints)
		{
			foreach (var constraint in constraints)
			{
				Visit(constraint);
			}
		}

		private void PrintOrderBy(SodaQueryComparator.Ordering ordering)
		{
			_builder.AppendFormat("(orderby {0} {1})",
				ordering.FieldPath().Aggregate((current, item) => current + "." + item),
				DirectionString(ordering.Direction()));
		}

		private static string DirectionString(SodaQueryComparator.Direction direction)
		{
			return direction.Equals(SodaQueryComparator.Direction.Descending) ? "desc" : "asc";
		}

		protected virtual void Visit(QConJoin join)
		{
#if SILVERLIGHT
            if (_visitedJoins.ContainsKey(join)) return;
#else
			if (_visitedJoins.Contains(join)) return;
#endif
			_builder.Append("(");
			VisitJoinBranch(join.Constraint2());
			_builder.AppendFormat(" {0} ", join.IsOr() ? "or" : "and");
			VisitJoinBranch(join.Constraint1());
			_builder.Append(")");
#if SILVERLIGHT
            _visitedJoins.Add(join, join);
#else
			_visitedJoins.Add(join);
#endif
		}

		private void VisitJoinBranch(QCon branch)
		{
			if (branch is QConJoin)
			{
				Visit(branch as QConJoin);
				return;
			}

			if (branch is QConObject)
			{
				PrintQConObject(branch as QConObject);
				return;
			}

			throw new NotSupportedException();
		}

		protected virtual void Visit(QConObject obj)
		{
			if (obj.HasJoins())
			{
				var constraints = obj.IterateJoins();
				while (constraints.MoveNext())
				{
					var constraint = (IConstraint) constraints.Current;
					Visit(constraint);
				}
				return;
			}
			PrintQConObject(obj);
		}

		private void PrintQConObject(QConObject obj)
		{
			_builder.AppendFormat("({0} {1} {2})",
				                      obj.GetField().Name(),
				                      EvaluatorToString(obj.Evaluator()),
				                      ValueToString(obj.GetObject()));
		}

		private static string EvaluatorToString(QE evaluator)
		{
			switch (evaluator.GetType().Name)
			{
				case "QEMulti":
				{
					var sb = new StringBuilder();
					foreach (QE qe in ((QEMulti)evaluator).Evaluators())
					{
						sb.Append(EvaluatorToString(qe));
					}
					return sb.ToString();
				}
				case "QE": return "==";
				case "QESmaller": return "<";
				case "QEGreater": return ">";
				case "QEEqual": return "=";
				case "QENot": return "not";
				case "QEStartsWith": return "startswith";
				case "QEEndsWith": return "endswith";
				case "QEContains": return "contains";
			}

			throw new NotSupportedException();
		}

		private static string ValueToString(object value)
		{	
			if (value is string) return string.Format("'{0}'", value);

			return value == null ? "null" : value.ToString();
		}

		private static string GetClassName(string fullname)
		{
			int pos = fullname.LastIndexOf(",");
			if (pos > -1)
				fullname = fullname.Substring(0, pos);

			pos = fullname.LastIndexOf("+");
			if (pos > -1)
				return ExtractSuffix(fullname, pos);

			pos = fullname.LastIndexOf(".");
			if (pos > -1)
				return ExtractSuffix(fullname, pos);

			return fullname;
		}

		private static string ExtractSuffix(string str, int pos)
		{
			return str.Substring(pos + 1, str.Length - pos - 1);
		}

		public override string ToString()
		{
			return _builder.ToString();
		}
	}
}
