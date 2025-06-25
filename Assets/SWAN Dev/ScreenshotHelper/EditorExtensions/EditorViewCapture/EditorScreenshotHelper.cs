#if UNITY_EDITOR

using System.IO;
using UnityEngine;
using UnityEditor;

namespace SDev.EditorUtil
{
    /// <summary>
    /// Take screenshots of editor Windows.
    /// Two ways to take screenshots with this script:
    /// In the editor, select your scene view, game view, any editor window or inspector tab...
    /// (Method 1) Tools > SWAN DEV > SSH Screenshot, or 
    /// (Method 2) Hotkeys: Shift + W
    /// </summary>
    public class EditorScreenshotHelper : MonoBehaviour
    {
        [MenuItem("Tools/SWAN DEV/SSH Screenshot (Shift+W) #w")] // %# : ctrl/cmd + shift
        private static void Screenshot()
        {
            // Get actvive EditorWindow
            EditorWindow activeWindow = EditorWindow.focusedWindow;
            if (activeWindow == null) return;

            // Get screen position and sizes
            Vector2 vec2Position = activeWindow.position.position;
            float sizeX = activeWindow.position.width;
            float sizeY = activeWindow.position.height;

            // Take Screenshot at given position and size
            Color[] colors = UnityEditorInternal.InternalEditorUtility.ReadScreenPixel(vec2Position, (int)sizeX, (int)sizeY);

            // Write result Color[] data into a Texture2D
            Texture2D result = new Texture2D((int)sizeX, (int)sizeY);
            result.SetPixels(colors);

            // Encode the Texture2D to a PNG, 
            // you might want to change this to JPG for way less file size but slightly worse quality
            // if you do don't forget to also change the file extension below
            byte[] bytes = result.EncodeToPNG();

            // In order to avoid bloading Texture2D into memory destroy it
            Object.DestroyImmediate(result);

            // Write the file to a folder in the project
            System.DateTime DT = System.DateTime.Now;
            string timeString = string.Format("_{0}-{1:00}-{2:00}_{3:00}-{4:00}-{5:00}-{6:000}", DT.Year, DT.Month, DT.Day, DT.Hour, DT.Minute, DT.Second, DT.Millisecond);

            string directory = Path.Combine(Application.dataPath, "Screenshots");
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            string savePath = Path.Combine(directory, "Screenshot" + timeString + ".png");
            File.WriteAllBytes(savePath, bytes);

            // Refresh the AssetsDatabase so the file actually appears in Unity
            AssetDatabase.Refresh();

            Debug.Log("Screenshot Hepler > New Screenshot saved (see 'Screenshots' folder in the project):\n" + savePath);
        }
    }
}
#endif
