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
using Db4objects.Db4o;
using OManager.DataLayer.Connection;
using OManager.DataLayer.Modal;
using OME.Logging.Common;

namespace OManager.DataLayer.PropertyTable
{

    public class ClassPropertiesTable
    {
        string m_className;
        ArrayList m_fieldEntries;
        int m_noOfObjects;

        public ClassPropertiesTable(string classname)
        {
            m_className = classname;  
        }
        public string ClassName
        {
            get { return m_className; }
            set { m_className = value; }
        }

        public int NoOfObjects
        {
            get { return m_noOfObjects; }
            set { m_noOfObjects = value; }
        }
       

        public ArrayList FieldEntries
        {
            get { return m_fieldEntries; }
            set { m_fieldEntries = value; }
        }

        public ClassPropertiesTable GetClassProperties()
        {
            try
            {
                ClassDetails clsDetails = new ClassDetails(m_className);

				m_fieldEntries = FieldProperties.FieldsFrom(m_className);
                m_noOfObjects = clsDetails.GetNumberOfObjects();
                return this;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return null;
            }
            
            
        }
		public void SetIndex(ArrayList fieldnames, string className, ArrayList Indexed)
        {
			for (int i = 0; i < fieldnames.Count; i++)
			{
				Db4oClient.SetIndex(fieldnames[i].ToString(), className, Convert.ToBoolean(Indexed[i]));
				
			}
        }

    }
}
