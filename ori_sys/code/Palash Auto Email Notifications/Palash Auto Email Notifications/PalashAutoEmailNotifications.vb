Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Configuration
Imports System.Data
Imports System.Runtime.Remoting.Services
Imports System.Diagnostics
Imports System.ServiceProcess
Imports System.IO
Imports System.Net.Mail
Imports System.Timers
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Imports System.Net
Imports System.Data.SqlClient



Public Class PalashAutoEmailNotifications

    Dim strSystemEmailId As String = ConfigurationSettings.AppSettings("systemEmailId").ToString
    Dim strEmailPassword As String = ConfigurationSettings.AppSettings("emailPassword").ToString
    Dim strSmtpServer As String = ConfigurationSettings.AppSettings("smtpServer").ToString
    Dim intSmtpPort As Integer = CInt(ConfigurationSettings.AppSettings("smtpPort").ToString)
    Dim strReportPath As String = ConfigurationSettings.AppSettings("ReportPath").ToString

    Dim strDBServerName As String = ConfigurationSettings.AppSettings("ServerName").ToString
    Dim strDBName As String = ConfigurationSettings.AppSettings("DBName").ToString
    Dim strDBUserName As String = ConfigurationSettings.AppSettings("UserName").ToString
    Dim strDBUserPassword As String = ConfigurationSettings.AppSettings("UserPwd").ToString

    Dim ToEmailID As String = Nothing
    Dim ReportName As String = Nothing
    Dim StartDate As System.Nullable(Of DateTime)
    Dim EndDate As System.Nullable(Of DateTime)
    Dim StartTime As System.Nullable(Of DateTime)
    Dim ReportFormat As Long = 0
    Dim Rpt As String = Nothing
    Dim ReportID As Long = 0


    Dim StrSQL As String = ""
    Dim cn As SqlConnection = Nothing
    Dim cmd As SqlCommand = Nothing
    Dim reader As SqlDataReader = Nothing

    Dim RecordList As List(Of clsMISConfigurationVO) = Nothing
    Dim UserList As List(Of clsMISReportVO) = Nothing




    Dim timer As New Timer()


    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.
        WriteLog("*****************************************************************************************")
        WriteLog("Service Started  :   " & Now.ToString)
        'WriteLog("*****************************************************************************************")
        'SendEmail()


        AddHandler timer.Elapsed, AddressOf OnElapsedTime
        'timer.Interval = CDbl(System.Configuration.ConfigurationSettings.AppSettings("TimeInterval").ToString) * 10000
        timer.Interval = 60 * 1000
        timer.Enabled = True
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        timer.Enabled = False
        WriteLog("Service Stopped  :   " & Now.ToString)
        WriteLog("*****************************************************************************************")
    End Sub

    'Private Sub SendEmail(Optional ByVal ToEmailId As String = "")
    '    Dim objMsg As New MailMessage
    '    Dim rptDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    '    WriteLog("*****************************************************************************************")
    '    WriteLog("Email Send Started  :   " & Now.ToString)

    '    'strReportPath = "D:\12 PALASH Dynamics\Razi Healthcare\Palash Notification Service\Palash Auto Email Notifications\Palash Auto Email Notifications\Reports\WeeklyServicewiseCollection.rpt"

    '    rptDoc.Load(strReportPath, CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault)
    '    rptDoc.SetDatabaseLogon(strDBUserName, strDBUserPassword, strDBServerName, strDBName)

    '    With objMsg
    '        .From = New MailAddress(strSystemEmailId)
    '        .To.Add(System.Configuration.ConfigurationSettings.AppSettings("ToEmailIds").ToString) ', nileshi@seedhealthcare.com,manisham@seedhealthcare.com,priyankag@seedhealthcare.com,pallavin@seedhealthcare.com,sailyp@seedhealthcare.com")
    '        .Subject = "Razi Clinic: Weekly Status Report"
    '        .Body = "This is a auto email generated using Palash Razi Application."
    '    End With

    '    objMsg.Attachments.Add(New Attachment(rptDoc.ExportToStream(ExportFormatType.PortableDocFormat), "Weekly Sales Report.pdf"))
    '    objMsg.Attachments.Add(New Attachment(rptDoc.ExportToStream(ExportFormatType.Excel), "Weekly Sales Report.xls"))


    '    Dim objSmtpClient As New SmtpClient(strSmtpServer, intSmtpPort)
    '    Dim objCredentials As New System.Net.NetworkCredential(strSystemEmailId, strEmailPassword)
    '    objSmtpClient.Credentials = objCredentials

    '    Try
    '        objSmtpClient.Send(objMsg)
    '        WriteLog("Email Request Sent To The Server Successfully  :   " & Now.ToString)
    '        WriteLog("*****************************************************************************************")
    '        objMsg = Nothing
    '    Catch ex As Exception
    '        WriteLog("Fail :   " & Now.ToString)
    '        objMsg = Nothing
    '    End Try
    'End Sub

    Protected Sub OnElapsedTime(ByVal source As Object, ByVal e As ElapsedEventArgs)
        'SendEmail()
        WriteLog("GetEmailDetails Function Started  :   " & Now.ToString)
        GetEmailDetails()

    End Sub



    Private Sub GetEmailDetails()

        Try
            RecordList = New List(Of clsMISConfigurationVO)
            cn = New SqlConnection()

            cn.ConnectionString = ConfigurationSettings.AppSettings("Connection").ToString

            cmd = New SqlCommand("CIMS_GetEmailDetailsForMIS", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cn.Open()
            reader = cmd.ExecuteReader()

            While reader.Read()
                Dim obj As New clsMISConfigurationVO()
                obj.ID = Convert.ToInt64((reader("Id")))
                obj.ClinicId = Convert.ToInt64((reader("ClinicId")))
                obj.ScheduleName = Convert.ToString((reader("ScheduleName")))
                obj.MISReportFormatId = Convert.ToInt64((reader("MISReportFormatId")))
                obj.ScheduleOn = Convert.ToInt64((reader("ScheduledOn")))
                obj.ScheduleDetails = Convert.ToString((reader("ScheduleDetails")))
                obj.ScheduleTime = Convert.ToDateTime((reader("ScheduleTime")))
                obj.ScheduleStartDate = Convert.ToDateTime((reader("ScheduleStartDate")))
                If Not IsDBNull(reader("ScheduleEndDate")) Then
                    obj.ScheduleEndDate = Convert.ToDateTime(reader("ScheduleEndDate"))
                End If
                RecordList.Add(obj)
            End While

            Dim ReportIdList As List(Of Long) = New List(Of Long)


            For Each item As Object In RecordList
                StartDate = item.ScheduleStartDate
                EndDate = item.ScheduleEndDate
                StartTime = item.ScheduleTime

                'Daily
                Dim ScheduleTime As DateTime = DateTime.Parse(StartTime.ToString())
                Dim DailyTime As DateTime = DateTime.Parse(DateTime.Now.ToString())
                Dim Str1 As String = Nothing
                Dim Str2 As String = Nothing

                Str1 = DailyTime.ToString("HH:mm")
                Str2 = ScheduleTime.ToString("HH:mm")
                Dim TempDate1 As DateTime = Convert.ToDateTime(Str1)
                Dim TempDate2 As DateTime = Convert.ToDateTime(Str2)

                'Weekly
              

                Dim ScheduleDetails As String = item.ScheduleDetails
                Dim Splitchar As Char() = {","c}
                Dim DayID As String() = ScheduleDetails.Split(Splitchar)

                Select Case item.ScheduleOn
                    Case 1

                        If StartDate IsNot Nothing AndAlso EndDate Is Nothing Then
                            If StartDate <= DateTime.Now.[Date].[Date] Then
                                'if (DailyTime.ToString("HH:mm") == ScheduleTime.ToString("HH:mm"))
                                If TempDate1 = TempDate2 Then
                                    'GetReportDetails(item.ID)
                                    ReportIdList.Add(item.ID)
                                End If
                            End If
                        ElseIf StartDate IsNot Nothing AndAlso EndDate IsNot Nothing Then
                            If StartDate <= DateTime.Now.[Date].[Date] AndAlso EndDate >= DateTime.Now.[Date].[Date] Then
                                'if (DailyTime.ToString("HH:mm") == ScheduleTime.ToString("HH:mm"))
                                If TempDate1 = TempDate2 Then
                                    'GetReportDetails(item.ID)
                                    ReportIdList.Add(item.ID)

                                End If

                            End If
                        End If
                        Exit Select

                    Case 2
                        Dim Day As DayOfWeek

                        Day = DateTime.Now.[Date].DayOfWeek
                        WriteLog("Day" & Day)

                        If StartDate IsNot Nothing AndAlso EndDate Is Nothing Then
                            If StartDate <= DateTime.Now.[Date].[Date] Then
                                For count As Integer = 0 To DayID.Length - 1
                                    Dim mDay As DayOfWeek
                                    'Dim strDay As String
                                    mDay = DirectCast(Convert.ToInt32(DayID(count)), DayOfWeek)
                                    ' strDay = Convert.ToString(mDay)
                                    If Day = mDay Then
                                        'if (DailyTime.ToString("HH:mm") == ScheduleTime.ToString("HH:mm"))
                                        If TempDate1 = TempDate2 Then
                                            'GetReportDetails(item.ID)
                                            ReportIdList.Add(item.ID)

                                        End If
                                    End If
                                Next

                            End If
                        ElseIf StartDate IsNot Nothing AndAlso EndDate IsNot Nothing Then
                            If StartDate <= DateTime.Now.[Date].[Date] AndAlso EndDate >= DateTime.Now.[Date].[Date] Then
                                For count As Integer = 0 To DayID.Length - 1
                                    Dim mDay As DayOfWeek
                                    'Dim strDay As String
                                    mDay = DirectCast(Convert.ToInt32(DayID(count)), DayOfWeek)
                                    'strDay = Convert.ToString(mDay)
                                    If Day = mDay Then
                                        'if (DailyTime.ToString("HH:mm") == ScheduleTime.ToString("HH:mm"))
                                        If TempDate1 = TempDate2 Then
                                            'GetReportDetails(item.ID)
                                            ReportIdList.Add(item.ID)
                                        End If
                                    End If

                                Next

                            End If
                        End If
                        Exit Select

                    Case 3
                        If StartDate IsNot Nothing AndAlso EndDate Is Nothing Then
                            If StartDate <= DateTime.Now.[Date].[Date] Then
                                If DateTime.Now.Day = Convert.ToInt32(item.ScheduleDetails) Then
                                    'if (DailyTime.ToString("HH:mm") == ScheduleTime.ToString("HH:mm"))
                                    If TempDate1 = TempDate2 Then
                                        'GetReportDetails(item.ID)
                                        ReportIdList.Add(item.ID)
                                    End If
                                End If
                            End If
                        ElseIf StartDate IsNot Nothing AndAlso EndDate IsNot Nothing Then
                            If StartDate <= DateTime.Now.[Date].[Date] AndAlso EndDate >= DateTime.Now.[Date].[Date] Then
                                If DateTime.Now.Day = Convert.ToInt32(item.ScheduleDetails) Then
                                    'if (DailyTime.ToString("HH:mm") == ScheduleTime.ToString("HH:mm"))
                                    If TempDate1 = TempDate2 Then
                                        'GetReportDetails(item.ID)
                                        ReportIdList.Add(item.ID)
                                    End If
                                End If

                            End If
                        End If
                        Exit Select


                End Select

              


            Next

            If ReportIdList IsNot Nothing And ReportIdList.Count > 0 Then
                For Each item In ReportIdList
                    GetReportDetails(item)
                Next


            End If
                   


        Catch ex As Exception
            WriteLog((("Error occured in GetEmailDetails Function :   " & " ") + DateTime.Now & " ") + ex.Message)
        Finally
            ' Close data reader object and database connection
            If reader IsNot Nothing Then
                reader.Close()
            End If

            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            WriteLog(("GetEmailDetails Function  completed:   " & " ") + DateTime.Now)
        End Try
    End Sub



    Private Sub GetReportDetails(ByVal iID As Long)
        Try
            UserList = New List(Of clsMISReportVO)
            cn = New SqlConnection()

            cn.ConnectionString = ConfigurationSettings.AppSettings("Connection").ToString

            cmd = New SqlCommand("CIMS_GetMISReportDetails", cn)
            cmd.CommandType = CommandType.StoredProcedure
            cn.Open()
            cmd.Parameters.Add("@MISID", SqlDbType.Int).Value = iID
            reader = cmd.ExecuteReader()

            While reader.Read()
                Dim ObjDetails As New clsMISReportVO()
                ObjDetails.MISID = Convert.ToInt64((reader("Id")))
                ObjDetails.Sys_MISReportId = Convert.ToInt64((reader("Sys_MISReportId")))
                ObjDetails.MISReportFormat = Convert.ToInt64((reader("MISReportFormatId")))
                ObjDetails.rptFileName = Convert.ToString((reader("rptFileName")))
                ObjDetails.ReportName = Convert.ToString((reader("ReportName")))
                ObjDetails.StaffTypeID = Convert.ToInt64((reader("StaffTypeId")))
                ObjDetails.StaffID = Convert.ToInt64((reader("StaffId")))
                ObjDetails.EmailID = Convert.ToString((reader("EmailID")))
                UserList.Add(ObjDetails)
            End While




            For Each item As Object In UserList
                ToEmailID = item.EmailID
                ReportName = item.ReportName
                ReportFormat = item.MISReportFormat
                Rpt = item.rptFileName
                ReportID = item.Sys_MISReportId

                If ToEmailID IsNot Nothing Then
                    WriteLog(("Send email Started  :   " & " ") + DateTime.Now & " " & ReportName)
                    SendEmail(ToEmailID, Rpt, ReportName, ReportFormat)

                End If
            Next




        Catch ex As Exception
            WriteLog((("Error occured in GetReportDetails Function :   " & " ") + DateTime.Now & " ") + ex.Message)
        Finally

            If reader IsNot Nothing Then
                reader.Close()
            End If

            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            WriteLog(("GetReportDetails function completed :   " & " ") + DateTime.Now)
        End Try



    End Sub



    Private Sub SendEmail(ByVal ToEmailId As String, ByVal rpt As String, ByVal rptNm As String, ByVal rptFormat As Long)
        Try
            Dim objMsg As New MailMessage()
            Dim rptDoc As New ReportDocument()

            WriteLog(("Email Send Started  :   " & " ") + DateTime.Now)

            If rpt IsNot Nothing Then
                rptDoc.Load(strReportPath & "\" & rpt, CrystalDecisions.[Shared].OpenReportMethod.OpenReportByDefault)
            End If

            'rptDoc.Load("D:\\Razi\\RAZIClinic.Web\\Reports\\WeeklyServicewiseCollection.rpt", CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault);

            rptDoc.SetDatabaseLogon(strDBUserName, strDBUserPassword, strDBServerName, strDBName)

            'If ReportID = 17 Then

            '    rptDoc.SetParameterValue("@ClinicID", 0)
            '    rptDoc.SetParameterValue("@FromDate", DateTime.Now.[Date].[Date])
            '    WriteLog(("Daily Sales report : " & " ") + DateTime.Now)
            'End If

            'for (int count = 0; count < rptDoc.ParameterFields.Count; count++)
            '{
            '    rptDoc.SetParameterValue(rptDoc.ParameterFields[count].Name, null);
            '}

            objMsg.From = New MailAddress(strSystemEmailId)

            If ToEmailId <> "" AndAlso ToEmailId IsNot Nothing Then
                objMsg.[To].Add(ToEmailId)
                objMsg.Subject = "Razi Clinic Report :" & rptNm
                objMsg.Body = "THIS EMAIL IS AUTOMATICALLY GENERATED. PLEASE DO NOT RESPOND TO THIS MAIL"

                If rptFormat = 1 Then
                    objMsg.Attachments.Add(New Attachment(rptDoc.ExportToStream(ExportFormatType.PortableDocFormat), (rptNm & ".pdf")))
                ElseIf rptFormat = 2 Then
                    objMsg.Attachments.Add(New Attachment(rptDoc.ExportToStream(ExportFormatType.Excel), (rptNm & ".xls")))
                End If
                WriteLog("Pdf/xls")
                Dim objSmtpClient As New SmtpClient(strSmtpServer, intSmtpPort)
                objSmtpClient.Credentials = New NetworkCredential(strSystemEmailId, strEmailPassword)

                Try
                    objSmtpClient.Send(objMsg)
                    WriteLog(("Email Request Sent To The Server Successfully  :   " & " ") + DateTime.Now)
                Catch

                    WriteLog(("Email sent Fail" & " ") + DateTime.Now)
                End Try
            End If
        Catch ex As Exception
            WriteLog("SendEmail  :   " & ex.Message & " " & Now.ToString)
        End Try


    End Sub



    Private Sub WriteLog(ByVal strMessage As String)
        Dim fs As FileStream = New FileStream("c:\Palash Email Notification Service Log.txt", FileMode.OpenOrCreate, FileAccess.Write)
        'Dim fs As FileStream = New FileStream(System.Windows.Forms.Application.ExecutablePath & "\Logs.txt", FileMode.OpenOrCreate, FileAccess.Write)
        Dim m_streamWriter As StreamWriter = New StreamWriter(fs)
        m_streamWriter.BaseStream.Seek(0, SeekOrigin.End)
        m_streamWriter.WriteLine(strMessage)
        m_streamWriter.Flush()
        m_streamWriter.Close()
    End Sub
End Class


Public Class clsMISConfigurationVO
    Private _id As Long
    Public Property ID() As Long
        Get
            Return _id
        End Get
        Set(ByVal value As Long)
            _id = value
        End Set
    End Property

    Public Property ClinicId() As Long
        Get
            Return m_ClinicId
        End Get
        Set(ByVal value As Long)
            m_ClinicId = Value
        End Set
    End Property
    Private m_ClinicId As Long
    Public Property ClinicName() As String
        Get
            Return m_ClinicName
        End Get
        Set(ByVal value As String)
            m_ClinicName = Value
        End Set
    End Property
    Private m_ClinicName As String
    Public Property ClinicCode() As String
        Get
            Return m_ClinicCode
        End Get
        Set(ByVal value As String)
            m_ClinicCode = Value
        End Set
    End Property
    Private m_ClinicCode As String
    Public Property MISTypeName() As String
        Get
            Return m_MISTypeName
        End Get
        Set(ByVal value As String)
            m_MISTypeName = Value
        End Set
    End Property
    Private m_MISTypeName As String
    Public Property ConfigDate() As System.Nullable(Of DateTime)
        Get
            Return m_ConfigDate
        End Get
        Set(ByVal value As System.Nullable(Of DateTime))
            m_ConfigDate = Value
        End Set
    End Property
    Private m_ConfigDate As System.Nullable(Of DateTime)

    Public Property ScheduleName() As String
        Get
            Return m_ScheduleName
        End Get
        Set(ByVal value As String)
            m_ScheduleName = Value
        End Set
    End Property
    Private m_ScheduleName As String

    Public Property MISTypeId() As Long
        Get
            Return m_MISTypeId
        End Get
        Set(ByVal value As Long)
            m_MISTypeId = Value
        End Set
    End Property
    Private m_MISTypeId As Long

    Public Property MISReportFormatId() As Long
        Get
            Return m_MISReportFormatId
        End Get
        Set(ByVal value As Long)
            m_MISReportFormatId = Value
        End Set
    End Property
    Private m_MISReportFormatId As Long

    Public Property ScheduleOn() As Long
        Get
            Return m_ScheduleOn
        End Get
        Set(ByVal value As Long)
            m_ScheduleOn = Value
        End Set
    End Property
    Private m_ScheduleOn As Long

    Public Property ScheduleDetails() As String
        Get
            Return m_ScheduleDetails
        End Get
        Set(ByVal value As String)
            m_ScheduleDetails = Value
        End Set
    End Property
    Private m_ScheduleDetails As String

    Public Property ScheduleTime() As System.Nullable(Of DateTime)
        Get
            Return m_ScheduleTime
        End Get
        Set(ByVal value As System.Nullable(Of DateTime))
            m_ScheduleTime = Value
        End Set
    End Property
    Private m_ScheduleTime As System.Nullable(Of DateTime)

    Public Property ScheduleStartDate() As System.Nullable(Of DateTime)
        Get
            Return m_ScheduleStartDate
        End Get
        Set(ByVal value As System.Nullable(Of DateTime))
            m_ScheduleStartDate = Value
        End Set
    End Property
    Private m_ScheduleStartDate As System.Nullable(Of DateTime)

    Public Property ScheduleEndDate() As System.Nullable(Of DateTime)
        Get
            Return m_ScheduleEndDate
        End Get
        Set(ByVal value As System.Nullable(Of DateTime))
            m_ScheduleEndDate = Value
        End Set
    End Property
    Private m_ScheduleEndDate As System.Nullable(Of DateTime)

    Public Property Status() As Boolean
        Get
            Return m_Status
        End Get
        Set(ByVal value As Boolean)
            m_Status = Value
        End Set
    End Property
    Private m_Status As Boolean
End Class


Public Class clsMISReportVO


    Private _id As Long
    Public Property MISID() As Long
        Get
            Return _id
        End Get
        Set(ByVal value As Long)
            _id = value
        End Set
    End Property


    Private _Sys_MISReportId As Long
    Public Property Sys_MISReportId() As Long
        Get
            Return _Sys_MISReportId
        End Get
        Set(ByVal value As Long)
            _Sys_MISReportId = value
        End Set
    End Property


    Private _rptFileName As String
    Public Property rptFileName() As String
        Get
            Return _rptFileName
        End Get
        Set(ByVal value As String)
            _rptFileName = value
        End Set
    End Property


    Private _ReportName As String
    Public Property ReportName() As String
        Get
            Return _ReportName
        End Get
        Set(ByVal value As String)
            _ReportName = value
        End Set
    End Property

    Public Property MISReportFormat() As Long
        Get
            Return m_MISReportFormat
        End Get
        Set(ByVal value As Long)
            m_MISReportFormat = Value
        End Set
    End Property
    Private m_MISReportFormat As Long

    Private _StaffTypeID As Long
    Public Property StaffTypeID() As Long
        Get
            Return _StaffTypeID
        End Get
        Set(ByVal value As Long)
            _StaffTypeID = value
        End Set
    End Property

    Private _StaffID As Long
    Public Property StaffID() As Long
        Get
            Return _StaffID
        End Get
        Set(ByVal value As Long)
            _StaffID = value
        End Set
    End Property

    Private _EmailID As String
    Public Property EmailID() As String
        Get
            Return _EmailID
        End Get
        Set(ByVal value As String)
            _EmailID = value
        End Set
    End Property



End Class

Public Class clsReportType
   
    Enum ReportFormat
        Pdf = 1
        Excel = 2

    End Enum

       

End Class

