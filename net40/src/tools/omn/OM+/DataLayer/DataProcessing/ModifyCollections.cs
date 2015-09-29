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
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;
using OManager.DataLayer.Connection;
using OManager.DataLayer.CommonDatalayer;
using OManager.DataLayer.Reflection;
using OME.Logging.Common;

namespace OManager.DataLayer.ObjectsModification
{
    public class ModifyCollections
    {
        private readonly IObjectContainer objectContainer;
        public ModifyCollections()
        {
            objectContainer = Db4oClient.Client;
        }

        public void EditCollections(IList objList, IList<int> offsetList, IList<string> names, IList<IType> types, object value)
        {
            try
            {
                SetFieldForCollection(objList, offsetList, names, types, value);
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
        }

        public static void SaveCollections(object topObject, int level)
        {
            try
            {
                Db4oClient.Client.Ext().Store(topObject, level);
				Db4oClient.Client.Commit();
            }
            catch (Exception oEx)
            {
				Db4oClient.Client.Rollback();
                LoggingHelper.HandleException(oEx);

            }
        }

        public void SetFieldForCollection(IList objList, IList<int> offsetList, IList<string> names, IList<IType> types, object value)
        {
            try
            {
            	for (int i = 0; i < objList.Count; i++)
                {
                	if ( (objList[i]) == null && !types[i].IsNullable) 
						continue;

                	object obj;
                	if (CheckForCollections(objList[i]))
                	{
                		int offset = offsetList[i];

                		object col = objList[i];

                		if (offsetList[i + 1].Equals(-2))
                		{
                			if (col is Array || col is IList)
                			{
                				if (objList[i - 1] is IList)
                				{
                					obj = ((IList)objList[i - 1])[offset];
                				}
                				else
                				{
                					obj = objList[i - 1];
                				}
                				
								SaveValues(obj, names[i], value, offsetList[i], types[i]);
                				break;
                			}

                		}
                		else if (col is Hashtable || col is IDictionary)
                		{
                			object key = col is Hashtable
                			             	? ((DictionaryEntry) objList[i + 1]).Key
                			             	: KeyAtIndex((IDictionary) col, offset);
                			
							SaveDictionary(objList[i - 1], names[i], value, key);
                		}
                	}
                	else if (types[i].IsEditable)
                	{
                		obj = objList[i - 1];
                		SaveValues(obj, names[i], value, offsetList[i - 1], types[i]);
                	}
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
        }

    	private static object KeyAtIndex(IDictionary dictionary, int index)
    	{
    		foreach (DictionaryEntry entry in dictionary)
    		{
				if (index == 0)
    				return entry.Key;
    			
				index--;
    		}
    		
			return null;
    	}

    	public void SaveDictionary(object targetObject, string attribName, object newValue, object key)
        {
            try
            {
                IReflectClass rclass = DataLayerCommon.ReflectClassFor(targetObject);
                if (rclass != null)
                {
                    IReflectField rfield = DataLayerCommon.GetDeclaredFieldInHeirarchy(rclass, attribName);
                    if (rfield != null)
                    {
                        if (!(rfield is GenericVirtualField))
                        {
                            object objValue1 = rfield.Get(targetObject);
                            ICollection col = (ICollection) objValue1;
                            if (col is Hashtable)
                            {
                                Hashtable hash = (Hashtable)col;
                                hash.Remove(key);
                                hash.Add(key, newValue);
                                rfield.Set(targetObject, hash);
                            }
                            else if (col is IDictionary)
                            {
                                IDictionary dict = (IDictionary)col;
                                dict.Remove(key);
                                dict.Add(key, newValue);
                                rfield.Set(targetObject, dict);
                            }

                        }
                    }
                }
            }
            catch (Exception oEx)
            {
                objectContainer.Rollback();
                LoggingHelper.HandleException(oEx);

            }

        }

    	public void SaveValues(object targetObject, string attribName, object newValue, int offset, IType type)
        {
            try
            {
                IReflectClass rclass = DataLayerCommon.ReflectClassFor(targetObject);
                IReflectField rfield = DataLayerCommon.GetDeclaredFieldInHeirarchy(rclass, attribName);
                if (rfield != null && !(rfield is GenericVirtualField))
                {
                    if (type.IsArray || type.IsCollection)
                    {
                    	IList list = rfield.Get(targetObject) as IList;
						if (null != list)
                        {
							list[offset] = newValue;
							rfield.Set(targetObject, list);
                        }
                    }
                    else
                    {
						SetObject(rfield, targetObject, newValue);
                    }
                }
            }
            catch (Exception oEx)
            {
                objectContainer.Rollback();
                LoggingHelper.HandleException(oEx);
            }
        }

        public bool CheckForCollections(object obj)
        {
            try
            {
                return DataLayerCommon.IsCollection(obj) || DataLayerCommon.IsArray(obj);
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return false;
            }
        }

        public void SetObject(IReflectField rfield, object containingObject, object newValue)
        {
            try
            {
				rfield.Set(containingObject, newValue);
            }
            catch (Exception oEx)
            {
                objectContainer.Rollback();
                LoggingHelper.HandleException(oEx);
            }
        }
    }
}
