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
using Db4objects.Db4o.Reflect;
using OManager.DataLayer.Connection;
using OManager.DataLayer.Modal;
using System.Collections;
using Db4objects.Db4o.Reflect.Generic;
using OManager.DataLayer.Reflection;
using OME.Logging.Common;

namespace OManager.DataLayer.PropertyTable
{
    public class FieldProperties
    {
		private string m_fieldName;
		private bool m_isIndexed;
		private bool m_isPublic;
		private readonly string m_dataType;
        private readonly IType m_type;
		public FieldProperties(string fieldName, string fieldType)
		{
			m_fieldName = fieldName;

            m_type = Db4oClient.TypeResolver.Resolve(fieldType);
            m_dataType = m_type.DisplayName;
		}

        public bool Indexed
        {
            get { return m_isIndexed; }
            set { m_isIndexed = value; }
        }

        public bool Public
        {
            get { return m_isPublic; }
            set { m_isPublic = value; }
        }

        public string Field
        {
            get { return m_fieldName; }
            set { m_fieldName = value; }
        }

        public string DataType
        {
            get { return m_dataType; }
        }

        public IType Type
        {
            get { return m_type; }
        }

        public static ArrayList FieldsFrom(string className)
        {
            try
            {
                ArrayList listFieldProperties = new ArrayList();
                ClassDetails clDetails = new ClassDetails(className);
            	foreach (IReflectField field in clDetails.GetFieldList())
                {
                    if (!(field is GenericVirtualField))
                    {
                    	FieldProperties fp = FieldPropertiesFor(className, field);
                    	listFieldProperties.Add(fp);
                    }
                }
                return listFieldProperties;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return null;
            }
        }

    	private static FieldProperties FieldPropertiesFor(string className, IReflectField field)
    	{
    		FieldProperties fp = new FieldProperties(field.GetName(), field.GetFieldType().GetName());
    		FieldDetails fd = new FieldDetails(className, fp.Field);
    		fp.m_isPublic = fd.GetModifier();
    		fp.m_isIndexed = fd.IsIndexed();
    		
			return fp;
    	}
    }

}
