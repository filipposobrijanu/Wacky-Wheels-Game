
using UnityEngine;

public class MinimumDemo : MonoBehaviour
{
    public Camera m_Camera;
    public MeshRenderer m_CubeMeshRenderer;

    [Space]
    public SDev.FileSaveUtil.AppPath ApplicationPath = SDev.FileSaveUtil.AppPath.PersistentDataPath;
    public string SubFolderName;
    public string FileName;

    public void TakeScreenshot()
    {
        ScreenshotHelper.iClear(); // Clear the old texture (if any) 
        ScreenshotHelper.iCaptureScreen((texture2D) =>
        {
            // Set the new (captured) screenshot texture to the cube renderer.
            m_CubeMeshRenderer.material.mainTexture = texture2D;

            SaveTexture(texture2D);
        });
    }

    public void CaptureWithCamera()
    {
        ScreenshotHelper.iClear(); // Clear the old texture (if any) 
        ScreenshotHelper.iCaptureWithCamera(m_Camera, (texture2D) =>
        {
            // Set the new (captured) screenshot texture to the cube renderer.
            m_CubeMeshRenderer.material.mainTexture = texture2D;

            SaveTexture(texture2D);
        });
    }

    private void SaveTexture(Texture2D texture2D)
    {
        // Example: Save to Application data path
        string savePath = SDev.FileSaveUtil.Instance.SaveTextureAsJPG(texture2D, ApplicationPath, SubFolderName, FileName);
        //string savePath = SDev.FileSaveUtil.Instance.SaveTextureAsJPG(texture2D, System.Environment.SpecialFolder.MyPictures, SubFolderName, FileName);
        Debug.Log("Result - Texture resolution: " + texture2D.width + " x " + texture2D.height + "\nSaved at: " + savePath);

        // Example: Save to mobile device gallery(iOS/Android). <- Requires Mobile Media Plugin (Included in Screenshot Helper Plus, and SwanDev GIF Assets)
        //MobileMedia.SaveImage(texture2D, SubFolderName, FileName, MobileMedia.ImageFormat.JPG);
    }

    public void Clear()
    {
        ScreenshotHelper.iClear();
    }
}
