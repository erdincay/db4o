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
using Db4objects.Db4o.CS;

namespace Db4objects.Db4o.Tutorial
{
    using System;
    using System.IO;
    using System.Reflection;
    using Db4odoc.Tutorial.F1;
    
	public class ExampleRunner
    {   
        /// <summary>
        /// Executes the method passed as argument if its signature is
        /// correct.
        /// </summary>
        /// <returns>true if the method was executed, false otherwise</returns>
        delegate bool Executor(MethodInfo method);

        
        Executor[] _executors = new Executor[] {
            new Executor(PlainExecutor),
            new Executor(ContainerExecutor),
            new Executor(LocalServerExecutor),
            new Executor(RemoteServerExecutor)
        };

        readonly static string DatabaseFileName = Path.Combine(
                               Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                               "formula1.odb");

        readonly static int ServerPort = 0xdb40;

        readonly static string ServerUser = "user";

        readonly static string ServerPassword = "password";

		
        public void Reset()
        {
            File.Delete(DatabaseFileName);
        }
        
        public void Run(string typeName, string method, TextWriter console)
        {
            TextWriter saved = Console.Out;         
            Console.SetOut(console);
            try
            {
                RunExample(typeName, method);               
            }
            finally
            {
                Console.SetOut(saved);
            }
        }
        
        void RunExample(string typeName, string method)
        {
        	Type type = typeof(Util).Assembly.GetType(typeName, true);
			MethodInfo example = type.GetMethod(method, BindingFlags.IgnoreCase|BindingFlags.Static|BindingFlags.Public);
            
            bool found = false;
            foreach (Executor _e in _executors)
            {
                if (_e(example))
                {
                    found = true;
                    break;
                }
            }
            
            if (!found)
            {
                throw new ArgumentException("No executor found for method '" + example + "'");
            }
        }
    
        static bool ContainerExecutor(MethodInfo method)
        {
            if (!CheckSignature(method, typeof(IObjectContainer)))
            {
                return false;
            }

            IObjectContainer container = Db4oEmbedded.OpenFile(Db4oEmbedded.NewConfiguration(), DatabaseFileName);
            try
            {
                method.Invoke(null, new object[] { container });
            }
            finally
            {
                container.Close();
            }
            return true;
        }
        
        static bool LocalServerExecutor(MethodInfo method)
        {
            if (!CheckSignature(method, typeof(IObjectServer)))
            {
                return false;
            }

            IObjectServer server = Db4oClientServer.OpenServer(Db4oClientServer.NewServerConfiguration(), DatabaseFileName, 0);
            try
            {                
                method.Invoke(null, new object[] { server });
            }
            finally
            {
                server.Close();
            }
            return true;
        }
        
        static bool RemoteServerExecutor(MethodInfo method)
        {
            if (!CheckSignature(method, typeof(int), typeof(string), typeof(string)))
            {
                return false;
            }

            IObjectServer server = Db4oClientServer.OpenServer(Db4oClientServer.NewServerConfiguration(), 
                DatabaseFileName, ServerPort);
            try
            {   
                server.GrantAccess(ServerUser, ServerPassword);
                method.Invoke(null, new object[] { ServerPort, ServerUser, ServerPassword });                
            }
            finally
            {
                server.Close();
            }
            return true;
        }
    
        static bool PlainExecutor(MethodInfo method)
        {
            if (0 != method.GetParameters().Length)
            {
                return false;
            }
            method.Invoke(null, new object[0]);
            return true;
        }
        
        static bool CheckSignature(MethodInfo method, params Type[] types)
        {
            ParameterInfo[] parameters = method.GetParameters();
            if (types.Length != parameters.Length)
            {
                return false;
            }
            
            for (int i=0; i<parameters.Length; ++i)
            {
                if (types[i] != parameters[i].ParameterType)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
