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
using Db4objects.Drs.Inside.Traversal;

namespace Db4objects.Drs.Inside.Traversal
{
	public interface ITraverser
	{
		/// <summary>
		/// Traversal will only stop when visitor.visit(...) returns false, EVEN IN
		/// THE PRESENCE OF CIRCULAR REFERENCES.
		/// </summary>
		/// <remarks>
		/// Traversal will only stop when visitor.visit(...) returns false, EVEN IN
		/// THE PRESENCE OF CIRCULAR REFERENCES. So it is up to the visitor to detect
		/// circular references if necessary. Transient fields are not visited. The
		/// fields of second class objects such as Strings and Dates are also not visited.
		/// </remarks>
		void TraverseGraph(object @object, IVisitor visitor);

		/// <summary>Should only be called during a traversal.</summary>
		/// <remarks>
		/// Should only be called during a traversal. Will traverse the graph
		/// for the received object too, using the current visitor. Used to
		/// extend the traversal to a possibly disconnected object graph.
		/// </remarks>
		void ExtendTraversalTo(object disconnected);
	}
}
