Imports MySql.Data.MySqlClient
Imports System.IO

Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports PdfSharp.Drawing.Layout

Imports System.Xml
Imports System.Text
Imports System.Xml.Serialization

Imports System.Globalization
Module Functii

    Public Sub Nonquery(ByVal sql As String)
        Dim dbconn As MySqlConnection = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        Try
            'Using dbconn As MySqlConnection = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            If dbconn.State = 0 Then
                dbconn.Open()
            End If

            Using dbcomm As MySqlCommand = New MySqlCommand(sql, dbconn)
                dbcomm.ExecuteNonQuery()
            End Using
            'End Using
        Catch ex As Exception
            MsgBox("Problem Nonquery: " & ex.Message.ToString)
        End Try
    End Sub
    Public Function NrCuv(ByVal valoare As Integer)
        Dim rezultat As String = ""
        Dim suma_rev As String = StrReverse(valoare)
        For i = 0 To suma_rev.Count - 1
            If i = 0 Then
                If Microsoft.VisualBasic.Val(suma_rev(0)) = 1 Then
                    rezultat = "siunu"
                End If
                If Microsoft.VisualBasic.Val(suma_rev(0)) = 2 Then
                    rezultat = "sidoi"
                End If
                If Microsoft.VisualBasic.Val(suma_rev(0)) = 3 Then
                    rezultat = "sitrei"
                End If
                If Microsoft.VisualBasic.Val(suma_rev(0)) = 4 Then
                    rezultat = "sipatru"
                End If
                If Microsoft.VisualBasic.Val(suma_rev(0)) = 5 Then
                    rezultat = "sicinci"
                End If
                If Microsoft.VisualBasic.Val(suma_rev(0)) = 6 Then
                    rezultat = "sisase"
                End If
                If Microsoft.VisualBasic.Val(suma_rev(0)) = 7 Then
                    rezultat = "sisapte"
                End If
                If Microsoft.VisualBasic.Val(suma_rev(0)) = 8 Then
                    rezultat = "siopt"
                End If
                If Microsoft.VisualBasic.Val(suma_rev(0)) = 9 Then
                    rezultat = "sinoua"
                End If
                If Microsoft.VisualBasic.Val(suma_rev(0)) = 0 Then
                    rezultat = ""
                End If
                If suma_rev.Length = 1 Then
                    rezultat = rezultat.Substring(2)
                End If
                'rezultat = String.Concat(rezultat)
            End If
            If i = 1 Then


                If Microsoft.VisualBasic.Val(suma_rev(1)) = 1 Then
                    If rezultat = "siunu" Then
                        rezultat = "siun"
                    End If
                    If rezultat.Length >= 2 Then
                        rezultat = rezultat.Substring(2) + "sprezece"
                    Else : rezultat = "zece"

                    End If
                End If
                If Microsoft.VisualBasic.Val(suma_rev(1)) = 2 Then
                    rezultat = "douazeci" + rezultat

                End If
                If Microsoft.VisualBasic.Val(suma_rev(1)) = 3 Then
                    rezultat = "treizeci" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(1)) = 4 Then
                    rezultat = "patruzeci" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(1)) = 5 Then
                    rezultat = "cincizeci" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(1)) = 6 Then
                    rezultat = "saizeci" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(1)) = 7 Then
                    rezultat = "saptezeci" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(1)) = 8 Then
                    rezultat = "optzeci" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(1)) = 9 Then
                    rezultat = "nouazeci" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(1)) = 0 Then
                    If rezultat.Length >= 2 Then
                        rezultat = "" + rezultat.Substring(2)
                    Else : rezultat = ""
                    End If
                End If
            End If

            If i = 2 Then
                If Microsoft.VisualBasic.Val(suma_rev(2)) = 1 Then
                    rezultat = "osuta" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(2)) = 2 Then
                    rezultat = "douasute" + rezultat

                End If
                If Microsoft.VisualBasic.Val(suma_rev(2)) = 3 Then
                    rezultat = "treisute" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(2)) = 4 Then
                    rezultat = "patrusute" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(2)) = 5 Then
                    rezultat = "cincisute" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(2)) = 6 Then
                    rezultat = "sasesute" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(2)) = 7 Then
                    rezultat = "saptesute" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(2)) = 8 Then
                    rezultat = "optsute" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(2)) = 9 Then
                    rezultat = "nouasute" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(2)) = 0 Then
                    rezultat = "" + rezultat
                End If
            End If
            If i = 3 Then

                If Microsoft.VisualBasic.Val(suma_rev(3)) = 1 Then
                    rezultat = "omie" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(3)) = 2 Then
                    rezultat = "douamii" + rezultat

                End If
                If Microsoft.VisualBasic.Val(suma_rev(3)) = 3 Then
                    rezultat = "treimii" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(3)) = 4 Then
                    rezultat = "patrumii" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(3)) = 5 Then
                    rezultat = "cincimii" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(3)) = 6 Then
                    rezultat = "sasemii" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(3)) = 7 Then
                    rezultat = "saptemii" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(3)) = 8 Then
                    rezultat = "optmii" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(3)) = 9 Then
                    rezultat = "nouamii" + rezultat
                End If
                If Microsoft.VisualBasic.Val(suma_rev(3)) = 0 Then
                    rezultat = "" + rezultat
                End If

            End If
        Next
        Return rezultat
    End Function
    Function Verifica_CNP(ByVal text As String)
        '279146358279
        Dim cnp_status As Boolean
        If IsNumeric(text) Then
            Dim lungime As Integer = text.Length
            If lungime = 13 Then
                Dim sex As Integer = CInt(text.First.ToString)
                If sex > 0 And sex < 7 Then
                    Dim cnp As String = text
                    Dim data As Date = Now

                    If Date.TryParseExact(text.Substring(1).Remove(6, 6), "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, Now) = True Then
                        Dim prod0 As Integer = Microsoft.VisualBasic.Val(cnp(0)) * 2
                        Dim prod1 As Integer = Microsoft.VisualBasic.Val(cnp(1)) * 7
                        Dim prod2 As Integer = Microsoft.VisualBasic.Val(cnp(2)) * 9
                        Dim prod3 As Integer = Microsoft.VisualBasic.Val(cnp(3)) * 1
                        Dim prod4 As Integer = Microsoft.VisualBasic.Val(cnp(4)) * 4
                        Dim prod5 As Integer = Microsoft.VisualBasic.Val(cnp(5)) * 6
                        Dim prod6 As Integer = Microsoft.VisualBasic.Val(cnp(6)) * 3
                        Dim prod7 As Integer = Microsoft.VisualBasic.Val(cnp(7)) * 5
                        Dim prod8 As Integer = Microsoft.VisualBasic.Val(cnp(8)) * 8
                        Dim prod9 As Integer = Microsoft.VisualBasic.Val(cnp(9)) * 2
                        Dim prod10 As Integer = Microsoft.VisualBasic.Val(cnp(10)) * 7
                        Dim prod11 As Integer = Microsoft.VisualBasic.Val(cnp(11)) * 9
                        Dim sum_prod As Integer = prod0 + prod1 + prod2 + prod3 + prod4 + prod5 + prod6 + prod7 + prod8 + prod9 + prod10 + prod11
                        Dim rest As Double = sum_prod Mod 11
                        Dim cc As Integer = Microsoft.VisualBasic.Val(cnp(12))
                        If rest = 10 Then
                            rest = 1
                        End If
                        If rest = cc Then
                            'MsgBox("CNP Bun")
                            cnp_status = True
                        ElseIf rest <> cc Then
                            cnp_status = False
                            'MsgBox("CNP Incorect 5 - control")              
                        Else : cnp_status = False 'MsgBox("CNP Incorect 4")
                        End If
                    Else : cnp_status = False 'MsgBox("CNP Incorect 3")
                    End If
                Else : cnp_status = False 'MsgBox("CNP Incorect 2")
                End If
            Else : cnp_status = False 'MsgBox("CNP Incorect 1")
            End If
        End If
        Return cnp_status
    End Function

    Public Function Verifica_CIF(ByVal text As String)
        ' 753217532
        Dim cheie As String = "235712357"
        Dim cnp As String = StrReverse(text)
        Dim cif_status As Boolean
        If IsNumeric(text) Then
            Dim cifra_verificare As Integer = Microsoft.VisualBasic.Val(cnp(0))
            Dim lungime As Integer = text.Length
            If lungime < 11 Then
                Dim sum_prod As Integer = 0
                For i = 1 To cnp.Count - 1
                    sum_prod += (Microsoft.VisualBasic.Val(cnp(i)) * Microsoft.VisualBasic.Val(cheie(i - 1)))
                Next
                Dim rest As Double = (sum_prod * 10) Mod 11
                If rest = 10 Then rest = 0
                If rest = cifra_verificare Then
                    cif_status = True
                Else : cif_status = False
                End If
            End If

        End If
        Return cif_status
    End Function

    Public Declare Sub keybd_event Lib "user32" ( _
       ByVal bVk As Byte, _
       ByVal bScan As Byte, _
       ByVal dwFlags As Integer, _
       ByVal dwExtraInfo As Integer _
   )
    Public Const VK_CAPITAL As Integer = &H14
    Public Const KEYEVENTF_EXTENDEDKEY As Integer = &H1
    Public Const KEYEVENTF_KEYUP As Integer = &H2
    Public Sub CAPS_ON()

        If My.Computer.Keyboard.CapsLock Then 'if CapsLock = Off
            'NIMIC
        Else
            keybd_event(VK_CAPITAL, &H45, KEYEVENTF_EXTENDEDKEY Or 0, 0) ' Simulate the Key Press
            keybd_event(VK_CAPITAL, &H45, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)  ' Simulate the Key Release
        End If
    End Sub
    Public Sub CAPS_OFF()

        If My.Computer.Keyboard.CapsLock Then 'if CapsLock = ON
            keybd_event(VK_CAPITAL, &H45, KEYEVENTF_EXTENDEDKEY Or 0, 0) ' Simulate the Key Press
            keybd_event(VK_CAPITAL, &H45, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)  ' Simulate the Key Release
        Else
            ' NIMIC
        End If
    End Sub
    Public Sub Import_Extras(ByVal XmlPath As String)
        Dim AppDataFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            '        Dim dbread As System.Data.SQLite.SQLiteDataReader
            If dbconn.State = 0 Then
                dbconn.Open()
            End If


            Dim xmldoc As New XmlDocument
            Dim status As String = ""
            Try
                xmldoc.Load(XmlPath)
            Catch ex As Exception
                MsgBox("Nu merge Netu'. Nu se poate actualiza")
            End Try
            Dim nodes As XmlNodeList = xmldoc.GetElementsByTagName("movement")
            Dim dbcomm As New MySqlCommand
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()

            For Each node As XmlNode In nodes

                Dim parentNode As XmlNode = node.ParentNode
                Dim data As XmlAttribute = parentNode.Attributes.ItemOf("date")

                Dim id_tranzactie As String = node.Item("ref").InnerXml
                'MsgBox(id_tranzactie)
                Dim data_procesare As String = node.Item("booking_date").InnerXml
                Dim detalii As String = node.Item("details").InnerXml
                Dim debit As String = Strings.Replace(node.Item("debit").InnerXml, "-", Nothing)
                Dim credit As String = node.Item("credit").InnerXml

               
                Dim suma As String = 0
                Dim tip_tranzactie As String = "-"
                'MsgBox(debit & " " & credit)
                If credit > 0 Then
                    tip_tranzactie = "Incasare"
                    suma = credit
                ElseIf credit <= 0 Then
                    tip_tranzactie = "Plata"
                    suma = debit
                End If
                Dim sql_ins As String = ""

                If detalii.Substring(0, 8) = "Comision" Then 'Or id_tranzactie.Substring(0, 3) = "CTP"
                    sql_ins = "INSERT IGNORE INTO banca_comisioane (data,tip_tranzactie,suma,id_tranzactie,descriere,data_procesare)" _
                                        & " VALUES (@data,@tip_tranzactie,@suma,@id_tranzactie,@descriere,@data_procesare)"
                Else
                    sql_ins = "INSERT IGNORE INTO banca_tranzactii (data,tip_tranzactie,suma,id_tranzactie,descriere,data_procesare)" _
                                        & " VALUES (@data,@tip_tranzactie,@suma,@id_tranzactie,@descriere,@data_procesare)"
                End If
                '-------------- insert

                dbcomm = New MySqlCommand(sql_ins, dbconn)
                dbcomm.Parameters.AddWithValue("@data", data_procesare)
                dbcomm.Parameters.AddWithValue("@tip_tranzactie", tip_tranzactie)
                dbcomm.Parameters.AddWithValue("@suma", suma)
                'dbcomm.Parameters.AddWithValue("@comision", id)
                dbcomm.Parameters.AddWithValue("@id_tranzactie", id_tranzactie)
                dbcomm.Parameters.AddWithValue("@descriere", detalii)
                dbcomm.Parameters.AddWithValue("@data_procesare", data_procesare)
                dbcomm.ExecuteNonQuery()
                ' End Using

            Next
            transaction.Commit()
            'If dbconn.State = ConnectionState.Open Then
            '    dbconn.Close()
            'End If
        End Using
        MsgBox("S-a Importat")
    End Sub

    Public Sub Insert_Firma(ByVal firma As String, id As String, forma_juridica As String, cui As String, ro As String, j As String, adresa As String, localitate As String, judet As String, tip As String, status As String, cont As String, banca As String)
        
    End Sub
End Module
