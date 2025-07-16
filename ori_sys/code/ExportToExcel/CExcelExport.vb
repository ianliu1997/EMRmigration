Imports System.Data.SqlClient
Imports System.Diagnostics.Process
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Excel
Imports System.IO

Public Class CExcelExport

    Dim objExcel As New Excel.Application
    Dim xlsSheet As New Excel.Worksheet

    Dim ml_oDT As Data.DataTable
    Dim iRow As Integer
    Dim icol As Integer
    Dim SqldbCon As SqlConnection
    Dim strConnection As String
    Private trans As SqlTransaction
    Private connectionStr As String = ""
    Private transMode As Boolean = False

    Dim sServerName As String
    Dim sstr As String
    Dim susername As String
    Dim suserpsw As String
    Dim dbName As String
    Dim strIniPath As String
    Dim sProvider As String
    Dim dbtype As String

    Public Property ConnectionString() As String
        Get
            Return strConnection
        End Get
        Set(ByVal Value As String)
            strConnection = Value
        End Set
    End Property

    Public Function GetConnection() As SqlConnection

        If IsNothing(SqldbCon) Then
            SqldbCon = New SqlConnection(strConnection)
        End If

        If SqldbCon.State = ConnectionState.Closed Or SqldbCon.State = ConnectionState.Broken Then
            SqldbCon.Open()
        End If

        Return SqldbCon
    End Function

    Public Sub CloseConnection()
        If IsNothing(SqldbCon) = False Then
            If Not (SqldbCon.State = Data.ConnectionState.Closed) Then _
                SqldbCon.Close()
            SqldbCon = Nothing
        End If
    End Sub


#Region "Transaction Methods"
    'Added By Varsha
    'dt 05/01/07
    Public Sub BeginTrans()
        '  con.Open()
        GetConnection()
        trans = SqldbCon.BeginTransaction()
        transMode = True
    End Sub


    Public Sub CommitTrans()
        trans.Commit()
        CloseConnection()
        'If SqldbCon.State <> Data.ConnectionState.Closed Then
        '    SqldbCon.Close()
        'End If
        SqldbCon = Nothing
        transMode = False
    End Sub


    Public Sub RollbackTrans()
        trans.Rollback()
        CloseConnection()
        'If SqldbCon.State <> Data.ConnectionState.Closed Then
        '    SqldbCon.Close()
        'End If
        SqldbCon = Nothing
        transMode = False
    End Sub

#End Region

