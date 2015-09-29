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
using System.Collections;
    
namespace Db4odoc.Tutorial.F1.Chapter4
{
    public class Car
    {
        string _model;
        Pilot _pilot;
        IList _history;
        
        public Car(string model) : this(model, new ArrayList())
        {
        }
        
        public Car(string model, IList history)
        {
            _model = model;
            _pilot = null;
            _history = history;
        }
        
        public Pilot Pilot
        {
            get
            {
                return _pilot;
            }
            
            set
            {
                _pilot = value;
            }
        }
        
        public string Model
        {
            get
            {
                return _model;
            }
        }
        
        public IList History
        {
        	get
        	{
	            return _history;
	        }
        }
        
        public void Snapshot()
        {
            _history.Add(new SensorReadout(Poll(), DateTime.Now, this));
        }
        
        protected double[] Poll()
        {
            int factor = _history.Count + 1;
            return new double[] { 0.1d*factor, 0.2d*factor, 0.3d*factor };
        }
        
        override public string ToString()
        {
			return string.Format("{0}[{1}]/{2}", _model, _pilot, _history.Count);
        }
    }
}
