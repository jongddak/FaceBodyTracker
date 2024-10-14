using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARcamInput : MonoBehaviour
{
    [SerializeField] RawImage arRawImage; // AR ī�޶� ������ ǥ���� UI ���
    [SerializeField] Texture staticInput;
    [SerializeField] Vector2 resolution = new Vector2(1920, 1080); // �ػ� ���� ����

    public Texture inputImageTexture
    {
        get
        {
            if (staticInput != null) return staticInput;
            return inputRT;
        }
    }

    ARCameraBackground arCameraBackground;
    RenderTexture inputRT; // RenderTexture �߰�

    void Start()
    {
        // AR ī�޶��� Background ������Ʈ�� ��������
        arCameraBackground = FindObjectOfType<ARCameraBackground>();

        // RenderTexture ����
        inputRT = new RenderTexture((int)resolution.x, (int)resolution.y, 0);

        if (arRawImage != null)
        {
            arRawImage.texture = inputRT; // RawImage�� RenderTexture �Ҵ�
        }

        if (arCameraBackground != null)
        {
            arCameraBackground.enabled = false; // AR ��� ��Ȱ��ȭ
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
                // AR ī�޶��� �ؽ�ó�� RenderTexture�� ��
                Graphics.Blit(mainTexture, inputRT);
            }
        }
    }

    void OnDestroy()
    {
        // RenderTexture ����
        if (inputRT != null) Destroy(inputRT);
    }
}
