using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe;
using Mediapipe.BlazePose;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class ARTest : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] Transform Head;  // 0  머리 

    [SerializeField] Transform Rightarm;   // 12 14 16  오른팔 
    [SerializeField] Transform Rightforearm;
    [SerializeField] Transform Righthand;// 손목

    [SerializeField] Transform Leftarm;   // 11 13 15  왼팔
    [SerializeField] Transform Leftforearm;
    [SerializeField] Transform Lefthand;


    [SerializeField] Transform Rightupleg;  // 24 26 28  오른 다리 
    [SerializeField] Transform Rightleg;
    [SerializeField] Transform Rightfoot;


    [SerializeField] Transform Leftupleg;  // 23 25 27  왼다리
    [SerializeField] Transform Leftleg;
    [SerializeField] Transform Leftfoot;

    
    [SerializeField] ARcamInput ARcamInput;



    [SerializeField] RawImage inputImageUI;  // 화면상에 보이는 캠용 

    [SerializeField] bool OnFps;
    private float deltaTime = 0f;
    // 트래커용 
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


   [SerializeField] Vector3[] worldpreviousLandmarks;// 이전 프레임의 좌표 저장


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
    private void Update()  // 일단은 트래킹 정보를 업데이트에서 불러오지만 성능에 영향을 많이 끼침 고려할 문제
    {
        
        detecter.ProcessImage(ARcamInput.inputImageTexture);
        

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f; // 프레임 카운터용

        // 랜드마크의 프레임당 움직임의 차이를 받아서 매핑한 관절의 움직임으로 변환해야함 
        // 프레임당 움직임 , 랜드마크는 0 ~ 32 까지 33개  
        //https://ai.google.dev/edge/mediapipe/solutions/vision/pose_landmarker?hl=ko
        // 사용 할 랜드마크는  0(머리) , 11 13 15 17 19 (왼팔) , 12 14 16 18 20(오른팔) , 23 25 27(왼다리) , 24 26 28(오른다리)
        // 17 19 왼손
        // 18 20 오른손   양손은 핸드 트래커가 추가로 필요 할 듯  포즈 트래커로는 안됨 
        // 더 자연스러운 움직임을 구현하려면 목 , 척추가 필요함
        // 목과 척추 좌표는 blazepose에서 제공하지 않기 때문에 따로 만들어야 할듯 
        // 목은 양 어깨의 중간 척추는 양 고관절 중간값 (좌표 + 좌표) / 2.0f 하고 y값만 조금 올리면 될듯
        // 가상의 지점을 만들어서 애니메이션 지정하면 될거같음 
        //1. 웹캠 출력값저장 
        //2. 얻은 출력값으로 3d 모델 움직임



        for (int i = 0; i < detecter.vertexCount; i++)  //detecter.vertexCount = 33
        {
            worldpreviousLandmarks[i] = detecter.GetPoseWorldLandmark(i);  // 월드기준 보정된 좌표임 
        }


    }

    void OnAnimatorIK(int layerIndex)
    {   
        

        // 오른손
        animator.SetIKPosition(AvatarIKGoal.RightHand, worldpreviousLandmarks[16]);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f); 
        
        Quaternion rightHandRotation = Quaternion.LookRotation(worldpreviousLandmarks[16]);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandRotation);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f); 

        
        // 왼손
        animator.SetIKPosition(AvatarIKGoal.LeftHand, worldpreviousLandmarks[15]);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f); 

        Quaternion lefttHandRotation = Quaternion.LookRotation(worldpreviousLandmarks[15]);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, rightHandRotation);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);


        // 오른 다리 
        animator.SetIKPosition(AvatarIKGoal.RightFoot, worldpreviousLandmarks[28]);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1.0f);

        Quaternion RightFootRotation = Quaternion.LookRotation(worldpreviousLandmarks[28]);
        animator.SetIKRotation(AvatarIKGoal.RightFoot, RightFootRotation);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1.0f);

        // 왼 다리
        animator.SetIKPosition(AvatarIKGoal.LeftFoot, worldpreviousLandmarks[27]);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1.0f);

        Quaternion LeftFootRotation = Quaternion.LookRotation(worldpreviousLandmarks[27]);
        animator.SetIKRotation(AvatarIKGoal.LeftFoot, LeftFootRotation);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1.0f);





    }
    private void OnAnimatorMove()
    {
        
        animator.GetBoneTransform(HumanBodyBones.RightLowerArm).transform.position = worldpreviousLandmarks[14];  // 오른팔
        animator.GetBoneTransform(HumanBodyBones.RightUpperArm).transform.position = worldpreviousLandmarks[16];

        animator.GetBoneTransform(HumanBodyBones.LeftLowerArm).transform.position = worldpreviousLandmarks[13];  // 왼팔 
        animator.GetBoneTransform(HumanBodyBones.LeftUpperArm).transform.position = worldpreviousLandmarks[15];


        animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).transform.position = worldpreviousLandmarks[24];  // 오른다리
        animator.GetBoneTransform(HumanBodyBones.RightUpperLeg).transform.position = worldpreviousLandmarks[26];

        animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).transform.position = worldpreviousLandmarks[23];  // 왼다리
        animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg).transform.position = worldpreviousLandmarks[25];
    }


    private void OnApplicationQuit()
    {
        detecter.Dispose();
    }


    public void OnoffFps()  // ui 버튼으로 fps 온오프 
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

    private void OnGUI()  // fps 카운터 
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
    public void OffCam() 
    {
        if (inputImageUI.gameObject.activeSelf == true)
        {
            inputImageUI.gameObject.SetActive(false);
        }
        else 
        {
            inputImageUI.gameObject.SetActive(true);
        }
    }

    public void OnoffTracker()  // ui 버튼으로 트래커 온오프 
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

    void OnRenderObject()  // 트래킹 on . off 옵션으로만 보이게 
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
