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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Typehandlers;
using Db4objects.Db4o.Diagnostic;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
    public class EnumInUntypedVariableTestCase : AbstractDb4oTestCase
    {
        enum EnumAsInteger
        {
            First,
            Second,
        }

        public class Item
        {
            public object _enum;

            public Item(object enum_)
            {
                _enum = enum_;
            }

            public override bool Equals(object obj)
            {
                Item rhs = (Item)obj;
                if (rhs == null) return false;

                if (rhs.GetType() != GetType()) return false;

                return Equals(_enum, rhs._enum);
            }

            public override string ToString()
            {
                return "Item _enum: " + _enum ;
            }
        }

        private DeletionListener _diagnosticListener;

        class DeletionListener : IDiagnosticListener
        {
            public bool _diagnosticListenerCalled;

            public void OnDiagnostic(IDiagnostic d)
            {
                if (d is DeletionFailed)
                {
                    _diagnosticListenerCalled = true;
                }
            }
        }

        protected override void Configure(IConfiguration config)
        {
            base.Configure(config);
            config.RegisterTypeHandler(new EnumTypeHandlerPredicate(), new EnumTypeHandler());
            config.ObjectClass(typeof(Item)).CascadeOnDelete(true);
            _diagnosticListener = new DeletionListener();

            // The diagnostic listener is installed so we detect
            // deletion failures before implementing delete
            // in the EnumTypeHandler. Exceptions are silently 
            // caught in ClassMetadata#DeleteMembers()

            config.Diagnostic().AddListener(_diagnosticListener);
        }

        protected override void Store()
        {
            Item item = new Item(EnumAsInteger.First);
            Store(item);
        }

        public void TestRetrieval()
        {
            Item item = (Item) RetrieveOnlyInstance(typeof (Item));
            Assert.AreEqual(EnumAsInteger.First, item._enum);
        }

        public void TestDelete()
        {
            Item item = (Item)RetrieveOnlyInstance(typeof(Item));
            Db().Delete(item);
            Assert.IsFalse(_diagnosticListener._diagnosticListenerCalled);
        }

    }
}
