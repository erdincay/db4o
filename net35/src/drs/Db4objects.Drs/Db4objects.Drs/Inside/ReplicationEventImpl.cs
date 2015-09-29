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
using Db4objects.Drs;
using Db4objects.Drs.Inside;

namespace Db4objects.Drs.Inside
{
	internal sealed class ReplicationEventImpl : IReplicationEvent
	{
		internal readonly ObjectStateImpl _stateInProviderA = new ObjectStateImpl();

		internal readonly ObjectStateImpl _stateInProviderB = new ObjectStateImpl();

		private bool _isConflict;

		internal IObjectState _actionChosenState;

		internal bool _actionWasChosen;

		internal bool _actionShouldStopTraversal;

		internal long _creationDate;

		public IObjectState StateInProviderA()
		{
			return _stateInProviderA;
		}

		public IObjectState StateInProviderB()
		{
			return _stateInProviderB;
		}

		public long ObjectCreationDate()
		{
			return _creationDate;
		}

		public bool IsConflict()
		{
			return _isConflict;
		}

		public void OverrideWith(IObjectState chosen)
		{
			if (_actionWasChosen)
			{
				throw new Exception();
			}
			//FIXME Use Db4o's standard exception throwing.
			_actionWasChosen = true;
			_actionChosenState = chosen;
		}

		public void StopTraversal()
		{
			_actionShouldStopTraversal = true;
		}

		internal void ResetAction()
		{
			_actionChosenState = null;
			_actionWasChosen = false;
			_actionShouldStopTraversal = false;
			_creationDate = -1;
		}

		internal void Conflict(bool isConflict)
		{
			_isConflict = isConflict;
		}
	}
}
