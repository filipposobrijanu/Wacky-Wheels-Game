// Created By SwanDEV 2021
using System.Collections;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Screenshot Helper codeless screenshot tool: capture images from the screen or using any specific camera in the scene.
/// [ HOW To USE ] Attach this script on a GameObject in the scene, or drag the CodelessScreenshotHelper prefab from the EditorExtensions folder to the scene.
/// </summary>
public class CodelessScreenshotHelper : MonoBehaviour
{
    [Header("[ Save Settings ]")]
    public FilePathName.AppPath m_SaveDirectory = FilePathName.AppPath.PersistentDataPath;
    public SaveFormat m_SaveFormat = SaveFormat.PNG;
    public enum SaveFormat
    {
        JPG = 0,
        PNG,
    }
    public string m_FolderName;
    [Tooltip("Optional filename without extension. (Will auto generate a filename base on date and time if this not provided)")]
    public string m_OptionalFileName;
    public string Save_FolderName { get { return m_FolderName; } set { m_FolderName = value; } }
    public string Save_OptionalFileName { get { return m_OptionalFileName; } set { m_OptionalFileName = value; } }
    //[Tooltip("Save image to External directory instead of in-app directory, the External directory can be accessed in Android Gallery or iOS Photos (Android & iOS only)")]
    //public bool m_SaveExternal = false;

    [Header("[ Burst Mode Capture Settings ]")]
    [Tooltip("The number of images to capture")]
    [Range(2, 999)] public int m_BurstCount = 10;
    [Tooltip("The burst interval (in seconds)")]
    [Range(0.05f, 10f)] public float m_BurstInterval = 0.1f;
    public float BurstCapture_Count { get { return m_AntiAliasingLevel; } set { m_AntiAliasingLevel = (int)value; } }
    public float BurstCapture_Interval { get { return m_ImageScale; } set { m_ImageScale = value; } }

    [Header("[ Screen Capture Settings ]")]
    [Tooltip("Capture fullscreen image. Ignore the screen position and size(width & height) values.")]
    public bool m_IsFullscreen = true;
    public Vector2 m_ScreenPosition = new Vector2(0, 0);
    public int m_Width = 360;
    public int m_Height = 360;
    public bool CaptureScreen_Fullscreen { get { return m_IsFullscreen; } set { m_IsFullscreen = value; } }
    public float CaptureScreen_PositionX { get { return m_ScreenPosition.x; } set { m_ScreenPosition.x = value; } }
    public float CaptureScreen_PositionY { get { return m_ScreenPosition.y; } set { m_ScreenPosition.y = value; } }
    public float CaptureScreen_Width { get { return m_Width; } set { m_Width = (int)value; } }
    public float CaptureScreen_Height { get { return m_Height; } set { m_Height = (int)value; } }

    [Header("[ Camera Capture Settings ]")]
    [Tooltip("The camera for capturing screenshot. Drag camera on this variable or click the 'Find Camera' button to setup.")]
    public Camera m_SelectedCamera;
    public Camera[] m_AllCameras;
    [Tooltip("The method for capturing image using camera. OnRenderImage: legacy mode for built-in render pipeline only; OnUpdateRender: universal mode suitable for all render pipelines.")]
    public ScreenshotHelper.RenderMode m_RenderMode = ScreenshotHelper.RenderMode.OnUpdateRender;
    [Tooltip("(For OnUpdateRender mode) The anti-aliasing level for the resulting texture, the greater value results in smoother object edges. Valid value: 1(OFF), 2, 4, 8")]
    [Range(1, 8)] public int m_AntiAliasingLevel = 4;
    [Range(0.1f, 4f)] public float m_ImageScale = 1.0f;
    public float CaptureCamera_AALevel { get { return m_AntiAliasingLevel; } set { m_AntiAliasingLevel = (int)value; } }
    public float CaptureCamera_Scale { get { return m_ImageScale; } set { m_ImageScale = value; } }

    /// <summary> 0: capture image from screen, 1: capture image from selected camera </summary>
    [HideInInspector] public int m_CaptureSourceIndex = 0;
    [HideInInspector] public int m_CurrCameraIndex = 0;

    [HideInInspector] public string m_BurstProgress = "0 / 0";
    [HideInInspector] public string m_BurstState = "Stopped";
    public string m_SavePath { get; private set; }

    public string GetSaveDirectory()
    {
        string directory = FilePathName.Instance.GetAppPath(m_SaveDirectory);
        return string.IsNullOrEmpty(m_FolderName) ? directory : Path.Combine(directory, m_FolderName);
    }

    public void FindCameras()
    {
        m_AllCameras = Camera.allCameras;

        if (m_AllCameras != null && m_AllCameras.Length > 0 && m_SelectedCamera == null)
        {
            m_SelectedCamera = m_AllCameras[0];
        }
    }

