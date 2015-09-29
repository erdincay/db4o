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
using OManager.DataLayer.CommonDatalayer;
namespace OManager.BusinessLayer.QueryManager
{
    public class OMQueryClause
    {

        private string m_Classname;//This will be a fully qualified classname like pilot.car.name

        public string Classname
        {
            get { return m_Classname; }
            //set { m_Classname = value; }
        }
        private string m_Operator;

        public string Operator
        {
            get { return m_Operator; }
            //set { m_Operator = value; }
        }
        private string m_Value;

        public string Value
        {
            get { return m_Value; }
            //set { m_Value = value; }
        }
        private string m_Fieldname;//Field inside the above class.

        public string Fieldname
        {
            get { return m_Fieldname; }
            //set { m_Fieldname = value; }
        }
        private string m_FieldType;

        public string FieldType
        {
            get { return m_FieldType; }
            //set { m_FieldType = value; }
        }

        private CommonValues.LogicalOperators m_clauseLogicalOperator;

        public CommonValues.LogicalOperators ClauseLogicalOperator
        {
            get { return m_clauseLogicalOperator; }
            set { m_clauseLogicalOperator = value; }
        }

        public OMQueryClause(string classname, string fieldname, string fieldoperator, string fieldvalue, CommonValues.LogicalOperators clauseLogicalOperator,string fieldtype)
        {
            m_clauseLogicalOperator = clauseLogicalOperator;
            m_Classname = DataLayerCommon.RemoveGFromClassName(classname);
            m_Fieldname = fieldname;
            m_Operator = fieldoperator;
            m_Value = fieldvalue;
            m_FieldType = fieldtype;  
 

        }
        

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", m_Fieldname, m_Operator, m_Value, m_clauseLogicalOperator);
        }


    }
}
