namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using PalashDynamics.ValueObjects.Administration.Menu;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Text;

    public class clsMenuDAL : clsBaseMenuDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsMenuDAL()
        {
            try
            {
                if (this.dbServer == null)
                {
                    this.dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                if (this.logManager == null)
                {
                    this.logManager = LogManager.GetInstance();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject GetMenuGeneralDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetMenuGeneralDetailsBizActionVO nvo = (clsGetMenuGeneralDetailsBizActionVO) valueObject;
            try
            {
                StringBuilder builder = new StringBuilder();
                if ((nvo.Status != null) && nvo.Status.Value)
                {
                    builder.Append("Status = 'True'");
                }
                else if ((nvo.Status != null) && !nvo.Status.Value)
                {
                    builder.Append("Status = 'False'");
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMenuDetailsList");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    List<clsMenuVO> list = new List<clsMenuVO>();
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            List<clsMenuVO> outer = (from flatItem in list select new clsMenuVO { 
                                Title = flatItem.Title,
                                ID = flatItem.ID,
                                ParentId = flatItem.ParentId,
                                Status = flatItem.Status
                            }).ToList<clsMenuVO>();
                            nvo.MenuList = outer.GroupJoin<clsMenuVO, clsMenuVO, long?, clsMenuVO>(outer, parentItem => new long?(parentItem.ID), childItem => childItem.ParentId, delegate (clsMenuVO parent, IEnumerable<clsMenuVO> children) {
                                parent.ChildMenuList = children.ToList<clsMenuVO>();
                                return parent;
                            }).Where<clsMenuVO>(delegate (clsMenuVO item) {
                                long? parentId = item.ParentId;
                                return ((parentId.GetValueOrDefault() == 0L) && (parentId != null));
                            }).ToList<clsMenuVO>();
                            break;
                        }
                        clsMenuVO item1 = new clsMenuVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Title = (string) DALHelper.HandleDBNull(reader["Title"]),
                            ParentId = (long?) DALHelper.HandleDBNull(reader["ParentId"]),
                            MenuOrder = new int?((int) DALHelper.HandleDBNull(reader["MenuOrder"])),
                            Status = false
                        };
                        list.Add(item1);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetMenuList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetMenuListBizActionVO nvo = valueObject as clsGetMenuListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMenuList");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.String, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.String, true);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MenuList == null)
                    {
                        nvo.MenuList = new List<clsMenuVO>();
                    }
                    while (reader.Read())
                    {
                        clsMenuVO item = new clsMenuVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Title = (string) DALHelper.HandleDBNull(reader["Title"]),
                            ImagePath = (string) DALHelper.HandleDBNull(reader["ImagePath"]),
                            Parent = (string) DALHelper.HandleDBNull(reader["Parent"]),
                            Module = (string) DALHelper.HandleDBNull(reader["Module"]),
                            Action = (string) DALHelper.HandleDBNull(reader["Action"]),
                            Header = (string) DALHelper.HandleDBNull(reader["Header"]),
                            Configuration = (string) DALHelper.HandleDBNull(reader["Configuration"]),
                            Mode = (string) DALHelper.HandleDBNull(reader["Mode"]),
                            Active = (bool) DALHelper.HandleDBNull(reader["Active"]),
                            MenuOrder = new int?((int) DALHelper.HandleDBNull(reader["MenuOrder"]))
                        };
                        nvo.MenuList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetUserMenu(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetUserMenuBizActionVO nvo = valueObject as clsGetUserMenuBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUserMenu");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MenuList == null)
                    {
                        nvo.MenuList = new List<clsMenuVO>();
                    }
                    while (reader.Read())
                    {
                        clsMenuVO item = new clsMenuVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Title = (string) DALHelper.HandleDBNull(reader["Title"]),
                            ImagePath = (string) DALHelper.HandleDBNull(reader["ImagePath"]),
                            Parent = (string) DALHelper.HandleDBNull(reader["Parent"]),
                            ParentId = new long?((long) DALHelper.HandleDBNull(reader["ParentID"])),
                            Module = (string) DALHelper.HandleDBNull(reader["Module"]),
                            Action = (string) DALHelper.HandleDBNull(reader["Action"]),
                            Header = (string) DALHelper.HandleDBNull(reader["Header"]),
                            Configuration = (string) DALHelper.HandleDBNull(reader["Configuration"]),
                            Mode = (string) DALHelper.HandleDBNull(reader["Mode"]),
                            Active = (bool) DALHelper.HandleDBNull(reader["Active"]),
                            MenuOrder = new int?((int) DALHelper.HandleDBNull(reader["MenuOrder"]))
                        };
                        nvo.MenuList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }
    }
}

