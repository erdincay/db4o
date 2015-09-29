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

namespace Db4objects.Db4o.Tests.CLI1
{
	public class AllTests : Db4oUnit.Extensions.Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new System.Type[]
				{
                    typeof(Aliases.AllTests),
					typeof(CrossPlatform.AllTests),
#if !CF && !SILVERLIGHT
					typeof(CsAppDomains),
					typeof(CsAssemblyVersionChange),
					typeof(CsImage),
					typeof(ShutdownMultipleContainer),
#endif
                    typeof(EnumTestCase),
					typeof(Events.EventRegistryTestCase),
					typeof(Foundation.AllTests),
                    typeof(Handlers.AllTests),
					typeof(Inside.AllTests),
					typeof(Internal.AllTests),
					typeof(NativeQueries.AllTests),
					typeof(Reflect.Net.AllTests),
					typeof(Soda.AllTests),
#if !SILVERLIGHT
                    typeof(CollectionBaseTestCase),
#endif
					typeof(CsCascadeDeleteToStructs),
					typeof(CsCollections),
					typeof(CsCustomTransientAttribute),
					typeof(CsDate),
					typeof(CsDelegate),
					typeof(CsDisposableTestCase),
					typeof(CsEnum),
					// typeof(CsEvaluationDelegate),  moved to Staging because it fails
#if !SILVERLIGHT
					typeof(CsMarshalByRef),
#endif
					typeof(CsType),
					typeof(StructsTestCase),
					typeof(CsStructsRegression),
					typeof(CsValueTypesTestCase),
                    typeof(CsSystemArrayTestCase),
					typeof(CultureInfoTestCase),
                    typeof(DictionaryBaseTestCase),

#if !SILVERLIGHT
					typeof(HybridDictionaryTestCase),
#endif
					typeof(ImageTestCase),
					typeof(JavaDateCompatibilityTestCase),
                    typeof(JavaSimpleChecksumCompatibilityTestCase),
					typeof(JavaUUIDCompatibilityTestCase),
					typeof(MDArrayTestCase),
#if !CF && !SILVERLIGHT
					typeof(Monitoring.AllTests),
#endif
					typeof(NonSerializedAttributeTestCase),
					typeof(ObjectSetAsListTestCase),
					typeof(ProtectedBaseConstructorTestCase),
				};
		}
	}
}
