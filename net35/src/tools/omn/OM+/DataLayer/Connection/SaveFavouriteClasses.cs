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
using OManager.BusinessLayer.Login;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using OME.Logging.Common;
using OME.Logging.Tracing;

namespace OManager.DataLayer.Connection
{
    class FavouriteList
    {
        List<FavouriteFolder> m_lstFavfolder;

        internal List<FavouriteFolder> lstFavFolder
        {
            get { return m_lstFavfolder; }
            set { m_lstFavfolder = value; }
        }
        ConnParams m_connParam;

        public ConnParams ConnParam
        {
            get { return m_connParam; }
            set { m_connParam = value; }
        }
        private long m_TimeOfCreation;
        public long TimeOfCreation
        {
            get { return m_TimeOfCreation; }
            set { m_TimeOfCreation = value; }
        }

        [Transient]
        IObjectContainer container = null;

       
        public FavouriteList(ConnParams connParam)
        {
            m_connParam = connParam;           
            m_lstFavfolder = new List<FavouriteFolder>();
            
        }
        internal void AddFolderToDatabase(FavouriteFolder favFolder)
        {
            try
            {
                container = Db4oClient.RecentConn;
                if (m_lstFavfolder != null)
                {
                    FavouriteList favList = FetchAllFavouritesForAConnection();
                   
                    if (favList == null)
                    {
                        favList = new FavouriteList(m_connParam);
                        List<FavouriteFolder> lstFavfolder = new List<FavouriteFolder>();
                        favList.m_TimeOfCreation = Sharpen.Runtime.CurrentTimeMillis();   
                        lstFavfolder.Add(favFolder);
                        container.Store(favList);
                        container.Commit();
                        return;
                    }
                    container.Activate(favList, 5);
                    List<FavouriteFolder> lstFavFolder = favList.lstFavFolder;
                   
                    bool check = false;
                    FavouriteFolder temp = null;
                    foreach (FavouriteFolder str in lstFavFolder)
                    {
                        if (str != null)
                        {
                            if (str.FolderName.Equals(favFolder.FolderName))
                            {
                                temp = str;
                                check = true;
                                break;
                            }
                        }
                    }
                    if (check == false)
                    {

                        lstFavFolder.Add(favFolder);
                    }
                    else
                    {
                        lstFavFolder.Remove(temp);
                        lstFavFolder.Add(favFolder);
                        container.Delete(temp);
                    }


                    favList.lstFavFolder = lstFavFolder;
                    container.Ext().Store(favList, 5);
                    container.Commit();


                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);  
            }
        }


        internal void RemoveFolderfromDatabase(FavouriteFolder favFolder)
        {
            try
            {
                container = Db4oClient.RecentConn;
                FavouriteList favList = FetchAllFavouritesForAConnection();


                List<FavouriteFolder> lstFavFolder = favList.lstFavFolder;
                bool check = false;
                FavouriteFolder temp = null;
                foreach (FavouriteFolder str in lstFavFolder)
                {
                    if (str.FolderName.Equals(favFolder.FolderName))
                    {
                        temp = str;
                        check = true;
                        break;
                    }
                }
                if (check == true)
                {
                    lstFavFolder.Remove(temp);
                    favList.lstFavFolder = lstFavFolder;
                    container.Delete(temp);
                    container.Ext().Store(favList, 5);
                    container.Commit();
                }
            }
            
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);  
            }

        }


        internal void RenameFolderInDatabase(FavouriteFolder oldFavFolder, FavouriteFolder newFavFolder)
        {
            try
            {
                container = Db4oClient.RecentConn;
                FavouriteList favList = FetchAllFavouritesForAConnection();


                List<FavouriteFolder> lstFavFolder = favList.lstFavFolder;
                bool check = false;
                FavouriteFolder temp = null;
                foreach (FavouriteFolder str in lstFavFolder)
                {
                    if (str != null)
                    {
                        if (str.FolderName.Equals(oldFavFolder.FolderName))
                        {
                            temp = str;
                            check = true;
                            break;
                        }
                    }
                }
                if (check == true)
                {
                    lstFavFolder.Remove(temp);
                    container.Delete(temp);
                    temp.FolderName = newFavFolder.FolderName;
                    lstFavFolder.Add(temp);
                    favList.lstFavFolder = lstFavFolder;
                    container.Ext().Store(favList, 5);
                    container.Commit();
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }

        }
       
        internal FavouriteList FindFolderWithClassesByFolderName(string folderName)
        {
            FavouriteList FavList = null;
            try
            {
                container = Db4oClient.RecentConn;
                IQuery query = container.Query();
                query.Constrain(typeof(FavouriteList));
                query.Descend("m_connParam").Descend("m_connection").Constrain(m_connParam.Connection);
                query.Descend("m_lstFavfolder").Descend("m_folderName").Constrain(folderName);
                IObjectSet objSet = query.Execute();
                if (objSet.Count != 0)
                {
                    FavList = (FavouriteList)objSet.Next();
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }

            return FavList ;
        }
      

        private FavouriteList FetchAllFavouritesForAConnection()
        {
            FavouriteList FavList = null;
            try
            {

                container = Db4oClient.RecentConn;
                IQuery query = container.Query();
                query.Constrain(typeof(FavouriteList));
                query.Descend("m_connParam").Descend("m_connection").Constrain(m_connParam.Connection);                
                IObjectSet objSet = query.Execute();
                if (objSet != null)
                {
                    if (objSet.Count != 0)
                    {
                        FavList = (FavouriteList)objSet.Next();
                    }
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }

            return FavList;
        }

        internal List<FavouriteFolder> ReturnFavouritFolderList()
        {
            List<FavouriteFolder> FavList = null;
            try
            {

                FavouriteList Fav = FetchAllFavouritesForAConnection();
                if (Fav != null)
                {
                    FavList = Fav.lstFavFolder;                    
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }

            return FavList;
        }

        internal long ReturnTimeWhenFavouriteListCreated()
        {
            FavouriteList Fav = null;

            try
            {

                Fav = FetchAllFavouritesForAConnection();
                if (Fav != null)
                    return Fav.TimeOfCreation > 0 ? Fav.TimeOfCreation : 0;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }

            return 0;
        }
        internal void RemoveFavouritFolderForAConnection()
        {

            try
            {
                FavouriteList Fav = FetchAllFavouritesForAConnection();
                if (Fav != null)
                {
                   for (int i = 0; i < Fav.lstFavFolder.Count; i++)
                    {
                        FavouriteFolder favFolder = Fav.lstFavFolder[i];
                        if (favFolder != null)

                            container.Delete(favFolder);


                    }
                    Fav.lstFavFolder.Clear();
                    Fav.m_TimeOfCreation = Sharpen.Runtime.CurrentTimeMillis();
                    container.Delete(Fav);
                    container.Ext().Store(Fav, 5);
                    container.Commit();
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }

            
        }

    }

   
}
