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
#if !SILVERLIGHT
using System.Drawing;
using System.Drawing.Imaging;
#endif
using System.IO;

using Db4objects.Db4o.Config;

using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
	public class ImageTestCase : AbstractDb4oTestCase
	{
#if !CF && !SILVERLIGHT

		public class ImageTranslator : IObjectConstructor
		{
			public object OnInstantiate(IObjectContainer container, object obj)
			{
				byte[] data = (byte[])obj;
				using (MemoryStream stream = new MemoryStream(data))
				{
					return Image.FromStream(stream);
				}
			}

			public object OnStore(IObjectContainer container, object obj)
			{
				Image img = (Image)obj;
				using (MemoryStream stream = new MemoryStream())
				{
					img.Save(stream, ImageFormat.Bmp);
					return stream.ToArray();
				}
			}

			public void OnActivate(IObjectContainer container, object applicationObject, object storedObject)
			{
			}

			public Type StoredClass()
			{
				return typeof(byte[]);
			}
		}

		public const int width = 128;
		public const int height = 64;

		protected override void Configure(IConfiguration cfg)
		{
			cfg.ObjectClass(typeof(Bitmap)).Translate(new ImageTranslator());
		}

		protected override void Store()
		{
			Bitmap b = new Bitmap(width, height);
			Db().Store(b);
		}

		public void _TestImage()
		{
			Bitmap b = (Bitmap) RetrieveOnlyInstance(typeof (Bitmap));
			Assert.IsNotNull(b);
			Assert.AreEqual(width, b.Width);
			Assert.AreEqual(height, b.Height);
		}
#endif
	}
}
