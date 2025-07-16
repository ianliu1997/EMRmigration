namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsQueueDAL : clsBaseQueueDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsQueueDAL()
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
            catch (Exception exception1)
            {
                throw exception1;
            }
        }

        public override IValueObject AddQueueList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddQueueListBizActionVO nvo = (clsAddQueueListBizActionVO) valueObject;
            try
            {
                clsQueueVO queueDetails = nvo.QueueDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientQueue");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.String, queueDetails.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, queueDetails.DateTime);
                this.dbServer.AddInParameter(storedProcCommand, "OPDNO", DbType.String, queueDetails.OPDNO);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, queueDetails.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, queueDetails.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId ", DbType.Int64, queueDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, queueDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "TokanNo ", DbType.Int64, queueDetails.SortOrder);
                this.dbServer.AddInParameter(storedProcCommand, "CurrentVisitStatus ", DbType.Int32, queueDetails.CurrentVisitStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetQueueByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetQueueByIDBizActionVO nvo = valueObject as clsGetQueueByIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientQueueByID");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.PatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.QueueDetails.DateTime = new DateTime?((DateTime) reader["Date"]);
                        nvo.QueueDetails.PatientId = (long) reader["PatientId"];
                        nvo.QueueDetails.FirstName = (string) reader["FirstName"];
                        nvo.QueueDetails.MiddleName = (string) reader["MiddleName"];
                        nvo.QueueDetails.LastName = (string) reader["LastName"];
                        nvo.QueueDetails.MRNO = (string) reader["MRNO"];
                        nvo.QueueDetails.OPDNO = (string) reader["OPDNO"];
                        nvo.QueueDetails.DepartmentID = (long) reader["DepartmentID"];
                        nvo.QueueDetails.DoctorID = (long) reader["DoctorID"];
                        nvo.QueueDetails.Status = (bool) reader["Status"];
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetQueueList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetQueueListBizActionVO nvo = (clsGetQueueListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientQueueList");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, nvo.LinkServer);
                if (nvo.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, nvo.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
                if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName);
                }
                if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                if ((nvo.MRNo != null) && (nvo.MRNo.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MRNo);
                }
                this.dbServer.AddInParameter(storedProcCommand, "CurrentVisitStatus", DbType.Int64, nvo.CurrentVisit);
                if ((nvo.ContactNo != null) && (nvo.ContactNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ContactNo1", DbType.String, nvo.ContactNo);
                }
                if ((nvo.TokenNo != null) && (nvo.TokenNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "TokanNo", DbType.String, nvo.TokenNo);
                }
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "SpecialRegID", DbType.Int64, nvo.SpecialRegID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.QueueList == null)
                    {
                        nvo.QueueList = new List<clsQueueVO>();
                    }
                    while (reader.Read())
                    {
                        clsQueueVO item = new clsQueueVO {
                            QueueID = (long) reader["ID"],
                            PatientId = (long) DALHelper.HandleDBNull(reader["PatientID"]),
                            PatientUnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            OPDNO = (string) DALHelper.HandleDBNull(reader["OPDNO"]),
                            MRNO = (string) DALHelper.HandleDBNull(reader["MRNo"]),
                            DepartmentID = (long) DALHelper.HandleDBNull(reader["VisitDepartmentID"]),
                            DoctorID = (long) DALHelper.HandleDBNull(reader["DoctorID"]),
                            FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FirstName"])),
                            MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["MiddleName"])),
                            LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LastName"])),
                            Discription = (string) DALHelper.HandleDBNull(reader["Description"]),
                            DoctorName = (string) DALHelper.HandleDBNull(reader["DoctorName"]),
                            UnitName = (string) DALHelper.HandleDBNull(reader["UnitName"])
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                        item.Date = new DateTime?(nullable.Value);
                        DateTime? nullable2 = DALHelper.HandleDate(reader["DateTime"]);
                        item.DateTime = new DateTime?(nullable2.Value);
                        item.TariffID = (long) DALHelper.HandleDBNull(reader["TariffID"]);
                        item.CompanyID = (long) DALHelper.HandleDBNull(reader["CompanyID"]);
                        item.Complaints = (string) DALHelper.HandleDBNull(reader["Complaints"]);
                        item.ReferredDoctor = (string) DALHelper.HandleDBNull(reader["ReferredDoctor"]);
                        item.Cabin = (string) DALHelper.HandleDBNull(reader["Cabin"]);
                        item.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        item.CurrentVisitStatus = (VisitCurrentStatus) DALHelper.HandleDBNull(reader["CurrentVisitStatus"]);
                        item.VisitID = (long) DALHelper.HandleDBNull(reader["VisitID"]);
                        item.VisitTypeID = (long?) DALHelper.HandleDBNull(reader["VisitTypeID"]);
                        item.VisitType = (string) DALHelper.HandleDBNull(reader["VisitType"]);
                        item.UnitID = (long) DALHelper.HandleDBNull(reader["UnitId"]);
                        item.UnitName = (string) DALHelper.HandleDBNull(reader["UnitName"]);
                        item.IsHealthPackage = (bool) DALHelper.HandleDBNull(reader["IsHealthPackage"]);
                        item.ServiceID = (long) DALHelper.HandleDBNull(reader["ServiceID"]);
                        item.ServiceName = (string) DALHelper.HandleDBNull(reader["Service"]);
                        item.SortOrder = Convert.ToString(DALHelper.HandleDBNull(reader["TokanNo"]));
                        item.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["RemarkForPatientType"]));
                        item.SpecialReg = Convert.ToString(DALHelper.HandleDBNull(reader["SpecialReg"]));
                        item.VisitDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["VisitDate"])));
                        item.ContactNo1 = (string) DALHelper.HandleDBNull(reader["ContactNo1"]);
                        nvo.QueueList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject UpdateDoctorInQueue(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateDoctorInQueueBizActionVO nvo = valueObject as clsUpdateDoctorInQueueBizActionVO;
            try
            {
                foreach (clsPackageServiceDetailsVO svo in nvo.QueueDetails)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AssignDoctorForPatientQueue");
                    this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitId);
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, svo.ServiceID);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, svo.DoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, svo.DepartmentID);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                }
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject UpdateQueueSortOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdatePatientSortOrderBizActionVO nvo = valueObject as clsUpdatePatientSortOrderBizActionVO;
            try
            {
                clsQueueVO queueDetails = nvo.QueueDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientSortOrder");
                this.dbServer.AddInParameter(storedProcCommand, "QueueID", DbType.Int64, queueDetails.QueueID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, queueDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SortOrder", DbType.Int64, queueDetails.SortOrder);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, queueDetails.Status);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
            }
            return nvo;
        }
    }
}

