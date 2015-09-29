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
using Db4objects.Db4o;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.TA;

namespace Db4odoc.Tutorial.F1.Chapter8
{
    public class Pilot : IActivatable
    {
        private readonly String _name;
        private int _points;
        
        [Transient]
        private IActivator _activator;

        public Pilot(String name,int points) 
        {
            this._name=name;
            this._points=points;
        }

        public int Points
        {
            get
            {
				Activate(ActivationPurpose.Read);
                return _points;
            }

            set
            {
				Activate(ActivationPurpose.Write);
                _points += value;
            }
        }

        public String Name
        {
            get
            {
				Activate(ActivationPurpose.Read);
                return _name;
            }
        }

        public override string  ToString()
        {
			Activate(ActivationPurpose.Read);
            return String.Format("{0}/{1}", _name, _points);
        }
        
        public void Activate(ActivationPurpose purpose) 
        {
            if(_activator != null) 
            {
                _activator.Activate(purpose);
            }
        }

        public void Bind(IActivator activator) 
        {
            if (_activator == activator)
            {
                return;
            }
            if (activator != null && null != _activator)
            {
                throw new System.InvalidOperationException();
            }
            _activator = activator;
        }
    }
}