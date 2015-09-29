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
using Db4objects.Db4o.Reflect;
using OManager.DataLayer.Connection;
using OManager.DataLayer.CommonDatalayer;
using OManager.BusinessLayer.Common;

using OME.Logging.Common;

namespace OManager.DataLayer.Modal
{
    class ClassDetails 
    {
        private readonly IObjectContainer objectContainer;
        private readonly string m_className;

        public  ClassDetails(string className)
        {
            m_className = DataLayerCommon.RemoveGFromClassName(className);
            objectContainer = Db4oClient.Client; 
        }

        public int GetNumberOfObjects()
        {
            try
            {
                return objectContainer.Ext().StoredClass(m_className).GetIDs().Length;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return 0;
            }
        }

        public Hashtable GetFields()
        {
            try
            {
                IReflectClass rClass = DataLayerCommon.ReflectClassForName(m_className);
                IReflectField[] rFields = DataLayerCommon.GetDeclaredFieldsInHeirarchy(rClass);
                Hashtable FieldList = new Hashtable();

                foreach (IReflectField field in rFields)
                {
                    if (!FieldList.ContainsKey(field.GetName()))
                    {
						
                    	FieldList.Add(
							field.GetName(), 
							field.GetFieldType().GetName());
                    }
                }
                return FieldList;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return null; 
            }
        }

        public int GetFieldCount()
        {
            try
            {
                IReflectClass rClass = DataLayerCommon.ReflectClassForName(m_className);
                if (rClass != null)
                {
                    string type1 = rClass.ToString();
                    type1 = DataLayerCommon.PrimitiveType(type1);
                    char[] arr = CommonValues.charArray;
                    type1 = type1.Trim(arr);
                    if (!CommonValues.IsPrimitive(type1) && !type1.Contains(BusinessConstants.DB4OBJECTS_SYS))
                    {
                        IReflectField[] rFields = DataLayerCommon.GetDeclaredFieldsInHeirarchy(rClass);
                        return rFields.Length;
                    }
                }
                return 0;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return 0; 
            }
        }

        public IReflectField[] GetFieldList()
        {
            try
            {
                IReflectClass rClass = DataLayerCommon.ReflectClassForName(m_className); 
                IReflectField[] rFields = DataLayerCommon.GetDeclaredFieldsInHeirarchy(rClass);
                return rFields;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return null;
            }
           
        }      
    }
}
