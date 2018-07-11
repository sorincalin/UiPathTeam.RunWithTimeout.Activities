If Not IsObject(application) Then
   Set SapGuiAuto  = GetObject("SAPGUI")
   Set application = SapGuiAuto.GetScriptingEngine
End If
If Not IsObject(connection) Then
   Set connection = application.Children(0)
End If
If Not IsObject(session) Then
   Set session    = connection.Children(0)
End If
If IsObject(WScript) Then
   WScript.ConnectObject session,     "on"
   WScript.ConnectObject application, "on"
End If
session.findById("wnd[0]").maximize
session.findById("wnd[0]/tbar[0]/okcd").text = "swbp"
session.findById("wnd[0]").sendVKey 0
session.findById("wnd[0]/tbar[0]/okcd").text = "sbwp"
session.findById("wnd[0]").sendVKey 0
session.findById("wnd[0]/usr/cntlSINWP_CONTAINER/shellcont/shell/shellcont[0]/shell").selectedNode = "          2"
session.findById("wnd[0]/usr/cntlSINWP_CONTAINER/shellcont/shell/shellcont[0]/shell").selectedNode = "          9"
session.findById("wnd[0]/usr/cntlSINWP_CONTAINER/shellcont/shell/shellcont[0]/shell").selectedNode = "         11"
session.findById("wnd[0]/usr/cntlSINWP_CONTAINER/shellcont/shell/shellcont[1]/shell/shellcont[0]/shell").pressToolbarButton "EREF"
