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
using System.Collections;
using System.Text;
using Db4objects.Db4o.Consistency;

namespace Db4objects.Db4o.Consistency
{
	public class ConsistencyReport
	{
		private const int MaxReportedItems = 50;

		internal readonly IList bogusSlots;

		internal readonly OverlapMap overlaps;

		internal readonly IList invalidObjectIds;

		internal readonly IList invalidFieldIndexEntries;

		internal ConsistencyReport(IList bogusSlots, OverlapMap overlaps, IList invalidClassIds
			, IList invalidFieldIndexEntries)
		{
			this.bogusSlots = bogusSlots;
			this.overlaps = overlaps;
			this.invalidObjectIds = invalidClassIds;
			this.invalidFieldIndexEntries = invalidFieldIndexEntries;
		}

		public virtual bool Consistent()
		{
			return bogusSlots.Count == 0 && overlaps.Overlaps().Count == 0 && overlaps.Dupes(
				).Count == 0 && invalidObjectIds.Count == 0 && invalidFieldIndexEntries.Count ==
				 0;
		}

		public virtual Sharpen.Util.ISet Overlaps()
		{
			return overlaps.Overlaps();
		}

		public virtual Sharpen.Util.ISet Dupes()
		{
			return overlaps.Dupes();
		}

		public override string ToString()
		{
			if (Consistent())
			{
				return "no inconsistencies detected";
			}
			StringBuilder message = new StringBuilder("INCONSISTENCIES DETECTED\n").Append(overlaps
				.Overlaps().Count + " overlaps\n").Append(overlaps.Dupes().Count + " dupes\n").Append
				(bogusSlots.Count + " bogus slots\n").Append(invalidObjectIds.Count + " invalid class ids\n"
				).Append(invalidFieldIndexEntries.Count + " invalid field index entries\n");
			message.Append("(slot lengths are non-blocked)\n");
			AppendInconsistencyReport(message, "OVERLAPS", overlaps.Overlaps());
			AppendInconsistencyReport(message, "DUPES", overlaps.Dupes());
			AppendInconsistencyReport(message, "BOGUS SLOTS", bogusSlots);
			AppendInconsistencyReport(message, "INVALID OBJECT IDS", invalidObjectIds);
			AppendInconsistencyReport(message, "INVALID FIELD INDEX ENTRIES", invalidFieldIndexEntries
				);
			return message.ToString();
		}

		private void AppendInconsistencyReport(StringBuilder str, string title, ICollection
			 entries)
		{
			if (entries.Count != 0)
			{
				str.Append(title + "\n");
				int count = 0;
				for (IEnumerator entryIter = entries.GetEnumerator(); entryIter.MoveNext(); )
				{
					object entry = entryIter.Current;
					str.Append(entry).Append("\n");
					count++;
					if (count > MaxReportedItems)
					{
						str.Append("and more...\n");
						break;
					}
				}
			}
		}
	}
}
