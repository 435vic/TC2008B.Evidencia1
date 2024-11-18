using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CodeScanner : MonoBehaviour
{
    public Camera qrCamera;
    public RenderTexture renderTexture;
    
    // Start is called before the first frame update
    void Start()
    {
        if (qrCamera.targetTexture == null)
        {
            qrCamera.targetTexture = renderTexture;
        }

        InvokeRepeating(nameof(ScanQRCode), 0f, 10f);
    }

    public void ScanQRCode()
    {
        StartCoroutine(CaptureAndDecode());
    }
    
    private IEnumerator CaptureAndDecode()
    {
        // Render the camera's view to the RenderTexture
        RenderTexture.active = renderTexture;
        qrCamera.Render();

        // Create a Texture2D from the RenderTexture
        Texture2D image = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        image.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        image.Apply();

        // Encode to PNG
        byte[] imageData = image.EncodeToPNG();

        // Send to Online API
        string apiUrl = "https://api.qrserver.com/v1/read-qr-code/";
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add( new MultipartFormFileSection("file", imageData, "file.png", "image/png"));
        UnityWebRequest request = UnityWebRequest.Post(apiUrl, formData);

        yield return request.SendWebRequest();

        // Handle response
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("QR Code Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error: " + request.downloadHandler.text);
        }

        // Clean up
        RenderTexture.active = null;
        Destroy(image);
    }
}
