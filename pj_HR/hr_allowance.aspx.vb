Imports System
Imports System.Data
Imports System.Data.OleDb



Partial Class hr_allowance
    Inherits System.Web.UI.Page

    Dim mlOBJGS As New IASClass.ucmGeneralSystem
    Dim mlOBJGF As New IASClass.ucmGeneralFunction
    Dim mlOBJPJ As New FunctionLocal

    Dim mlREADER As OleDb.OleDbDataReader
    Dim mlSQL As String
    Dim mlREADER2 As OleDb.OleDbDataReader
    Dim mlSQL2 As String
    Dim mlRSTEMP As OleDb.OleDbDataReader
    Dim mlSQLTEMP As String
    Dim mlKEY As String
    Dim mlRECORDSTATUS As String
    Dim mlSPTYPE As String
    Dim mlFUNCTIONPARAMETER As String
    Dim mlI As Integer
    Dim mlCURRENTDATE As String = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString()

    Dim mlDATATABLE As DataTable
    Dim mlDATAROW As DataRow




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        mlTITLE.Text = "Allowance V2.00"
        Me.Title = System.Configuration.ConfigurationManager.AppSettings("mgTITLE") & "Allowance V2.00"
        mlOBJGS.Main()
        If mlOBJGS.ValidateExpiredDate() = True Then
            Exit Sub
        End If
        If Session("mgACTIVECOMPANY") = "" Then Session("mgACTIVECOMPANY") = mlOBJGS.mgCOMPANYDEFAULT
        mlOBJGS.mgACTIVECOMPANY = Session("mgACTIVECOMPANY")


        If Page.IsPostBack = False Then
            pnSEARCHSITECARD.Visible = False
            mlOBJGS.XM_UserLog(Session("mgUSERID"), Session("mgNAME"), "", "", "")
        Else
        End If
    End Sub

    Protected Sub mlDATAGRID_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles mlDATAGRID.ItemCommand
        mlKEY = e.CommandArgument
        Select Case e.CommandName
            Case "BrowseRecord"
                mlMESSAGE.Text = "View Request for " & e.CommandArgument
                RetrieveFields()
                pnFILL.Visible = True
            Case "EditRecord"
                mlMESSAGE.Text = "Edit Request for  " & e.CommandArgument
                mlSYSCODE.Value = "edit"
                EditRecord()
            Case "DeleteRecord"
                mlMESSAGE.Text = "Delete Request for  " & e.CommandArgument
                mlSYSCODE.Value = "delete"
                DeleteRecord()
            Case Else
                ' Ignore Other
        End Select
    End Sub

    Protected Sub btNewRecord_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btNewRecord.Click
        NewRecord()
    End Sub

    Protected Sub btSaveRecord_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btSaveRecord.Click
        If pnFILL.Visible = True Then
            SaveRecord()
        End If
    End Sub

    Protected Sub btCancelOperation_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btCancelOperation.Click
        DisableCancel()
    End Sub

    Sub SearchRecord()
        Dim mlSQL As String

        If pnFILL.Visible = False Then
            ClearFields()
            EnableCancel()
            pnFILL.Visible = True
            Exit Sub
        End If

        Try
            mlSQL = ""

            If mlOBJGF.IsNone(mlSQL) = False Then
                Try
                    mlSQL2 = "SELECT "
                    RetrieveFieldsDetail(mlSQL)
                Catch ex As Exception
                End Try
            End If


        Catch ex As Exception
        End Try
    End Sub

    Protected Sub btSearchRecord_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btSearchRecord.Click
        SearchRecord()
    End Sub


    Public Sub RetrieveFields()
        DisableCancel()

        mlSQL = "SELECT * FROM WHERE DocNo = '" & Trim(mlKEY) & "'"
        mlREADER = mlOBJGS.DbRecordset(mlSQL)
        If mlREADER.HasRows Then
            mlREADER.Read()
            txDOCUMENTNO.Text = mlREADER("DocNo") & ""
        End If
    End Sub

    Sub RetrieveFieldsDetail(ByVal mpSQL As String)
        If mpSQL = "" Then
            mlSQL2 = "SELECT * FROM " & _
                " WHERE RecordStatus='New' ORDER BY DocNo"
        Else
            mlSQL2 = mpSQL
        End If
        mlREADER2 = mlOBJGS.DbRecordset(mlSQL2)
        mlDATAGRID.DataSource = mlREADER2
        mlDATAGRID.DataBind()

        mlOBJGS.CloseFile(mlREADER2)
    End Sub


    Sub DeleteRecord()
        mlSPTYPE = "Delete"
        Try
            Sql_1(mlSPTYPE, mlKEY)
        Catch ex As Exception
        End Try

        mlSYSCODE.Value = ""
        RetrieveFields()
    End Sub

    Sub NewRecord()
        mlOBJGS.mgNEW = True
        mlOBJGS.mgEDIT = False
        EnableCancel()
        ClearFields()
        LoadComboData()
        txDOCUMENTNO.Text = "--AUTONUMBER--"
        mlOBJPJ.SetTextBox(True, txDOCUMENTNO)

        txDOCDATE.Text = mlCURRENTDATE
        mlOBJPJ.SetTextBox(True, txDOCDATE)
        btDOCDATE.Visible = False
    End Sub

    Sub EditRecord()
        mlOBJGS.mgNEW = False
        mlOBJGS.mgEDIT = True
        ClearFields()
        LoadComboData()
        RetrieveFields()
        EnableCancel()
    End Sub


    Private Sub EnableCancel()
        btNewRecord.Visible = False
        btSaveRecord.Visible = True
        pnFILL.Visible = True

        mlOBJPJ.SetTextBox(False, txDOCUMENTNO)

    End Sub

    Private Sub DisableCancel()
        btNewRecord.Visible = True
        btSaveRecord.Visible = False
        pnFILL.Visible = False

        mlOBJPJ.SetTextBox(True, txDOCUMENTNO)

    End Sub

    Sub ClearFields()
        txDOCUMENTNO.Text = ""

    End Sub


    Sub LoadComboData()

        Dim mlDG As DataGridItem
        Dim mlLOOP As Boolean
        Dim mlTYPEL2 As String

        Dim mlTYPEL As String
        mlTYPEL = ""
        mlSQL = "SELECT * FROM XM_UNIVERSALLOOKUPLIN WHERE UniversalID = 'HR_ALLOWANCE'"
        mlREADER = mlOBJGS.DbRecordset(mlSQL)
        While mlREADER.Read
            mlTYPEL = mlTYPEL & (IIf(mlOBJGF.IsNone(mlTYPEL) = True, "", "#")) & Trim(mlREADER("LinCode"))
        End While

        For Each mlDG In mlDATAGRID3.Items
            mlLOOP = True
            Dim mlOB_TYPE As DropDownList = CType(mlDG.FindControl("ddTYPE"), DropDownList)
            Dim mlPANEL As Panel = CType(mlDG.FindControl("pnSEARCHUSERID"), Panel)
            Dim mlTX_NILAI As TextBox = CType(mlDG.FindControl("txNILAIALLOWANCE"), TextBox)


            mlPANEL.Visible = False
            mlI = 0
            Do While mlLOOP = True

                mlTYPEL2 = mlOBJGF.GetStringAtPosition(Trim(mlTYPEL), mlI, "#")
                If mlTYPEL2 = "" Then
                    mlLOOP = False
                Else
                    mlOB_TYPE.Items.Add(mlTYPEL2)
                End If
                mlI = mlI + 1
            Loop
        Next mlDG


        'ddENTITY.Items.Clear()
        'ddENTITY.Items.Add("ISS")
        'mlSQLTEMP = "SELECT * FROM XM_UNIVERSALLOOKUPLIN WHERE UniversalID='ISS_Entity'"
        'mlRSTEMP = mlOBJGS.DbRecordset(mlSQLTEMP, "PB", "ISS")
        'While mlRSTEMP.Read
        '    ddENTITY.Items.Add(Trim(mlRSTEMP("LinCode")))
        'End While
    End Sub

    Sub SaveRecord()
        Dim mlSQLHR As String = ""
        Dim mlSQLDT As String = ""

        mlOBJGS.mgMESSAGE = ValidateForm()
        If mlOBJGF.IsNone(mlOBJGS.mgMESSAGE) = False Then
            mlMESSAGE.Text = mlOBJGS.mgMESSAGE
            Exit Sub
        End If

        mlSPTYPE = "New"
        If mlSYSCODE.Value = "edit" Then
            mlSPTYPE = "Edit"
        End If

        Try
            mlKEY = Trim(txDOCUMENTNO.Text)
            Sql_1(mlSPTYPE, mlKEY)
        Catch ex As Exception
        End Try

        mlSYSCODE.Value = ""
        ClearFields()
        DisableCancel()
    End Sub

    Function ValidateForm() As String
        ValidateForm = ""

        If mlOBJGS.mgNEW = True Then

        End If

    End Function

    ''
    Protected Sub btSEARCHSITECARD_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btSEARCHSITECARD.Click
        If pnSEARCHSITECARD.Visible = False Then
            pnSEARCHSITECARD.Visible = True
        Else
            pnSEARCHSITECARD.Visible = False
        End If
    End Sub


    Protected Sub btSEARCHSITECARDSUBMIT_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btSEARCHSITECARDSUBMIT.Click
        If mlOBJGF.IsNone(mlSEARCHSITECARD.Text) = False Then SearchSiteCard(1, mlSEARCHSITECARD.Text, ddENTITY.Text)
    End Sub

    Protected Sub mlDATAGRIDSITECARD_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles mlDATAGRIDSITECARD.ItemCommand
        Try
            txSITECARD.Text = CType(e.Item.Cells(0).Controls(0), LinkButton).Text
            lbSITEDESC.Text = CType(e.Item.Cells(1).Controls(0), LinkButton).Text
            mlDATAGRIDSITECARD.DataSource = Nothing
            mlDATAGRIDSITECARD.DataBind()
            pnSEARCHSITECARD.Visible = False

        Catch ex As Exception
        End Try
    End Sub

    Sub SearchSiteCard(ByVal mpTYPE As Byte, ByVal mpNAME As String, ByVal mpCOMPANYID As String)
        Select Case mpTYPE
            Case "1"
                mlSQLTEMP = "SELECT SiteCardID,SiteCardName FROM OP_USER_SITE WHERE SiteCardName LIKE  '%" & mlSEARCHSITECARD.Text & "%' AND EntityID = '" & Trim(mpCOMPANYID) & "' "
                mlRSTEMP = mlOBJGS.DbRecordset(mlSQLTEMP, "PB", "ISSP3")
                mlDATAGRIDSITECARD.DataSource = mlRSTEMP
                mlDATAGRIDSITECARD.DataBind()
        End Select
    End Sub

    Protected Sub btSITECARD_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btSITECARD.Click
        'mlSQLTEMP = "SELECT SiteCardID,SiteCardName FROM OP_USER_SITE WHERE SiteCardID LIKE  '" & txSITECARD.Text & "' AND EntityID = '" & Trim(ddENTITY.Text) & "' "
        mlSQLTEMP = "SELECT SiteCardID,SiteCardName FROM OP_USER_SITE WHERE SiteCardID LIKE  '" & txSITECARD.Text & "' "
        mlRSTEMP = mlOBJGS.DbRecordset(mlSQLTEMP, "PB", "ISSP3")
        If mlRSTEMP.HasRows Then
            mlRSTEMP.Read()
            lbSITEDESC.Text = mlRSTEMP("SiteCardName") & ""

        End If
        mlOBJGS.CloseFile(mlRSTEMP)
    End Sub


    ''
    Protected Sub mlDATAGRID3_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles mlDATAGRID3.ItemCommand
        mlKEY = e.CommandArgument
        Select Case e.CommandName
            Case "BrowseRecord"
                mlMESSAGE.Text = "View Request for " & e.CommandArgument
                RetrieveFields()
            Case "EditRecord"
                mlMESSAGE.Text = "Edit Request for  " & e.CommandArgument
                mlSYSCODE.Value = "edit"
                EditRecord()
            Case "DeleteRecord"
                mlMESSAGE.Text = "Delete Request for  " & e.CommandArgument
                mlSYSCODE.Value = "delete"
                DeleteRecord()

            Case "Showpanel_UserID"
                GRID_ShowPanelUserID()

            Case "Complete_Name"
                GRID_CompleteUserName()

            Case "SEARCHUSERIDSUBMIT"

            Case Else
                ' Ignore Other
        End Select
    End Sub

    Sub GRID_ShowPanelUserID()
        Dim mlDG As DataGridItem

        For Each mlDG In mlDATAGRID3.Items
            Dim mlPANEL As Panel = CType(mlDG.FindControl("pnSEARCHUSERID"), Panel)
            Dim mlNO As String

            mlNO = mlDG.Cells("1").Text
            If mlNO = mlKEY Then
                If mlPANEL.Visible = False Then
                    mlPANEL.Visible = True
                Else
                    mlPANEL.Visible = False
                End If
            End If
        Next mlDG
    End Sub


    Sub GRID_CompleteUserName()
        Dim mlDG As DataGridItem

        For Each mlDG In mlDATAGRID3.Items
            Dim mlPANEL As Panel = CType(mlDG.FindControl("pnSEARCHUSERID"), Panel)
            Dim mlUSERID As TextBox = CType(mlDG.FindControl("txUSERID"), TextBox)
            Dim mlUSERNAME As Label = CType(mlDG.FindControl("txUSERDESC"), Label)
            Dim mlNO As String

            mlNO = mlDG.Cells("1").Text
            If mlNO = mlKEY Then
                mlUSERNAME.Text = mlOBJGS.ADGeneralLostFocus("USER", Trim(mlUSERID.Text))
            End If
        Next mlDG
    End Sub


    Sub AddRow(ByVal mpROWSNO As Integer)

        mlDATATABLE = New DataTable
        mlDATATABLE.Columns.Add("Linno", GetType(Integer))

        mlI = 1
        For mlI = 1 To mpROWSNO
            mlDATAROW = mlDATATABLE.NewRow
            mlDATAROW("Linno") = mlI
            mlDATATABLE.Rows.Add(mlDATAROW)
        Next
        mlDATAGRID3.DataSource = mlDATATABLE
        mlDATAGRID3.DataBind()
    End Sub


    Protected Sub btROWSNO_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btROWSNO.Click
        AddRow(txROWSNO.Text)
        LoadComboData()
    End Sub


    Sub Sql_1(ByVal mpSTATUS As String, ByVal mpCODE As String)
        Dim mlSTATUS As String
        Dim mlNEWRECORD As Boolean

        Try
            mlNEWRECORD = False

            Select Case mpSTATUS
                Case "Edit", "Delete"
                    mlSQL = ""
                    mlSQL = mlSQL & mlOBJPJ.ISS_OP_UserSiteCard_ToLog(mlKEY, mpSTATUS, Session("mgUSERID"))
            End Select

            Select Case mpSTATUS
                Case "New"
                    mlNEWRECORD = True
                    If txDOCUMENTNO.Text = "--AUTONUMBER--" Then
                        mlKEY = mlOBJGS.GetModuleCounter("" & mlFUNCTIONPARAMETER, "00000000")
                        txDOCUMENTNO.Text = mlKEY
                    Else
                        mlKEY = Trim(txDOCUMENTNO.Text)
                    End If

                Case "Edit"
                    mlSTATUS = "Edit"
                    mlNEWRECORD = True
                    mlSQL = mlSQL & " DELETE FROM  WHERE DocNo = '" & mlKEY & "';"

                Case "Delete"
                    mlSTATUS = "Delete"
                    mlSQL = mlSQL & " DELETE FROM  WHERE DocNo = '" & mlKEY & "';"
            End Select
            If mlOBJGF.IsNone(mlSQL) = False Then mlOBJGS.ExecuteQuery(mlSQL)
            mlSQL = ""


            mlI = 0
            If mlNEWRECORD = True Then
                mlSQL = ""
                mlI = mlI + 1
                mlSQL = mlSQL & " INSERT INTO " & _
                            " RecordStatus,RecUserID,RecDate) VALUES ( " & _
                            " '" & mlFUNCTIONPARAMETER & "','" & mlKEY & "','" & mlOBJGF.FormatDate(Now) & "'," & _
                            " 'New','" & Session("mgUSERID") & "','" & mlOBJGF.FormatDate(Now) & "');"
            End If

            Select Case mpSTATUS
                Case "New"
                    mlSQL = mlSQL & mlOBJPJ.ISS_OP_UserSiteCard_ToLog(mlKEY, mpSTATUS, Session("mgUSERID"))
            End Select
            mlOBJGS.ExecuteQuery(mlSQL)
            mlSQL = ""

        Catch ex As Exception
            mlOBJGS.XMtoLog("", "", "" & mlKEY, Err.Description, "New", Session("mgUSERID"), mlOBJGF.FormatDate(Now))
        End Try
    End Sub





End Class
