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
using Db4objects.Db4o.Ext ;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;
using OManager.DataLayer.Connection;
using OManager.DataLayer.CommonDatalayer;
using OManager.DataLayer.Reflection;
using OME.Logging.Common;

namespace OManager.DataLayer.Modal
{
    class FieldDetails 
    {
        private readonly IObjectContainer objectContainer;
        private readonly string m_classname; 
        private readonly string m_fieldname;

        public FieldDetails(string classname, string fieldname)
        {
            string CorrectfieldName = fieldname;
            int intIndexof = CorrectfieldName.IndexOf('.') + 1;
            CorrectfieldName = CorrectfieldName.Substring(intIndexof, CorrectfieldName.Length - intIndexof);

            m_classname = DataLayerCommon.RemoveGFromClassName(classname);
            m_fieldname = CorrectfieldName;
            objectContainer = Db4oClient.Client; 
        }

        public bool IsIndexed()
        {
			try
			{
				IStoredClass storedClass = objectContainer.Ext().StoredClass(m_classname);
				if (null == storedClass)
					return false;

				IStoredField field = DataLayerCommon.GetDeclaredStoredFieldInHeirarchy(storedClass, m_fieldname);
				if (field == null)
					return false;

				return field.HasIndex();
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
				return false;
			}
        }

        public  bool IsPrimitive()
        {
            try
            {             
                IReflectClass rClass = DataLayerCommon.ReflectClassForName(m_classname);
                IReflectField rField = DataLayerCommon.GetDeclaredFieldInHeirarchy(rClass, m_fieldname);
                return rField.GetFieldType().IsPrimitive();
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return false ;
            }
        }

        public bool GetModifier()
        {
            try
            {
                IReflectClass rClass = DataLayerCommon.ReflectClassForName(m_classname);
                if (rClass != null)
                {
                    IReflectField rField = DataLayerCommon.GetDeclaredFieldInHeirarchy(rClass, m_fieldname);
                  
					if (rField != null)
                    {

                        return rField.IsPublic();
                    }
                }
                return false; 
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return false;
            }
        }

        public bool IsCollection()
        {
            try
            {
            	IReflectClass rClass = DataLayerCommon.ReflectClassForName(m_classname);
                if (rClass != null)
                {
                    IReflectField rField = DataLayerCommon.GetDeclaredFieldInHeirarchy(rClass, m_fieldname);
                    if (rField != null)
                    {
                    	return rField.GetFieldType().IsCollection();
                    }
                }
                return false;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return false;
            }
        }

        public bool IsArray()
        {
            try
            {
                IReflectClass rClass = DataLayerCommon.ReflectClassForName(m_classname);
                if (rClass != null)
                {
                    IReflectField rField = DataLayerCommon.GetDeclaredFieldInHeirarchy(rClass, m_fieldname);
                    if (rField != null)
                    {
                        return rField.GetFieldType().IsArray();
                    }
                }
				return false;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return false;
            }
        }       


        public IType GetFieldType()
        {
            GenericReflector reflecotr = objectContainer.Ext().Reflector();
            IReflectClass klass = reflecotr.ForName(m_classname);

            IReflectField rfield = DataLayerCommon.GetDeclaredFieldInHeirarchy(klass, m_fieldname);

            return Db4oClient.TypeResolver.Resolve(rfield.GetFieldType());
        }
    }
}
