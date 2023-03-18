using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class CopyTemplateScripts
{
    static string GetCurrentFileName([System.Runtime.CompilerServices.CallerFilePath] string fileName = null)
    {
        return fileName;
    }

    static CopyTemplateScripts()
    {
        System.IO.DirectoryInfo source = new System.IO.DirectoryInfo(GetCurrentFileName());
        source = new System.IO.DirectoryInfo(source.Parent.FullName + "\\Scripts");
        System.IO.DirectoryInfo dest = new System.IO.DirectoryInfo(Application.dataPath + "\\ScriptTemplates");
        
        var files = source.GetFiles();
        foreach (var file in files)
        {
            try
            {
                FileUtil.CopyFileOrDirectory(file.FullName, dest.FullName + file.Name); 
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
                continue;
            }
        }
    }
}