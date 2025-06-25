// Created by SwanDEV 2019
using UnityEngine;
using System;

public class CameraRenderBase : MonoBehaviour
{
    protected Action<Texture2D> _onCaptureCallback = null;
    protected Action<RenderTexture> _onCaptureCallbackRTex = null;

    protected bool _toCapture = true;
    protected int _targetWidth = 0;
    protected int _targetHeight = 0;

    /// <summary> A value for scaling the texture. (Scale the image size down to the minimum of 0.1X and up to 4X, the maximum texture size depends on device GPU) </summary>
    protected float _scale = 1f;

    protected Camera rCamera;

    public bool _toDestroyScript;
    
    private bool _destroySourceOnConverted = true;

    private void _Init()
    {
        _toCapture = true;
        _toDestroyScript = false;
    }

    public void SetOnCaptureCallback(Action<Texture2D> onCaptured, float scale)
    {
        _onCaptureCallback = onCaptured;
        _onCaptureCallbackRTex = null;
        _scale = Mathf.Clamp(scale, 0.1f, 4f);
        _targetWidth = 0;
        _targetHeight = 0;
        _Init();
    }

    public void SetOnCaptureCallback(Action<Texture2D> onCaptured, int width = 0, int height = 0)
    {
        _onCaptureCallback = onCaptured;
        _onCaptureCallbackRTex = null;
        _scale = 1f;
        _targetWidth = width;
        _targetHeight = height;
        _Init();
    }

    public void SetOnCaptureCallback(Action<RenderTexture> onCaptured, float scale)
    {
        _onCaptureCallback = null;
        _onCaptureCallbackRTex = onCaptured;
        _scale = Mathf.Clamp(scale, 0.1f, 4f);
        _targetWidth = 0;
        _targetHeight = 0;
        _Init();
    }

    public void SetOnCaptureCallback(Action<RenderTexture> onCaptured, int width = 0, int height = 0)
    {
        _onCaptureCallback = null;
        _onCaptureCallbackRTex = onCaptured;
        _scale = 1f;
        _targetWidth = width;
        _targetHeight = height;
        _Init();
    }

    protected Texture2D _RenderTextureToTexture2D(RenderTexture source)
    {
        RenderTexture.active = source;
        Texture2D tex = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false); // 24
        tex.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;

        if (_destroySourceOnConverted) Destroy(source);
        return tex;
    }

    protected Texture2D _CutOutAndCropTexture(RenderTexture source, Rect cutoutRect, Vector2 cropAspectRatio, bool isFullScreen)
    {
        float W = cutoutRect.width, H = cutoutRect.height;
        _CalSizeWithAspectRatio(ref W, ref H, cropAspectRatio);
        W = (int)W;
        H = (int)H;

        RenderTexture.active = source;
        Texture2D texture2D = new Texture2D((int)W, (int)H, TextureFormat.RGBA32, false); // 24
        if (isFullScreen)
        {
            texture2D.ReadPixels(new Rect((source.width - texture2D.width) / 2, (source.height - texture2D.height) / 2, texture2D.width, texture2D.height), 0, 0);
        }
        else
        {
            if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2
                || SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3
                || SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLCore
                || SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Metal)
            {   // Origin(0, 0) is at the bottom left corner
                texture2D.ReadPixels(new Rect(cutoutRect.x, cutoutRect.y, W, H), 0, 0);
            }
            else // DX, Vulkan
            {   // Origin(0, 0) is at the top left corner
                texture2D.ReadPixels(new Rect((cutoutRect.x + (cutoutRect.width - W) / 2f), (source.height - cutoutRect.y - cutoutRect.height + (cutoutRect.height - H) / 2f), W, H), 0, 0);
            }
        }

        texture2D.Apply();
        RenderTexture.active = null;

        if (_destroySourceOnConverted) Destroy(source);
        return texture2D;
    }

    protected RenderTexture _CutOutAndCropRenderTexture(RenderTexture source, Rect rect, Vector2 targetSize, bool isFullScreen)
    {
        RenderTexture rt = new RenderTexture((int)targetSize.x, (int)targetSize.y, 24);
        Texture2D temp = _CutOutAndCropTexture(source, rect, targetSize, isFullScreen);
        Graphics.Blit(temp, rt);
        Destroy(temp);
        return rt;
    }

    public void Clear()
    {
        _onCaptureCallback = null;
        _onCaptureCallbackRTex = null;
        _toCapture = false;
        _toDestroyScript = true;
    }

    protected static void _CalSizeWithAspectRatio(ref float originWidth, ref float originHeight, Vector2 targetAspectRatio)
    {
        float W = originWidth;
        float H = originHeight;
        float originRatio = W / H;
        float targetRatio = targetAspectRatio.x / targetAspectRatio.y;

        if (originRatio > targetRatio)
        {
            if (targetRatio == 1)
            {   // 1:1 image
                if (W > H)  W = H; else if (H > W) H = W;
            }
            else W = H * targetRatio;
        }
        else if (originRatio < targetRatio)
        {
            if (targetRatio == 1)
            {   // 1:1 image
                if (W > H) W = H; else if (H > W) H = W;
            }
            else H = W / targetRatio;
        }

        originWidth = W;
        originHeight = H;
    }

    protected void _SystemTextureLimit(ref int width, ref int height)
    {
        float maxSize = SystemInfo.maxTextureSize;
        if (width > maxSize || height > maxSize)
        {
            if (width > height)
            {
                height = (int)(height * (maxSize / width));
                width = (int)maxSize;
            }
            else if (width < height)
            {
                width = (int)(width * (maxSize / height));
                height = (int)maxSize;
            }
            else width = height = (int)maxSize;
#if UNITY_EDITOR
            Debug.Log("The target texture size is larger than the max that supports by the current device. Limited to the max size : " + maxSize);
#endif
        }
    }
}
