using UnityEngine;
using UnityEditor;
using System.IO;


[InitializeOnLoad]
public class CopyTemplateScripts
{
    static string GetCurrentFileName([System.Runtime.CompilerServices.CallerFilePath] string fileName = null)
    {
        return fileName;
    }

    static CopyTemplateScripts()
    {
        DirectoryInfo source = new DirectoryInfo(GetCurrentFileName());
        source = new DirectoryInfo(source.Parent.FullName + "\\Scripts");
        DirectoryInfo dest = Directory.CreateDirectory(Application.dataPath + "\\ScriptTemplates");

        var files = source.GetFiles();
        foreach (var file in files)
        {
            if (Path.GetExtension(file.Name) == ".meta")
            {
                continue;
            }

            string destFullName = dest.FullName + "/" + file.Name;
            if (File.Exists(destFullName))
            {
                continue; 
            }

            FileUtil.CopyFileOrDirectoryFollowSymlinks(file.FullName, destFullName);
        }
    }
}