using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Pixelater : MonoBehaviour
{
    [Range(1, 100)]
    public int _pixelate = 1;

    private void Start()
    {
        SetResolution();
    }
    public void SetResolution()
    {
        int setWidth = 1280;
        int setHeight = 720;
        Screen.SetResolution(setWidth, setHeight, true);
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        source.filterMode = FilterMode.Point;
        RenderTexture resultTexture = RenderTexture.GetTemporary(source.width / _pixelate, source.height / _pixelate, 0, source.format);
        resultTexture.filterMode = FilterMode.Point;

        Graphics.Blit(source, resultTexture);
        Graphics.Blit(resultTexture, destination);
        RenderTexture.ReleaseTemporary(resultTexture);
    }
}