#Region "Execute Non Query"

    Public Function Execute(ByVal strSql As String) As Int16
        Dim cmd As SqlCommand
        Dim intRec As Int16

        cmd = New SqlCommand(strSql, GetConnection)
        intRec = cmd.ExecuteNonQuery
        Execute = intRec
        CloseConnection()
        cmd.Dispose()
        cmd = Nothing
    End Function

    Public Overloads Function ExecuteNonQuery(ByVal spName As String, ByVal ParamArray parameterValues() As Object) As Array
        Dim cmd As SqlCommand
        Dim cmdParameters As SqlParameter()
        Dim rtnValue() As Object

        'Dim SqldbCon As SqlConnection = New SqlConnection(ConfigurationManager.AppSettings("DbConnection"))
        Dim SqldbCon As SqlConnection = New SqlConnection(strConnection)

        Try
            'Creates new instance of the command oject
            cmd = New SqlCommand

            SqldbCon.Open()

            With cmd
                'Sets the connection 
                .Connection = SqldbCon 'GetConnection()
                .CommandType = Data.CommandType.StoredProcedure
                .CommandText = spName
                'Retrieves the list of Parameters
                SqlCommandBuilder.DeriveParameters(cmd)
                If True Then
                    cmd.Parameters.RemoveAt(0)
                End If

                cmdParameters = New SqlParameter(cmd.Parameters.Count - 1) {}
                'Copies the parameters in the sqlParamter
                cmd.Parameters.CopyTo(cmdParameters, 0)
                'Assigns the values to the corresponding paramters
                AssignParamValues(cmdParameters, parameterValues)

                'Executes the Stored Procedure and returns data reader
                .ExecuteNonQuery()

                Dim para As SqlParameter
                Dim i As Short = 0
                For Each para In cmd.Parameters
                    If para.Direction = Data.ParameterDirection.InputOutput Or Data.ParameterDirection.Output Then
                        ReDim Preserve rtnValue(i)
                        rtnValue(i) = para.Value
                        i = i + 1
                    End If
                Next
            End With
            'CloseConnection()

        Catch ex As Exception

        Finally
            cmd.Parameters.Clear()
            cmd = Nothing

            If IsNothing(SqldbCon) = False Then
                If Not (SqldbCon.State = Data.ConnectionState.Closed) Then _
                    SqldbCon.Close()
                SqldbCon = Nothing
            End If
        End Try

        Return rtnValue

    End Function

    Public Overloads Function ExecuteNonQuery(ByVal spName As String, ByVal blnReturnVal As Boolean, ByVal ParamArray parameterValues() As Object) As Integer
        Dim cmd As SqlCommand
        Dim cmdParameters As SqlParameter()
        Dim rowsAff As Integer
        'Dim SqldbCon As SqlConnection = New SqlConnection(ConfigurationManager.AppSettings("DbConnection"))
        Dim SqldbCon As SqlConnection = New SqlConnection(strConnection)

        Try
            'Creates new instance of the command oject
            cmd = New SqlCommand


            SqldbCon.Open()

            With cmd
                'Sets the connection 
                '.Connection = GetConnection()
                .Connection = SqldbCon
                .CommandType = Data.CommandType.StoredProcedure
                .CommandText = spName
                'Retrieves the list of Parameters
                SqlCommandBuilder.DeriveParameters(cmd)
                If Not blnReturnVal Then
                    cmd.Parameters.RemoveAt(0)
                End If

                cmdParameters = New SqlParameter(cmd.Parameters.Count - 1) {}
                'Copies the parameters in the sqlParamter
                cmd.Parameters.CopyTo(cmdParameters, 0)
                'Assigns the values to the corresponding paramters
                AssignParamValues(cmdParameters, parameterValues)

                'Executes the Stored Procedure and returns data reader
                rowsAff = .ExecuteNonQuery()
                ' rowsAff = cmdParameters(0).Value
            End With
            'CloseConnection()
        Catch ex As Exception

        Finally
            cmd.Parameters.Clear()
            cmd = Nothing

            If IsNothing(SqldbCon) = False Then
                If Not (SqldbCon.State = Data.ConnectionState.Closed) Then _
                    SqldbCon.Close()
                SqldbCon = Nothing
            End If

        End Try

        Return rowsAff

    End Function

    Public Overloads Function ExecuteNonQuery(ByVal strsql As String) As Integer
        'Executes the sql statement and returns date reader
        Dim cmd As SqlCommand
        Dim dtrd As SqlDataReader
        Dim lngRowsAfftected As Long

        'Dim SqldbCon As SqlConnection = New SqlConnection(ConfigurationManager.AppSettings("DbConnection"))
        ''Dim SqldbCon As SqlConnection = New SqlConnection(strConnection)
        ''SqldbCon.Open()

        cmd = New SqlCommand(strsql, GetConnection)
        ' cmd = New SqlCommand(strsql, SqldbCon)

        'populates the data reader with records 
        'and closes the connection after using the data reader
        lngRowsAfftected = cmd.ExecuteNonQuery()

        'If IsNothing(SqldbCon) = False Then
        '    If Not (SqldbCon.State = Data.ConnectionState.Closed) Then _
        '        SqldbCon.Close()
        '    SqldbCon = Nothing
        'End If

        If IsNothing(SqldbCon) = False Then
            If Not (SqldbCon.State = ConnectionState.Closed) Then _
                SqldbCon.Close()
            SqldbCon = Nothing
        End If

        Return lngRowsAfftected

    End Function

    Public Overloads Function ExecuteNonQuery(ByVal spName As String, ByVal blnReturnVal As Boolean, _
                                                ByRef OutputParameter As Object, ByVal ParamArray parameterValues() As Object) As Integer

        Dim cmd As SqlCommand
        Dim dtrd As SqlDataReader
        Dim cmdParameters As SqlParameter()
        Dim rowsAff As Integer
        'Dim SqldbCon As SqlConnection = New SqlConnection(ConfigurationManager.AppSettings("DbConnection"))
        Dim SqldbCon As SqlConnection = New SqlConnection(strConnection)

        Try
            'Creates new instance of the command oject
            cmd = New SqlCommand


            SqldbCon.Open()

            With cmd
                'Sets the connection 
                '.Connection = GetConnection()
                .Connection = SqldbCon
                .CommandType = CommandType.StoredProcedure
                .CommandText = spName
                'Retrieves the list of Parameters
                SqlCommandBuilder.DeriveParameters(cmd)
                If Not blnReturnVal Then
                    cmd.Parameters.RemoveAt(0)
                End If

                cmdParameters = New SqlParameter(cmd.Parameters.Count - 1) {}
                'Copies the parameters in the sqlParamter
                cmd.Parameters.CopyTo(cmdParameters, 0)
                'Assigns the values to the corresponding paramters
                AssignParamValues(cmdParameters, parameterValues)

                'Executes the Stored Procedure and returns data reader
                rowsAff = .ExecuteNonQuery()
                ' rowsAff = cmdParameters(0).Value
                'Loop to get the out parameter value

                For i As Integer = 0 To cmd.Parameters.Count - 1
                    'If cmd.Parameters(i).Direction = ParameterDirection.Output Then
                    If cmdParameters(i).Direction = ParameterDirection.InputOutput Then
                        OutputParameter = cmdParameters(i).Value
                        Exit For
                    End If
                Next

            End With
            'CloseConnection()
        Catch ex As Exception
            'MsgBox(ex.Message)
        Finally
            cmd.Parameters.Clear()
            cmd = Nothing

            If IsNothing(SqldbCon) = False Then
                If Not (SqldbCon.State = ConnectionState.Closed) Then _
                    SqldbCon.Close()
                SqldbCon = Nothing
            End If

        End Try

        Return rowsAff

    End Function


    Public Function ExecuteSql(ByVal strsql As String) As SqlDataReader
        'Executes the sql statement and returns date reader
        Dim cmd As SqlCommand
        Dim dtrd As SqlDataReader

        'Dim SqldbCon As SqlConnection = New SqlConnection(ConfigurationSettings.AppSettings("DbConnection"))
        'SqldbCon.Open()

        cmd = New SqlCommand(strsql, GetConnection)
        'cmd = New SqlCommand(strsql, SqldbCon)
        'populates the data reader with records 
        'and closes the connection after using the data reader
        dtrd = cmd.ExecuteReader()

        Return dtrd

    End Function

