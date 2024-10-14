using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class mobileCam : MonoBehaviour
{
    [SerializeField] string webCamName;
    [SerializeField] Texture staticInput;

    // Provide input image Texture.
    public Texture inputImageTexture
    {
        get
        {
            if (staticInput != null) return staticInput;
            return inputRT;
        }
    }

    WebCamTexture webCamTexture;
    RenderTexture inputRT;

    void Start()
    {
        if (staticInput == null)
        {
            // 모바일 기기의 전면 카메라 선택
            foreach (var device in WebCamTexture.devices)
            {
                if (device.isFrontFacing)
                {
                    webCamName = device.name;
                    break;
                }
            }

            // 해상도를 명시하지 않음으로 기본 해상도 사용
            webCamTexture = new WebCamTexture(webCamName);
            webCamTexture.Play();
        }

        // WebCamTexture의 실제 해상도에 맞춰 RenderTexture 생성
        inputRT = new RenderTexture(webCamTexture.width, webCamTexture.height, 0);
    }

    void Update()
    {
        if (staticInput != null) return;
        if (!webCamTexture.didUpdateThisFrame) return;

        var aspect1 = (float)webCamTexture.width / webCamTexture.height;
        var aspect2 = (float)inputRT.width / inputRT.height;
        var aspectGap = aspect2 / aspect1;

        var vMirrored = webCamTexture.videoVerticallyMirrored;
        var scale = new Vector2(aspectGap, vMirrored ? -1 : 1);
        var offset = new Vector2((1 - aspectGap) / 2, vMirrored ? 1 : 0);

        Graphics.Blit(webCamTexture, inputRT, scale, offset);
    }

    void OnDestroy()
    {
        if (webCamTexture != null) Destroy(webCamTexture);
        if (inputRT != null) Destroy(inputRT);
    }
}