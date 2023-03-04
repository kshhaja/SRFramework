using UnityEditor;
using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;

public class TemplateScriptGenerator
{
    public enum TemplateScriptType
    {
        Execution,

    }

    [MenuItem("Gameplay/Scripts/New Execution")]
    static void CreateExecutionFile()
    {
        Create("81-Gameplay__Scripts-NewExecution.cs.txt");
    }

    static void Create(string templateFileName)
    {
        string[] res = System.IO.Directory.GetFiles(Application.dataPath, templateFileName, SearchOption.AllDirectories);
        if (res.Length == 0)
        {
            Debug.LogError("error message ....");
        }

        string filePath = res[0];
        string folderPath = res[0].Replace(templateFileName, "").Replace("\\", "/");

        TextAsset templateTextFile = AssetDatabase.LoadAssetAtPath(filePath, typeof(TextAsset)) as TextAsset;

        string contents = "";
        //If the text file is available, lets get the text in it
        //And start replacing the place holder data in it with the 
        //options we created in the editor window
        //if (templateTextFile != null)
        //{
        //    contents = templateTextFile.text;
        //    contents = contents.Replace("CHARACTERCLASS_NAME_HERE", characterName.Replace(" ", ""));
        //    contents = contents.Replace("CHARACTER_NAME_HERE", characterName);
        //    contents = contents.Replace("CHARACTER_HEALTH_HERE", characterHealth.ToString());
        //    contents = contents.Replace("CHARACTER_COST_HERE", characterCost.ToString());
        //    contents = contents.Replace("CHARACTER_BAD_GUY_HERE", isBadGuy.ToString().ToLower());
        //}
        //else
        //{
        //    Debug.LogError("Can't find the CharacterTemplate.txt file! Is it at the path YOUR_PROJECT/Assets/CharacterTemplate.txt?");
        //}

        ////Let's create a new Script named "CHARACTERNAME.cs"
        //using (StreamWriter sw = new StreamWriter(string.Format(Application.dataPath + "/{0}.cs",
        //                                                   new object[] { characterName.Replace(" ", "") })))
        //{
        //    sw.Write(contents);
        //}
        //Refresh the Asset Database
        AssetDatabase.Refresh();
    }
}
