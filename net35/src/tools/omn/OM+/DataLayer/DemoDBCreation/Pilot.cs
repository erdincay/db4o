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

namespace DemoDbCreation
{
    public class Pilot
    {
        string _name;
        int _points;
        bool _hasWon;
        Single _single;
        Double _double;
        SByte _sbyte;
        Byte _byte;
        Children _children;

        public Children Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public Byte ByteProp
        {
            get { return _byte; }
            set { _byte = value; }
        }

        public SByte SbyteProp
        {
            get { return _sbyte; }
            set { _sbyte = value; }
        }
 
        public Double Dbl
        {
            get { return _double; }
            set { _double = value; }
        }
        public Single SingleProp
        {
            get { return _single; }
            set { _single = value; }
        }
        public bool HasWon
        {
            get { return _hasWon; }
            set { _hasWon = value; }
        }
        
        public Pilot(string name, int points)
        {
            _name = name;
            _points = points;
        }
        
        public int Points
        {
            get
            {
                return _points;
            }
        }
        
        public void AddPoints(int points)
        {
            _points += points;
        }
        
        public string Name
        {
            get
            {
                return _name;
            }
        }
        
        
    }
}