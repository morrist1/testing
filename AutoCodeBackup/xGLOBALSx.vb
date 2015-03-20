
Module xGLOBALSx

    'Create structure to hold all file paths
    Public Structure Path
        'Create nested structure to hold all source locations
        Public Structure Source
            'Declare paths that contain source data to backup
            Private Const TDrive As String = "\\SVCHAFILE\TEAMS\"
            Public Const SPCDotNetSourceCode As String = TDrive & "Instrument Manufacturing SPC\VB Forms & Macro Source Code Backup"
            Public Const SPCJSLSourceCode As String = TDrive & "Instrument Manufacturing SPC\Scripts"
            Public Const SPCDatabases As String = TDrive & "Instrument Manufacturing SPC\Databases"
            Public Const SPCDocumentation As String = TDrive & "Instrument Manufacturing SPC\Documentation"
            Public Const CodeLockProjects As String = TDrive & "Test Engineering\CodeLock"
        End Structure

        'Create nested structure to hold all destination locations
        Public Structure Destination
            Private Const RootLocalDir As String = "C:\Users\tjmorrison\SkyDrive\!-PROGRAMMING-!\! MS.NET !\x-CODE FROM NETWORK-x\"
            Public Const SPCDotNetSourceCode As String = RootLocalDir & "SPC Source Code"
            Public Const SPCJSLSourceCode As String = RootLocalDir & "SPC JMP Automation Scripts"
            Public Const SPCDatabases As String = RootLocalDir & "SPC Databases"
            Public Const SPCDocumentation As String = RootLocalDir & "SPC Documentation"
            Public Const CodeLockProjects As String = RootLocalDir & "CodeLock Projects"
        End Structure
    End Structure

End Module
