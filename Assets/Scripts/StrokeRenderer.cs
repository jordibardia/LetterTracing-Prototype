using System.IO;
using UnityEngine;

public class StrokeRenderer : MonoBehaviour
{
    public Camera renderCamera;

    public int textureWidth = 28;
    public int textureHeight = 28;

    public string savePath = "output.jpg";

    private float paddingFactor = 1.1f;

    public void RenderStrokeToImage(Bounds strokeBounds)
    {
        AdjustCameraToBounds(strokeBounds);

        RenderTexture renderTexture = new RenderTexture(textureWidth, textureHeight, 24);
        renderCamera.targetTexture = renderTexture;

        renderCamera.Render();

        Texture2D texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, textureWidth, textureHeight), 0, 0);
        texture.Apply();

        byte[] bytes = texture.EncodeToJPG();
        File.WriteAllBytes(savePath, bytes);

        Debug.Log($"Saved image");

        // Cleanup
        RenderTexture.active = null;
        renderCamera.targetTexture = null;
        Destroy(renderTexture);
    }

    private void AdjustCameraToBounds(Bounds strokeBounds)
    {
        float maxSize = Mathf.Max(strokeBounds.size.x, strokeBounds.size.y) * paddingFactor;

        renderCamera.transform.position = new Vector3(strokeBounds.center.x, strokeBounds.center.y, renderCamera.transform.position.z);
        renderCamera.orthographic = true;
        renderCamera.orthographicSize = maxSize / 2.0f;
    }
}
