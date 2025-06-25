// Created by SwanDEV 2017
using UnityEngine;

public class CameraOnRender : CameraRenderBase
{
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination);

        if (_toDestroyScript) Destroy(this); // remove this script from the camera
        if (!_toCapture) return;
        _toCapture = false;

        RenderTexture renderTexture = null;
        Vector2 targetSize;
        bool customSize = _targetWidth > 0 && _targetHeight > 0;
        if (customSize)
        {
            _SystemTextureLimit(ref _targetWidth, ref _targetHeight);

            float W = Mathf.Max(_targetWidth, _targetHeight);
            float H = W;
            _CalSizeWithAspectRatio(ref W, ref H, new Vector2(source.width, source.height));
            float scale = Mathf.Max(_targetWidth / W, _targetHeight / H);
            W = Mathf.Round(W * scale);
            H = Mathf.Round(H * scale);

            renderTexture = new RenderTexture((int)W, (int)H, 24);
            targetSize = new Vector2(_targetWidth, _targetHeight);
        }
        else
        {
            int W = (int)(source.width * _scale);
            int H = (int)(source.height * _scale);
            _SystemTextureLimit(ref W, ref H);

            renderTexture = new RenderTexture(W, H, 24);
            targetSize = new Vector2(W, H);
        }

        if (_onCaptureCallback != null)
        {
            if (source.width != renderTexture.width || source.height != renderTexture.height)
            {
                Graphics.Blit(source, renderTexture);
                _onCaptureCallback(_CutOutAndCropTexture(renderTexture, new Rect(0, 0, renderTexture.width, renderTexture.height), targetSize, isFullScreen: true));
            }
            else
            {
                _onCaptureCallback(_CutOutAndCropTexture(source, new Rect(0, 0, renderTexture.width, renderTexture.height), targetSize, isFullScreen: true));
            }
            _onCaptureCallback = null;
            if (renderTexture) Destroy(renderTexture);
#if UNITY_EDITOR
            Debug.Log("OnRenderImage - Texture2D * " + _scale);
#endif
        }
        else if (_onCaptureCallbackRTex != null)
        {
            Graphics.Blit(source, renderTexture);
            _onCaptureCallbackRTex(_CutOutAndCropRenderTexture(renderTexture, new Rect(0, 0, renderTexture.width, renderTexture.height), targetSize, isFullScreen: true));
            _onCaptureCallbackRTex = null;
#if UNITY_EDITOR
            Debug.Log("OnRenderImage - RenderTexture * " + _scale);
#endif
        }
    }
}
