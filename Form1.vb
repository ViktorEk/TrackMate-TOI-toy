Public Class Form1

    Private workingFolder As String = ""
    Private finalFile As String = ""
    Private initialised As Boolean = False
    Private shutdown As Boolean = False

    'TODO: replace tag system to allow individual tags instead of a string, as of right now



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Icon = My.Resources.if_csv_199249
        If Not My.Settings.lastPositionOnScreen = New Point() Then Me.Location = My.Settings.lastPositionOnScreen
        If Not My.Settings.lastSizeOfWindow = New Size() Then Me.Size = My.Settings.lastSizeOfWindow

        TextBox3.Text = My.Settings.lastItem_a
        If IsNumeric(My.Settings.lastItem_a) Then NumericUpDown2.Value = My.Settings.lastItem_a
        NumericUpDown3.Value = My.Settings.lastItem_b
        TextBox4.Text = My.Settings.lastPrefix
        NumericUpDown1.Value = My.Settings.lastUnitLength

        Label7.Text = ""
        Label9.Text = ""

        ChangeWorkingFolder()
        UpdateUI()
    End Sub
    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        If shutdown Then Me.Close()
    End Sub
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        My.Settings.lastPositionOnScreen = Me.Location
        My.Settings.lastSizeOfWindow = Me.Size
        My.Settings.lastUnitLength = NumericUpDown1.Value
        My.Settings.lastPrefix = TextBox4.Text
        My.Settings.lastItem_a = TextBox3.Text
        My.Settings.lastItem_b = NumericUpDown3.Value

        If initialised And (TextBox1.TextLength > 0 Or TextBox2.TextLength > 0) Then
            If MsgBox("Close the program? There might be unsaved changes.", MsgBoxStyle.OkCancel, "Confirm") = MsgBoxResult.Cancel Then
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub UpdateUI()
        finalFile = workingFolder & "\" & TextBox4.Text & TextBox3.Text & "_" & NumericUpDown3.Value.ToString & ".csv"

        Label6.Text = workingFolder
        Label7.Text = finalFile
        Label9.Text = TextBox4.Text & TextBox3.Text & "_" & NumericUpDown3.Value.ToString & ".csv"
        Label7.Location = New Point(Panel1.Width - 26 - Label7.Width, Label7.Location.Y)
        Label9.Location = New Point(Panel1.Width - 22 - Label9.Width, Label9.Location.Y)
        Label9.BackColor = If(IO.File.Exists(finalFile), Color.OrangeRed, Color.DarkSeaGreen)
        Label14.Visible = IO.File.Exists(finalFile)
        NumericUpDown2.Enabled = IsNumeric(TextBox3.Text)
        If IsNumeric(TextBox3.Text) Then NumericUpDown2.Value = TextBox3.Text

        If TextBox4.TextLength > 0 Then
            Button2.Enabled = True
        Else : Button2.Enabled = False
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ChangeWorkingFolder()
    End Sub

    Private Sub ChangeWorkingFolder()
        If My.Settings.lastFolder.Length > 0 Then OpenFileDialog1.InitialDirectory = My.Settings.lastFolder
        OpenFileDialog1.Title = "Welcome! Please select a working folder"
        OpenFileDialog1.FileName = "Current folder"

        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            workingFolder = IO.Directory.GetParent(OpenFileDialog1.FileName).FullName
            My.Settings.lastFolder = workingFolder
            UpdateUI()
            initialised = True
        ElseIf workingFolder = "" Then
            MsgBox("Working folder cannot be nothing. Closing...", MsgBoxStyle.Critical)
            shutdown = True
        End If
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged, NumericUpDown3.ValueChanged, TextBox3.TextChanged
        If workingFolder.Length > 0 Then UpdateUI()
    End Sub

    Private Sub Export()
        Try
            Dim outputs() As String = {TextBox1.Text, TextBox2.Text}
            For i = 0 To 1
                Dim output As String = outputs(i)
                If Not output.Replace(",", "").Length Mod NumericUpDown1.Value = 0 Then
                    If MsgBox("Warning! The '" & If(i = 0, "TOIs", "Ignore") & "' output is not evenly divisible by the unit length (" & NumericUpDown1.Value.ToString & "). Make sure that all input is formatted correctly." & vbLf & vbLf & "Continue?", MsgBoxStyle.YesNo, "Warning") = MsgBoxResult.No Then
                        Exit Sub
                    End If
                End If
            Next

            Using w As New IO.StreamWriter(finalFile)
                For Each output As String In outputs
                    If CheckBox3.Checked Then
                        'Clean, heavy
                        output = output.Replace(" ", "")
                        output = output.Replace(".", "")
                        output = output.Replace("-", "")
                        output = output.Replace(",,", "")
                        output = output.Replace(",,,", "")

                        'Clean, fine
                        output = output.Trim(",")
                    End If

                    w.WriteLine(output)
                Next
            End Using

            MsgBox("Exported successfully!", MsgBoxStyle.Information, "Success")
        Catch ex As Exception
            MsgBox("Something went wrong trying to export, message: " & vbLf & ex.Message)
        End Try

        Try
            If CheckBox1.Checked Then
                TextBox1.Clear()
                TextBox2.Clear()
            End If
            If CheckBox5.Checked Then
                NumericUpDown3.Value += 1
            End If
        Catch ex As Exception
            MsgBox("Post-export clean-up was not successful, with message: " & vbLf & ex.Message)
        End Try

    End Sub

    Private Sub Button2_click(sender As Object, e As EventArgs) Handles Button2.Click
        If IO.File.Exists(finalFile) Then
            If MsgBox("A file with this name exists in the working folder. Overwrite?", MsgBoxStyle.YesNo, "Warning") = MsgBoxResult.Yes Then
                Export()
            End If
        Else
            Export()
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged, CheckBox5.CheckedChanged
        My.Settings.topMost = CheckBox2.Checked
        Me.TopMost = CheckBox2.Checked
    End Sub

    Private Sub TextBox_TextChanged(sender As TextBox, e As EventArgs) Handles TextBox1.TextChanged, TextBox2.TextChanged
        If TextBox1.TextLength > 0 Then
            Label12.Text = "Total items: " & TextBox1.Text.Split(",").Count
        Else : Label12.Text = "Total items: 0"
        End If
        If TextBox2.TextLength > 0 Then
            Label13.Text = "Total items: " & TextBox2.Text.Split(",").Count
        Else : Label13.Text = "Total items: 0"
        End If

        If CheckBox4.Checked And sender.TextLength > 0 Then
            If (sender.TextLength - sender.Text.LastIndexOf(",") > NumericUpDown1.Value + 1) And (Not sender.Text.ToArray.Last = ",") Then
                sender.Text = sender.Text.Insert(sender.TextLength - 1, ",")
                sender.SelectionStart = sender.TextLength
            End If
        End If
    End Sub

    Private Sub TextBox_Click(sender As TextBox, e As EventArgs) Handles TextBox2.Click, TextBox1.Click
        If Not sender.Focused Then sender.SelectionStart = sender.TextLength
    End Sub

    Private Sub Form1_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        UpdateUI()
    End Sub

    Private Sub Draw_focus(sender As Object, e As EventArgs) Handles MyBase.Activated
        TextBox1.Focus()
        TextBox1.SelectionStart = TextBox1.TextLength
    End Sub
    Private Sub Lose_focus(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        CheckBox2.Focus()
    End Sub

    Private Sub NumericUpDown2_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown2.ValueChanged
        If IsNumeric(TextBox3.Text) Then TextBox3.Text = NumericUpDown2.Value
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        NumericUpDown1.Enabled = CheckBox4.Checked
    End Sub
End Class
