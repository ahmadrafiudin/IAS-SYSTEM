Imports System
Imports System.Data
Imports System.Web.UI.HtmlControls
Imports System.Drawing
Imports System.Data.OleDb
Imports System.IO



Partial Class mm_resetpwd
    Inherits System.Web.UI.Page

    Dim mlOBJGS As New IASClass.ucmGeneralSystem
    Dim mlOBJGF As New IASClass.ucmGeneralFunction
    Dim mlREADER As OleDb.OleDbDataReader
    Dim mlSQL As String
    Dim mlTABLE As String
    Dim mlENCRYPTCODE As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        mlTITLE.Text = "Update Member Password V2.01"
        Me.Title = System.Configuration.ConfigurationManager.AppSettings("mgTITLE") & "Update Member Password V2.01"
        mlENCRYPTCODE = System.Configuration.ConfigurationManager.AppSettings("mgENCRYPTCODE")
        mlOBJGS.Main()
        If mlOBJGS.ValidateExpiredDate() = True Then
            Exit Sub
        End If
        If Session("mgACTIVECOMPANY") = "" Then Session("mgACTIVECOMPANY") = mlOBJGS.mgCOMPANYDEFAULT
        mlOBJGS.mgACTIVECOMPANY = Session("mgACTIVECOMPANY")

        mlTABLE = "AR_DIST"
        If Page.IsPostBack = False Then
            mlOBJGS.XM_UserLog(Session("mgUSERID"), Session("mgNAME"), "mm_resetpwd", "Member Reset Pswrd", "")
        End If
    End Sub

    Sub ClearFields()
        mlOLDPWD.Text = ""
        mlNEWPWD.Text = ""
        mlCNEWPWD.Text = ""
    End Sub

    Protected Sub btSaveRecord_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btSaveRecord.Click
        SaveRecord()
    End Sub

    Sub SaveRecord()
        Dim mlOLDPWDN As String
        Dim mlNEWPWDN As String

        Try
            mlOLDPWDN = mlOLDPWD.Text
            mlOLDPWDN = mlOBJGF.Encrypt(mlOLDPWDN, mlENCRYPTCODE)

            mlNEWPWDN = mlNEWPWD.Text
            mlNEWPWDN = mlOBJGF.Encrypt(mlNEWPWDN, mlENCRYPTCODE)

            mlSQL = "SELECT * FROM " & mlTABLE & " WHERE DistID='" & Session("mgUSERID") & "' AND Password = '" & mlOLDPWDN & "'"
            mlREADER = mlOBJGS.DbRecordset(mlSQL)
            If mlREADER.HasRows Then

                mlSQL = "UPDATE " & mlTABLE & " SET Password = '" & mlNEWPWDN & "' WHERE DistID='" & Session("mgUSERID") & "'"
                mlOBJGS.ExecuteQuery(mlSQL)
                mlMESSAGE.Text = "Password Berhasil di Update "
            Else
                mlMESSAGE.Text = "Password Lama Anda Salah "
            End If
            mlOBJGS.CloseFile(mlREADER)

        Catch ex As Exception
            mlMESSAGE.Text = "Password Update Failed "
        End Try
    End Sub
End Class
