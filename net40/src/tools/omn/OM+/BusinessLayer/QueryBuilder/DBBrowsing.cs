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
using OManager.BusinessLayer.Common;

namespace OManager.BusinessLayer.ObjectExplorer
{
    public class QueryHelper
    {
        public static string[] ConditionsFor(string typeName)
        {
            string[] operatorList;

            switch (typeName)
            { 
                case BusinessConstants.STRING:
                    operatorList = CommonValues.StringConditions;
                    break;

                case BusinessConstants.CHAR:
                    operatorList = CommonValues.CharacterCondition;
                    break;

                case BusinessConstants.INT16:
                case BusinessConstants.DOUBLE:
                case BusinessConstants.DECIMAL:
                case BusinessConstants.INT32:
                case BusinessConstants.INT64:
                case BusinessConstants.INTPTR:
                case BusinessConstants.UINT16:
                case BusinessConstants.UINT32:
                case BusinessConstants.UINT64:
                case BusinessConstants.UINTPTR:
                case BusinessConstants.SINGLE:
                case BusinessConstants.SBYTE:
                case BusinessConstants.BYTE:
                    operatorList = CommonValues.NumericConditions;
                    break;

                case BusinessConstants.BOOLEAN:
                    operatorList = CommonValues.BooleanConditions;
                    break;

                case BusinessConstants.DATETIME:
                    operatorList = CommonValues.DateTimeConditions;
                    break;

                default:
                    operatorList = new string[0];
                    break;
            }

            return operatorList;
        }

        public static string[] GetOperators()
        {
            return CommonValues.Operators;
        }
    }
}
