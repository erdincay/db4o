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

namespace OME.Logging.Exceptions
{
    public enum CriticalityLevel
    {
        LOW = 1,
        MEDIUM,
        HIGH
    }

    public enum StackTraceType
    {
        BRIEF = 1,
        ENHANCED
    }

    public class OMEException : Exception
    {
        #region Public Members
        public string ErrorCode;//ExMassage is a constant string that is defined in Constant.cs
        public string ExMessage;
        public string SysErrorMessage;
         public string AdditionalInfoToWrite; // Additional information internaly in the exception log.
        public CriticalityLevel criticalityLevel; //Defined in Enum.cs
        public StackTraceType traceInfo;
        public Exception innerException; 
        #endregion 

        #region Constructor and Destructor
        public OMEException()
        {
            ExMessage = string.Empty;
            criticalityLevel = CriticalityLevel.LOW;
            traceInfo = StackTraceType.BRIEF;
            
        }

        public OMEException(string errorCode, Exception ex,CriticalityLevel level)
        {
            ErrorCode = errorCode;
            innerException = ex ;
            criticalityLevel = level ;
            traceInfo = StackTraceType.BRIEF;
        }

        public OMEException(string errorCode, string errorMessage,CriticalityLevel level)
        {
            ErrorCode = errorCode;
            ExMessage = errorMessage; 
            
            criticalityLevel =level;
            traceInfo = StackTraceType.BRIEF;
        }

        public OMEException(string errorCode, string errorMessage, CriticalityLevel level, Exception innerException)
        {
            ErrorCode = errorCode;
            ExMessage = errorMessage;
            this.innerException = innerException;
            criticalityLevel = level;
            traceInfo = StackTraceType.BRIEF;
        }


        public OMEException(string errorCode, string errorMessage, CriticalityLevel level, string additionalInfoToWrite)
        {
            ErrorCode = errorCode;
            ExMessage = errorMessage;
            AdditionalInfoToWrite = additionalInfoToWrite;
            criticalityLevel = level;
            traceInfo = StackTraceType.BRIEF;
        }

        public OMEException(string errorCode, Exception ex, CriticalityLevel level, StackTraceType traceinfo)
        {
            ErrorCode = errorCode;             
            innerException = ex;
            criticalityLevel = level;
            traceInfo = traceinfo;
        }

        public OMEException(string errorCode, CriticalityLevel level, StackTraceType traceinfo)
        {
            ErrorCode = errorCode;
            criticalityLevel = level;
            traceInfo = traceinfo;
        }

        public OMEException(string errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ExMessage = errorMessage;
            criticalityLevel = CriticalityLevel.HIGH;
            traceInfo = StackTraceType.ENHANCED;
        }

        #endregion 
    }
}
