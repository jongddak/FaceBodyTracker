using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARcamInput : MonoBehaviour
{
    [SerializeField] RawImage arRawImage; // AR 카메라 영상을 표시할 UI 요소
    [SerializeField] Texture staticInput;
    [SerializeField] Vector2 resolution = new Vector2(1920, 1080); // 해상도 지정 가능

    public Texture inputImageTexture
    {
        get
        {
            if (staticInput != null) return staticInput;
            return inputRT;
        }
    }

    ARCameraBackground arCameraBackground;
    RenderTexture inputRT; // RenderTexture 추가

    void Start()
    {
        // AR 카메라의 Background 컴포넌트를 가져오기
        arCameraBackground = FindObjectOfType<ARCameraBackground>();

        // RenderTexture 생성
        inputRT = new RenderTexture((int)resolution.x, (int)resolution.y, 0);

        if (arRawImage != null)
        {
            arRawImage.texture = inputRT; // RawImage에 RenderTexture 할당
        }

        if (arCameraBackground != null)
        {
            arCameraBackground.enabled = false; // AR 배경 비활성화
        }
    }

    void Update()
    {
        if (staticInput != null) return;

        if (arCameraBackground && arRawImage != null)
        {
            var mainTexture = arCameraBackground.material.mainTexture;

            if (mainTexture != null)
            {
                // AR 카메라의 텍스처를 RenderTexture에 블릿
                Graphics.Blit(mainTexture, inputRT);
            }
        }
    }

    void OnDestroy()
    {
        // RenderTexture 해제
        if (inputRT != null) Destroy(inputRT);
    }
}
