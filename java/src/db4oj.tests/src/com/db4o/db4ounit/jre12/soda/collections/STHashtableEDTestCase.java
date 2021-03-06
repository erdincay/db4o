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
package com.db4o.db4ounit.jre12.soda.collections;
import java.util.*;

import com.db4o.query.*;


/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class STHashtableEDTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase {
	
	public static class ExtendHashtable extends Hashtable{
	}
	
	protected ExtendHashtable vec(Object[] objects){
		ExtendHashtable h = new ExtendHashtable();
		for (int i = 0; i < objects.length; i++) {
			h.put(objects[i], new Integer(i));
		}
		return h;
	}

	public Object[] createData() {
		return new Object[] {
			vec(new Object[] { new Integer(6778), new Integer(6779)}), 
			vec(new Object[] { new Integer(6778), new Integer(6789)}),
			vec(new Object[] { "foo677", new STElement("bar677", "barbar677")}),
			vec(new Object[] { "foo6772", new STElement("bar677", "barbar2677")})
		};
	}
	
	public void testDefaultContainsInteger() {
		Query q = newQuery();
		
		q.constrain(vec(new Object[] { new Integer(6778)}));
		expect(q, new int[] { 0, 1 });
	}

	public void testDefaultContainsString() {
		Query q = newQuery();
		
		q.constrain(vec(new Object[] { "foo677" }));
		expect(q, new int[] { 2 });
	}

	public void testDefaultContainsTwo() {
		Query q = newQuery();
		
		q.constrain(vec(new Object[] { new Integer(6778), new Integer(6789)}));
		expect(q, new int[] { 1 });
	}

	public void testDefaultContainsObject() {
		Query q = newQuery();
		
		q.constrain(vec(new Object[] { new STElement("bar677", null)}));
		expect(q, new int[] { 2, 3 });
	}
	
}