#End Region

#Region "Assign Param"
    Private Sub AssignParamValues(ByVal commandParameters() As SqlParameter, ByVal parameterValues() As Object)
        Dim i As Short
        Dim j As Short

        If (commandParameters Is Nothing) And (parameterValues Is Nothing) Then
            'do nothing if we get no data
            Return
        End If

        ' we must have the same number of values as we pave parameters to put them in
        If commandParameters.Length <> parameterValues.Length Then
            Throw New ArgumentException("Parameter count does not match Parameter Value count.")
        End If

        'value array
        j = commandParameters.Length - 1
        For i = 0 To j
            commandParameters(i).Value = parameterValues(i)
        Next

    End Sub



#End Region


    Public Function ExecuteScalar(ByVal sql As String) As Object
        Dim result As Object = Nothing
        ' Dim con1 As SqlConnection = New SqlConnection(connectionStr)
        ' con1.Open()
        Try
            Dim com As SqlCommand = New SqlCommand(sql, GetConnection)
            result = com.ExecuteScalar()

        Catch ex As Exception

        Finally
            If IsNothing(SqldbCon) = False Then
                If Not (SqldbCon.State = ConnectionState.Closed) Then _
                    SqldbCon.Close()
                SqldbCon = Nothing
            End If
            'con1.Close()
        End Try
        Return result
    End Function

    Public Function ExecuteDataSet(ByVal strsql As String) As DataSet
        Dim dtad As SqlDataAdapter
        Dim dtst As DataSet
        dtst = New DataSet
        dtad = New SqlDataAdapter(strsql, GetConnection)
        dtad.Fill(dtst)
        CloseConnection()
        Return dtst
    End Function


#Region "Execute Data Table"

    Public Overloads Function ExecuteDatatable(ByVal spName As String) As Data.DataTable

        Dim cmd As SqlCommand
        'Dim SqldbCon As SqlConnection = New SqlConnection(ConfigurationManager.AppSettings("DbConnection"))
        Dim SqldbCon As SqlConnection = New SqlConnection(strConnection)
        Dim da As SqlDataAdapter
        Dim dt As New Data.DataTable
        cmd = New SqlCommand(spName, SqldbCon)
        da = New SqlDataAdapter(cmd)
        da.Fill(dt)
        SqldbCon.Close()
        cmd = Nothing
        Return dt
    End Function

    Public Overloads Function ExecuteDataTable(ByVal spName As String, ByVal blnReturnVal As Boolean, ByVal ParamArray parameterValues() As Object) As Data.DataTable
        Dim dtad As SqlDataAdapter
        Dim dtTb As Data.DataTable

        Dim cmd As SqlCommand
        Dim cmdParameters As SqlParameter()
        'Dim SqldbCon As SqlConnection = New SqlConnection(ConfigurationManager.AppSettings("DbConnection"))
        Dim SqldbCon As SqlConnection = New SqlConnection(strConnection)


        Try
            SqldbCon.Open()
            'Creates new instance of the command oject
            cmd = New SqlCommand

            With cmd
                'Sets the connection 
                .Connection = SqldbCon
                .CommandType = Data.CommandType.StoredProcedure
                .CommandText = spName
                'Retrieves the list of Parameters
                SqlCommandBuilder.DeriveParameters(cmd)
                If Not blnReturnVal Then
                    cmd.Parameters.RemoveAt(0)
                End If

                cmdParameters = New SqlParameter(cmd.Parameters.Count - 1) {}
                'Copies the parameters in the sqlParamter
                cmd.Parameters.CopyTo(cmdParameters, 0)
                'Assigns the values to the corresponding paramters
                AssignParamValues(cmdParameters, parameterValues)
            End With

            dtTb = New Data.DataTable
            dtad = New SqlDataAdapter(cmd)
            dtad.Fill(dtTb)

            'CloseConnection()
        Catch ex As Exception

        Finally
            cmd.Parameters.Clear()
            cmd.Dispose()
            dtad.Dispose()

            If IsNothing(SqldbCon) = False Then
                If Not (SqldbCon.State = Data.ConnectionState.Closed) Then _
                    SqldbCon.Close()
                SqldbCon = Nothing
            End If

            cmd = Nothing
        End Try

        Return dtTb
    End Function

