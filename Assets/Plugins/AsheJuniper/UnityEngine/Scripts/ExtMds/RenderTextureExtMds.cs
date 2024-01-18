using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RenderTextureExtMds
{
    public static Texture2D ToTexture2D(
        this RenderTexture renderTexture,
        TextureFormat format = TextureFormat.RGB24
    )
    {
        Texture2D texture = new Texture2D(
            renderTexture.width,
            renderTexture.height,
            format,
            false
        );

        RenderTexture.active = renderTexture;

        var area = new Rect(
            0,
            0,
            renderTexture.width,
            renderTexture.height
        );

        texture.ReadPixels(
            area,
            0,
            0
        );

        texture.Apply();

        return texture;
    }
}
