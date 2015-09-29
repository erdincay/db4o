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
using System.Collections.Generic;

#if !CF && !SILVERLIGHT
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Db4oUnit;
#endif

using Db4objects.Db4o.Collections;

namespace Db4objects.Db4o.Tests.CLI2.Collections.Transparent.Dictionary
{
	partial class ActivatableDictionaryTestCase
	{
		protected override IDictionary<string, ICollectionElement> NewPlainCollection()
		{
			return new Dictionary<string, ICollectionElement>();
		}

		protected override IDictionary<string, ICollectionElement> SingleCollection()
		{
			CollectionHolder<IDictionary<string, ICollectionElement>> holder = (CollectionHolder<IDictionary<string, ICollectionElement>>)RetrieveOnlyInstance(typeof(CollectionHolder<IDictionary<string, ICollectionElement>>));
			return holder.Collection;
		}

		protected override IDictionary<string, ICollectionElement> NewActivatableCollection(IDictionary<string, ICollectionElement> template)
		{
			return new ActivatableDictionary<string, ICollectionElement>(template);
		}

		protected override KeyValuePair<string, ICollectionElement> NewElement(string value)
		{
			return new KeyValuePair<string, ICollectionElement>(value, new Element(value));
		}

		protected override KeyValuePair<string, ICollectionElement> NewActivatableElement(string value)
		{
			return new KeyValuePair<string, ICollectionElement>("activatable-" + value, new ActivatableElement(value));
		}

		private ICollectionElement NewItem(string key)
		{
			return NewElement(key).Value;
		}

#if !CF && !SILVERLIGHT
		private void AssertSerializable(IDictionary<string, ICollectionElement> dictionary)
		{
			MemoryStream stream = new MemoryStream();
			IFormatter formatter = new BinaryFormatter();

			formatter.Serialize(stream, dictionary);
			stream.Position = 0;
			formatter = new BinaryFormatter();
			IDictionary<string, ICollectionElement> actual = (IDictionary<string, ICollectionElement>)formatter.Deserialize(stream);
			Assert.AreEqual(NewPopulatedPlainCollection()[ExistingKey], actual[ExistingKey]);
			Assert.IsFalse(actual.ContainsKey(NonExistingKey));
		}
#endif
	}
}
