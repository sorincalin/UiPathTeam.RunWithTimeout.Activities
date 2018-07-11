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
session.findById("wnd[0]").resizeWorkingPane 98,17,false
session.findById("wnd[0]/usr/cntlTREE_CONTAINER/shellcont/shell").selectItem "N4","Column1"
session.findById("wnd[0]/usr/cntlTREE_CONTAINER/shellcont/shell").ensureVisibleHorizontalItem "N4","Column1"
session.findById("wnd[0]/usr/cntlTREE_CONTAINER/shellcont/shell").doubleClickItem "N4","Column1"
session.findById("wnd[0]/usr/cntlCUST_200/shellcont/shell").currentCellRow = 7
session.findById("wnd[0]/usr/cntlCUST_200/shellcont/shell").doubleClickCurrentCell
