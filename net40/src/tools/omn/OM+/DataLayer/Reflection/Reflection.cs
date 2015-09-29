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
using System.Collections.Generic;
using Db4objects.Db4o.Reflect;

namespace OManager.DataLayer.Reflection
{
    public interface ITypeResolver
    {
        IType Resolve(string typeFQN);
        IType Resolve(IReflectClass klass);
    }

    public class TypeResolver : ITypeResolver
    {
		private readonly IReflector _reflector;
		private readonly IDictionary<string, IType> _resolved = new Dictionary<string, IType>();

		public TypeResolver(IReflector reflector)
		{
			_reflector = reflector;
		}

		public IType Resolve(string typeFQN)
		{
			return Resolve(_reflector.ForName(typeFQN));
		}

        public IType Resolve(IReflectClass klass)
        {
            if (klass == null)
            {
                return null;
            }

            string className = klass.GetName();
            if (!_resolved.ContainsKey(className))
            {
                _resolved[className] = new TypeImpl(klass, this);
            }

            return _resolved[className];

        }
    }
}
