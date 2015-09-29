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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Tests.CLI1
{

    public class JavaSimpleChecksumCompatibilityTestCase : JavaCompatibilityTestCaseBase
    {
        private String _expectedJavaOutput;

        public void Test()
        {
            int[] ints = new int[]{
				int.MinValue, 
				-1, 
				0, 
				1, 
				int.MaxValue};
            int bufferLength = Const4.IntLength * ints.Length;
            ByteArrayBuffer buffer = new ByteArrayBuffer(bufferLength);
            for (int i = 0; i < ints.Length; i++)
            {
                buffer.WriteInt(ints[i]);
            }
            long checkSum = CRC32.CheckSum(buffer._buffer, 0, buffer._buffer.Length);

            _expectedJavaOutput = checkSum.ToString();
            RunTest();
        }

        protected override string ExpectedJavaOutput()
        {
            return _expectedJavaOutput;
        }

        protected override void PopulateContainer(IObjectContainer container)
        {
            //  do nothing
        }

        protected override JavaSnippet JavaCode()
        {
            return new JavaSnippet("com.db4o.test.compatibility.Program", @"
package com.db4o.test.compatibility;

import com.db4o.foundation.*;
import com.db4o.internal.*;

public class Program {
	public static void main(String[] args) {

		int[] ints = new int[]{
				Integer.MIN_VALUE, 
				-1, 
				0, 
				1, 
				Integer.MAX_VALUE};
		int bufferLength = Const4.INT_LENGTH * ints.length;
		ByteArrayBuffer buffer = new ByteArrayBuffer(bufferLength);
		for (int i = 0; i < ints.length; i++) {
			buffer.writeInt(ints[i]);
		}
		long checkSum = CRC32.checkSum(buffer._buffer, 0, buffer._buffer.length);
		System.out.print(checkSum);

	}

}");
        }

    }
}
