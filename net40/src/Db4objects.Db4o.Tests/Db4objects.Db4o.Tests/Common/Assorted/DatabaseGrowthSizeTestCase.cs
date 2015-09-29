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
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;
using Db4objects.Db4o.Internal.Fileheader;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class DatabaseGrowthSizeTestCase : AbstractDb4oTestCase, IOptOutMultiSession
		, IOptOutIdSystem
	{
		private const int Size = 10000;

		private static readonly int MaximumHeaderSize = HeaderSize();

		private const int Reserve = Const4.PointerLength * 3;

		public static void Main(string[] args)
		{
			new DatabaseGrowthSizeTestCase().RunSolo();
		}

		private static int HeaderSize()
		{
			NewFileHeaderBase fileHeader = FileHeader.NewCurrentFileHeader();
			FileHeaderVariablePart variablePart = fileHeader.CreateVariablePart(null);
			return fileHeader.Length() + variablePart.MarshalledLength() + FileHeader.TransactionPointerLength
				 + Reserve;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.DatabaseGrowthSize(Size);
			config.BlockSize(3);
			Db4oLegacyConfigurationBridge.AsIdSystemConfiguration(config).UsePointerBasedSystem
				();
		}

		public virtual void Test()
		{
			Assert.IsGreater(Size, FileSession().FileLength());
			Assert.IsSmaller(Size + MaximumHeaderSize, FileSession().FileLength());
			DatabaseGrowthSizeTestCase.Item item = DatabaseGrowthSizeTestCase.Item.NewItem(Size
				);
			Store(item);
			Assert.IsGreater(Size * 2, FileSession().FileLength());
			Assert.IsSmaller(Size * 2 + MaximumHeaderSize, FileSession().FileLength());
			object retrievedItem = ((DatabaseGrowthSizeTestCase.Item)RetrieveOnlyInstance(typeof(
				DatabaseGrowthSizeTestCase.Item)));
			Assert.AreSame(item, retrievedItem);
		}

		public class Item
		{
			public byte[] _payload;

			public Item()
			{
			}

			public static DatabaseGrowthSizeTestCase.Item NewItem(int payloadSize)
			{
				DatabaseGrowthSizeTestCase.Item item = new DatabaseGrowthSizeTestCase.Item();
				item._payload = new byte[payloadSize];
				return item;
			}
		}
	}
}
