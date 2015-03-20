
Public Class frmMain

    'Create an error counter to flag user software keep encountering errors
    Private ErrorCounter As Integer = 0

    Private Sub Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Configure the timer & minimize
        TMRRunForEver.Interval = 60 * 1000 * 55
        TMRRunForEver.Start()
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub TMRRunForEver_Tick(sender As System.Object, e As System.EventArgs) Handles TMRRunForEver.Tick
        'Stop the timer, run the backup, restart the timer
        TMRRunForEver.Enabled = False
        Call BackupCode()
        TMRRunForEver.Start()
    End Sub

    Private Sub BackupCode()
        'Only backup between 8PM and 9PM to prevent files from being locked during the day
        If Now.Hour < 20 Or Now.Hour > 21 Then
            'Exit if during day
            Exit Sub
        End If

        'First check if the network is up
        If My.Computer.FileSystem.DirectoryExists(Path.Source.CodeLockProjects) = False Then
            MessageBox.Show("The network appears to be down.  Your code could not be automatically backed up.  This is just an FYI.  The software will try again later.", "Network Down", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        'Check the error counter before beginning
        If ErrorCounter >= 3 Then
            'If the app keeps erroring out, prompt user
            MessageBox.Show("There have been 3 consecutive errors while attempting to auto backup your software.  You may want to debug the application to figure out why.", "Multiple Consecutive Errors", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        Try
            'If the network is up, try and copy all VB & .NET source code locally
            My.Computer.FileSystem.CopyDirectory(Path.Source.SPCDotNetSourceCode, Path.Destination.SPCDotNetSourceCode, True)
        Catch ex As Exception
            'Inc the error counter & exit
            ErrorCounter += 1
            Exit Sub
        End Try

        Try
            'Try to copy the JSL scripts
            My.Computer.FileSystem.CopyDirectory(Path.Source.SPCJSLSourceCode, Path.Destination.SPCJSLSourceCode, True)
        Catch ex As Exception
            'Inc the error counter & exit
            ErrorCounter += 1
            Exit Sub
        End Try

        Try
            'Try and copy all documentation over
            My.Computer.FileSystem.CopyDirectory(Path.Source.SPCDocumentation, Path.Destination.SPCDocumentation, True)
        Catch ex As Exception
            'Inc the error counter & exit
            ErrorCounter += 1
            Exit Sub
        End Try

        'Make sure the databases dir exists locally
        My.Computer.FileSystem.CreateDirectory(Path.Destination.SPCDatabases)

        'Get all files (e.g. databases) in the source database dir (don't copy whole dir so backups aren't copied)
        Dim AllDBFiles As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(Path.Source.SPCDatabases)

        'Make sure files came back
        If AllDBFiles IsNot Nothing AndAlso AllDBFiles.Count > 0 Then
            'If there are files, copy each of them to the local location
            For Each FILE As String In AllDBFiles
                Try
                    'Copy file over if NOT a journal
                    If FILE.ToUpper.Contains("-JOURNAL") = False Then
                        My.Computer.FileSystem.CopyFile(FILE, Path.Destination.SPCDatabases & "\" & GetLastPartOfPath(FILE), True)
                    End If
                Catch ex As Exception
                    'Inc the error counter & exit
                    ErrorCounter += 1
                    Exit Sub
                End Try
            Next
        End If

        'Make sure the codelock dir exists locally
        My.Computer.FileSystem.CreateDirectory(Path.Destination.CodeLockProjects)

        'Get all the projects in codelock
        Dim AllCodeLockProjs As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetDirectories(Path.Source.CodeLockProjects)

        'Make sure there were projects
        If AllCodeLockProjs IsNot Nothing AndAlso AllCodeLockProjs.Count > 0 Then
            'If there are proejcts, copy them
            For Each FOLDER As String In AllCodeLockProjs
                'Do not copy old revs
                If FOLDER.ToUpper.Contains("OLDREVS") = False Then
                    Try
                        'Copy dir
                        My.Computer.FileSystem.CopyDirectory(FOLDER, Path.Destination.CodeLockProjects & "\" & GetLastPartOfPath(FOLDER), True)
                    Catch ex As Exception
                        'Inc the error counter & exit
                        ErrorCounter += 1
                        Exit Sub
                    End Try
                End If
            Next
        End If

        'If it gets to end, clear error counter
        ErrorCounter = 0
    End Sub

    Private Function GetLastPartOfPath(ByVal FullFilePath As String) As String
        GetLastPartOfPath = String.Empty

        'Check that the string passed in is a full file path
        If FullFilePath.Contains("\") = True Then
            'If so, use the last "\" to get the file name
            Try
                'Use try in case "\" is the last character in the string
                GetLastPartOfPath = Strings.Mid(FullFilePath, InStrRev(FullFilePath, "\") + 1)
            Catch ex As Exception
                'No action needed on failure
            End Try
        End If
    End Function

End Class