    public void SetCaptureSource(int index)
    {
        m_CaptureSourceIndex = index;
    }

    public void SetCamera(int index)
    {
        if (m_AllCameras == null || m_AllCameras.Length == 0) return;
        m_CurrCameraIndex = Mathf.Clamp(index, 0, m_AllCameras.Length - 1);
        if (m_CurrCameraIndex < m_AllCameras.Length) m_SelectedCamera = m_AllCameras[m_CurrCameraIndex];
    }

    public void Capture()
    {
        if (!Application.isPlaying) return;

        m_BurstState = "Stopped";

        if (m_CaptureSourceIndex == 0) // Capture from screen
        {
            if (!m_IsFullscreen && m_Width > 0 && m_Height > 0)
            {
                ScreenshotHelper.iCapture(m_ScreenPosition, new Vector2(m_Width, m_Height), (texture) =>
                {
                    _SaveTexture(texture);
                    ScreenshotHelper.iClear(false);
                });
            }
            else
            {
                ScreenshotHelper.iCaptureScreen((texture) =>
                {
                    _SaveTexture(texture);
                    ScreenshotHelper.iClear(false);
                });
            }
        }
        else // Capture from selected camera
        {
            if (m_SelectedCamera == null) FindCameras();
            ScreenshotHelper.AntiAliasingLevel = m_AntiAliasingLevel;
            ScreenshotHelper.SetRenderMode(m_RenderMode);

            ScreenshotHelper.iCaptureWithCamera(m_SelectedCamera, m_ImageScale, (texture) =>
            {
                _SaveTexture(texture);
                ScreenshotHelper.iClear(false);
            });
        }
    }

    public void BurstCapture()
    {
        if (!Application.isPlaying) return;
        _isBurstOnGoing = true;
        StartCoroutine(_BurstCapture());
    }

    public void StopBurstCapture()
    {
        if (!Application.isPlaying) return;
        _isBurstOnGoing = false;
    }

    private bool _isBurstOnGoing = false;
    private IEnumerator _BurstCapture()
    {
        for (int i = 0; i < m_BurstCount; i++)
        {
            if (!_isBurstOnGoing)
            {
                m_BurstState = "Stopped";
                m_BurstProgress = (i + 1) + " / " + m_BurstCount + " (Manually Stopped)";
                yield break;
            }
            Capture();
            m_BurstState = "On Going..";
            m_BurstProgress = (i + 1) + " / " + m_BurstCount;
            yield return new WaitForSeconds(m_BurstInterval);
        }
        m_BurstState = "Stopped";
    }

    private void _SaveTexture(Texture2D texture)
    {
        //if (m_SaveExternal)
        //{
        //    string fileName = string.IsNullOrEmpty(m_OptionalFileName.Trim()) ? FilePathName.Instance.GetFileNameWithoutExt(true) : m_OptionalFileName;
        //    string folderName = string.IsNullOrEmpty(m_FolderName.Trim()) ? Application.productName : m_FolderName;
        //    m_SavePath = MobileMedia.SaveImage(texture, folderName, fileName, m_SaveFormat == SaveFormat.JPG ? MobileMedia.ImageFormat.JPG : MobileMedia.ImageFormat.PNG);
        //}
        //else
        {
            m_SavePath = FilePathName.Instance.SaveTextureAs(texture, m_SaveDirectory, m_FolderName, (m_SaveFormat == SaveFormat.JPG), m_OptionalFileName);
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(CodelessScreenshotHelper))]
public class CodelessScreenshotHelperCustomEditor : Editor
{
    private static string[] cameraOptions = new string[] { };
    private static string[] captureSources = new string[] { "From Screen", "From Camera" };

    private static bool showHelpsMessage = false;
    private string helpsMessage = "[ HOW ] Attach the CodelessScreenshotHelper script on a GameObject in the scene, or drag the CodelessScreenshotHelper prefab from the EditorExtensions folder to the scene."
        + "\n\n(1) Select a capture source: from Screen or Camera."
        + "\n(2) Start capture images by clicking the 'Capture', 'Start Captures' buttons."
        + "\n\n( What else? Modify other settings as appropriate. And, you can also reference the methods and dynamic parameters in the CodelessScreenshotHelper to your UI components like Button, Slider, InputField, and Toggle, etc. in the scene, this allows you to take screenshots at runtime in your app )"
        + "\n\n* If you are using the CodelessScreenshot prefab in your build, you may click the 'Apply All' button on the prefab inspector to save the prefab after changing the setting value(s), or just 'Unpack' the prefab. (This prevents the prefab from reverting to its last saved status)";

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CodelessScreenshotHelper mono = (CodelessScreenshotHelper)target;

