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
using System.Text;
using OManager.BusinessLayer.Common;
using OME.Logging.Common;
using OME.Logging.Tracing;

namespace OManager.BusinessLayer.QueryManager
{
    public class OMQueryGroup
    {
        private List<OMQueryClause> m_listQueryClauses;
        private CommonValues.LogicalOperators m_groupLogicalOperator;

        public OMQueryGroup()
        {
           
            m_listQueryClauses = new List<OMQueryClause>();

        }
        public List<OMQueryClause> ListQueryClauses
        {
            get { return m_listQueryClauses; }
            set { m_listQueryClauses = value; }
        }

        public CommonValues.LogicalOperators GroupLogicalOperator
        {
            get { return m_groupLogicalOperator; }
            set { m_groupLogicalOperator = value; }
        }

        public void AddOMQueryClause(OMQueryClause omQueryClause)
        {

            m_listQueryClauses.Add(omQueryClause);
        }

        public override string ToString()
        {
            string strClause = string.Empty;
            try
            {
                foreach (OMQueryClause om in m_listQueryClauses)
                {
                    strClause = strClause + string.Format("{0} ", om.ToString());

                }
                if (strClause != string.Empty)
                {
                    string subStr = strClause.Substring(strClause.Length - 3, 3);

                    if (subStr == "OR ")
                        strClause = strClause.Substring(0, strClause.Length - 3);
                    else
                        strClause = strClause.Substring(0, strClause.Length - 4);


                    strClause = string.Format("{0}({1}) ", GroupLogicalOperator, strClause);

                }
                

                

            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                
            }
            return strClause;
        }
    }
}
