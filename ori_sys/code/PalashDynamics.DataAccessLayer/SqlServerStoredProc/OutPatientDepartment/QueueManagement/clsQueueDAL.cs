using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using System.Data;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement;
using System.Data.Common;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.DataAccessLayer;


namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsQueueDAL : clsBaseQueueDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsQueueDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql Object
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
                throw ex;
            }

        }


        public override IValueObject GetQueueList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetQueueListBizActionVO BizActionObj = (clsGetQueueListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientQueueList");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, BizActionObj.LinkServer);
                if (BizActionObj.LinkServer != null)
                dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, BizActionObj.LinkServer.Replace(@"\", "_"));
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DepartmentID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorID);

                if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName);
                

                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName);
                //added by akshays 
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                if (BizActionObj.MRNo != null && BizActionObj.MRNo.Length > 0)
                    dbServer.AddInParameter(command, "MRNo", DbType.String, BizActionObj.MRNo);
                dbServer.AddInParameter(command, "CurrentVisitStatus", DbType.Int64, BizActionObj.CurrentVisit);
                if (BizActionObj.ContactNo != null && BizActionObj.ContactNo.Length != 0)
                    dbServer.AddInParameter(command, "ContactNo1", DbType.String,BizActionObj.ContactNo);
                if (BizActionObj.TokenNo != null && BizActionObj.TokenNo.Length != 0)
                    dbServer.AddInParameter(command, "TokanNo", DbType.String,BizActionObj.TokenNo);
               
                //closed by akshays

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                //By Anjali.................
                dbServer.AddInParameter(command, "SpecialRegID", DbType.Int64, BizActionObj.SpecialRegID);
                //...........................

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    if (BizActionObj.QueueList == null)
                        BizActionObj.QueueList = new List<clsQueueVO>();
                    while (reader.Read())
                    {
                        clsQueueVO objQueueVO = new clsQueueVO();
                        objQueueVO.QueueID = (long)reader["ID"];
                        objQueueVO.PatientId = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        objQueueVO.PatientUnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objQueueVO.OPDNO = (string)DALHelper.HandleDBNull(reader["OPDNO"]);
                        objQueueVO.MRNO = (string)DALHelper.HandleDBNull(reader["MRNo"]);


                        objQueueVO.DepartmentID = (long)DALHelper.HandleDBNull(reader["VisitDepartmentID"]);
                        objQueueVO.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);


                        objQueueVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        objQueueVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                        objQueueVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        objQueueVO.Discription = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objQueueVO.DoctorName = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
                        objQueueVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);

                        objQueueVO.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        //objQueueVO.InTime = (string)DALHelper.HandleDBNull(reader["InTime"]);
                        objQueueVO.DateTime = (DateTime)DALHelper.HandleDate(reader["DateTime"]);
                        objQueueVO.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                        objQueueVO.CompanyID = (long)DALHelper.HandleDBNull(reader["CompanyID"]);
                        objQueueVO.Complaints = (string)DALHelper.HandleDBNull(reader["Complaints"]);
                        objQueueVO.ReferredDoctor = (string)DALHelper.HandleDBNull(reader["ReferredDoctor"]);
                        objQueueVO.Cabin = (string)DALHelper.HandleDBNull(reader["Cabin"]);

                        objQueueVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objQueueVO.CurrentVisitStatus = (VisitCurrentStatus)DALHelper.HandleDBNull(reader["CurrentVisitStatus"]);
                        objQueueVO.VisitID = (long)DALHelper.HandleDBNull(reader["VisitID"]);
                        objQueueVO.VisitTypeID = (long?)DALHelper.HandleDBNull(reader["VisitTypeID"]);
                        objQueueVO.VisitType = (string)DALHelper.HandleDBNull(reader["VisitType"]);
                        objQueueVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                        objQueueVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);

                        objQueueVO.IsHealthPackage = (bool)DALHelper.HandleDBNull(reader["IsHealthPackage"]);
                        objQueueVO.ServiceID = (long)DALHelper.HandleDBNull(reader["ServiceID"]);
                        objQueueVO.ServiceName = (string)DALHelper.HandleDBNull(reader["Service"]);
                        objQueueVO.SortOrder = Convert.ToString(DALHelper.HandleDBNull(reader["TokanNo"]));
                        objQueueVO.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["RemarkForPatientType"]));

                        //By Anjali......................
                        objQueueVO.SpecialReg = Convert.ToString(DALHelper.HandleDBNull(reader["SpecialReg"]));
                        //................................

                        objQueueVO.VisitDate = (Convert.ToDateTime(DALHelper.HandleDate(reader["VisitDate"])));

                        //get column ContactNo to show on Patient Queue grid : by AniketK on 16/10/2018
                        objQueueVO.ContactNo1 = (string)DALHelper.HandleDBNull(reader["ContactNo1"]);

                        BizActionObj.QueueList.Add(objQueueVO);
                    }

                }

                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }

            catch (Exception ex)
            {
                throw ex;
            }


            finally
            {

            }
            return BizActionObj;
        }

        public override IValueObject AddQueueList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddQueueListBizActionVO BizActionobj = (clsAddQueueListBizActionVO)valueObject;

            try
            {
                clsQueueVO objQueue = BizActionobj.QueueDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePatientQueue");

                dbServer.AddInParameter(command, "PatientID", DbType.String, objQueue.PatientId);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, objQueue.DateTime);
                dbServer.AddInParameter(command, "OPDNO", DbType.String, objQueue.OPDNO);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objQueue.DepartmentID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objQueue.DoctorID);
                dbServer.AddInParameter(command, "UnitId ", DbType.Int64, objQueue.UnitID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objQueue.Status);
                dbServer.AddInParameter(command, "TokanNo ", DbType.Int64, objQueue.SortOrder);
                dbServer.AddInParameter(command, "CurrentVisitStatus ", DbType.Int32, objQueue.CurrentVisitStatus);

                int intStatus = dbServer.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }

            return BizActionobj;


        }

        public override IValueObject UpdateQueueSortOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdatePatientSortOrderBizActionVO BizActionobj = valueObject as clsUpdatePatientSortOrderBizActionVO;

            try
            {
                clsQueueVO objQueueVO = BizActionobj.QueueDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePatientSortOrder");



                dbServer.AddInParameter(command, "QueueID", DbType.Int64, objQueueVO.QueueID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objQueueVO.UnitID);
                dbServer.AddInParameter(command, "SortOrder", DbType.Int64, objQueueVO.SortOrder);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objQueueVO.Status);
                int intStatus = dbServer.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {


            }
            finally
            {

            }
            return BizActionobj;
        }

        public override IValueObject GetQueueByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetQueueByIDBizActionVO BizAction = valueObject as clsGetQueueByIDBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientQueueByID");
                DbDataReader reader;

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizAction.PatientUnitID);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizAction.QueueDetails.DateTime = (DateTime)reader["Date"];
                        BizAction.QueueDetails.PatientId = (long)reader["PatientId"];
                        BizAction.QueueDetails.FirstName = (string)reader["FirstName"];
                        BizAction.QueueDetails.MiddleName = (string)reader["MiddleName"];
                        BizAction.QueueDetails.LastName = (string)reader["LastName"];
                        BizAction.QueueDetails.MRNO = (string)reader["MRNO"];
                        BizAction.QueueDetails.OPDNO = (string)reader["OPDNO"];
                        BizAction.QueueDetails.DepartmentID = (long)reader["DepartmentID"];
                        BizAction.QueueDetails.DoctorID = (long)reader["DoctorID"];
                        BizAction.QueueDetails.Status = (bool)reader["Status"];

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

            return BizAction;
        }

        public override IValueObject UpdateDoctorInQueue(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateDoctorInQueueBizActionVO BizActionobj = valueObject as clsUpdateDoctorInQueueBizActionVO;

            try
            {
                foreach (var item in BizActionobj.QueueDetails)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AssignDoctorForPatientQueue");

                    dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionobj.VisitId);
                    dbServer.AddInParameter(command, "ServiceID", DbType.Int64, item.ServiceID);
                    dbServer.AddInParameter(command, "DoctorID", DbType.Int64, item.DoctorID);
                    dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, item.DepartmentID);
                    int intStatus = dbServer.ExecuteNonQuery(command);
                }

            }
            catch (Exception ex)
            {


            }
            finally
            {

            }
            return BizActionobj;

        }
    }
}