#End Region

    Public Function ExportToExcel(ByVal strSql As String, ByVal strReportName As String, ByVal strPath As String, _
                                Optional ByVal oDT As Data.DataTable = Nothing)

        Try
            If Not oDT Is Nothing Then
                ml_oDT = oDT
            Else
                Try
                    ml_oDT = ExecuteDatatable(strSql)
                Catch ex As Exception

                End Try
            End If
            '
            If ml_oDT.Rows.Count < 0 Then
                Exit Function
            End If

            KillExcelInstanceFromMemory()

            With objExcel
                '.Visible = True
                .Workbooks.Add()
                .WindowState = XlWindowState.xlMaximized
                .SheetsInNewWorkbook = 1
                xlsSheet = objExcel.Worksheets.Add
                xlsSheet.Activate()
            End With
            xlsSheet.Name = "Report Name"
            ExportColumnHeadings(strReportName)
            iRow = 4

            For i As Integer = 0 To ml_oDT.Rows.Count - 1
                iRow = Increment(iRow)
                icol = 1
                For j As Integer = 0 To ml_oDT.Columns.Count - 1
                    With xlsSheet
                        If ml_oDT.Rows(i).Item(j).ToString = "" Or IsDBNull(ml_oDT.Rows(i).Item(j)) = True Then
                            .Cells(iRow, icol).value = " - "
                        Else
                            .Cells(iRow, icol).value = ml_oDT.Rows(i).Item(j).ToString
                        End If
                        icol = Increment(icol)
                    End With
                Next
            Next
            objExcel.Workbooks(1).SaveAs(strPath & "\" & strReportName & ".xls")
            objExcel.DisplayAlerts = False
            objExcel.Quit()

            GC.Collect()
            GC.WaitForPendingFinalizers()


            KillExcelInstanceFromMemory()
        Catch ex As Exception
        Finally
            System.Runtime.InteropServices.Marshal.ReleaseComObject(objExcel)
            KillExcelInstanceFromMemory()
        End Try

    End Function

    Private Function Increment(ByRef i As Integer, _
                                Optional ByVal intIncrementBy As Integer = 1) As Integer
        i += intIncrementBy
        Return i
    End Function

    Private Function KillExcelInstanceFromMemory()
        'Dim KillExcelProcess As Process()
        Try
            '    KillExcelProcess = Process.GetProcessesByName("Excel")
            '    For i As Integer = 0 To KillExcelProcess.Length - 1
            '        KillExcelProcess(i).Kill()
            '    Next
            Dim procList() As Process = Process.GetProcesses()

            Dim k As Integer

            For k = 0 To procList.GetUpperBound(0) Step k + 1

                If procList(k).ProcessName = "EXCEL" Then

                    procList(k).Close()

                    procList(k).Dispose()





                End If

                'Dim proc As System.Diagnostics.Process

                'For Each proc In System.Diagnostics.Process.GetProcessesByName("EXCEL")
                '    proc.Kill()
                'Next



            Next


        Catch ex As Exception


        End Try
    End Function

    Private Function ExportColumnHeadings(ByVal Heading As String)
        xlsSheet.Cells(1, 1).value = Heading.Split("_")

        xlsSheet.Cells.Range("A1", "A1").Font.Size = 12
        xlsSheet.Cells.Range("A1", "A1").Font.Bold = True

        xlsSheet.Cells(2, 1).value = "Date : " & Date.Now
        xlsSheet.Cells.Range("A2", "A2").Font.Size = 8
        xlsSheet.Cells.Range("A2", "A2").Font.Bold = True


        With ml_oDT
            For i As Integer = 0 To .Columns.Count - 1
                icol = Increment(icol)
                xlsSheet.Cells(4, icol).value = .Columns(i).Caption
                xlsSheet.Cells.ColumnWidth = 12
                xlsSheet.Cells.Range("A4", "AC4").Font.Bold = True
                xlsSheet.Cells.Range("A4", "AC4").WrapText = True
            Next
        End With
    End Function
End Class




