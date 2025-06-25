// Created by SwanDEV 2019
using UnityEngine;

public class CameraOnUpdateRender : CameraRenderBase
{
    /// <summary>
    /// [Camera capture methods Only] The anti-aliasing level for the resulting texture, 
    /// the greater value results in the edges of the image look smoother. Available value: 1(OFF), 2, 4, 8.
    /// The greater value will increase the memory consumption for the result texture, please adjust the values as need.
    /// (This value will not be applied if your project AntiAliasing level is enabled and greater)
    /// </summary>
    public int m_AntiAliasingLevel = 4;

    private void Update()
    {
        OnUpdateRender();
    }

    private void OnUpdateRender()
    {
        if (_toDestroyScript) Destroy(this); // remove this script from the camera
        if (!_toCapture) return;
        _toCapture = false;

        if (rCamera == null) rCamera = GetComponent<Camera>();

        //Display display = Display.displays != null ? Display.displays[rCamera.targetDisplay] : null;
        //int W = display != null ? display.renderingWidth : Screen.width;
        //int H = display != null ? display.renderingHeight : Screen.height;
        int W, H;
        if (Display.displays != null && rCamera.targetDisplay < Display.displays.Length)
        {
            Display display = Display.displays[rCamera.targetDisplay];
            W = display.renderingWidth;
            H = display.renderingHeight;
        }
        else
        {
            W = rCamera.pixelWidth;
            H = rCamera.pixelHeight;
        }

        RenderTexture renderTexture = new RenderTexture(W, H, 24);
        m_AntiAliasingLevel = Mathf.Clamp(m_AntiAliasingLevel, 1, 8);
        if (m_AntiAliasingLevel == 3 || m_AntiAliasingLevel == 5) m_AntiAliasingLevel = 4; else if (m_AntiAliasingLevel == 6 || m_AntiAliasingLevel == 7) m_AntiAliasingLevel = 8;
        if (QualitySettings.antiAliasing < m_AntiAliasingLevel) renderTexture.antiAliasing = m_AntiAliasingLevel;
        rCamera.targetTexture = renderTexture;
        rCamera.Render();
        rCamera.targetTexture = null;

        bool customSize = _targetWidth > 0 && _targetHeight > 0;
        bool subScreenCam = rCamera.rect.width < 1f || rCamera.rect.height < 1f || rCamera.rect.x > 0f || rCamera.rect.y > 0f;
        if (subScreenCam || customSize || _scale != 1f)
        {
            int width = customSize ? _targetWidth : (int)(rCamera.pixelWidth * _scale);
            int height = customSize ? _targetHeight : (int)(rCamera.pixelHeight * _scale);
            _SystemTextureLimit(ref width, ref height);
            Vector2 targetSize = new Vector2(width, height);
            renderTexture = _CutOutAndCropRenderTexture(renderTexture, new Rect(Mathf.CeilToInt(rCamera.pixelRect.x), rCamera.pixelRect.y, rCamera.pixelRect.width, rCamera.pixelRect.height), targetSize, isFullScreen: false);
        }

        if (_onCaptureCallback != null)
        {
            _onCaptureCallback(_RenderTextureToTexture2D(renderTexture));
            _onCaptureCallback = null;
#if UNITY_EDITOR
            Debug.Log("OnUpdateRender - Texture2D * " + _scale);
#endif
        }
        else if (_onCaptureCallbackRTex != null)
        {
            _onCaptureCallbackRTex(renderTexture);
            _onCaptureCallbackRTex = null;
#if UNITY_EDITOR
            Debug.Log("OnUpdateRender - RenderTexture * " + _scale);
#endif
        }
    }
}
