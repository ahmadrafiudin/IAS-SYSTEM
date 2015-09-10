
Imports System
Imports System.Data
Imports System.Data.OleDb

Partial Class backoffice_ap_mr_entry
    Inherits System.Web.UI.Page

    Dim mlOBJGS As New IASClass.ucmGeneralSystem
    Dim mlOBJGF As New IASClass.ucmGeneralFunction
    Dim mlOBJPJ As New FunctionLocal

    Dim mlREADER As OleDb.OleDbDataReader
    Dim mlSQL As String
    Dim mlREADER2 As OleDb.OleDbDataReader
    Dim mlSql_2 As String
    Dim mlKEY As String
    Dim mlKEY2 As String
    Dim mlKEY3 As String
    Dim mlRECORDSTATUS As String
    Dim mlSPTYPE As String
    Dim mlFUNCTIONPARAMETER As String

    Dim mlDATATABLE As DataTable
    Dim mlDATAROW As DataRow
    Dim mlI As Integer

    Dim mlSQLTEMP As String
    Dim mlRSTEMP As OleDb.OleDbDataReader
    Dim mlCURRENTDATE As String = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString()
    Dim mlCURRENTBVMONTH As String = DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString()
    Dim mlSHOWTOTAL As Boolean
    Dim mlSHOWPRICE As Boolean
    Dim mlUSERLEVEL As String

    Dim mlCOMPANYTABLENAME As String
    Dim mlCOMPANYID As String
    Dim mlPARAM_COMPANY As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        mlTITLE.Text = "Material Requisition Entry V2.11"
        Me.Title = System.Configuration.ConfigurationManager.AppSettings("mgTITLE") & "Material Requisition Entry V2.11"

        mlOBJGS.Main()
        If mlOBJGS.ValidateExpiredDate() = True Then
            Exit Sub
        End If
        If Session("mgACTIVECOMPANY") = "" Then Session("mgACTIVECOMPANY") = mlOBJGS.mgCOMPANYDEFAULT
        mlOBJGS.mgACTIVECOMPANY = Session("mgACTIVECOMPANY")

        'Session("mgUSERID") = "52919"
        'Session("mgNAME") = "52919"


        mlPARAM_COMPANY = Left(Trim(Request("mpFP")), 1)
        If mlPARAM_COMPANY = "" Then mlPARAM_COMPANY = "1"
        mlFUNCTIONPARAMETER = Trim(Request("mpFP"))
        
        Select Case mlPARAM_COMPANY
            Case "", "1"
                ddENTITY.Items.Clear()
                ddENTITY.Text = "ISS"
                ddENTITY.Items.Add("ISS")
                mlTITLE.Text = mlTITLE.Text & " (ISS)"
            Case "2"
                ddENTITY.Items.Clear()
                ddENTITY.Text = "IFS"
                ddENTITY.Items.Add("IFS")
                mlTITLE.Text = mlTITLE.Text & " (IFS - Facility Services)"
            Case "3"
                ddENTITY.Items.Clear()
                ddENTITY.Text = "IPM"
                ddENTITY.Items.Add("IPM")
                mlTITLE.Text = mlTITLE.Text & " (IPM - Parking Management)"
            Case "4"
                ddENTITY.Items.Clear()
                ddENTITY.Text = "ICS"
                ddENTITY.Items.Add("ICS")
                mlTITLE.Text = mlTITLE.Text & " (ICS - Catering Services)"
        End Select

        mlCOMPANYTABLENAME = "ISS Servisystem, PT$"
        mlCOMPANYID = mlCOMPANYID
        Select Case ddENTITY.Text
            Case "ISS"
                mlCOMPANYTABLENAME = "ISS Servisystem, PT$"
                mlCOMPANYID = "ISSN3"
            Case "IPM"
                mlCOMPANYTABLENAME = "ISS Parking Management$"
                mlCOMPANYID = "IPM3"
            Case "ICS"
                mlCOMPANYTABLENAME = "ISS CATERING SERVICES$"
                mlCOMPANYTABLENAME = "ISS Catering Service 5SP1$"
                mlCOMPANYID = "ICS5"
            Case "IFS"
                mlCOMPANYTABLENAME = "INTEGRATED FACILITY SERVICES$"
                mlCOMPANYID = "IFS3"
        End Select



        If Page.IsPostBack = False Then
            LoadComboData2()
            tr2.Visible = False
            pnSEARCHSITECARD.Visible = False
            pnSEARCHITEMKEY.Visible = False
            tbTABLE1.Visible = False
            DisableCancel()
            RetrieveFieldsDetail()
            mlSHOWTOTAL = False
            mlSHOWPRICE = False
            btITEMKEYADD.Visible = False
            mlOBJGS.XM_UserLog(Session("mgUSERID"), Session("mgNAME"), "pc_mr_entry", "Mr. Request", "")
            GetMonth()
        Else
        End If
    End Sub

    Protected Sub btITEMKEY_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btITEMKEY.Click
        mpITEMDESC.Text = mlOBJPJ.ISS_INGeneralLostFocus("ITEMKEY", mpITEMKEY.Text)
        If mpITEMDESC.Text <> "" Then
            btITEMKEYADD.Visible = True
        End If
    End Sub

    Protected Sub btSEARCHITEMKEY_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btSEARCHITEMKEY.Click
        If pnSEARCHITEMKEY.Visible = False Then
            pnSEARCHITEMKEY.Visible = True
        Else
            pnSEARCHITEMKEY.Visible = False
        End If
    End Sub

    Protected Sub btSEARCHITEMKEYSUBMIT_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btSEARCHITEMKEYSUBMIT.Click
        If mlOBJGF.IsNone(mpSEARCHITEMKEY.Text) = False Then SearchItem(1, mpSEARCHITEMKEY.Text)
    End Sub

    Protected Sub mlDATAGRIDITEMKEY_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles mlDATAGRIDITEMKEY.ItemCommand
        Try
            mpITEMKEY.Text = CType(e.Item.Cells(0).Controls(0), LinkButton).Text
            mpITEMDESC.Text = CType(e.Item.Cells(1).Controls(0), LinkButton).Text
            mlDATAGRIDITEMKEY.DataSource = Nothing
            mlDATAGRIDITEMKEY.DataBind()
            pnSEARCHITEMKEY.Visible = False
            btITEMKEYADD.Visible = True
        Catch ex As Exception
        End Try
    End Sub

    Sub SearchItem(ByVal mpTYPE As Byte, ByVal mpNAME As String)
        Select Case mpTYPE
            Case "1"
                mlSQLTEMP = "SELECT No_, Description FROM [" & mlCOMPANYTABLENAME & "Item] INV WHERE Description LIKE  '%" & mpNAME & "%'"
                mlRSTEMP = mlOBJGS.DbRecordset(mlSQLTEMP, "PB", mlCOMPANYID)
                mlDATAGRIDITEMKEY.DataSource = mlRSTEMP
                mlDATAGRIDITEMKEY.DataBind()
        End Select
    End Sub


    Protected Sub mlDATAGRID_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles mlDATAGRID.ItemCommand
        mlKEY = e.CommandArgument
        mlKEY2 = mlOBJGF.GetStringAtPosition(mlKEY, 1, "#")
        mlKEY3 = mlOBJGF.GetStringAtPosition(mlKEY, 2, "#")
        mlKEY = mlOBJGF.GetStringAtPosition(mlKEY, 0, "#")


        Select Case e.CommandName
            Case "BrowseRecord"
                mlMESSAGE.Text = "Update Request for " & e.CommandArgument
                ClearFields()
                LoadComboData()
                RetrieveFields()
                RetrieveFieldsDetail21()
                RetrieveFieldsDetail41()
                FillDetail(mlKEY)
                ReadOnlyMode()
                pnFILL.Visible = True
                pnGRID2.Visible = True
                pnGRID3.Visible = True
                pnNOTEMP.Visible = False
                VisiblePrice()

            Case "EditRecord"
                mlMESSAGE.Text = "Edit Request for  " & e.CommandArgument
                mlSYSCODE.Value = "edit"
                EditRecord()
                pnFILL.Visible = True
                pnGRID2.Visible = True
                pnGRID3.Visible = True
            Case "DeleteRecord"
                mlMESSAGE.Text = "Delete Request for  " & e.CommandArgument
                mlSYSCODE.Value = "delete"
                DeleteRecord()
            Case Else
                ' Ignore Other
        End Select
    End Sub


    Protected Sub mlDATAGRID_ItemBound(ByVal Source As Object, ByVal E As DataGridItemEventArgs) Handles mlDATAGRID.ItemDataBound
        If E.Item.ItemType = ListItemType.Item Or E.Item.ItemType = ListItemType.AlternatingItem Then
            mlI = 5
            Dim mlDOCDATE5 As Date = Convert.ToDateTime(E.Item.Cells(mlI).Text)
            E.Item.Cells(mlI).Text = mlDOCDATE5.ToString("d")
            E.Item.Cells(mlI).HorizontalAlign = HorizontalAlign.Right

        End If
    End Sub


    Protected Sub mlDATAGRID2_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles mlDATAGRID2.ItemCommand
        mlKEY = e.CommandArgument
        Select Case e.CommandName
            Case "BrowseRecord"
                mlMESSAGE.Text = "Update Request for " & e.CommandArgument
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

    Public Sub RetrieveFields()
        mlSQL = "SELECT * FROM AP_MR_REQUESTHR WHERE DocNo = '" & Trim(mlKEY) & "'"
        mlREADER = mlOBJGS.DbRecordset(mlSQL, "PB", "ISSP3")
        If mlREADER.HasRows Then
            mlREADER.Read()
            Try
                mpMR_TEMPLATE.SelectedValue = mlREADER("MRType") & "-" & mlREADER("MRDesciption") & ""
            Catch ex As Exception
                mpMR_TEMPLATE.Items.Add(mlREADER("MRType") & "-" & mlREADER("MRDesciption") & "")
            End Try

            mpDOCUMENTNO.Text = mlREADER("DocNo") & ""
            mpDOCDATE.Text = IIf(mlOBJGF.IsNone(mlREADER("DocDate")), "", mlOBJGF.ConvertDateIntltoIDN(mlREADER("DocDate"), "/") & "")
            mpSITECARD.Text = mlREADER("SiteCardID") & ""
            mpSITEDESC.Text = mlREADER("SiteCardName") & ""
            mpLOCATION.Text = mlREADER("Location") & ""

            Try
                mpDEPT.SelectedValue = mlREADER("DeptID") & ""
            Catch ex As Exception
                mpDEPT.Items.Add(mlREADER("DeptID") & "")
            End Try

            mpPERIOD.Text = mlREADER("BVMonth") & ""
            mpDESC.Text = mlREADER("Description") & ""

            txADDR.Text = mlREADER("Do_Address") & ""
            txCITY.Text = mlREADER("Do_City") & ""
            Try
                ddSTATE.SelectedValue = mlREADER("Do_State") & ""
            Catch ex As Exception
                ddSTATE.Items.Add(mlREADER("Do_State") & "")
            End Try

            Try
                ddCOUNTRY.SelectedValue = mlREADER("Do_Country") & ""
            Catch ex As Exception
                ddCOUNTRY.Items.Add(mlREADER("Do_Country") & "")
            End Try

            Try
                ddDEPTCODE.SelectedValue = mlREADER("DeptCode") & ""
            Catch ex As Exception
                ddDEPTCODE.Items.Add(mlREADER("DeptCode") & "")
            End Try

            txZIP.Text = mlREADER("Do_Zip") & ""
            txPHONE1.Text = mlREADER("DO_Phone1") & ""
            txPHONE1_PIC.Text = mlREADER("PIC_ContactNo") & ""

            hfPOSTINGUSERID1.Value = mlREADER("PostingUserID1") & ""
            hfPOSTINGUSERID2.Value = mlREADER("PostingUserID2") & ""
            hfPOSTINGUSERID3.Value = mlREADER("PostingUserID3") & ""
            hfPOSTINGUSERID4.Value = mlREADER("PostingUserID4") & ""
            hfPOSTINGUSERID5.Value = mlREADER("PostingUserID5") & ""
            hfPOSTINGNAME1.Value = mlREADER("PostingName1") & ""
            hfPOSTINGNAME2.Value = mlREADER("PostingName2") & ""
            hfPOSTINGNAME3.Value = mlREADER("PostingName3") & ""
            hfPOSTINGNAME4.Value = mlREADER("PostingName4") & ""
            hfPOSTINGNAME5.Value = mlREADER("PostingName5") & ""
            hfPOSTINGDATE1.Value = mlREADER("PostingDate1") & ""
            hfPOSTINGDATE2.Value = mlREADER("PostingDate2") & ""
            hfPOSTINGDATE3.Value = mlREADER("PostingDate3") & ""
            hfPOSTINGDATE4.Value = mlREADER("PostingDate4") & ""
            hfPOSTINGDATE5.Value = mlREADER("PostingDate5") & ""
            hfRECORDSTATUS.Value = mlREADER("RecordStatus") & ""

            ' Added by Rafi (2014-11-25)           
            'ddlPeriodeMR.SelectedItem.Text = mlREADER("BVMonth").ToString()
            lbDESCRIPTION.Text = mlREADER("Keterangan").ToString()

            For n As Integer = 0 To ddlPeriodeMR.Items.Count - 1
                If ddlPeriodeMR.Items(n).Value = Left(mlREADER("BVMonth").ToString(), 2) Then
                    ddlPeriodeMR.SelectedIndex = n
                    Exit For
                End If
            Next

        End If
    End Sub

    Sub RetrieveFieldsDetail()
        mlSql_2 = "SELECT DocNo,DocDate as Date,MRType,SiteCardID,SiteCardName,BVMonth as Period, Do_Address as Delivery_Address,Do_City AS City,RecordStatus as MRStatus,PostingUserID1 AS CreateID,PostingName1 AS Name" & _
                " FROM AP_MR_REQUESTHR  WHERE ParentCode = '" & mlFUNCTIONPARAMETER & "' AND " & _
                " (" & _
                " PostingUserID1 = '" & Session("mgUSERID") & "' OR PostingUserID2 = '" & Session("mgUSERID") & "' OR PostingUserID3 = '" & Session("mgUSERID") & "'" & _
                " OR SiteCardID IN" & _
                " (" & _
                " SELECT SiteCardID FROM OP_USER_SITE WHERE UserID = '" & Trim(Session("mgUSERID")) & "' AND RecordStatus='New' " & _
                " )" & _
                " )" & _
                " AND (RecordStatus<>'New' AND RecordStatus<>'Post' AND RecordStatus<>'Delete')" & _
                " ORDER BY DocNo"
        mlREADER2 = mlOBJGS.DbRecordset(mlSql_2, "PB", "ISSP3")
        mlDATAGRID.DataSource = mlREADER2
        mlDATAGRID.DataBind()
    End Sub

    Sub DeleteRecord()
        If CheckRecordForEditing(Session("mgUSERID"), mlKEY2, mlKEY3) = False Then
            Exit Sub
        End If

        mlSPTYPE = "Delete"
        Try
            Sql_1(mlSPTYPE, mlKEY)
        Catch ex As Exception
        End Try

        mlSYSCODE.Value = ""
        RetrieveFields()
        RetrieveFieldsDetail()
    End Sub

    Sub NewRecord()
        mlOBJGS.mgNEW = True
        mlOBJGS.mgEDIT = False
        EnableCancel()
        ClearFields()
        LoadComboData()
        mpDOCUMENTNO.Text = "--AUTONUMBER--"
        mlOBJPJ.SetTextBox(True, mpDOCUMENTNO)
    End Sub

    Sub EditRecord()
        mlMESSAGE.Text = ""

        If CheckRecordForEditing(Session("mgUSERID"), mlKEY2, mlKEY3) = False Then
            Exit Sub
        End If

        mlOBJGS.mgNEW = False
        mlOBJGS.mgEDIT = True
        ClearFields()
        LoadComboData()
        EnableCancel()
        mlOBJPJ.SetTextBox(True, mpDOCUMENTNO)

        RetrieveFields()
        RetrieveFieldsDetail2()
        RetrieveFieldsDetail4()
        FillDetail(mlKEY)

        pnNOTEMP.Visible = False
        pnFILL.Visible = True
        pnGRID2.Visible = True
        pnGRID3.Visible = True
        VisiblePrice()
    End Sub

    Function CheckRecordForEditing(ByVal mpUSERID As String, ByVal mpSITECARDID As String, ByVal mpSTATUS As String) As Boolean
        CheckRecordForEditing = True

        Dim mlUSERLEVEL As String
        mlUSERLEVEL = Left(mlOBJPJ.ISS_MR_UserLevel(mpUSERID, mpSITECARDID), 1)
        If mlUSERLEVEL = "0" Then
            mlMESSAGE.Text = " <br /> Site Card " & mpSITECARD.Text & " belum diberikan Akses kepada Anda, silahkan hub Head Office (HO)"
            CheckRecordForEditing = False
        ElseIf Left(UCase(mpSTATUS), 4) = "WAIT" Then
            If CInt(Right(mpSTATUS, 1)) > CInt(mlUSERLEVEL) Then
                mlMESSAGE.Text = " <br /> Site Card " & mpSITECARD.Text & " telah melewati level otorisasi akses Anda, silahkan hub atasan Anda"
                CheckRecordForEditing = False
            End If
        End If

        If mpSTATUS = "New" Or mpSTATUS = "Post" Then
            mlMESSAGE.Text = "Record telah dikunci untuk di Edit"
            CheckRecordForEditing = False
        End If
    End Function

    Private Sub EnableCancel()
        btNewRecord.Visible = False
        btSaveRecord.Visible = True
        pnFILL.Visible = False
        pnGRID.Visible = False
        pnFILTER.Visible = True
        pnGRID2.Visible = False
        pnGRID3.Visible = False
        pnNOTEMP.Visible = False

        btDOCDATE.Visible = False
        btCALENDAR1.Visible = False
        btCALENDAR2.Visible = False
        btPERIOD.Visible = False
        btSITECARD.Visible = True
        mlOBJPJ.SetTextBox(False, mpDOCUMENTNO)
        mlOBJPJ.SetTextBox(True, mpDOCDATE)
        mlOBJPJ.SetTextBox(False, mpSITECARD)
        mlOBJPJ.SetTextBox(False, mpLOCATION)
        mpDEPT.Enabled = True
        mlOBJPJ.SetTextBox(False, mpPERIOD)
        mlOBJPJ.SetTextBox(False, mpDESC)
        mpLOCSAVE.Enabled = True
        mlOBJPJ.SetTextBox(False, txPERCENTAGE)

        mlOBJPJ.SetTextBox(False, txADDR)
        mlOBJPJ.SetTextBox(False, txCITY)
        ddSTATE.Enabled = True
        ddCOUNTRY.Enabled = True
        mlOBJPJ.SetTextBox(False, txZIP)
        mlOBJPJ.SetTextBox(False, txPHONE1)
        mlOBJPJ.SetTextBox(False, txPHONE1_PIC)
        ddDEPTCODE.Enabled = True

        TC1.VISIBLE = False
        tr2.visible = False
    End Sub

    Private Sub DisableCancel()
        btNewRecord.Visible = True
        btSaveRecord.Visible = False
        pnFILL.Visible = False
        pnGRID.Visible = True
        pnFILTER.Visible = False
        pnGRID2.Visible = False
        pnGRID3.Visible = False
        pnNOTEMP.Visible = False

        btDOCDATE.Visible = False
        btCALENDAR1.Visible = False
        btCALENDAR2.Visible = False
        btPERIOD.Visible = False
        btSITECARD.Visible = False
        mlOBJPJ.SetTextBox(True, mpDOCUMENTNO)
        mlOBJPJ.SetTextBox(True, mpDOCDATE)
        mlOBJPJ.SetTextBox(True, mpSITECARD)
        mlOBJPJ.SetTextBox(True, mpLOCATION)
        mpDEPT.Enabled = False
        mlOBJPJ.SetTextBox(True, mpPERIOD)
        mlOBJPJ.SetTextBox(True, mpDESC)
        mpLOCSAVE.Enabled = False
        mlOBJPJ.SetTextBox(True, txPERCENTAGE)

        mlOBJPJ.SetTextBox(True, txADDR)
        mlOBJPJ.SetTextBox(True, txCITY)
        ddSTATE.Enabled = False
        ddCOUNTRY.Enabled = False
        mlOBJPJ.SetTextBox(True, txZIP)
        mlOBJPJ.SetTextBox(True, txPHONE1)
        mlOBJPJ.SetTextBox(True, txPHONE1_PIC)
        ddDEPTCODE.Enabled = False

        TC1.VISIBLE = True
    End Sub

    Private Sub ReadOnlyMode()
        Dim mlDG As DataGridItem
        For Each mlDG In mlDATAGRID2.Items
            Dim mlQTY As TextBox = CType(mlDG.FindControl("mpQTY"), TextBox)
            Dim mlBAL2 As TextBox = CType(mlDG.FindControl("mpBAL"), TextBox)
            Dim mlSIZE2 As Label = CType(mlDG.FindControl("mpSIZE"), Label)
            Dim mlDESCDT2 As TextBox = CType(mlDG.FindControl("mpDESCDT"), TextBox)
            mlOBJPJ.SetTextBox(True, mlQTY)
            mlOBJPJ.SetTextBox(True, mlBAL2)
            mlOBJPJ.SetTextBox(True, mlDESCDT2)
        Next mlDG

        Dim mlDG2 As DataGridItem
        For Each mlDG2 In mlDATAGRID3.Items
            Dim mlQTY As TextBox = CType(mlDG2.FindControl("mpQTY"), TextBox)
            mlOBJPJ.SetTextBox(True, mlQTY)
        Next mlDG2
    End Sub

    Sub FillDetail(ByVal mpDOCNO As String)
        Dim mlCODEID As String

        mlSql_2 = "SELECT * FROM AP_MR_REQUESTDT WHERE DocNo='" & mpDOCNO & "'"
        mlREADER2 = mlOBJGS.DbRecordset(mlSql_2, "PB", "ISSP3")
        While mlREADER2.Read
            Dim mlDG As DataGridItem
            For Each mlDG In mlDATAGRID2.Items
                mlCODEID = mlDG.Cells("2").Text
                If mlREADER2("ItemKey") = mlCODEID Then
                    Dim mlQTY As TextBox = CType(mlDG.FindControl("mpQTY"), TextBox)
                    Dim mlBAL2 As TextBox = CType(mlDG.FindControl("mpBAL"), TextBox)
                    Dim mlSIZE2 As Label = CType(mlDG.FindControl("mpSIZE"), Label)
                    Dim mlDESCDT2 As TextBox = CType(mlDG.FindControl("mpDESCDT"), TextBox)
                    mlQTY.Text = mlREADER2("Qty") & ""
                    mlBAL2.Text = mlREADER2("Qty_Bal") & ""
                    mlSIZE2.Text = mlREADER2("RequestDesc") & ""
                    mlDESCDT2.Text = mlREADER2("Description2") & ""
                End If
            Next mlDG
        End While

        Dim mlCODEID2 As String
        mlSql_2 = "SELECT * FROM AP_MR_REQUESTDT2 WHERE DocNo='" & mpDOCNO & "'"
        mlREADER2 = mlOBJGS.DbRecordset(mlSql_2, "PB", "ISSP3")
        While mlREADER2.Read
            Dim mlDG2 As DataGridItem
            For Each mlDG2 In mlDATAGRID3.Items
                mlCODEID = mlDG2.Cells("1").Text
                mlCODEID2 = mlDG2.Cells("3").Text
                If mlREADER2("ItemKey") = mlCODEID And mlREADER2("fsize") = mlCODEID2 Then
                    Dim mlQTY As TextBox = CType(mlDG2.FindControl("mpQTY"), TextBox)
                    mlQTY.Text = mlREADER2("Qty") & ""
                End If
            Next mlDG2
        End While
    End Sub

    Sub ClearFields()
        mpDOCUMENTNO.Text = ""
        mpDOCDATE.Text = mlCURRENTDATE
        mpDESC.Text = ""
        mpSITECARD.Text = ""
        mpSITEDESC.Text = ""
        mpLOCATION.Text = ""
        ''mpPERIOD.Text = mlCURRENTBVMONTH
        mpDESC.Text = ""
        mlLINKDOC.Text = ""
        mlMESSAGE.Text = ""

        txADDR.Text = ""
        txCITY.Text = ""
        txZIP.Text = ""
        txPHONE1.Text = ""
        txPHONE1_PIC.Text = ""

        hfPOSTINGUSERID1.Value = ""
        hfPOSTINGUSERID2.Value = ""
        hfPOSTINGUSERID3.Value = ""
        hfPOSTINGUSERID4.Value = ""
        hfPOSTINGUSERID5.Value = ""
        hfPOSTINGNAME1.Value = ""
        hfPOSTINGNAME2.Value = ""
        hfPOSTINGNAME3.Value = ""
        hfPOSTINGNAME4.Value = ""
        hfPOSTINGNAME5.Value = ""
        hfPOSTINGDATE1.Value = ""
        hfPOSTINGDATE2.Value = ""
        hfPOSTINGDATE3.Value = ""
        hfPOSTINGDATE4.Value = ""
        hfPOSTINGDATE5.Value = ""
        hfRECORDSTATUS.Value = ""

        lbITEMCART.Text = ""
        lbITEMCART2.Value = ""
    End Sub

    Sub LoadComboData2()
        mpMR_TEMPLATE.Items.Clear()
        mpMR_TEMPLATE.Items.Add("Pilih")
        mpMR_TEMPLATE.Items.Add("No Template")
        mlSQLTEMP = "SELECT * FROM XM_UNIVERSALLOOKUPLIN WHERE UniversalID='MR_FORM' ORDER BY LinCode"
        mlRSTEMP = mlOBJGS.DbRecordset(mlSQLTEMP, "PB", "ISS")
        While mlRSTEMP.Read
            mpMR_TEMPLATE.Items.Add(Trim(mlRSTEMP("LinCode")) & "-" & mlRSTEMP("Description"))
        End While
    End Sub

    Sub LoadComboData()

        mpDEPT.Items.Clear()
        mpDEPT.Items.Add("Pilih")
        mlSQLTEMP = "SELECT DISTINCT Name FROM  [" & mlCOMPANYTABLENAME & "Resource] ORDER BY Name"
        mlRSTEMP = mlOBJGS.DbRecordset(mlSQLTEMP, "PB", mlCOMPANYID)
        While mlRSTEMP.Read
            mpDEPT.Items.Add(Trim(mlRSTEMP("Name")))
        End While
        mpDEPT.Items.Add("Lainnya")

        ddDEPTCODE.Items.Clear()
        ddDEPTCODE.Items.Add("Pilih")
        mlSQLTEMP = "SELECT * FROM  [" & mlCOMPANYTABLENAME & "Dimension Value] WHERE [DIMENSION CODE]='DEPT' ORDER BY NAME"
        mlRSTEMP = mlOBJGS.DbRecordset(mlSQLTEMP, "PB", mlCOMPANYID)
        While mlRSTEMP.Read
            ddDEPTCODE.Items.Add(Trim(mlRSTEMP("Code") & "-" & mlRSTEMP("Name")))
        End While
        mpDEPT.Items.Add("Lainnya")

        ddSTATE.Items.Add("Pilih")
        mlSQL = "SELECT LinCode,Description FROM XM_UNIVERSALLOOKUPLIN WHERE UniversalID = 'PROPINSI'"
        mlREADER = mlOBJGS.DbRecordset(mlSQL)
        While mlREADER.Read
            ddSTATE.Items.Add(Trim(mlREADER("LinCode")) & "-" & mlREADER("Description"))
        End While

        ddCOUNTRY.Items.Add("IDN-Indonesia")
        mlSQL = "SELECT LinCode,Description FROM XM_UNIVERSALLOOKUPLIN WHERE UniversalID = 'NEGARA'"
        mlREADER = mlOBJGS.DbRecordset(mlSQL)
        While mlREADER.Read
            ddCOUNTRY.Items.Add(Trim(mlREADER("LinCode")) & "-" & mlREADER("Description"))
        End While

        
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
            mlKEY = mpDOCUMENTNO.Text
            Sql_1(mlSPTYPE, mlKEY)
        Catch ex As Exception
        End Try

        mlSYSCODE.Value = ""
        ClearFields()
        DisableCancel()
        RetrieveFieldsDetail()

        mlMESSAGE.Text = mlMESSAGE.Text & "<br>" & " MR Anda Adalah " & "<a href=ap_doc_mr.aspx?mpID=" & mlKEY & ">" & mlKEY & " (click to view) " & "</a>"
        mlLINKDOC.Text = "<font Color=blue> Click to melihat MR Document Anda </font>"
        mlLINKDOC.NavigateUrl = ""
        mlLINKDOC.Attributes.Add("onClick", "window.open('ap_doc_mr.aspx?mpID=" & mlKEY & "','','');")
    End Sub

    Protected Sub btSUBMITTEMPLATE_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btSUBMITTEMPLATE.Click
        Dim mlTEMPLATEID As String
        mlTEMPLATEID = Trim(mlOBJGF.GetStringAtPosition(mpMR_TEMPLATE.Text, 0, "-"))
        If mlTEMPLATEID = "No Template" Then
            mlMESSAGE.Text = "Fasilitas No Template saat ini sedang dinon aktifkan"
            Exit Sub
        End If

        Try
            RetrieveFieldsDetail2()
            btSaveRecord.Visible = True
            NewRecord()
            pnNOTEMP.Visible = False
            pnFILL.Visible = True
            pnGRID2.Visible = True
            VisiblePrice()

            RetrieveFieldsDetail4()
            pnGRID3.Visible = True

        Catch ex As Exception
        End Try
    End Sub

    Protected Sub btITEMCART_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btITEMCART.Click
        RetrieveFieldsDetail2()
    End Sub

    Sub RetrieveFieldsDetail2()
        Dim mlTEMPLATEID As String

        Dim mlLOOP As Boolean
        Dim mlLINE As String
        Dim mlITEMKEY2 As String
        Dim mlITEMDESC2 As String
        Dim mlFIRST As Boolean

        mlTEMPLATEID = Trim(mlOBJGF.GetStringAtPosition(mpMR_TEMPLATE.Text, 0, "-"))
        Try
            Select Case mlTEMPLATEID
                Case "No Template"
                    If lbITEMCART2.Value = "" Then
                        EnableCancel()
                        pnNOTEMP.Visible = True
                        Exit Sub
                    End If

                    mlI = 0
                    mlSql_2 = ""
                    mlFIRST = True
                    mlLOOP = True
                    Do While mlLOOP
                        mlLINE = mlOBJGF.GetStringAtPosition(lbITEMCART2.Value, mlI, "#")
                        If mlLINE = "" Then
                            mlLOOP = False
                        Else
                            mlITEMKEY2 = mlOBJGF.GetStringAtPosition(mlLINE, 0, "-")
                            mlITEMDESC2 = mlOBJGF.GetStringAtPosition(mlLINE, 1, "-")
                            If mlFIRST = False Then
                                mlSql_2 = mlSql_2 & " UNION ALL "
                            Else
                                mlFIRST = False
                            End If
                            mlSql_2 = mlSql_2 & "SELECT '' as Linno, '" & mlITEMKEY2 & "' as Itemkey,'" & mlITEMDESC2 & "' AS Description,'' AS Uom, '0' as UnitPrice, '0' as TotalPrice "
                            mlI = mlI + 1
                        End If
                    Loop

                    mlREADER2 = mlOBJGS.DbRecordset(mlSql_2, "PB", "ISSP3")
                    mlDATAGRID2.DataSource = mlREADER2
                    mlDATAGRID2.DataBind()
                    mlOBJGS.CloseFile(mlREADER2)

                Case Else
                    'mlSql_2 = "SELECT '' as Linno, MRDT.Itemkey,MRDT.Description,Uom, '0' as UnitPrice, '0' as TotalPrice " & _
                    '        " FROM AP_MR_TEMPLATEDT MRDT " & _
                    '        " WHERE MRDT.DocNo='" & mlTEMPLATEID & "'" & _
                    '        " ORDER BY MRDT.DocNo,MRDT.Description"

                    mlSql_2 = "SELECT distinct '' as Linno, MRDT.Itemkey,MRDT.Description,Uom, '0' as UnitPrice, '0' as TotalPrice, " & _
                            "         CASE WHEN INM.ITEMKEY IS NULL THEN 0 ELSE 1 END AS FlagView " & _
                            " FROM AP_MR_TEMPLATEDT MRDT " & _
                            " left join IN_INMAST_ADDINFO INM " & _
                            "   on mrdt.itemkey = iNM.ItemKey " & _
                            " WHERE MRDT.DocNo='" & mlTEMPLATEID & "'" 
                            '" ORDER BY MRDT.DocNo,MRDT.Description"
                    mlREADER2 = mlOBJGS.DbRecordset(mlSql_2, "PB", "ISSP3")
                    mlDATAGRID2.DataSource = mlREADER2
                    mlDATAGRID2.DataBind()
                    mlOBJGS.CloseFile(mlREADER2)
            End Select

        Catch ex As Exception
        End Try
    End Sub

    Sub RetrieveFieldsDetail21()
        Dim mlTEMPLATEID As String
        mlTEMPLATEID = Trim(mlOBJGF.GetStringAtPosition(mpMR_TEMPLATE.Text, 0, "-"))
        Try
            Select Case mlTEMPLATEID
                Case "No Template"
                Case Else
            End Select

            mlSql_2 = "SELECT Linno, Itemkey, Description,Uom, UnitPrice,TotalPrice " & _
                            " FROM AP_MR_REQUESTDT MRDT WHERE DocNo='" & mlKEY & "'" & _
                            " ORDER BY MRDT.DocNo,MRDT.Description"
            mlREADER2 = mlOBJGS.DbRecordset(mlSql_2, "PB", "ISSP3")
            mlDATAGRID2.DataSource = mlREADER2
            mlDATAGRID2.DataBind()
            mlOBJGS.CloseFile(mlREADER2)

        Catch ex As Exception
        End Try
    End Sub

    Sub RetrieveFieldsDetail4()
        Dim mlTEMPLATEID As String
        mlTEMPLATEID = Trim(mlOBJGF.GetStringAtPosition(mpMR_TEMPLATE.Text, 0, "-"))

        Try
            mlSql_2 = "SELECT distinct '' as Linno,LinCode as ItemKey, Description, AdditionalDescription1 as fsize" & _
                   " FROM XM_UNIVERSALLOOKUPLIN WHERE UniversalID='Item_Size' AND AdditionalDescription2='" & ddENTITY.Text & "'" & _
                   " AND LinCode IN " & _
                   " (" & _
                   " SELECT MRDT.Itemkey FROM AP_MR_TEMPLATEDT MRDT WHERE MRDT.DocNo='" & mlTEMPLATEID & "'" & _
                   " )" 
                   '" ORDER BY LinCode"
            mlREADER2 = mlOBJGS.DbRecordset(mlSql_2, "PB", "ISSP3")
            mlDATAGRID3.DataSource = mlREADER2
            mlDATAGRID3.DataBind()
            mlOBJGS.CloseFile(mlREADER2)
        Catch ex As Exception
        End Try
    End Sub

    Sub RetrieveFieldsDetail41()
        Dim mlTEMPLATEID As String
        mlTEMPLATEID = Trim(mlOBJGF.GetStringAtPosition(mpMR_TEMPLATE.Text, 0, "-"))
        Try
            mlSql_2 = "SELECT Linno, Itemkey, Description,fSize, Qty " & _
                            " FROM AP_MR_REQUESTDT2 MRDT WHERE DocNo='" & mlKEY & "'" & _
                            " ORDER BY MRDT.DocNo,MRDT.Description"
            mlREADER2 = mlOBJGS.DbRecordset(mlSql_2, "PB", "ISSP3")
            mlDATAGRID3.DataSource = mlREADER2
            mlDATAGRID3.DataBind()
            mlOBJGS.CloseFile(mlREADER2)
        Catch ex As Exception
        End Try
    End Sub

    Sub VisiblePrice()
        'tbTABLE1.Visible = False
        '        mlDATAGRID2.Columns(6).Visible = False
        '        mlDATAGRID2.Columns(7).Visible = False

    End Sub

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
            mpSITECARD.Text = CType(e.Item.Cells(0).Controls(0), LinkButton).Text
            mpSITEDESC.Text = CType(e.Item.Cells(1).Controls(0), LinkButton).Text
            mlDATAGRIDSITECARD.DataSource = Nothing
            mlDATAGRIDSITECARD.DataBind()
            pnSEARCHSITECARD.Visible = False
            SiteLocation()
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
        mlSQLTEMP = "SELECT SiteCardID,SiteCardName FROM OP_USER_SITE WHERE SiteCardID LIKE  '" & mpSITECARD.Text & "' AND EntityID = '" & Trim(ddENTITY.Text) & "' "
        mlRSTEMP = mlOBJGS.DbRecordset(mlSQLTEMP, "PB", "ISSP3")
        If mlRSTEMP.HasRows Then
            mlRSTEMP.Read()
            mpSITEDESC.Text = mlRSTEMP("SiteCardName") & ""
            SiteLocation()
        End If
        mlOBJGS.CloseFile(mlRSTEMP)
    End Sub

    ''
    Sub SiteLocation()
        Dim mlMRLEVEL As Byte
        Try
            mlMRLEVEL = Left(mlOBJPJ.ISS_MR_UserLevel(Trim(Session("mgUSERID")), mpSITECARD.Text), 1)
            If mlMRLEVEL = "0" Then
                mlMESSAGE.Text = " Site Card " & mpSITECARD.Text & " belum diberikan Akses kepada Anda, silahkan hub Head Office (HO)"
                mpSITECARD.Text = ""
                mpSITEDESC.Text = ""
                Exit Sub
            End If

            If mlMRLEVEL >= "2" Then
                tr2.Visible = True
            End If

            mpLOCATION.Text = mlOBJPJ.ISS_XMGeneralLostFocus("SITECARD_ADDR_ALL", Trim(mpSITECARD.Text))

            If txADDR.Text = "" Then
                mlSQLTEMP = "SELECT * FROM XM_ADDRESSBOOK WHERE AddressCode='ARSHIP' AND AddressKey='" & Trim(mpSITECARD.Text) & "'"
                mlRSTEMP = mlOBJGS.DbRecordset(mlSQLTEMP)
                If mlRSTEMP.HasRows Then
                    mlRSTEMP.Read()
                    txADDR.Text = mlRSTEMP("Address") & ""
                    txCITY.Text = mlRSTEMP("City") & ""
                    If mlRSTEMP("State") <> "" Then ddSTATE.Items.Add(mlRSTEMP("State"))
                    txPHONE1.Text = mlRSTEMP("Phone1") & " " & mlRSTEMP("Phone2") & ""
                    txPHONE1_PIC.Text = mlRSTEMP("DefaultPIC") & " " & mlRSTEMP("Mobile1") & " " & mlRSTEMP("Mobile2")

                    txADDR.Text = Replace(txADDR.Text, "&nbsp;", "")
                    txCITY.Text = Replace(txCITY.Text, "&nbsp;", "")
                    txPHONE1.Text = Replace(txPHONE1.Text, "&nbsp;", "")
                    txPHONE1_PIC.Text = Replace(txPHONE1_PIC.Text, "&nbsp;", "")
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Sub CancelOperation()
        mlMESSAGE.Text = ""
        ClearFields()
        DisableCancel()
        RetrieveFieldsDetail()
    End Sub

    Function ValidateForm() As String

        ValidateForm = ""
        mpPERIOD.Text = ddlPeriodeMR.SelectedItem.Text

        btSITECARD_Click(Nothing, Nothing)

        If mpSITECARD.Text = "" Then
            ValidateForm = ValidateForm & " <br /> Site Card tidak boleh kosong"
        End If

        If mpSITEDESC.Text = "" Then
            ValidateForm = ValidateForm & " <br /> Nama Site Card tidak boleh kosong"
        End If

        If txADDR.Text = "" Then
            ValidateForm = ValidateForm & " <br /> Alamat untuk Pengiriman tidak boleh kosong"
        End If

        If txCITY.Text = "" Then
            ValidateForm = ValidateForm & " <br /> Kota untuk Pengiriman tidak boleh kosong"
        End If

        If ddSTATE.Text = "Pilih" Then
            ValidateForm = ValidateForm & " <br /> Propinsi untuk Pengiriman tidak boleh kosong"
        End If

        If mpPERIOD.Text = "" Then
            ValidateForm = ValidateForm & " <br /> Periode MR tidak boleh kosong"
        Else
            Dim mlSTR1 As String
            Dim mlSTR2 As String
            mlSTR1 = mlOBJGF.GetStringAtPosition(mpPERIOD.Text, 0, "/")
            mlSTR2 = mlOBJGF.GetStringAtPosition(mpPERIOD.Text, 1, "/")

            

            Try
                Dim mlSTR(2) As String
                mlSTR = mpPERIOD.Text.Split("/")

                If Len(mlSTR(0)) > "2" Or Len(mlSTR(1)) > "4" Then
                    ValidateForm = ValidateForm & " <br /> Format Periode MR adalah MM/YYYY = 2 digit Bulan + 4 digit Tahun tanpa spasi"
                End If
            Catch ex As Exception
            End Try
            
            If IsNumeric(mlSTR1) = False Or IsNumeric(mlSTR2) = False Then
                ValidateForm = ValidateForm & " <br /> Format Periode MR adalah MM/YYYY = 2 digit Bulan + 4 digit Tahun, contoh MR Agustus 2013 diisi dengan 08/2013"
            End If

            If Len(mpPERIOD.Text) > "7" Then
                ValidateForm = ValidateForm & " <br /> Format Periode MR adalah MM/YYYY = 2 digit Bulan + 4 digit Tahun (7 digit) "
            End If
        End If

        If Left(mlOBJPJ.ISS_MR_UserLevel(Trim(Session("mgUSERID")), mpSITECARD.Text), 1) = "0" Then
            ValidateForm = ValidateForm & " <br /> Site Card " & mpSITECARD.Text & " belum diberikan Akses kepada Anda, silahkan hub Head Office (HO)"
        End If

        Dim mlDG As DataGridItem
        Dim mlDG2 As DataGridItem
        Dim mlQTY2DT As Double
        Dim mlSIZEHR As String
        Dim mlHAVINGDETAILS As Boolean
        Dim mlANYDETAIL As Boolean

        mlHAVINGDETAILS = False
        mlANYDETAIL = False
        For Each mlDG In mlDATAGRID2.Items
            Dim mlQTY As TextBox = CType(mlDG.FindControl("mpQTY"), TextBox)
            Dim mlSIZE2 As Label = CType(mlDG.FindControl("mpSIZE"), Label)

            If IsNumeric(mlQTY.Text) = True Then
                If mlHAVINGDETAILS = False Then mlHAVINGDETAILS = True

                mlQTY2DT = 0
                mlSIZEHR = ""
                For Each mlDG2 In mlDATAGRID3.Items
                    If ucase(mlDG2.Cells("1").Text) = ucase(mlDG.Cells("2").Text) Then
                        mlANYDETAIL = True
                        Dim mlQTY2 As TextBox = CType(mlDG2.FindControl("mpQTY"), TextBox)

                        If IsNumeric(mlQTY2.Text) = True Then
                            mlSIZEHR = mlSIZEHR & IIf(mlSIZEHR = "", "", ", ") & mlDG2.Cells("3").Text & "=" & mlQTY2.Text
                            mlQTY2DT = mlQTY2DT + mlQTY2.Text
                        End If
                    End If
                Next mlDG2

                mlSIZE2.Text = mlSIZEHR

                If mlANYDETAIL = True Then
                    If mlQTY2DT <> mlQTY.Text Then
                        ValidateForm = ValidateForm & " <br /> Total Qty Ukuran untuk Kode " & mlDG.Cells("2").Text & " tidak sama = " & mlQTY.Text & " # " & mlQTY2DT
                    End If
                    mlANYDETAIL = False
                End If
            End If
        Next mlDG

        If mlHAVINGDETAILS = False Then
            ValidateForm = ValidateForm & " <br /> Details tidak ditemukan"
        End If

    End Function

    Function GetUnitPrice(ByVal mpPITEMKEY As String) As Double
        Dim mlSQLUPRICE As String
        Dim mlRSUPRICE As OleDbDataReader

        GetUnitPrice = 0
        mlSQLUPRICE = "SELECT * FROM [" & mlCOMPANYTABLENAME & "Item] WHERE [No_] = '" & mpPITEMKEY & "' "
        mlRSUPRICE = mlOBJGS.DbRecordset(mlSQLUPRICE, "PB", mlCOMPANYID)
        If mlRSUPRICE.HasRows Then
            mlRSUPRICE.Read()
            GetUnitPrice = IIf(IsDBNull(mlRSUPRICE("Last Direct Cost")) = True, 0, mlRSUPRICE("Last Direct Cost"))
        End If
    End Function


    Protected Sub btITEMKEYADD_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btITEMKEYADD.Click
        If mpITEMDESC.Text <> "" Then
            lbITEMCART.Text = lbITEMCART.Text & IIf(mlOBJGF.IsNone(lbITEMCART.Text) = False, ",<br>", lbITEMCART.Text) & Trim(mpITEMKEY.Text) & "-" & Trim(mpITEMDESC.Text)
            lbITEMCART2.Value = lbITEMCART2.Value & IIf(mlOBJGF.IsNone(lbITEMCART2.Value) = False, "#", lbITEMCART2.Value) & Trim(mpITEMKEY.Text) & "-" & Trim(mpITEMDESC.Text)

            mpITEMKEY.Text = ""
            mpITEMDESC.Text = ""
            btITEMKEYADD.Visible = False
        End If
    End Sub

    Protected Sub btCLEARCART_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btCLEARCART.Click
        lbITEMCART.Text = ""
        mpITEMKEY.Text = ""
        mpITEMDESC.Text = ""
    End Sub

    Function FindUDocNo(ByVal mpDOCNO As String) As String
        FindUDocNo = ""
        mlSQLTEMP = "SELECT RecUDocNo FROM AP_MR_REQUESTHR_Log WHERE DocNo = '" & mpDOCNO & "' ORDER BY RecUDocNo Desc"
        mlRSTEMP = mlOBJGS.DbRecordset(mlSQLTEMP, "PB", "ISSP3")
        If mlRSTEMP.HasRows Then
            mlRSTEMP.Read()
            If mlOBJGF.IsNone(mlRSTEMP("RecUDocNo")) = True Then
                Return "1"
            Else
                Return mlRSTEMP("RecUDocNo") + 1
            End If
        Else
            Return "1"
        End If
    End Function

    Function Sql_2(ByVal mpSITECARDID As String, ByVal mpSITECARDDESC As String, ByVal mpSITEADDRESS As String, ByVal mpCITY As String, ByVal mpSTATE As String, ByVal mpCOUNTRY As String) As String
        mlSQLTEMP = "SELECT * FROM AR_SITECARD_ADDINFO WHERE SiteCardId = '" & mpSITECARDID & "'"
        mlRSTEMP = mlOBJGS.DbRecordset(mlSQLTEMP, "PB", "ISSP3")
        mlSQLTEMP = ""
        If mlRSTEMP.HasRows Then
            mlSQLTEMP = mlSQLTEMP & " UPDATE AR_SITECARD_ADDINFO SET Address = '" & mpSITEADDRESS & "'," & _
                    " City= '" & mpCITY & "', State= '" & mpSTATE & "',Country= '" & mpCOUNTRY & "'," & _
                    " RecUserID = '" & Session("mgUSERID") & "', RecDate = '" & mlOBJGF.FormatDate(Now) & "'" & _
                    " WHERE SiteCardID = '" & mpSITECARDID & "'"
        Else
            mlSQLTEMP = mlSQLTEMP & "INSERT INTO AR_SITECARD_ADDINFO (ParentCode,SiteCardID,Description,Address,City,State,Country," & _
                    " RecordStatus,RecUserID,RecDate) " & _
                    " VALUES ('','" & mpSITECARDID & "','" & mpSITECARDDESC & "','" & mpSITEADDRESS & "'," & _
                    " '" & mpCITY & "','" & mpSTATE & "','" & mpCOUNTRY & "'," & _
                    " 'New','" & Session("mgUSERID") & "','" & mlOBJGF.FormatDate(Now) & "')"
        End If
        Return mlSQLTEMP
    End Function

    Sub Sql_1(ByVal mpSTATUS As String, ByVal mpCODE As String)
        Dim mlSTATUS As String
        Dim mlNEWRECORD As Boolean
        Dim mlI As Byte
        Dim mlHAVINGDETAIL As Boolean
        Dim mlMRTYPE As String
        Dim mlMRDESCRIPTION As String
        Dim mlMRLINE As Integer

        Dim mlUNITPRICE As Double
        Dim mlTOTALUNITPRICE As Double
        Dim mlTOTALPRICE As Double
        Dim mlDEPT2 As String

        Dim mlSql_2 As String
        Dim mlUDOCNO As String

        Dim mlSC_STATE As String
        Dim mlSC_BRANCH As String
        Dim mlSC_BRANCHCODE As String
        Dim mlSC_BRANCHNAME As String

        Dim mlPOSTUSERID1 As String
        Dim mlPOSTUSERID2 As String
        Dim mlPOSTUSERID3 As String
        Dim mlPOSTUSERID4 As String
        Dim mlPOSTUSERID5 As String
        Dim mlPOSTUSERNAME1 As String
        Dim mlPOSTUSERNAME2 As String
        Dim mlPOSTUSERNAME3 As String
        Dim mlPOSTUSERNAME4 As String
        Dim mlPOSTUSERNAME5 As String
        Dim mlPOSTUSERDATE1 As Date
        Dim mlPOSTUSERDATE2 As Date
        Dim mlPOSTUSERDATE3 As Date
        Dim mlPOSTUSERDATE4 As Date
        Dim mlPOSTUSERDATE5 As Date
        Dim mlRECSTATUS As String

        Try
            mlMRTYPE = mlOBJGF.GetStringAtPosition(mpMR_TEMPLATE.Text, "0", "-")
            mlMRDESCRIPTION = mlOBJGF.GetStringAtPosition(mpMR_TEMPLATE.Text, "1", "-")
            mlNEWRECORD = False
            mlMRLINE = 0
            mlTOTALPRICE = 0
            mlUDOCNO = FindUDocNo(mlKEY)

            mlSC_STATE = ""
            mlSC_BRANCH = ""
            mlSC_BRANCHCODE = ""
            mlSC_BRANCHNAME = ""

            mlPOSTUSERID1 = hfPOSTINGUSERID1.Value
            mlPOSTUSERID2 = hfPOSTINGUSERID2.Value
            mlPOSTUSERID3 = hfPOSTINGUSERID3.Value
            mlPOSTUSERID4 = hfPOSTINGUSERID4.Value
            mlPOSTUSERID5 = hfPOSTINGUSERID5.Value
            mlPOSTUSERNAME1 = hfPOSTINGNAME1.Value
            mlPOSTUSERNAME2 = hfPOSTINGNAME2.Value
            mlPOSTUSERNAME3 = hfPOSTINGNAME3.Value
            mlPOSTUSERNAME4 = hfPOSTINGNAME4.Value
            mlPOSTUSERNAME5 = hfPOSTINGNAME5.Value
            If hfPOSTINGDATE1.Value <> "" Then mlPOSTUSERDATE1 = mlOBJGF.FormatDate(hfPOSTINGDATE1.Value)
            If hfPOSTINGDATE2.Value <> "" Then mlPOSTUSERDATE2 = mlOBJGF.FormatDate(hfPOSTINGDATE2.Value)
            If hfPOSTINGDATE3.Value <> "" Then mlPOSTUSERDATE3 = mlOBJGF.FormatDate(hfPOSTINGDATE3.Value)
            If hfPOSTINGDATE4.Value <> "" Then mlPOSTUSERDATE4 = mlOBJGF.FormatDate(hfPOSTINGDATE4.Value)
            If hfPOSTINGDATE5.Value <> "" Then mlPOSTUSERDATE5 = mlOBJGF.FormatDate(hfPOSTINGDATE5.Value)
            mlRECSTATUS = hfRECORDSTATUS.Value
            If mlRECSTATUS = "" Then mlRECSTATUS = "Wait1"

            Select Case mpSTATUS
                Case "Edit", "Delete"
                    mlSQL = ""
                    mlSQL = mlSQL & mlOBJPJ.ISS_MR_MREntry_ToLog(mlKEY, mpSTATUS, Session("mgUSERID"), mlUDOCNO)
            End Select

            Select Case mpSTATUS
                Case "New"
                    mlNEWRECORD = True
                    If mpDOCUMENTNO.Text = "--AUTONUMBER--" Then
                        mlKEY = mlOBJGS.GetModuleCounter("MR_Entry_" & mlFUNCTIONPARAMETER, "00000000")
                        mpDOCUMENTNO.Text = mlKEY
                    Else
                        mlKEY = Trim(mpDOCUMENTNO.Text)
                    End If

                Case "Edit"
                    mlSTATUS = "Edit"
                    mlNEWRECORD = True

                    mlSQL = mlSQL & " DELETE FROM AP_MR_REQUESTHR WHERE DocNo = '" & mlKEY & "';"
                    mlSQL = mlSQL & " DELETE FROM AP_MR_REQUESTDT WHERE DocNo = '" & mlKEY & "';"
                    mlSQL = mlSQL & " DELETE FROM AP_MR_REQUESTDT2 WHERE DocNo = '" & mlKEY & "';"

                Case "Delete"
                    mlSTATUS = "Delete"
                    mlSQL = mlSQL & " DELETE FROM AP_MR_REQUESTHR WHERE DocNo = '" & mlKEY & "';"
                    mlSQL = mlSQL & " DELETE FROM AP_MR_REQUESTDT WHERE DocNo = '" & mlKEY & "';"
                    mlSQL = mlSQL & " DELETE FROM AP_MR_REQUESTDT2 WHERE DocNo = '" & mlKEY & "';"
            End Select
            If mlOBJGF.IsNone(mlSQL) = False Then mlOBJGS.ExecuteQuery(mlSQL, "PB", "ISSP3")
            mlSQL = ""

	    Dim mlDG As DataGridItem
			
            mlDEPT2 = IIf(mpDEPT.Text = "Pilih", "", mpDEPT.Text)
            If mlNEWRECORD = True Then
                mlSQL = ""

                mlHAVINGDETAIL = False
                mlI = 0
                
                For Each mlDG In mlDATAGRID2.Items
                    Dim mlQTY As TextBox = CType(mlDG.FindControl("mpQTY"), TextBox)
                    Dim mlBAL2 As TextBox = CType(mlDG.FindControl("mpBAL"), TextBox)
                    Dim mlSIZE2 As Label = CType(mlDG.FindControl("mpSIZE"), Label)
                    Dim mlDESCDT2 As TextBox = CType(mlDG.FindControl("mpDESCDT"), TextBox)

                    If IsNumeric(mlQTY.Text) = True Then
                        If mlHAVINGDETAIL = False Then mlHAVINGDETAIL = True
                        mlI = mlI + 1

                        mlUNITPRICE = Replace(GetUnitPrice(mlDG.Cells("2").Text), ",", ".")                       
                        mlTOTALUNITPRICE = mlUNITPRICE * Replace(Trim(mlQTY.Text), ",", ".")
                        mlTOTALPRICE = Replace(mlTOTALPRICE + mlTOTALUNITPRICE, ",", ".")
                        mlMRLINE = mlMRLINE + 1

                        mlQTY.Text = Replace(mlQTY.Text, ",", ".")
                        mlBAL2.Text = Replace(mlBAL2.Text, ",", ".")
                        mlSIZE2.Text = Replace(mlSIZE2.Text, ",", ".")

                        mlSQL = mlSQL & "INSERT INTO AP_MR_REQUESTDT (DocNo,Linno,ItemKey,Description,Uom,Qty,QtyDO," & _
                            " UnitPoint,UnitPrice,TotalPoint,TotalPrice," & _
                            " Qty_Bal,RequestDesc,Description2)" & _
                            " VALUES (" & _
                            "'" & mlKEY & "','" & CDbl(mlI) & "','" & mlDG.Cells("2").Text & "','" & mlDG.Cells("3").Text & "','" & mlDG.Cells("4").Text & "', " & _
                            "'" & Trim(mlQTY.Text) & "','0'," & _
                            "'0','" & CDbl(mlUNITPRICE) & "','0','" & CDbl(mlTOTALUNITPRICE) & "'," & _
                            "'" & Trim(mlBAL2.Text) & "','" & Trim(mlSIZE2.Text) & "','" & Trim(mlDESCDT2.Text) & "'" & _
                            " );"
                    End If
                Next mlDG
            End If

            If mlHAVINGDETAIL = False Then
                mlMESSAGE.Text = "Transaksi Gagal diSimpan" & vbNewLine & "Detail Transaksi tidak ditemukan"
                CancelOperation()
                Exit Sub
            End If

            mlI = 0
            Dim mlDG2 As DataGridItem

            Dim mlSQLDT3 As String
            Dim mlDOCNO3 As String
            Dim mlITEMKEYDT3 As String
            Dim mlDESCDT3 As String
            Dim mlFIRST3 As Boolean
            Dim mlORIQTY3 As Double
            Dim mlORIITEMKEY3 As String
            Dim mlORISIZE As String
	    Dim mlITEMCODECHECK As Boolean
			
			
            mlDOCNO3 = ""
            mlITEMKEYDT3 = ""
            mlSQLDT3 = ""
            mlDESCDT3 = ""
            mlFIRST3 = True
	    mlITEMCODECHECK = false

            For Each mlDG2 In mlDATAGRID3.Items
                Dim mlQTY2 As TextBox = CType(mlDG2.FindControl("mpQTY"), TextBox)
                mlQTY2.Text = Replace(mlQTY2.Text, ",", ".")

		mlITEMCODECHECK = False

                If IsNumeric(mlQTY2.Text) = True Then
                    mlI = mlI + 1
                    mlQTY2.Text = Replace(mlQTY2.Text, ",", ".")

                    '''' Remarked by Rafi (2015-04-10) cause ItemCode di mlDATAGRID3 harus di cek dl di mlDATAGRID2, biar semua ItemCode yg dipilih SAMA...
                    ''''
                    ''mlSQL = mlSQL & "INSERT INTO AP_MR_REQUESTDT2 (DocNo,Linno,ItemKey,Description,fSize,Qty,QtyDO)" & _
                    ''                       " VALUES (" & _
                    ''                       "'" & mlKEY & "','" & CDbl(mlI) & "','" & mlDG2.Cells("1").Text & "','" & mlDG2.Cells("2").Text & "','" & mlDG2.Cells("3").Text & "', " & _
                    ''                       "'" & Trim(mlQTY2.Text) & "','0'" & _
                    ''                       " );"

                    ''mlDOCNO3 = mlKEY
                    ''mlORIITEMKEY3 = mlDG2.Cells("1").Text
                    ''mlORIQTY3 = Trim(mlQTY2.Text)
                    ''mlORISIZE = mlDG2.Cells("3").Text

                    ''If mlITEMKEYDT3 = mlORIITEMKEY3 Then
                    ''    mlDESCDT3 = mlDESCDT3 & IIf(mlOBJGF.IsNone(mlDESCDT3) = True, "", ", ") & mlORISIZE & "=" & mlORIQTY3
                    ''Else
                    ''    If mlFIRST3 = True Then
                    ''        mlFIRST3 = False
                    ''    Else
                    ''        mlSQLDT3 = mlSQLDT3 & "UPDATE AP_MR_REQUESTDT SET Description3='" & mlDESCDT3 & "' WHERE DocNo='" & mlDOCNO3 & "' AND ItemKey = '" & mlITEMKEYDT3 & "';"
                    ''    End If

                    ''    mlITEMKEYDT3 = mlORIITEMKEY3
                    ''    mlDESCDT3 = mlORISIZE & "=" & mlORIQTY3
                    ''End If


                    For Each mlDG In mlDATAGRID2.Items      '''' Looping Cek ItemCode dr mlDATAGRID2
                        If UCase(Trim(mlDG.Cells("2").Text)) = UCase(Trim(mlDG2.Cells("1").Text)) Then
                            mlITEMCODECHECK = True

                            mlSQL = mlSQL & "INSERT INTO AP_MR_REQUESTDT2 (DocNo,Linno,ItemKey,Description,fSize,Qty,QtyDO)" & _
                                           " VALUES (" & _
                                           "'" & mlKEY & "','" & CDbl(mlI) & "','" & mlDG2.Cells("1").Text & "','" & mlDG2.Cells("2").Text & "','" & mlDG2.Cells("3").Text & "', " & _
                                           "'" & Trim(mlQTY2.Text) & "','0'" & _
                                           " );"

                            mlDOCNO3 = mlKEY
                            mlORIITEMKEY3 = mlDG2.Cells("1").Text
                            mlORIQTY3 = Trim(mlQTY2.Text)
                            mlORISIZE = mlDG2.Cells("3").Text

                            If mlITEMKEYDT3 = mlORIITEMKEY3 Then
                                mlDESCDT3 = mlDESCDT3 & IIf(mlOBJGF.IsNone(mlDESCDT3) = True, "", ", ") & mlORISIZE & "=" & mlORIQTY3
                            Else
                                If mlFIRST3 = True Then
                                    mlFIRST3 = False
                                Else
                                    mlSQLDT3 = mlSQLDT3 & "UPDATE AP_MR_REQUESTDT SET Description3='" & mlDESCDT3 & "' WHERE DocNo='" & mlDOCNO3 & "' AND ItemKey = '" & mlITEMKEYDT3 & "';"
                                End If

                                mlITEMKEYDT3 = mlORIITEMKEY3
                                mlDESCDT3 = mlORISIZE & "=" & mlORIQTY3
                            End If
                        End If
                    Next mlDG
                    '''' Cek FlagItemCodeCheck, jika False berarti ada Item yang tidak sama dan proses tidak akan dilanjutkan...
                    If mlITEMCODECHECK = False Then
                        Exit For
                    End If
		
		Else
                    mlITEMCODECHECK = True		'''' jika tidak ada data yang dipilih di DATAGRID3, maka dianggap valid
                End If
            Next mlDG2
	
	    '''' cek jk di DATAGRID3 tdk ada datanya, maka dianggap valid
            If mlDATAGRID3.Items.Count <= 0 Then
                mlITEMCODECHECK = True
            End If

	   '''' Cek FlagItemCodeCheck, jika False berarti ada Item yang tidak sama dan proses tidak akan dilanjutkan...
            If mlITEMCODECHECK = False Then
                mlMESSAGE.Text = "Transaksi Gagal diSimpan" & vbNewLine & " Item Code yang dipilih tidak sesuai dengan Item Ukuran"
                CancelOperation()
                Exit Sub
            End If
			
            If mlITEMKEYDT3 = mlORIITEMKEY3 Then
            Else
                mlSQLDT3 = mlSQLDT3 & "UPDATE AP_MR_REQUESTDT SET Description3='" & mlDESCDT3 & "' WHERE DocNo='" & mlDOCNO3 & "' AND ItemKey = '" & mlITEMKEYDT3 & "';"
            End If
            mlSQL = mlSQL & mlSQLDT3

            mlSQLTEMP = "SELECT SC.[CustomerNo_],SC.[SiteCode],SC.[LineNo_],SC.Branch," & _
                " BC.[Branch Location], BC.Code,BC.[Branch Location], BC.Name " & _
                " FROM [" & mlCOMPANYTABLENAME & "CustServiceSite]  SC,  [" & mlCOMPANYTABLENAME & "Location] BC" & _
                " WHERE SC.Branch = BC.[Branch Location]" & _
                " AND  SC.[LineNo_] = '" & Trim(mpSITECARD.Text) & "'"
            mlRSTEMP = mlOBJGS.DbRecordset(mlSQLTEMP, "PB", mlCOMPANYID)
            If mlRSTEMP.HasRows Then
                mlRSTEMP.Read()
                mlSC_STATE = ""
                mlSC_BRANCH = mlRSTEMP("Branch Location") & ""
                mlSC_BRANCHCODE = mlRSTEMP("Code") & ""
                mlSC_BRANCHNAME = mlRSTEMP("Name") & ""
            End If


            Select Case mpSTATUS
                Case "New"
                    mlPOSTUSERID1 = Trim(Session("mgUSERID"))
                    mlPOSTUSERNAME1 = Trim(Session("mgNAME"))
                    mlPOSTUSERDATE1 = mlOBJGF.FormatDate(Now)
                Case Else
            End Select


            mpPERIOD.Text = ddlPeriodeMR.SelectedItem.Text

            ' Added by Rafi (25-11-2014), Field Keterangan di Table AP_MR_REQUESTHR supaya Data Keterangan bisa diSave

            mlSQL = mlSQL & " INSERT INTO AP_MR_REQUESTHR (ParentCode,MRType,MRDesciption,DocNo,DocDate,SiteCardID,SiteCardName," & _
                " Location,DeptID,BVMonth,Description,MRLine," & _
                " PercentageMR,TotalPoint,TotalAmount," & _
                " SC_State,SC_Branch,SC_BranchCode,SC_BranchName," & _
                " DeptCode,Do_Address,Do_City,Do_State,Do_Country,Do_Zip,DO_Phone1,PIC_ContactNo," & _
                " EntityID," & _
                " PostingUserID1,PostingName1,PostingDate1," & _
                " PostingUserID2,PostingName2,PostingDate2," & _
                " PostingUserID3,PostingName3,PostingDate3," & _
                " PostingUserID4,PostingName4,PostingDate4," & _
                " PostingUserID5,PostingName5,PostingDate5," & _
                " Keterangan, " & _
                " RecordStatus,RecUserID,RecDate)" & _
                " VALUES ( " & _
                " '" & mlFUNCTIONPARAMETER & "','" & mlMRTYPE & "','" & mlMRDESCRIPTION & "'," & _
                " '" & mlKEY & "'," & _
                " '" & mlOBJGF.FormatDate(Now) & "'," & _
                " '" & Trim(mpSITECARD.Text) & "','" & Trim(mpSITEDESC.Text) & "'," & _
                " '" & Trim(mpLOCATION.Text) & "'," & _
                " '" & Trim(mlDEPT2) & "','" & _
                Trim(mpPERIOD.Text) & "','" & _
                Trim(mpDESC.Text) & "'," & _
                " '" & mlMRLINE & "'," & _
                " '" & Trim(txPERCENTAGE.Text) & "','0','" & CDbl(mlTOTALPRICE) & "'," & _
                " '" & Trim(mlSC_STATE) & "','" & Trim(mlSC_BRANCH) & "','" & Trim(mlSC_BRANCHCODE) & "','" & Trim(mlSC_BRANCHNAME) & "'," & _
                " '" & Trim(ddDEPTCODE.Text) & "', '" & Trim(txADDR.Text) & "','" & Trim(txCITY.Text) & "','" & Trim(ddSTATE.Text) & "'," & _
                " '" & Trim(ddCOUNTRY.Text) & "','" & Trim(txZIP.Text) & "','" & Trim(txPHONE1.Text) & "','" & Trim(txPHONE1_PIC.Text) & "'," & _
                " '" & Trim(ddENTITY.Text) & "'," & _
                " '" & Trim(mlPOSTUSERID1) & "', '" & Trim(mlPOSTUSERNAME1) & "', '" & mlOBJGF.FormatDate(mlPOSTUSERDATE1) & "'," & _
                " '" & Trim(mlPOSTUSERID2) & "', '" & Trim(mlPOSTUSERNAME2) & "', '" & Trim(mlPOSTUSERDATE2) & "'," & _
                " '" & Trim(mlPOSTUSERID3) & "', '" & Trim(mlPOSTUSERNAME3) & "', '" & Trim(mlPOSTUSERDATE3) & "'," & _
                " '" & Trim(mlPOSTUSERID4) & "', '" & Trim(mlPOSTUSERNAME4) & "', '" & Trim(mlPOSTUSERDATE4) & "'," & _
                " '" & Trim(mlPOSTUSERID5) & "', '" & Trim(mlPOSTUSERNAME5) & "', '" & Trim(mlPOSTUSERDATE5) & "'," & _
                " '" & Trim(mpDESC.Text) & "', " & _
                " '" & mlRECSTATUS & "','" & Session("mgUSERID") & "','" & mlOBJGF.FormatDate(Now) & "');"


            Select Case mpSTATUS
                Case "New"
                    mlSql_2 = ""
                    If mpLOCSAVE.Checked = True Then
                        mlSql_2 = Sql_2(Trim(mpSITECARD.Text), Trim(mpSITEDESC.Text), Trim(mpLOCATION.Text), Trim(txCITY.Text), Trim(ddSTATE.Text), Trim(ddCOUNTRY.Text))
                    End If
                    mlOBJGS.ExecuteQuery(mlSql_2, "PB", "ISSP3")
            End Select

            Select Case mpSTATUS
                Case "New", "Edit", "Delete"
                    mlSQL = mlSQL & mlOBJPJ.ISS_MR_MREntry_ToLog(mlKEY, mpSTATUS, Session("mgUSERID"), mlUDOCNO)
            End Select
            mlOBJGS.ExecuteQuery(mlSQL, "PB", "ISSP3")
            mlSQL = ""

            Dim mlOBJPJ_UT As New IASClass_Local_ut.ucmLOCAL_ut
            mlSQL = "SELECT * FROM AP_MR_REQUESTDT WHERE DocNo = '" & mlKEY & "'"
            mlRSTEMP = mlOBJGS.DbRecordset(mlSQL, "PB", "ISSP3")
            If mlRSTEMP.HasRows = False Then
                mlOBJPJ_UT.Sendmail_1("1", "sugianto@iss.co.id", "", "", "Error Save MR Online", mlKEY & " - " & Session("mgUSERID"))
                mlMESSAGE.Text = "Error ditemukan ketika MR disimpan" & "Save Fail"
                Exit Sub
            End If
            mlSQL = ""

            Select Case mpSTATUS
                Case "New"

                    Dim mlEMAIL_STATUS As String
                    Dim mlEMAIL_TO As String
                    Dim mlEMAIL_SUBJECT As String
                    Dim mlEMAIL_BODY As String
                    Dim mlLINKSERVER1 As String
                    Dim mlBCC As String

                    Dim mlRECSTATUSDESC As String
                    mlRECSTATUSDESC = ""
                    Select Case mlRECSTATUS
                        Case "Wait1"
                            mlRECSTATUSDESC = "Permintaan Baru, Ke Proses Review"
                    End Select

                    mlBCC = "iassystem_log@iss.co.id"
                    mlEMAIL_TO = ""
                    mlLINKSERVER1 = System.Configuration.ConfigurationManager.AppSettings("mgLINKEDSERVER1")

                    mlSQLTEMP = "SELECT EmailAddr FROM " & mlLINKSERVER1 & "AD_USERPROFILE  WHERE UserID IN " & _
                                " (SELECT UserID FROM OP_USER_SITE WHERE UserLevel < '3' AND SiteCardID='" & Trim(mpSITECARD.Text) & "') "
                    mlRSTEMP = mlOBJGS.DbRecordset(mlSQLTEMP, "PB", "ISSP3")
                    While mlRSTEMP.Read
                        mlEMAIL_TO = mlEMAIL_TO & IIf(mlOBJGF.IsNone(Trim(mlEMAIL_TO)) = True, "", ",") & mlRSTEMP("EmailAddr") & ""
                    End While

                    mlEMAIL_TO = IIf(mlOBJGF.IsNone(Trim(mlEMAIL_TO)) = True, "", mlEMAIL_TO & ",") & ""
                    If mlOBJGF.IsNone(Trim(mlEMAIL_TO)) = False Then
                        mlEMAIL_SUBJECT = "" & " Permintaan MR untuk " & Trim(mpSITECARD.Text) & "-" & mpSITEDESC.Text
                        mlEMAIL_BODY = ""
                        mlEMAIL_BODY = mlEMAIL_BODY & "Dear : " & Session("mgUSERID") & "-" & Session("mgNAME")
                        mlEMAIL_BODY = mlEMAIL_BODY & "<br><br>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "Terima kasih telah melakukan transaksi permintaan MR "
                        mlEMAIL_BODY = mlEMAIL_BODY & "<br> Berikut ini adalah informasi transaksi yang telah Anda lakukan :"
                        mlEMAIL_BODY = mlEMAIL_BODY & "<br><br>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "<table border=0>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "<tr><td valign=top>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "Tanggal	</td><td valign=top>:</td><td valign=top>" & Now
                        mlEMAIL_BODY = mlEMAIL_BODY & "</td></tr>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "<tr><td valign=top>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "Periode MR	</td><td valign=top>:</td><td valign=top>" & ddlPeriodeMR.SelectedItem.Text
                        mlEMAIL_BODY = mlEMAIL_BODY & "</td></tr>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "<tr><td valign=top>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "Jenis(Transaksi) </td><td valign=top>:</td><td valign=top>" & mlMRTYPE & "-" & mlMRDESCRIPTION
                        mlEMAIL_BODY = mlEMAIL_BODY & "</td></tr>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "<tr><td valign=top>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "No Dokumen  </td><td valign=top>:</td><td valign=top>" & mlKEY
                        mlEMAIL_BODY = mlEMAIL_BODY & "</td></tr>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "<tr><td valign=top>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "Lokasi  </td><td valign=top>:</td><td valign=top>" & Trim(mpSITECARD.Text) & "-" & Trim(mpSITEDESC.Text)
                        mlEMAIL_BODY = mlEMAIL_BODY & "</td></tr>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "<tr><td valign=top>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "Alamat  </td><td valign=top>:</td><td valign=top>" & Trim(txADDR.Text)
                        mlEMAIL_BODY = mlEMAIL_BODY & "</td></tr>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "<tr><td valign=top>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "Kota  </td><td valign=top>:</td><td valign=top>" & Trim(txCITY.Text)
                        mlEMAIL_BODY = mlEMAIL_BODY & "</td></tr>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "<tr><td valign=top>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "Propinsi  </td><td valign=top>:</td><td valign=top>" & Trim(ddSTATE.Text)
                        mlEMAIL_BODY = mlEMAIL_BODY & "</td></tr>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "<tr><td valign=top>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "Status Transaksi  </td><td valign=top>:</td><td valign=top>" & mlRECSTATUS & " -> " & mlRECSTATUSDESC
                        mlEMAIL_BODY = mlEMAIL_BODY & "</td></tr>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "<tr><td valign=top>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "</table>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "<br>Terima Kasih"
                        mlEMAIL_BODY = mlEMAIL_BODY & "<br><br><i>Email ini dikirim Otomatis oleh Sistem Komputer, Email ini tidak perlu dibalas/</i>"
                        mlEMAIL_BODY = mlEMAIL_BODY & "<br><i>This is an automatically generated email by computer system, please do not reply </i>"
                        mlEMAIL_STATUS = mlOBJPJ_UT.Sendmail_1("1", mlEMAIL_TO, "", mlBCC, mlEMAIL_SUBJECT, mlEMAIL_BODY)
                    End If


                    'Email Update Delivery Address
                    If mpLOCSAVE.Checked = True Then
                        mlEMAIL_TO = ""
                        mlEMAIL_TO = IIf(mlOBJGF.IsNone(Session("mgUSERMAIL")), "", Session("mgUSERMAIL") & ",")
                        Select Case ddENTITY.Text
                            Case "ISS"
                                mlEMAIL_TO = mlEMAIL_TO & "emilia@iss.co.id"
                            Case "IPM"
                                mlEMAIL_TO = mlEMAIL_TO & "winarsih@iss.co.id"
                            Case "ICS"
                                'mlEMAIL_TO = mlEMAIL_TO & "emilia@iss.co.id"
                            Case "IFS"
                                mlEMAIL_TO = mlEMAIL_TO & "henyo@iss.co.id"
                            Case Else
                                mlEMAIL_TO = mlEMAIL_TO & "ops.regional@iss.co.id"
                        End Select


                        If mlOBJGF.IsNone(Trim(mlEMAIL_TO)) = False Then
                            mlEMAIL_SUBJECT = "" & " Permintaan Pergantian Alamat Pengiriman untuk " & Trim(mpSITECARD.Text) & "-" & mpSITEDESC.Text
                            mlEMAIL_BODY = ""
                            mlEMAIL_BODY = mlEMAIL_BODY & "Dear Operation"
                            mlEMAIL_BODY = mlEMAIL_BODY & "<br><br>"
                            mlEMAIL_BODY = mlEMAIL_BODY & "<br> Berikut ini adalah informasi permintaan perubahan alamat pengiriman oleh " & Session("mgUSERID") & "-" & Session("mgNAME")
                            mlEMAIL_BODY = mlEMAIL_BODY & "<br><br>"
                            mlEMAIL_BODY = mlEMAIL_BODY & "<table border=0>"
                            mlEMAIL_BODY = mlEMAIL_BODY & "<tr><td valign=top>"
                            mlEMAIL_BODY = mlEMAIL_BODY & "Tanggal	</td><td valign=top>:</td><td valign=top>" & Now
                            mlEMAIL_BODY = mlEMAIL_BODY & "</td></tr>"
                            mlEMAIL_BODY = mlEMAIL_BODY & "<tr><td valign=top>"
                            mlEMAIL_BODY = mlEMAIL_BODY & "SiteCard </td><td valign=top>:</td><td valign=top>" & Trim(mpSITECARD.Text) & " - " & Trim(mpSITEDESC.Text)
                            mlEMAIL_BODY = mlEMAIL_BODY & "</td></tr>"
                            mlEMAIL_BODY = mlEMAIL_BODY & "<tr><td valign=top>"
                            mlEMAIL_BODY = mlEMAIL_BODY & "Alamat  </td><td valign=top>:</td><td valign=top>" & Trim(txADDR.Text)
                            mlEMAIL_BODY = mlEMAIL_BODY & "</td></tr>"
                            mlEMAIL_BODY = mlEMAIL_BODY & "<tr><td valign=top>"
                            mlEMAIL_BODY = mlEMAIL_BODY & "Kota  </td><td valign=top>:</td><td valign=top>" & Trim(txCITY.Text)
                            mlEMAIL_BODY = mlEMAIL_BODY & "</td></tr>"
                            mlEMAIL_BODY = mlEMAIL_BODY & "<tr><td valign=top>"
                            mlEMAIL_BODY = mlEMAIL_BODY & "Propinsi  </td><td valign=top>:</td><td valign=top>" & Trim(ddSTATE.Text)
                            mlEMAIL_BODY = mlEMAIL_BODY & "</td></tr>"
                            mlEMAIL_BODY = mlEMAIL_BODY & "<tr><td valign=top>"
                            mlEMAIL_BODY = mlEMAIL_BODY & "Pic Contact </td><td valign=top>:</td><td valign=top>" & Trim(txPHONE1_PIC.Text)
                            mlEMAIL_BODY = mlEMAIL_BODY & "</td></tr>"
                            mlEMAIL_BODY = mlEMAIL_BODY & "</table>"
                            mlEMAIL_BODY = mlEMAIL_BODY & "<br>Terima Kasih"
                            mlEMAIL_BODY = mlEMAIL_BODY & "<br><br><i>Email ini dikirim Otomatis oleh Sistem Komputer, Email ini tidak perlu dibalas/</i>"
                            mlEMAIL_BODY = mlEMAIL_BODY & "<br><i>This is an automatically generated email by computer system, please do not reply </i>"
                            mlEMAIL_STATUS = mlOBJPJ_UT.Sendmail_1("1", mlEMAIL_TO, "", mlBCC, mlEMAIL_SUBJECT, mlEMAIL_BODY)
                        End If

                    End If
            End Select

        Catch ex As Exception
            mlMESSAGE.Text = "Error ditemukan ketika MR disimpan" & "Save Fail"
            mlOBJGS.XMtoLog("MR", "MRRequest", "MRRequest" & mlKEY, Err.Description, "New", Session("mgUSERID"), mlOBJGF.FormatDate(Now))
        End Try
    End Sub


    ' -- Added by Rafi 2014-11-17 
    Protected Sub GetMonth()
        'Dim Tgl As DateTime = Convert.ToDateTime(DateTime.Now.Year.ToString("0000") & "/01" & "/01")

        'Dim Tgl As DateTime = Convert.ToDateTime(DateTime.Now.Year.ToString("0000") & "/" & DateTime.Now.Month.ToString("00") & "/01")
        'Dim IDMonth As [String] = Tgl.Month.ToString("00")
        'ddlPeriodeMR.Items.Clear()
        'For i As Integer = 1 To 12
        '    'ddlPeriodeMR.Items.Add(New ListItem(Tgl.ToLongDateString().Substring(3, Tgl.ToLongDateString().Length - 8), Tgl.Month.ToString("00")))
        '    ddlPeriodeMR.Items.Add(New ListItem(Tgl.ToShortDateString().Substring(3, Tgl.ToShortDateString().Length - 3), Tgl.Month.ToString("00")))
        '    Tgl = Tgl.AddMonths(1)
        'Next

        Dim mlSQLPeriode As String, mlDt_Periode As New DataTable

        mlSQLPeriode = " Select * from dbo.fnPeriodeMR()"
        mlRSTEMP = mlOBJGS.DbRecordset(mlSQLPeriode, "AD", "AD")
        mlDt_Periode.Load(mlRSTEMP)

        ddlPeriodeMR.Items.Clear()

        For i As Integer = 0 To mlDt_Periode.Rows.Count - 1
            ddlPeriodeMR.Items.Add(New ListItem(mlDt_Periode.Rows(i)("PeriodeMR").ToString(), mlDt_Periode.Rows(i)("IDPeriode").ToString()))
        Next

        For n As Integer = 0 To ddlPeriodeMR.Items.Count - 1
            If ddlPeriodeMR.Items(n).Value = DateTime.Now.Month.ToString("00") Then
                ddlPeriodeMR.SelectedIndex = n
                Exit For
            End If
        Next

    End Sub

    'Protected Sub ddlPeriodeMR_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPeriodeMR.SelectedIndexChanged
    '    ddlYearMR.SelectedValue = Year(DateAndTime.Now)
    '    If (ddlPeriodeMR.SelectedValue > "12") Or (ddlPeriodeMR.SelectedValue < Month(DateAndTime.Now).ToString("00")) Then
    '        ddlYearMR.SelectedIndex = ddlYearMR.SelectedIndex + 1
    '    End If
    '    mpPERIOD.Text = ddlPeriodeMR.SelectedValue.ToString() & "/" & ddlYearMR.SelectedValue.ToString()

    'End Sub

    Protected Sub mlDATAGRID2_ItemDataBound(sender As Object, e As DataGridItemEventArgs) Handles mlDATAGRID2.ItemDataBound
        If e.Item.ItemIndex >= 0 Then
            Dim HL As HyperLink = DirectCast(e.Item.FindControl("Hyperlink2"), HyperLink)
            Dim FlagView As Label = DirectCast(e.Item.FindControl("lblFlagView"), Label)

            HL.Visible = IIf(FlagView.Text = "1", True, False)
        End If

    End Sub
End Class
