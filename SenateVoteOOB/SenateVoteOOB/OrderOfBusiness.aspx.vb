Imports System.Messaging
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.CompilationMode
Imports System.Web.UI.Page

Public Class OrderOfBusiness
    Inherits System.Web.UI.Page

    Private gBillCalendarPage, gSenatorSubject, gWorkArea, gCOOB As String
    Private gReceiveQueueName As String = ".\PRIVATE$\senatevotequeue"


    Private Sub CheckDataTimer_Tick(sender As Object, e As System.EventArgs) Handles CheckDataTimer.Tick
        Try
            Dim strHtml As New StringBuilder
            Dim strBody As String = ""
            Dim vote_label As String = ""
            Dim label As String = ""
            Dim msgText As String = ""
            Dim i As Integer
            Dim Paramters() As String
            Dim tmpText As String = ""
            Dim txt As String = ""

            '*** check message from Queue
            If (MessageQueue.Exists(gReceiveQueueName)) = False Then
                MessageQueue.Create(gReceiveQueueName)
            End If

            Dim queue As New MessageQueue(gReceiveQueueName)
            queue.Formatter = New XmlMessageFormatter(New String() {"System.String"})
            Dim qenum As MessageEnumerator

            qenum = queue.GetMessageEnumerator2
            While qenum.MoveNext
                Dim m As Message = qenum.Current
                If m.Label = "UPDATE" Then
                    msgText = m.Body
                    m = queue.Receive(New TimeSpan(1000))
                    If msgText <> "" Then
                        Paramters = Split(msgText, "||")
                        For i = 0 To Paramters.Length - 1
                            txt = Mid(Paramters(i), 1, InStr(Paramters(i), " -") - 1)
                            tmpText = Mid(Paramters(i), InStr(Paramters(i), " -") + 3)
                            Select Case txt
                                Case "gBillCalendarPage"
                                    gBillCalendarPage = tmpText
                                Case "gSenatorSubject"
                                    gSenatorSubject = tmpText
                                Case "gWorkArea"
                                    gWorkArea = tmpText
                                    gWorkArea = Replace(gWorkArea, Chr(13) & Chr(10), "<br>")       '--- Chr(13) & Chr(10) = vbCrlf
                                Case "gCOOB"
                                    gCOOB = tmpText
                            End Select
                        Next
                    End If

                    Select Case UCase(gCOOB)
                        Case "WELCOME"
                            divOOBHeader.Visible = False
                            divBillPage.Visible = False
                            divSenatorSubject.Visible = False
                            lblWelcome.Visible = True
                            lblOrderOBusiness.Visible = False
                            lblWorkArea.Visible = False
                            lblOOBText.Visible = False
                            lblBillCalendarPage.Visible = False
                            lblSenatorSubject.Visible = False
                            lblWelcome.Font.Size = 40
                        Case "ORDER OF BUSINESS"
                            divOOBHeader.Visible = True
                            divOOB.Visible = True
                            divBillPage.Visible = False
                            divSenatorSubject.Visible = False
                            divText.Attributes("class") = "content_text_center"
                            divText.Visible = True
                            lblOOBText.Visible = True
                            lblOrderOBusiness.Visible = True
                            lblOrderOBusiness.Text = "Order of Business"
                            lblSenatorSubject.Visible = False
                            lblBillCalendarPage.Visible = False
                            lblWorkArea.Visible = False
                            lblWelcome.Visible = False
                        Case "ADJOURNMENT", "CONVENE", "RECESS", "SINE DIE", "CLEAR DISPLAY", "START"
                            divOOB.Visible = True
                            divOOBHeader.Visible = True
                            divBillPage.Visible = False
                            If UCase(gCOOB) = "CLEAR DISPLAY" Or UCase(gCOOB) = "START" Then
                                divSenatorSubject.Visible = False
                                lblSenatorSubject.Visible = False
                            Else
                                divSenatorSubject.Visible = True
                                lblSenatorSubject.Visible = True
                                lblSenatorSubject.Text = gSenatorSubject
                            End If

                            divText.Visible = True
                            divText.Attributes("class") = "content_text"
                            lblOOBText.Visible = False
                            lblWelcome.Visible = False

                            lblBillCalendarPage.Visible = False
                            lblOrderOBusiness.Visible = True
                            lblWorkArea.Visible = True

                            If UCase(gCOOB) <> "CLEAR DISPLAY" Then
                                lblOrderOBusiness.Text = gCOOB
                                lblWorkArea.Text = gWorkArea
                            Else
                                lblOrderOBusiness.Text = ""
                                lblWorkArea.Text = ""
                            End If
                        Case "INTRODUCTION OF BILLS", "HOUSE MESSAGES", "COMMITTEE REPORTS", "MOTIONS AND RESOLUTIONS", "LOCAL BILLS", "BILLS ON THIRD READING"
                            divText.Attributes("class") = "content_text"
                            divOOB.Visible = True
                            divOOBHeader.Visible = True
                            divBillPage.Visible = True
                            divSenatorSubject.Visible = True
                            divText.Visible = True
                            lblOrderOBusiness.Visible = True
                            lblBillCalendarPage.Visible = True
                            lblSenatorSubject.Visible = True
                            If UCase(gCOOB) = "COMMITTEE REPORTS" Then
                                divSenatorSubject.Visible = True
                            Else
                                divSenatorSubject.Visible = True
                            End If
                            lblWorkArea.Visible = True
                            lblWelcome.Visible = False
                            lblOOBText.Visible = False
                            lblOrderOBusiness.Text = gCOOB
                            lblBillCalendarPage.Text = gBillCalendarPage
                            lblSenatorSubject.Text = gSenatorSubject
                            lblWorkArea.Text = gWorkArea
                    End Select
                End If
            End While
            queue.Close()

        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

