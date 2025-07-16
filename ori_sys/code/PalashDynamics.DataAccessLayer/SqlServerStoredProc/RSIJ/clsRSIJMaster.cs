using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.RSIJ;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects.RSIJ;
using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.IPD;
using System.IO;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.RSIJ
{
    class clsRSIJMasterDAL : clsBaseRSIJMasterDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        public bool chkFlag = true;
        #endregion

        private clsRSIJMasterDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                //Create Instance of the database object and BaseSql object  GetDatabaseReference
                dbServer = HMSConfigurationManager.GetDatabaseReference();
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override IValueObject GetMasterList(IValueObject valueObject, clsUserVO UserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetRSIJMasterListBizActionVO BizActionObj = (clsGetRSIJMasterListBizActionVO)valueObject;
            try
            {
                StringBuilder FilterExpression = new StringBuilder();

                if (BizActionObj.IsActive.HasValue)
                    FilterExpression.Append("Status = '" + BizActionObj.IsActive.Value + "'");

                if (BizActionObj.Parent != null)
                {
                    if (FilterExpression.Length > 0)
                        FilterExpression.Append(" And ");
                    FilterExpression.Append(BizActionObj.Parent.Value.ToString() + "='" + BizActionObj.Parent.Key.ToString() + "'");
                }

                //Take storeprocedure name as input parameter and creates DbCommand Object.
                DbCommand command = dbServer.GetStoredProcCommand("EMR_GetMasterList");
                //Adding MasterTableName as Input Parameter to filter record
                dbServer.AddInParameter(command, "MasterTableName", DbType.String, BizActionObj.MasterTable.ToString());
                dbServer.AddInParameter(command, "FilterExpression", DbType.String, FilterExpression.ToString());
                dbServer.AddInParameter(command, "Code", DbType.String, BizActionObj.CodeColumn);
                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.DescriptionColumn);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.MasterList == null)
                    {
                        BizActionObj.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //Add the object value in list
                        BizActionObj.MasterList.Add(new MasterListItem(Convert.ToString(reader["Code"]), reader["Description"].ToString()));//HandleDBNull(reader["Date"])));
                    }
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                BizActionObj.Error = ex.Message;  //"Error Occured";
            }


            return BizActionObj;
        }

        public override IValueObject GetDoctorDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetRSIJDoctorDepartmentDetailsBizActionVO BizAction = (clsGetRSIJDoctorDepartmentDetailsBizActionVO)valueObject;
            try
            {
                DbCommand command;
                if (BizAction.IsForReferral == false)
                {
                    if (BizAction.IsServiceWiseDoctorList == true)
                    {
                        command = dbServer.GetStoredProcCommand("CIMS_FillServiceWiseDoctorCombobox");
                        dbServer.AddInParameter(command, "ServiceId", DbType.Int64, BizAction.ServiceId);
                        dbServer.AddInParameter(command, "AllRecord", DbType.Boolean, BizAction.AllRecord);
                    }
                    else if (BizAction.FromRoster == true)
                    {
                        command = dbServer.GetStoredProcCommand("CIMS_FillDoctorForRosterCombobox");
                        dbServer.AddInParameter(command, "Date", DbType.DateTime, BizAction.Date);
                    }
                    else
                    {
                        command = dbServer.GetStoredProcCommand("EMR_FillDoctorCombobox");
                        dbServer.AddInParameter(command, "DoctorCode", DbType.String, BizAction.DoctorCode);
                    }
                    dbServer.AddInParameter(command, "DepartmentCode", DbType.String, BizAction.DepartmentCode);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("EMR_FillDoctorComboboxSpecialisationWise");
                    dbServer.AddInParameter(command, "SpecialCode", DbType.String, BizAction.SpecialCode);
                }

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizAction.MasterList == null)
                    {
                        BizAction.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        if (BizAction.FromBill == false && BizAction.AllRecord == false)
                        {
                            if (BizAction.IsServiceWiseDoctorList == false)
                            {
                                BizAction.MasterList.Add(new MasterListItem(Convert.ToString(reader["Code"]), Convert.ToString(reader["Description"])));
                            }
                            else
                            {
                                BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"])), reader["Description"].ToString(), true, Convert.ToDouble(DALHelper.HandleDBNull((reader["Rate"])))));

                            }
                        }
                        else if (BizAction.AllRecord == true)
                        {
                            BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString(), (long)reader["ServiceID"], Convert.ToDouble(Convert.ToDecimal((reader["Rate"])))));
                        }
                        else
                        {
                            BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString(), (long)reader["SpecializationID"]));

                        }
                    }
                }
            }


            catch (Exception ex)
            {
                throw;
            }
            return BizAction;
        }

        public override IValueObject GetOPDQueueList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRSIJQueueListBizActionVO BizActionObj = (clsGetRSIJQueueListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("EMR_GetPatientQueueList");

                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);

                dbServer.AddInParameter(command, "DeptCode", DbType.String, BizActionObj.DeptCode);
                dbServer.AddInParameter(command, "DoctorCode", DbType.String, BizActionObj.DoctorCode);

                if (BizActionObj.FirstName != null && BizActionObj.PatientName.Length != 0)
                    dbServer.AddInParameter(command, "PatientName", DbType.String, BizActionObj.PatientName + "%");

                if (BizActionObj.MRNo != null && BizActionObj.MRNo.Length > 0)
                    dbServer.AddInParameter(command, "MRNo", DbType.String, BizActionObj.MRNo + "%");

                if (BizActionObj.NoReg != null && BizActionObj.NoReg.Length > 0)
                    dbServer.AddInParameter(command, "RegNo", DbType.String, BizActionObj.NoReg + "%");

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.QueueList == null)
                        BizActionObj.QueueList = new List<clsQueueVO>();
                    while (reader.Read())
                    {
                        clsQueueVO objQueueVO = new clsQueueVO();
                        objQueueVO.VisitID = Convert.ToInt64(reader["VisitID"]);
                        objQueueVO.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        objQueueVO.PatientId = Convert.ToInt64(reader["PatientID"]);
                        objQueueVO.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objQueueVO.OPDNO = Convert.ToString(reader["OPDNO"]);
                        objQueueVO.DepartmentCode = Convert.ToString(reader["DepartmentID"]);
                        objQueueVO.DeptDescription = Convert.ToString(reader["DepartmentName"]);
                        objQueueVO.NoReg = Convert.ToString(reader["NoReg"]);
                        objQueueVO.MRNO = Convert.ToString(reader["MRNo"]);
                        objQueueVO.DoctorCode = Convert.ToString(reader["DoctorID"]);
                        objQueueVO.DoctorName = Convert.ToString(reader["DoctorName"]);
                        objQueueVO.DateTime = objQueueVO.Date;
                        objQueueVO.PatientType = "OPD";
                        objQueueVO.Complaints = Convert.ToString(reader["complaints"]);
                        objQueueVO.ReferredDoctor = Convert.ToString(reader["ReferredDoctor"]);
                        objQueueVO.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateofBirth"]);
                        objQueueVO.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
                        objQueueVO.VisitType = Convert.ToString(reader["VisitType"]);
                        objQueueVO.VisitTypeID = Convert.ToInt64(reader["VisitTypeID"]); ;
                        objQueueVO.VisitStatus = Convert.ToBoolean(reader["CurrentVisitStatus"]);
                        objQueueVO.PatientCategoryId = Convert.ToInt64(reader["PatientCategoryID"]);
                        BizActionObj.QueueList.Add(objQueueVO);
                    }

                }

                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }

            catch (Exception ex)
            {
                throw;
            }

            return BizActionObj;
        }

        /// <summary>
        /// This Method is Used to Get all the Diagnosis Master Table Records.
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVo"></param>
        /// <returns></returns>
        public override IValueObject GetDiagnosisList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRSIJDiagnosisMasterBizactionVO BizActionObj = (clsGetRSIJDiagnosisMasterBizactionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDiagnosisMasterDetailsList");
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                if (BizActionObj.Code != null && BizActionObj.Code != "" && BizActionObj.Code.Length != 0)
                    dbServer.AddInParameter(command, "Code", DbType.String, "%" + BizActionObj.Code + "%");

                if (BizActionObj.Diagnosis != null && BizActionObj.Diagnosis != "" && BizActionObj.Diagnosis.Length != 0)
                    dbServer.AddInParameter(command, "Diagnosis", DbType.String, "%" + BizActionObj.Diagnosis + "%");
                //IsICD9 flag is used to tell the database to get the records from the ICOPIM table notfrom the Category Table.
                dbServer.AddInParameter(command, "IsICD9", DbType.Boolean, BizActionObj.IsICD9);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DiagnosisDetails == null)
                        BizActionObj.DiagnosisDetails = new List<clsEMRDiagnosisVO>();
                    if (!BizActionObj.IsICD9)
                    {
                        while (reader.Read())
                        {
                            clsEMRDiagnosisVO diagnosisVO = new clsEMRDiagnosisVO();
                            diagnosisVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            diagnosisVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                            diagnosisVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                            diagnosisVO.Diagnosis = Convert.ToString(DALHelper.HandleDBNull(reader["Diagnosis"]));
                            //diagnosisVO.ICDId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ICDId"]));
                            diagnosisVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                            BizActionObj.DiagnosisDetails.Add(diagnosisVO);
                        }
                        reader.NextResult();
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            clsEMRDiagnosisVO diagnosisVO = new clsEMRDiagnosisVO();
                            diagnosisVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            diagnosisVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                            diagnosisVO.Categori = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                            //diagnosisVO.ServiceRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceRate"]));
                            diagnosisVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                            diagnosisVO.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                            //diagnosisVO.Class = Convert.ToString(reader["Class"]);
                          //diagnosisVO.DTD = Convert.ToString(reader["DTD"]);
                            diagnosisVO.Diagnosis = Convert.ToString(DALHelper.HandleDBNull(reader["Diagnosis"]));
                            diagnosisVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                            //diagnosisVO.TemplateID = Convert.ToInt64(reader["TemplateID"]);
                            //diagnosisVO.TemplateName = Convert.ToString(reader["TemplateName"]);
                            BizActionObj.DiagnosisDetails.Add(diagnosisVO);
                        }
                        reader.NextResult();
                    }
                        reader.NextResult();
                }
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                dbServer = null;
            }
            return BizActionObj;

        }

        /// <summary>
        /// This Method is Used to Get all the Drug Records.
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVo"></param>
        /// <returns></returns>
        public override IValueObject GetItemList(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetRSIJItemListBizActionVO objBizAction = valueObject as clsGetRSIJItemListBizActionVO;
            DbCommand command;
            DbDataReader reader = null;
            try
            {
                if (objBizAction.IsInsuraceDrug)
                {
                    command = dbServer.GetStoredProcCommand("EMR_NewGetItemListMultipleFiltered");
                    dbServer.AddInParameter(command, "OPDNO", DbType.String, objBizAction.OPDNO);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("EMR_GetItemListMultipleFiltered");
                }
                dbServer.AddInParameter(command, "MoleculeCode", DbType.String, objBizAction.MoleculeCode);
                dbServer.AddInParameter(command, "BrandName", DbType.String, objBizAction.BrandName);
                dbServer.AddInParameter(command, "IsQtyShow", DbType.Boolean, objBizAction.IsQtyShow);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizAction.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objBizAction.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objBizAction.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);
                dbServer.AddInParameter(command, "IsFromCurrentMedication", DbType.Boolean, objBizAction.IsFromCurrentMed);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ItemList == null)
                        objBizAction.ItemList = new List<clsRSIJItemMasterVO>();
                    while (reader.Read())
                    {
                        clsRSIJItemMasterVO objList = new clsRSIJItemMasterVO();
                        objList.DrugCode = Convert.ToString(reader["DrugCode"]);
                        objList.DrugName = Convert.ToString(reader["DrugName"]);
                        objList.StockQty = Convert.ToDouble(DALHelper.HandleDBNull((reader["AvailableStock"])));
                        objList.MoleculeName = Convert.ToString(reader["MoleculeName"]);
                        objBizAction.ItemList.Add(objList);
                    }
                }
                reader.NextResult();
                objBizAction.TotalRowCount = (int)dbServer.GetParameterValue(command, "TotalRows");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader.IsClosed == false)
                {
                    reader.Close();
                }
            }
            return valueObject;
        }

        public override IValueObject GetMasterListByTableName(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetRSIJLaboratoryServiceBizActionVO BizActionObj = (clsGetRSIJLaboratoryServiceBizActionVO)valueObject;

            try
            {
                DbCommand command = null;
                if (BizActionObj.ServiceType == "Pathology")
                {
                    command = dbServer.GetStoredProcCommand("EMR_GetLaboratoryDetailsList");
                    dbServer.AddInParameter(command, "GroupName", DbType.String, "%" + BizActionObj.GroupName + "%");
                    dbServer.AddInParameter(command, "SpecializationCode", DbType.String, "%" + BizActionObj.SpecializationCode+ "%");
                }
                else if (BizActionObj.ServiceType == "Radiology")
                {
                    command = dbServer.GetStoredProcCommand("EMR_GetRadiologyDetailsList");
                }
                else if (BizActionObj.ServiceType == "Diagnostik")
                {
                    command = dbServer.GetStoredProcCommand("EMR_GetDiagnostikDetailsList");
                    if (!String.IsNullOrEmpty(BizActionObj.ServiceDepartment))
                        dbServer.AddInParameter(command, "ServiceDepartment", DbType.String, "%" + BizActionObj.ServiceDepartment + "%");
                }

                if (BizActionObj.ServiceCode != null && BizActionObj.ServiceCode != "" && BizActionObj.ServiceCode.Length != 0)
                    dbServer.AddInParameter(command, "ServiceCode", DbType.String, "%" + BizActionObj.ServiceCode + "%");

                if (BizActionObj.Description != null && BizActionObj.Description != "" && BizActionObj.Description.Length != 0)
                    dbServer.AddInParameter(command, "Description", DbType.String, "%" + BizActionObj.Description + "%");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceDetails == null)
                        BizActionObj.ServiceDetails = new List<clsServiceMasterVO>();
                    while (reader.Read())
                    {
                        clsServiceMasterVO serviceVO = new clsServiceMasterVO();
                        serviceVO.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"]));
                        serviceVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        serviceVO.Group = Convert.ToString(reader["GroupName"]);
                        serviceVO.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationCode"]));
                        BizActionObj.ServiceDetails.Add(serviceVO);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }

            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetDoctorScheduleTime(IValueObject valueObject, clsUserVO objUserVO)
        {
            
            
            throw new NotImplementedException();
        }

        
    }
}
