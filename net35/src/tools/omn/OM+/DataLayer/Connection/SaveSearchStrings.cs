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
using OManager.BusinessLayer.Login;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using OME.Logging.Common;
using OME.Logging.Tracing;

namespace OManager.DataLayer.Connection
{
    

    public class GroupofSearchStrings
    {

        ConnParams m_connParam;

        public ConnParams ConnParam
        {
            get { return m_connParam; }
            set { m_connParam = value; }
        }
        List<SeachString> m_SearchStringList;
        [Transient]
        IObjectContainer container = null;

        public List<SeachString> SearchStringList
        {
            get { return m_SearchStringList; }
            
        }
        private long m_TimeOfCreation;
        public long TimeOfCreation
        {
            get { return m_TimeOfCreation; }
            set { m_TimeOfCreation = value; }
        }

        public GroupofSearchStrings(ConnParams connParam)
        {           
            m_connParam = connParam;
            m_SearchStringList = new List<SeachString>(); 
        }

        internal void AddSearchStringToList(SeachString strToAdd)
        {
            try
            {
                container = Db4oClient.RecentConn;
                if (m_SearchStringList != null)
                {
                    GroupofSearchStrings groupSearchString = FetchAllSearchStringsForAConnection();

                    if (groupSearchString == null)
                    {
                        groupSearchString = new GroupofSearchStrings(m_connParam);                       
                        List<SeachString> l=new List<SeachString>();
                        l.Add(strToAdd);
                        groupSearchString.m_SearchStringList = l;
                        groupSearchString.m_TimeOfCreation = Sharpen.Runtime.CurrentTimeMillis(); 
                        container.Store(groupSearchString);
                        container.Commit();
                        return; 
                    }

                    List<SeachString> searchStringForConnection = groupSearchString.m_SearchStringList;

                    if (searchStringForConnection.Count >= 20)
                    {
                        bool check = false;
                        SeachString temp=null;
                        foreach (SeachString str in searchStringForConnection)
                        {
                            if (str.SearchString.Equals(strToAdd.SearchString))
                            {
                                temp = str;
                                check = true;
                                break;
                            }
                        }
                        if (check == false)
                        {
                            temp = searchStringForConnection[searchStringForConnection.Count - 1];
                            searchStringForConnection.Remove(temp);
                            strToAdd.Timestamp = DateTime.Now;
                            searchStringForConnection.Add(strToAdd);

                        }
                        else
                        {
                            searchStringForConnection.Remove(temp);
                            strToAdd.Timestamp = DateTime.Now;
                            searchStringForConnection.Add(strToAdd);

                        }
                        
                        container.Delete(temp);  
                        CompareSearchStringTimestamps cmp = new CompareSearchStringTimestamps();
                        searchStringForConnection.Sort(cmp);
                        groupSearchString.m_SearchStringList = searchStringForConnection;
                        container.Ext().Store(groupSearchString, 5);
                        container.Commit();

                    }

                    else
                    {
                        bool checkstr = false;
                        SeachString temp=null;
                        foreach (SeachString str in searchStringForConnection)
                        {
                            if (str.SearchString.Equals(strToAdd.SearchString))
                            {
                                temp = str;
                                checkstr = true;
                                break;
                            }
                        }
                        if (checkstr == false)
                        {                            
                            searchStringForConnection.Add(strToAdd);

                        }
                        else
                        {
                            searchStringForConnection.Remove(temp);
                            strToAdd.Timestamp = DateTime.Now;
                            searchStringForConnection.Add(strToAdd);
                            container.Delete(temp);
                        }
                        
                        
                        CompareSearchStringTimestamps cmp = new CompareSearchStringTimestamps();
                        searchStringForConnection.Sort(cmp);
                        groupSearchString.m_SearchStringList = searchStringForConnection;
                        container.Ext().Store(groupSearchString, 5);
                        container.Commit();

                    }


                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                container = null;
                
            }
        }

        private  GroupofSearchStrings FetchAllSearchStringsForAConnection()
        {
            GroupofSearchStrings grpSearchStrings = null;
            try
            {
                container = Db4oClient.RecentConn;
                IQuery query = container.Query();
                query.Constrain(typeof(GroupofSearchStrings));
                query.Descend("m_connParam").Descend("m_connection").Constrain(m_connParam.Connection);
                IObjectSet objSet = query.Execute();
                if (objSet.Count != 0)
                {
                    grpSearchStrings = (GroupofSearchStrings)objSet.Next();
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }

            return grpSearchStrings;
        }

        public long ReturnTimeWhenSearchStringCreated()
        {
            try
            {

                GroupofSearchStrings grpSearchStrings = FetchAllSearchStringsForAConnection();
                if (grpSearchStrings != null)
                    return grpSearchStrings.TimeOfCreation > 0 ? grpSearchStrings.TimeOfCreation : 0;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
            return 0;
            
        }
        internal List<string> ReturnStringList()
        {
            List<string> stringList = null;
            try
            {

                GroupofSearchStrings groupSearchList = FetchAllSearchStringsForAConnection();
                if (groupSearchList != null)
                {
                    List<SeachString> searchStringList = groupSearchList.m_SearchStringList;
                    stringList = new List<string>();
                    foreach (SeachString str in searchStringList)
                    {
                        stringList.Add(str.SearchString);
                    }
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }

            return stringList;
        }
        internal void RemovesSearchStringsForAConnection()
        {

            try
            {
                GroupofSearchStrings grpSearchString = FetchAllSearchStringsForAConnection();
                if (grpSearchString != null)
                {
                 
                    for (int i = 0; i < grpSearchString.m_SearchStringList.Count; i++)
                    {
                        SeachString sString = grpSearchString.m_SearchStringList[i];
                        //foreach (SeachString sString in grpSearchString.m_SearchStringList)

                        if (sString != null)
                        {
                            //grpSearchString.m_SearchStringList.Remove(sString);
                            container.Delete(sString);
                        }

                    }
                    grpSearchString.m_SearchStringList.Clear();
                    grpSearchString.m_TimeOfCreation = Sharpen.Runtime.CurrentTimeMillis();
                    container.Ext().Store(grpSearchString, 5);  
                    container.Commit();
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }


        }
    }
    public class CompareSearchStringTimestamps : IComparer<SeachString>
    {
        public int Compare(SeachString s1, SeachString s2)
        {

            return s2.Timestamp.CompareTo(s1.Timestamp);
        }
    }
}
