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
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;
using OManager.BusinessLayer.Common;
using OManager.DataLayer.Connection;
using OME.Logging.Common;

namespace OManager.DataLayer.CommonDatalayer
{
    public class DataLayerCommon
    {
    	public static string Db4oVersion
    	{
			get { return Db4oFactory.Version(); }
    	}

        public static IReflectField GetDeclaredFieldInHeirarchy(IReflectClass aClass, string attribute)
        {
            try
            {
                while (aClass != null)
                {
					IReflectField refField = GetDeclaredField(aClass, attribute);
                    if (refField != null)
                        return refField;

                    aClass = aClass.GetSuperclass();
                }
            }
            catch (Exception e)
            {
                LoggingHelper.HandleException(e);
            }
            return null;

        }

        public static IStoredField GetDeclaredStoredFieldInHeirarchy(IStoredClass aClass, string attribute)
        {
            try
            {
                IStoredField sField = null;
                if (aClass == null)
                    return null;

                while (aClass != null)
                {

                    sField = aClass.StoredField(attribute, null);
                    if (sField != null)
                        break;
                    aClass = aClass.GetParentStoredClass();

                }
                return sField;
            }
            catch (Exception e)
            {
                LoggingHelper.HandleException(e);
            }
            return null;

        }

        public static IReflectField GetDeclaredField(IReflectClass aClass, string attribute)
        {
            try
            {
            	return aClass.GetDeclaredField(attribute);
            }
            catch (Exception e)
            {
                LoggingHelper.HandleException(e);
            }
            return null;
        }

        public static IReflectField[] GetDeclaredFieldsInHeirarchy(IReflectClass aClass) //67
        {
            try
            {
                return GetFieldList(aClass).ToArray();
            }
            catch (Exception e)
            {
                LoggingHelper.HandleException(e);
            }
            return null;
        }

        public static List<IReflectField> GetFieldList(IReflectClass aClass)
        {
            try
            {
                if (aClass == null)
                    return null;

                List<IReflectField> ret = NonVirtualFieldsFor(aClass);
                IReflectClass parent = aClass.GetSuperclass();
                if (parent != null && !(parent.GetName().StartsWith("System.")))
                    ret.AddRange(GetFieldList(parent));

				return ret;
            }
            catch (Exception e)
            {
                LoggingHelper.HandleException(e);
            }
            return null;
        }

        public static List<IReflectField> NonVirtualFieldsFor(IReflectClass aClass)
        {
            try
            {
                List<IReflectField> ret = new List<IReflectField>();
            	foreach (IReflectField field in aClass.GetDeclaredFields())
            	{
					if (!(field is GenericVirtualField))
						ret.Add(field);
            	}
                return ret;
            }
            catch (Exception e)
            {
                LoggingHelper.HandleException(e);
            }
            return null;
        }
       

        public static string RemoveGFromClassName(string name)
        {
			return name.Contains("(G) ") ? name.Replace("(G) ", "") : name;
        }

        public static IReflectClass ReflectClassForName(string classname)
        {
            try
            {
                IObjectContainer objectContainer = Db4oClient.Client;
                if (classname != string.Empty)
                {
                   classname= RemoveGFromClassName(classname);
                    return objectContainer.Ext().Reflector().ForName(classname);
                }
            }
            catch (Exception e)
            {
                LoggingHelper.HandleException(e);
            }
            return null;
        }
		public static IReflectClass ReflectClassFor(object obj)
        {
            try
            {
                if (obj != null)
                {
                    IObjectContainer objectContainer = Db4oClient.Client;                   

                    return objectContainer.Ext().Reflector().ForObject(obj);
                }
            }
            catch (Exception e)
            {
                LoggingHelper.HandleException(e);
            }
            return null;
           
        }
        public static bool IsCollection(object expandedObj)
        {
            try
            {
                if (expandedObj != null)
                {
                    IObjectContainer objectContainer = Db4oClient.Client;
                    IReflectClass refClass = objectContainer.Ext().Reflector().ForObject(expandedObj);
                    return refClass.IsCollection();
                }
                return false; 
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return false;
            }
        }

        public static bool IsArray(object expandedObj)
        {
            try
            {
                if (expandedObj != null)
                {
                    IReflectClass refClass = ReflectClassFor(expandedObj);
                    if (refClass != null)
                    {
                        return refClass.IsArray();
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


        public static  bool IsPrimitive(object expandedObj)
        {
            try
            {
               IReflectClass refClass = ReflectClassFor(expandedObj);// objectContainer.Ext().Reflector().ForObject(expandedObj);              
               if (refClass != null)
               {
                   string type = PrimitiveType(refClass.GetName());
                   return CommonValues.IsPrimitive(type);
               }
               return false;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return false;
            }
        }
        public static bool CheckForDatetimeOrString(object expandedObj)
        {
            try
            {
                IReflectClass refClass = ReflectClassFor(expandedObj);// objectContainer.Ext().Reflector().ForObject(expandedObj);              
                if (refClass != null)
                {
                    string type = PrimitiveType(refClass.GetName());
                    return CommonValues.IsDateTimeOrString(type); 
                }
                return false;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return false;
            }
        }

        public static string PrimitiveType(string type)
        {
            if (type.Contains(","))
            {
                int Index = type.LastIndexOf(',');
                type = type.Substring(0, Index);
            }
            return type;
        }
       
    }
}