        mono.m_CurrCameraIndex = GUILayout.SelectionGrid(mono.m_CurrCameraIndex, cameraOptions, 2);
        mono.SetCamera(mono.m_CurrCameraIndex);

        GUILayout.Label("Find all Cameras in the scene:");
        if (GUILayout.Button("Find Cameras"))
        {
            _SetupCamera(mono);
        }

        GUILayout.Label("\n\nSelect Source: capture image(s) from " + (mono.m_CaptureSourceIndex == 0 ? "Screen" : "Camera"));
        mono.m_CaptureSourceIndex = GUILayout.SelectionGrid(mono.m_CaptureSourceIndex, captureSources, 2);
        mono.SetCaptureSource(mono.m_CaptureSourceIndex);

        GUILayout.Label("[ Start Capture Image ]");
        if (GUILayout.Button("Capture (Single)") && Application.isPlaying)
        {
            _SetupCamera(mono);
            mono.Capture();
        }

        GUILayout.Label(" Burst Mode ( Count: " + mono.m_BurstCount + ", Interval: " + mono.m_BurstInterval + "s )");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Start Captures (Burst)") && Application.isPlaying)
        {
            _SetupCamera(mono);
            mono.BurstCapture();
        }
        if (GUILayout.Button("Stop Captures (Burst)"))
        {
            // Stop the burst mode capture.
            mono.StopBurstCapture();
        }
        GUILayout.EndHorizontal();

        GUILayout.Label("Burst Progress: " + mono.m_BurstProgress
            + "\nBurst State: " + mono.m_BurstState
            + "\n\nLast File Name: " + Path.GetFileName(mono.m_SavePath) + "\n");


        if (GUILayout.Button("View Image"))
        {
            if (!Application.isPlaying || string.IsNullOrEmpty(mono.m_SavePath))
            {
                Debug.Log("Please play your scene and capture image in the Editor.");
                return;
            }
            _OpenURL(mono.m_SavePath);
        }

        if (GUILayout.Button("Copy Image Path"))
        {
            if (!Application.isPlaying || string.IsNullOrEmpty(mono.m_SavePath))
            {
                Debug.Log("Please play your scene and capture image in the Editor.");
                return;
            }
            TextEditor te = new TextEditor();
            te.text = mono.m_SavePath;
            te.SelectAll();
            te.Copy();
            Debug.Log("Copied: " + te.text);
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Show Save Directory"))
        {
            string directory = mono.GetSaveDirectory();
            if (string.IsNullOrEmpty(directory)) return;
            if (Directory.Exists(directory))
                EditorUtility.RevealInFinder(directory);
            else
                Debug.LogWarning("Directory not exist: " + directory);
        }

        if (GUILayout.Button("Copy Save Directory"))
        {
            string directory = mono.GetSaveDirectory();
            if (string.IsNullOrEmpty(directory)) return;
            TextEditor te = new TextEditor();
            te.text = directory;
            te.SelectAll();
            te.Copy();
            Debug.Log("Copied: " + directory);
        }


        GUILayout.Space(10);

        bool isLightSkin = !EditorGUIUtility.isProSkin;
        Color tipTextColor = isLightSkin ? new Color(0.12f, 0.12f, 0.12f, 1f) : Color.cyan;
        GUIStyle helpBoxStyle = new GUIStyle(EditorStyles.textArea);
        helpBoxStyle.normal.textColor = tipTextColor;

        GUIStyle tipsStyle = new GUIStyle(EditorStyles.boldLabel);
        tipsStyle.normal.textColor = tipTextColor;

        showHelpsMessage = GUILayout.Toggle(showHelpsMessage, " Help (How To Use? Click here...)", tipsStyle);
        if (showHelpsMessage) GUILayout.Label(helpsMessage, helpBoxStyle);

        GUILayout.Space(10);
        if (GUILayout.Button("Write A Review (THANK YOU)"))
        {
            _OpenURL("https://www.swanob2.com/screenshot-helper");
        }
    }

    private void _OpenURL(string url)
    {
#if UNITY_EDITOR_OSX
        System.Diagnostics.Process.Start(url);
#else
        Application.OpenURL(url);
#endif
    }

    private void _SetupCamera(CodelessScreenshotHelper mono)
    {
        mono.FindCameras();
        _SetCameraOptions(Camera.allCameras, mono);
    }

    private void _SetCameraOptions(Camera[] cameras, CodelessScreenshotHelper mono)
    {
        mono.m_CurrCameraIndex = 0;
        cameraOptions = new string[cameras.Length];
        for (int i = 0; i < cameras.Length; i++)
        {
            cameraOptions[i] = cameras[i].name;
            if (mono.m_SelectedCamera && mono.m_SelectedCamera == cameras[i]) mono.m_CurrCameraIndex = i;
        }
    }
}
#endif
