#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class FixMergeConflicts
{
    // This creates a new button at the very top of your Unity window!
    [MenuItem("Tools/Fix Scene Merge Conflicts")]
    public static void FixConflicts()
    {
        // The exact path based on your git error logs
        string path = "Assets/Scenes/khalilMap1.scene";

        if (!File.Exists(path))
        {
            Debug.LogError("Could not find the scene file. Make sure you are in the JockeyRun Unity project.");
            return;
        }

        string[] lines = File.ReadAllLines(path);
        List<string> newLines = new List<string>();
        
        bool insideConflict = false;
        bool insideTheirs = false;
        int removedCount = 0;

        // Hunt down the Git markers line by line
        foreach (string line in lines)
        {
            if (line.StartsWith("<<<<<<<"))
            {
                insideConflict = true;
                insideTheirs = false;
                removedCount++;
                continue;
            }
            if (line.StartsWith("======="))
            {
                insideTheirs = true;
                continue;
            }
            if (line.StartsWith(">>>>>>>"))
            {
                insideConflict = false;
                insideTheirs = false;
                continue;
            }

            if (!insideConflict)
            {
                newLines.Add(line);
            }
            else if (insideConflict && !insideTheirs)
            {
                // Keep your local changes (the new white, grey, and brown horses!)
                newLines.Add(line);
            }
            // If it is remote changes from GitHub, we silently drop them
        }

        // Save the cleaned file
        if (removedCount > 0)
        {
            File.WriteAllLines(path, newLines.ToArray());
            Debug.Log($"Success! Surgically removed {removedCount} merge conflict blocks and saved your horses.");
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.Log("No merge conflict markers were found in the file.");
        }
    }
}
#endif