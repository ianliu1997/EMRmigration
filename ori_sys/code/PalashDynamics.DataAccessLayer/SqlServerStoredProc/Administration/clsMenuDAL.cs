using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Administration.Menu;


namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsMenuDAL : clsBaseMenuDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsMenuDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                //Create Instance of the database object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }

                //Create Instance of the LogManager object 

                if (logManager == null)
                {
                    logManager = LogManager.GetInstance();
                }
                #endregion

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public override IValueObject GetUserMenu(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetUserMenuBizActionVO BizActionObj = valueObject as clsGetUserMenuBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetUserMenu");
                DbDataReader reader;

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.MenuList == null)
                        BizActionObj.MenuList = new List<clsMenuVO>();
                    while (reader.Read())
                    {
                        clsMenuVO objVO = new clsMenuVO();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.Title = (string)DALHelper.HandleDBNull(reader["Title"]);
                        objVO.ImagePath = (string)DALHelper.HandleDBNull(reader["ImagePath"]);
                        objVO.Parent = (string)DALHelper.HandleDBNull(reader["Parent"]);
                        objVO.ParentId = (long)DALHelper.HandleDBNull(reader["ParentID"]);
                        objVO.Module = (string)DALHelper.HandleDBNull(reader["Module"]);
                        objVO.Action = (string)DALHelper.HandleDBNull(reader["Action"]);
                        objVO.Header = (string)DALHelper.HandleDBNull(reader["Header"]);
                        objVO.Configuration = (string)DALHelper.HandleDBNull(reader["Configuration"]);
                        objVO.Mode = (string)DALHelper.HandleDBNull(reader["Mode"]);
                        objVO.Active = (bool)DALHelper.HandleDBNull(reader["Active"]);
                        objVO.MenuOrder = (int)DALHelper.HandleDBNull(reader["MenuOrder"]);

                        BizActionObj.MenuList.Add(objVO);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }

            return valueObject;
        }

        public override IValueObject GetMenuList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetMenuListBizActionVO BizActionObj = valueObject as clsGetMenuListBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMenuList");
                DbDataReader reader;

                // clsPatientGeneralVO objPatientVO = BizActionObj.PatientDetails;

                dbServer.AddInParameter(command, "ID", DbType.String, BizActionObj.ID);
                dbServer.AddInParameter(command, "Status", DbType.String, true);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.MenuList == null)
                        BizActionObj.MenuList = new List<clsMenuVO>();
                    while (reader.Read())
                    {
                        clsMenuVO objVO = new clsMenuVO();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.Title = (string)DALHelper.HandleDBNull(reader["Title"]);
                        objVO.ImagePath = (string)DALHelper.HandleDBNull(reader["ImagePath"]);
                        objVO.Parent = (string)DALHelper.HandleDBNull(reader["Parent"]);
                        objVO.Module = (string)DALHelper.HandleDBNull(reader["Module"]);
                        objVO.Action = (string)DALHelper.HandleDBNull(reader["Action"]);
                        objVO.Header = (string)DALHelper.HandleDBNull(reader["Header"]);
                        objVO.Configuration = (string)DALHelper.HandleDBNull(reader["Configuration"]);
                        objVO.Mode = (string)DALHelper.HandleDBNull(reader["Mode"]);
                        objVO.Active = (bool)DALHelper.HandleDBNull(reader["Active"]);
                        objVO.MenuOrder = (int)DALHelper.HandleDBNull(reader["MenuOrder"]);

                        BizActionObj.MenuList.Add(objVO);
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }

            return valueObject;
        }


        public override IValueObject GetMenuGeneralDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetMenuGeneralDetailsBizActionVO BizActionObj = (clsGetMenuGeneralDetailsBizActionVO)valueObject;
            try
            {
                StringBuilder FilterExpression = new StringBuilder();
                if (BizActionObj.Status.HasValue && BizActionObj.Status.Value == true)
                    FilterExpression.Append("Status = 'True'");
                else if (BizActionObj.Status.HasValue && BizActionObj.Status.Value == false)
                    FilterExpression.Append("Status = 'False'");

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMenuDetailsList");
                //dbServer.AddInParameter(command, "FilterExpression", DbType.String, FilterExpression.ToString());
                //dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.InputSearchExpression);
                //dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    List<clsMenuVO> temp = new List<clsMenuVO>();
                    while (reader.Read())
                    {
                        clsMenuVO objMenu = new clsMenuVO();
                        objMenu.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objMenu.Title = (string)DALHelper.HandleDBNull(reader["Title"]);
                        objMenu.ParentId = (long?)DALHelper.HandleDBNull(reader["ParentId"]);
                        objMenu.MenuOrder = (int)DALHelper.HandleDBNull(reader["MenuOrder"]);
                        // objMenu.MenuType = (int)reader["MenuType"];
                        objMenu.Status = false;

                        temp.Add(objMenu);
                    }

                    //Create a flat list of hierarchical items:   
                    var hierarchicalList = temp.Select(flatItem =>
                        new clsMenuVO()
                        {
                            Title = flatItem.Title,
                            ID = flatItem.ID,
                            ParentId = flatItem.ParentId,
                            Status = flatItem.Status
                        }).ToList();


                    //Join the items from the list with groups of items from the same list and select the roots.   
                    //i.e. join each parent with its children.   
                    var theRoots = hierarchicalList.GroupJoin(hierarchicalList,
                        parentItem => parentItem.ID,
                        childItem => childItem.ParentId,
                        (parent, children) =>
                        {
                            parent.ChildMenuList = children.ToList();
                            return parent;
                        }).Where(item => item.ParentId == 0).ToList();

                    BizActionObj.MenuList = theRoots;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;
        }
    }
}
