using System.Activities;
using System.ComponentModel;

namespace UiPathTeam.RunWithTimeout.Activities
{
    [DisplayName("Run Script With Timeout")]
    [Description("Runs a variety of scripts using the Windows Script Host (cscript.exe).")]
    public class RunScriptWithTimeoutActivity : RunProcessWithTimeoutActivity
    {
        protected override void ValidateFilename(CodeActivityContext context, bool lookForFilesInPATH)
        {
            base.ValidateFilename(context, false); // For scripts we don't look in directories from PATH
        }
        protected override void Execute(CodeActivityContext context)
        {
            var scriptFilename = FileName.Get(context);

            FileName.Set(context, "cscript.exe");
            Arguments.Set(context, "//Nologo " + "\"" + scriptFilename + "\"" + " " + Arguments.Get(context));

            base.Execute(context);
        }
    }
}