#Region "Un-using codes"


    'If UCase(gCOOB) <> "ADJOURNMENT" And UCase(gCOOB) <> "CONVENE" And UCase(gCOOB) <> "RECESS" And UCase(gCOOB) <> "SINE DIE" And UCase(gCOOB) <> "HOUSE MESSAGES" And UCase(gCOOB) = "ORDER OF BUSINESS" And UCase(gCOOB) = "WELCOME" Then
    '    lblWelcome.Visible = False
    '    lblOOBText.Visible = False
    '    divBillPage.Visible = True
    '    divSenatorSubject.Visible = True
    '    divText.Visible = True
    '    lblOrderOBusiness.Text = gCOOB
    '    lblBillCalendarPage.Text = gBillCalendarPage
    '    lblSenatorSubject.Text = gSenatorSubject
    '    lblWorkArea.Text = gWorkArea
    '    divBillPage.Attributes("class") = "content_bill"
    '    divBillPage.Attributes("class") = "content_subject"
    '    divText.Attributes("class") = "content_text"
    'Else
    '    If UCase(gCOOB) = "WELCOME" Then
    '        divText.Attributes("class") = "content_welcome"
    '        lblWelcome.Visible = True
    '        lblOrderOBusiness.Visible = False
    '        lblWorkArea.Visible = False
    '        lblOOBText.Visible = False
    '    End If
    '    If UCase(gCOOB) = "ORDER OF BUSINESS" Then
    '        lblOrderOBusiness.Text = "Order of Business"
    '        lblWorkArea.Visible = False
    '        lblOOBText.Visible = True
    '        divBillPage.Visible = False
    '        divSenatorSubject.Visible = False
    '        lblWelcome.Visible = False
    '        divText.Attributes("class") = "content_text_center"
    '    End If
    '    If UCase(gCOOB) = "WELCOME" Then
    '        divText.Attributes("class") = "content_welcome"
    '        lblWelcome.Visible = True
    '        lblOrderOBusiness.Visible = False
    '        lblWorkArea.Visible = False
    '        lblOOBText.Visible = False
    '    End If
    '    If UCase(gCOOB) <> "ORDER OF BUSINESS" And UCase(gCOOB) <> "WELCOME" Then
    '        lblOrderOBusiness.Visible = True
    '        lblOrderOBusiness.Text = gCOOB
    '        lblWorkArea.Visible = True
    '        lblOOBText.Visible = False
    '        divBillPage.Visible = False
    '        divSenatorSubject.Visible = False
    '        lblWelcome.Visible = False
    '        divText.Attributes("class") = "content_text"
    '    End If
    'End If

    'If UCase(gCOOB) = "WELCOME" Or UCase(gCOOB) = "ADJOURNMENT" Or UCase(gCOOB) = "CONVENE" Or UCase(gCOOB) = "RECESS" Or UCase(gCOOB) = "SINE DIE" Or UCase(gCOOB) = "HOUSE MESSAGES" Or UCase(gCOOB) = "ORDER OF BUSINESS" Then
    '    divBillPage.Visible = False
    '    divSenatorSubject.Visible = False
    '    lblWorkArea.Visible = True
    '    lblWelcome.Visible = False
    '    lblWorkArea.Text = gWorkArea
    '    If UCase(gCOOB) = "WELCOME" Then
    '        divText.Attributes("class") = "content_welcome"
    '        lblWelcome.Visible = True
    '        lblOrderOBusiness.Visible = False
    '        lblWorkArea.Visible = False
    '        lblOOBText.Visible = False
    '    End If
    '    If UCase(gCOOB) = "ORDER OF BUSINESS" Then
    '        lblOrderOBusiness.Text = "Order of Business"
    '        lblWorkArea.Visible = True
    '        lblOOBText.Visible = True
    '        divBillPage.Visible = False
    '        divSenatorSubject.Visible = False
    '        lblWelcome.Visible = False
    '        divText.Attributes("class") = "content_text_center"
    '    End If
    'Else
    '    divBillPage.Visible = True
    '    divSenatorSubject.Visible = True
    '    lblWorkArea.Visible = True
    '    lblWelcome.Visible = False

    '    lblOrderOBusiness.Text = gCOOB
    '    lblBillCalendarPage.Text = gBillCalendarPage
    '    lblSenatorSubject.Text = gSenatorSubject
    '    lblWorkArea.Text = gWorkArea
    'End If

    ''*** 1 OTHER Order of Business (Adjournment, Sine Die, Recess...)
    'If UCase(gCOOB) = "ADJOURNMENT" Or UCase(gCOOB) = "CONVENE" Or UCase(gCOOB) = "RECESS" Or UCase(gCOOB) = "SINE DIE" Or UCase(gCOOB) = "HOUSE MESSAGES" Then
    '    strHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>")
    '    strHtml.Append("<html xmlns='http://www.w3.org/1999/xhtml'>")
    '    strHtml.Append("<head id='Head1' runat='server'>")
    '    strHtml.Append("<title>Alabama Senate Voting System</title>")
    '    strHtml.Append("<meta http-equiv='refresh' content='2'/>")
    '    strHtml.Append("<link rel='stylesheet' type='text/css' href='Styles/styleIE.css' />")
    '    strHtml.Append("</head>")
    '    strHtml.Append("<body class ='Body'>")
    '    strHtml.Append("<div class='header'>")
    '    strHtml.Append("<div class='headgraphic' >")
    '    strHtml.Append("</div> <!-- end div headgraphic -->")
    '    strHtml.Append("</div> <!-- end div header-->")
    '    strHtml.Append("<!-- Begin Insertion Point -->")
    '    strHtml.Append("<div class='order_of_business'>")
    '    strHtml.Append("<div class='order_of_business_heading'>")
    '    If UCase(gCOOB) = "ADJOURNMENT" Then
    '        strHtml.Append("<p style='margin-top:20px;'>Adjournment </p>")
    '    ElseIf UCase(gCOOB) = "CONVENE" Then
    '        strHtml.Append("<p style='margin-top:20px;'>Convene </p>")
    '    ElseIf UCase(gCOOB) = "RECESS" Then
    '        strHtml.Append("<p style='margin-top:20px;'>Recess </p>")
    '    ElseIf UCase(gCOOB) = "SINE DIE" Then
    '        strHtml.Append("<p style='margin-top:20px;'>Sine Die </p>")
    '    ElseIf UCase(gCOOB) = "HOUSE MESSAGES" Then
    '        strHtml.Append("<p style='margin-top:20px;'>House Messages </p>")
    '    End If
    '    strHtml.Append("</div>")
    '    strHtml.Append("</div>")
    '    strHtml.Append("<div class='page'>")
    '    strHtml.Append("<div class='content'>")
    '    strHtml.Append("<div class='content_recess'>")
    '    strHtml.Append("")

    '    strHtml.Append("</div>")
    '    strHtml.Append("</div>    <!-- end div content -->")
    '    strHtml.Append("<br class='clearfix' />")
    '    strHtml.Append("</div> <!-- </div> end div page -->")
    '    strHtml.Append("<!-- End Insertion Point-->")
    '    strHtml.Append("</body>")
    '    strHtml.Append("</html>")
    'Else
    '    '*** 2 Order Of Business
    '    If gCOOB = "ORDER OF BUSINESS" Then
    '        strHtml.Append("")
    '        strHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>")
    '        strHtml.Append("<html xmlns='http://www.w3.org/1999/xhtml'>")
    '        strHtml.Append("<head id='Head1' runat='server'>")
    '        strHtml.Append("	<title>Alabama Senate Voting System</title>")
    '        strHtml.Append("<meta http-equiv='refresh' content='2'/>")
    '        strHtml.Append("<link rel='stylesheet' type='text/css' href='Styles/styleIE.css' />")
    '        strHtml.Append("</head>")
    '        strHtml.Append("<body class ='Body'>")
    '        strHtml.Append("<div class='header'>")
    '        strHtml.Append("<div class='headgraphic' >")
    '        strHtml.Append("</div> <!-- end div headgraphic -->")
    '        strHtml.Append("</div> <!-- end div header-->")
    '        strHtml.Append("<!-- Begin Insertion Point -->")
    '        strHtml.Append("<div class='order_of_business'>")
    '        strHtml.Append(" <div class='order_of_business_heading'>")
    '        strHtml.Append("<p style='margin-top:20px;'>Order of Business </p>")
    '        strHtml.Append("</div>")
    '        strHtml.Append("</div>   ")
    '        strHtml.Append("<div class='page'>  ")
    '        strHtml.Append(" <div class='content'>")
    '        strHtml.Append("<div class='content_text_center'>")
    '        strHtml.Append("<br>")
    '        strHtml.Append("Introduction of Bills")
    '        strHtml.Append("<br>")
    '        strHtml.Append("House Messages")
    '        strHtml.Append("<br>")
    '        strHtml.Append("Committee Reports")
    '        strHtml.Append("<br>")
    '        strHtml.Append("Motions and Resolutions")
    '        strHtml.Append("<br>")
    '        strHtml.Append("Local Bills")
    '        strHtml.Append("<br>")
    '        strHtml.Append("Bills on Third Reading")
    '        strHtml.Append("</div>")
    '        strHtml.Append("</div>      <!-- end div content -->")
    '        strHtml.Append(" <br class='clearfix' />")
    '        strHtml.Append("</div> <!-- </div> end div page --> ")
    '        strHtml.Append("</body>")
    '        strHtml.Append("</html>")
    '    ElseIf gCOOB = "WELCOME" Then
    '        '*** 3 Welcome
    '        strHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>")
    '        strHtml.Append("<html xmlns='http://www.w3.org/1999/xhtml'>")
    '        strHtml.Append("<head id='Head1' runat='server'>")
    '        strHtml.Append("<title>Alabama Senate Voting System</title>")
    '        strHtml.Append("<link rel='stylesheet' type='text/css' href='Styles/styleIE.css' />")
    '        strHtml.Append("<meta http-equiv='refresh' content='2'/>")
    '        strHtml.Append("<div class='content'>")
    '        strHtml.Append("</head>")
    '        strHtml.Append("<body class ='Body'>")
    '        strHtml.Append("<div class='header'>")
    '        strHtml.Append("<div class='headgraphic' >")
    '        strHtml.Append("</div> <!-- end div headgraphic -->")
    '        strHtml.Append("</div> <!-- end div header-->")
    '        strHtml.Append("<!-- Begin Insertion Point -->")
    '        strHtml.Append("<div class='order_of_business'>")
    '        strHtml.Append("<div class='order_of_business_heading'>")
    '        strHtml.Append("<p style='margin-top:20px;'>&nbsp; </p>")
    '        strHtml.Append("</div>")
    '        strHtml.Append("</div>")
    '        strHtml.Append("<div class='page'>")
    '        strHtml.Append("<div class='content'>")
    '        strHtml.Append("<div class='content_welcome'>")
    '        strHtml.Append("<p style='text-align:center; font-size:100px; font-weight:bold; line-height:100%;height:120px;'>Welcome </p>")
    '        strHtml.Append("<p style='text-align:center; font-size:60px;height:100px;'>to the </p>")
    '        strHtml.Append("<p style='text-align:center; font-size:105px; line-height:105%;height:120px;'>Alabama Senate </p>")
    '        strHtml.Append("</div>")
    '        strHtml.Append("</div>      <!-- end div content -->")
    '        strHtml.Append("<br class='clearfix' />")
    '        strHtml.Append("</div> <!-- </div> end div page -->")
    '        strHtml.Append("<!-- End Insertion Point-->")
    '        strHtml.Append("</body>")
    '        strHtml.Append("</html>")
    '    ElseIf gCOOB <> "ORDER OF BUSINESS" And gCOOB <> "ORDER OF BUSINESS" Then
    '        '*** 4 Bill On Third Reading
    '        strHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>")
    '        strHtml.Append("<html xmlns='http://www.w3.org/1999/xhtml'>")
    '        strHtml.Append("<head id='Head1' runat='server'>")
    '        strHtml.Append("<title>Alabama Senate Voting System</title>")
    '        strHtml.Append("<meta http-equiv='refresh' content='2'/>")
    '        strHtml.Append("<link rel='stylesheet' type='text/css' href='Styles/styleIE.css' />")
    '        strHtml.Append("</head>")
    '        strHtml.Append("<body class ='Body'>")
    '        strHtml.Append("<div class='header'>")
    '        strHtml.Append("<div class='headgraphic' >")
    '        strHtml.Append("</div> <!-- end div headgraphic -->")
    '        strHtml.Append("</div> <!-- end div header-->")
    '        strHtml.Append("<!-- Begin Insertion Point -->")
    '        strHtml.Append("<div class='order_of_business'>")
    '        strHtml.Append("<div class='order_of_business_heading'>")
    '        strHtml.Append("<p style='margin-top:20px;'>")

    '        'strHtml.Append(gCOOB)
    '        lblOrderOBusiness.Text = gCOOB

    '        strHtml.Append("</p>")
    '        strHtml.Append("</div>")
    '        strHtml.Append("</div>")
    '        strHtml.Append("<div class='page'>")

    '        If UCase(gCOOB) = UCase("COMMITTEE REPORTS") Or (gCOOB = "" And gWorkArea = "") Then
    '            strHtml.Append("<div class='content'>")
    '        Else
    '            strHtml.Append("<div class='content'>")
    '            strHtml.Append("<div class='content_bill'>")
    '            If gBillCalendarPage <> "" Then
    '                ' strHtml.Append(gBillCalendarPage)
    '                lblBillCalendarPage.Text = gBillCalendarPage
    '            End If
    '            strHtml.Append("</div>")
    '            strHtml.Append("<div class='content_subject'>")
    '            If gSenatorSubject <> "" Then
    '                'strHtml.Append(" by " & gSenatorSubject)
    '                lblSenatorSubject.Text = gSenatorSubject
    '            End If
    '            strHtml.Append("</div>")
    '            strHtml.Append("<div class='content_text'>")
    '            '--- put work area data here
    '            strHtml.Append(" <div class='content_text'>")

    '            If Strings.Right(gWorkArea, 2) <> vbCrLf Then
    '                gWorkArea = gWorkArea & vbCrLf
    '            End If
    '            Do
    '                '  strHtml.Append(Mid(gWorkArea, 1, InStr(gWorkArea, vbCrLf) - 1))
    '                lblWorkArea.Text = Mid(gWorkArea, 1, InStr(gWorkArea, vbCrLf) - 1)
    '                strHtml.Append("<BR>")
    '                If InStr(gWorkArea, vbCrLf) = Len(gWorkArea) - 1 Then
    '                    Exit Do
    '                End If
    '                gWorkArea = Mid(gWorkArea, InStr(gWorkArea, vbCrLf) + 2)
    '            Loop
    '            strHtml.Append("</div>")
    '            strHtml.Append("  </div>")
    '        End If
    '        strHtml.Append("  </div>      <!-- end div content -->")
    '        strHtml.Append(" <br class='clearfix' />")
    '        strHtml.Append("</div> <!-- </div> end div page -->")
    '        strHtml.Append("  <!-- End Insertion Point-->")
    '        strHtml.Append("</body>")
    '        strHtml.Append("</html>")
    '    End If
    'End If

    ''*** 5 write file
    'Response.Write(strHtml)
#End Region

End Class