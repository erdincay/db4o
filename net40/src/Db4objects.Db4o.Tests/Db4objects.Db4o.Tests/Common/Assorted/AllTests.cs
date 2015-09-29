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
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class AllTests : ComposibleTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Assorted.AllTests().RunAll();
		}

		protected override Type[] TestCases()
		{
			return ComposeTests(new Type[] { typeof(AliasesQueryingTestCase), typeof(AliasesTestCase
				), typeof(CallbackTestCase), typeof(CanUpdateFalseRefreshTestCase), typeof(CascadeDeleteDeletedTestCase
				), typeof(CascadedDeleteReadTestCase), typeof(ChangeIdentity), typeof(CommitTimeStampsNoSchemaChangesTestCase
				), typeof(CommitTimestampTestCase), typeof(CloseUnlocksFileTestCase), typeof(ComparatorSortTestCase
				), typeof(DatabaseGrowthSizeTestCase), typeof(DatabaseUnicityTest), typeof(DbPathDoesNotExistTestCase
				), typeof(DeleteSetTestCase), typeof(DeleteReaddChildReferenceTestSuite), typeof(
				DeleteUpdateTestCase), typeof(DescendToNullFieldTestCase), typeof(DualDeleteTestCase
				), typeof(ExceptionsOnNotStorableFalseTestCase), typeof(ExceptionsOnNotStorableIsDefaultTestCase
				), typeof(GetSingleSimpleArrayTestCase), typeof(HandlerRegistryTestCase), typeof(
				IndexCreateDropTestCase), typeof(IndexedBlockSizeQueryTestCase), typeof(InvalidOffsetInDeleteTestCase
				), typeof(KnownClassesTestCase), typeof(LazyObjectReferenceTestCase), typeof(LockedTreeTestCase
				), typeof(LongLinkedListTestCase), typeof(ManyRollbacksTestCase), typeof(MultiDeleteTestCase
				), typeof(ObjectUpdateFileSizeTestCase), typeof(ObjectConstructorTestCase), typeof(
				ObjectContainerMemberTestCase), typeof(PlainObjectTestCase), typeof(PeekPersistedTestCase
				), typeof(PersistentIntegerArrayTestCase), typeof(PersistStaticFieldValuesTestSuite
				), typeof(PreventMultipleOpenTestCase), typeof(QueryByInterface), typeof(QueryingDoesNotProduceClassMetadataTestCase
				), typeof(QueryingReadOnlyWithNewClassTestCase), typeof(ReAddCascadedDeleteTestCase
				), typeof(RenamingClassAfterQueryingTestCase), typeof(RepeatDeleteReaddTestCase)
				, typeof(RollbackDeleteTestCase), typeof(RollbackTestCase), typeof(RollbackUpdateTestCase
				), typeof(RollbackUpdateCascadeTestCase), typeof(SimplestPossibleNullMemberTestCase
				), typeof(SimplestPossibleTestCase), typeof(SimplestPossibleParentChildTestCase)
				, typeof(StaticFieldUpdateTestCase), typeof(StaticFieldUpdateConsistencyTestCase
				), typeof(SystemInfoTestCase), typeof(TransientCloneTestCase), typeof(UnknownReferenceDeactivationTestCase
				), typeof(WithTransactionTestCase) });
		}

		#if !SILVERLIGHT
		protected override Type[] ComposeWith()
		{
			return new Type[] { typeof(PersistTypeTestCase), typeof(ConcurrentRenameTestCase)
				 };
		}
		#endif // !SILVERLIGHT
	}
}
