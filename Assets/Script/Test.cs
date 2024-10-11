using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe;
using Mediapipe.BlazePose;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class Test : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] Transform Head;  // 0  �Ӹ� 

    [SerializeField] Transform Rightarm;   // 12 14 16  ������ 
    [SerializeField] Transform Rightforearm;
    [SerializeField] Transform Righthand;// �ո�

    [SerializeField] Transform Leftarm;   // 11 13 15  ����
    [SerializeField] Transform Leftforearm;
    [SerializeField] Transform Lefthand;


    [SerializeField] Transform Rightupleg;  // 24 26 28  ���� �ٸ� 
    [SerializeField] Transform Rightleg;
    [SerializeField] Transform Rightfoot;


    [SerializeField] Transform Leftupleg;  // 23 25 27  �޴ٸ�
    [SerializeField] Transform Leftleg;
    [SerializeField] Transform Leftfoot;

    [SerializeField] WebCamInput WebCamInput;



    [SerializeField] RawImage inputImageUI;  // ȭ��� ���̴� ķ�� 

    [SerializeField] bool OnFps;
    private float deltaTime = 0f;
    // Ʈ��Ŀ�� 
    [SerializeField] bool Ontracker;
    [SerializeField] Camera mainCamera;
    Material material;
    [SerializeField] Shader shader;
    [SerializeField, Range(0, 1)] float humanExistThreshold = 0.5f;
    readonly List<Vector4> linePair = new List<Vector4>
    {
        new Vector4(0, 1), new Vector4(1, 2), new Vector4(2, 3), new Vector4(3, 7), new Vector4(0, 4),
        new Vector4(4, 5), new Vector4(5, 6), new Vector4(6, 8), new Vector4(9, 10), new Vector4(11, 12),
        new Vector4(11, 13), new Vector4(13, 15), new Vector4(15, 17), new Vector4(17, 19), new Vector4(19, 15),
        new Vector4(15, 21), new Vector4(12, 14), new Vector4(14, 16), new Vector4(16, 18), new Vector4(18, 20),
        new Vector4(20, 16), new Vector4(16, 22), new Vector4(11, 23), new Vector4(12, 24), new Vector4(23, 24),
        new Vector4(23, 25), new Vector4(25, 27), new Vector4(27, 29), new Vector4(29, 31), new Vector4(31, 27),
        new Vector4(24, 26), new Vector4(26, 28), new Vector4(28, 30), new Vector4(30, 32), new Vector4(32, 28)
    };


    [SerializeField] Vector3[] worldpreviousLandmarks;// ���� �������� ��ǥ ����

  //  [SerializeField] Transform[] bodyjoints;// ����
  //  [SerializeField] Vector3[] curPos; // ���������� �� 

    private const float positionThreshold = 0.001f;
    private BlazePoseDetecter detecter;
    private void Start()
    {
        Ontracker = false;
        OnFps = false;
        detecter = new BlazePoseDetecter();
        worldpreviousLandmarks = new Vector3[33];

        material = new Material(shader);
    }
    private void Update()  // �ϴ��� Ʈ��ŷ ������ ������Ʈ���� �ҷ������� ���ɿ� ������ ���� ��ħ ����� ����
    {
        detecter.ProcessImage(WebCamInput.inputImageTexture);
        inputImageUI.texture = WebCamInput.inputImageTexture;

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f; // ������ ī���Ϳ�

        // ���帶ũ�� �����Ӵ� �������� ���̸� �޾Ƽ� ������ ������ ���������� ��ȯ�ؾ��� 
        // �����Ӵ� ������ , ���帶ũ�� 0 ~ 32 ���� 33��  
        //https://ai.google.dev/edge/mediapipe/solutions/vision/pose_landmarker?hl=ko
        // ��� �� ���帶ũ��  0(�Ӹ�) , 11 13 15(����) , 12 14 16(������) , 23 25 27(�޴ٸ�) , 24 26 28(�����ٸ�)

        //1. ��ķ ��°����� 
        //2. ���� ��°����� 3d �� ������



        for (int i = 0; i < detecter.vertexCount; i++)  //detecter.vertexCount = 33
        {
            worldpreviousLandmarks[i] = detecter.GetPoseWorldLandmark(i);  // ������� ������ ��ǥ�� 
        }


    }

    void OnAnimatorIK(int layerIndex)
    {
        // �������� Ư�� ��ǥ ��ġ�� �̵�
        animator.SetIKPosition(AvatarIKGoal.RightHand, worldpreviousLandmarks[16]);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f); 
        // ������ ȸ�� ����
        Quaternion rightHandRotation = Quaternion.LookRotation(worldpreviousLandmarks[16]);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandRotation);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f); 

        
        // �޼�
        animator.SetIKPosition(AvatarIKGoal.LeftHand, worldpreviousLandmarks[15]);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f); 

        Quaternion lefttHandRotation = Quaternion.LookRotation(worldpreviousLandmarks[15]);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, rightHandRotation);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f); 




    }
    private void OnAnimatorMove()
    {
        animator.GetBoneTransform(HumanBodyBones.RightLowerArm).transform.position = worldpreviousLandmarks[14];
        animator.GetBoneTransform(HumanBodyBones.RightUpperArm).transform.position = worldpreviousLandmarks[16];

        animator.GetBoneTransform(HumanBodyBones.LeftLowerArm).transform.position = worldpreviousLandmarks[13];
        animator.GetBoneTransform(HumanBodyBones.LeftUpperArm).transform.position = worldpreviousLandmarks[15];
    }


    private void OnApplicationQuit()
    {
        detecter.Dispose();
    }
    public void OnoffFps()  // ui ��ư���� Ʈ��Ŀ �¿��� 
    {
        if (OnFps == true)
        {
            OnFps = false;
        }
        else if (OnFps == false)
        {
            OnFps = true;
        }
    }

    private void OnGUI()
    {
        if (OnFps == true)
        {
            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(30, 30, Screen.width, Screen.height);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = 25;
            style.normal.textColor = Color.green;

            float ms = deltaTime * 1000f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.} FPS ({1:0.0} ms)", fps, ms);

            GUI.Label(rect, text, style);
        }
    }


    public void OnoffTracker()  // ui ��ư���� Ʈ��Ŀ �¿��� 
    {
        if (Ontracker == true)
        {
            Ontracker = false;
        }
        else if (Ontracker == false) 
        {
            Ontracker = true;   
        }
    }

    void OnRenderObject()  // Ʈ��ŷ on . off �ɼ����θ� ���̰� 
    {
        if (Ontracker == true)
        {
            material.SetBuffer("_worldVertices", detecter.worldLandmarkBuffer);

            material.SetInt("_keypointCount", detecter.vertexCount);
            material.SetFloat("_humanExistThreshold", humanExistThreshold);
            material.SetVectorArray("_linePair", linePair);
            material.SetMatrix("_invViewMatrix", mainCamera.worldToCameraMatrix.inverse);


            material.SetPass(2);
            Graphics.DrawProceduralNow(MeshTopology.Triangles, 6, 35);

            material.SetPass(3);
            Graphics.DrawProceduralNow(MeshTopology.Triangles, 6, detecter.vertexCount);
        }
    }
}
