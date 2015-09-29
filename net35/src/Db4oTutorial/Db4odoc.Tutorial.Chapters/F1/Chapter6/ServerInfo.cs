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
namespace Db4odoc.Tutorial.F1.Chapter6
{
    /// <summary>
    /// Configuration used for StartServer and StopServer.
    /// </summary>
    public class ServerInfo
    {
        /// <summary>
        /// the host to be used.
        /// If you want to run the client server examples on two computers,
        /// enter the computer name of the one that you want to use as server. 
        /// </summary>
        public const string HOST = "localhost";  

        /// <summary>
        /// the database file to be used by the server.
        /// </summary>
        public const string FILE = "formula1.yap";

        /// <summary>
        /// the port to be used by the server.
        /// </summary>
        public const int PORT = 4488;

        /// <summary>
        /// the user name for access control.
        /// </summary>
        public const string USER = "db4o";
    
        /// <summary>
        /// the pasword for access control.
        /// </summary>
        public const string PASS = "db4o";
    }
}