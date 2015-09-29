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
using System.Text;
using OManager.BusinessLayer.Common;
using OME.Logging.Common;
using OME.Logging.Tracing;

namespace OManager.BusinessLayer.QueryManager
{
    public class OMQuery
    {
        private List<OMQueryGroup> m_listQueryGroup ;
        private string m_baseClass;
        private Hashtable m_attributeList;
        private DateTime m_queryTimeStamp;

        public OMQuery(string baseclass,DateTime queryTimeStamp )
        {
            m_baseClass = baseclass;
            m_queryTimeStamp = queryTimeStamp;
            m_listQueryGroup= new List<OMQueryGroup>();       
            m_attributeList= new Hashtable();
        }
        public DateTime QueryTimestamp
        {
            get { return m_queryTimeStamp; }
            set { m_queryTimeStamp = value; }
        }


        public Hashtable AttributeList
        {
            get { return m_attributeList; }
            set { m_attributeList = value; }
        }

        public string BaseClass
        {
            get { return m_baseClass; }
            set { m_baseClass = value; }
        }
        private string queryString;

        public string QueryString
        {
            get { return queryString; }
            set { queryString = value; }
        }


        

        //OmQuery change name to QueryGroupList
        public List<OMQueryGroup> ListQueryGroup
        {
            get { return m_listQueryGroup; }
            set { m_listQueryGroup = value; } 
        }

        //public Common.Common.LogicalOperators logicalOperator;

        public void AddOMQuery(OMQueryGroup omQuerygroup)
        {

            //ValidateOMQueryToAdd(omQueryClause);
            ListQueryGroup.Add(omQuerygroup);
        }      

       
        public override string ToString()
        {
            try
            {
                string groupString = string.Empty;
                foreach (OMQueryGroup group in m_listQueryGroup)
                {

                    groupString = groupString + string.Format("{0}", group.ToString());
                }

                groupString = groupString.Replace("EMPTY", "");

                return groupString;
                
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return string.Empty;  
            }
           
        }
    }
   
    public class CompareQueryTimestamps : IComparer<OMQuery>
    {
        public int Compare(OMQuery q1, OMQuery q2)
        {
            
                return q2.QueryTimestamp.CompareTo(q1.QueryTimestamp);
        }
    }
}
