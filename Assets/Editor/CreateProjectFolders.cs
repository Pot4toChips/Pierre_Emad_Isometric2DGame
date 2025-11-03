using UnityEditor;
using System.IO;
using UnityEngine;

public class CreateProjectFolders
{
    [MenuItem("Tools/Project/Create Default Folders")]
    private static void CreateFolders()
    {
        string projectPath = Application.dataPath;

        // folder list
        string[] folders =
        {
            // ROOT
            "_Project",

            // CODE
            "_Project/Code", 
            "_Project/Code/Core",
            "_Project/Code/Input",
            "_Project/Code/Player",
            "_Project/Code/AI",
            "_Project/Code/UI",
            "_Project/Code/Combat",
            "_Project/Code/Inventory",
            "_Project/Code/Dialogue",
            "_Project/Code/ScriptableObjects",

            // ART
            "_Project/Art",
            "_Project/Art/Sprites",
            "_Project/Art/Animations",
            "_Project/Art/Materials",

            // AUDIO
            "_Project/Audio",
            "_Project/Audio/SFX",
            "_Project/Audio/Music",

            // SCRIPTABLE OBJECTS
            "_Project/ScriptableObjects",

            // PREFABS
            "_Project/Prefabs",
            "_Project/Prefabs/Player",
            "_Project/Prefabs/Enemies",
            "_Project/Prefabs/UI",
            "_Project/Prefabs/Items",
            "_Project/Prefabs/Environment",

            // SCENES
            "_Project/Scenes",
            "_Project/Scenes/TestScenes",

            // UI
            "_Project/UI",
            "_Project/UI/Fonts",
            "_Project/UI/Icons",

            // EXTERNAL
            "_ThirdParty"
        };

        Debug.Log("Starting to create project folders...");

        foreach (string folder in folders)
        {
            string path = Path.Combine(projectPath, folder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Debug.Log($"Created folder: {folder}");
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Project folder structure created successfully!");
    }
}