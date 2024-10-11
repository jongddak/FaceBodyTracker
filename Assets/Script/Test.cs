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

    [SerializeField] WebCamInput WebCamInput;



    [SerializeField] RawImage inputImageUI;

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

  //  [SerializeField] Transform[] bodyjoints;// 현재
  //  [SerializeField] Vector3[] curPos; // 움직여야할 값 

    private const float positionThreshold = 0.001f;
    private BlazePoseDetecter detecter;
    private void Start()
    {
        detecter = new BlazePoseDetecter();
        worldpreviousLandmarks = new Vector3[33];

        material = new Material(shader);
    }
    private void Update()
    {
        detecter.ProcessImage(WebCamInput.inputImageTexture);
        inputImageUI.texture = WebCamInput.inputImageTexture;
        // Vector3 hipCenter = (detecter.GetPoseWorldLandmark(23) + detecter.GetPoseWorldLandmark(24)) / 2.0f;
        // 좌표변환용 기준점 
        // 랜드마크의 프레임당 움직임의 차이를 받아서 매핑한 관절의 움직임으로 변환해야함 
        // 프레임당 움직임 , 랜드마크는 0 ~ 32 까지 33개  
        //https://ai.google.dev/edge/mediapipe/solutions/vision/pose_landmarker?hl=ko
        // 사용 할 랜드마크는  0(머리) , 11 13 15(왼팔) , 12 14 16(오른팔) , 23 25 27(왼다리) , 24 26 28(오른다리)
        // ex 어깨가 움직이려면 손목의 좌표변화 만큼 회전해야함 팔
        // 현재 랜드마크 와 이전 랜드마크 필요함

        //1. 웹캠 출력 획득 이전값 , 현재값저장(차이) 
        //2. 얻은 출력값을 3d 모델에 맞게 변경 
        //3. 변경된 차이 값으로 모델을 움직임 


        for (int i = 0; i < detecter.vertexCount; i++)
        {
            worldpreviousLandmarks[i] = detecter.GetPoseWorldLandmark(i);  // 월드기준 보정된 좌표임 
        }


    }

    void OnAnimatorIK(int layerIndex)
    {
        // 오른손이 특정 목표 위치로 이동
        animator.SetIKPosition(AvatarIKGoal.RightHand, worldpreviousLandmarks[16]);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f); // 가중치 100% 적용
        // 오른손 회전 설정
        Quaternion rightHandRotation = Quaternion.LookRotation(worldpreviousLandmarks[16]);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandRotation);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f); // 회전 가중치 설정

        
        // 왼손
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

    void OnRenderObject()  // 트래킹 on . off 옵션으로만 보이게 
    {
        // Use predicted pose world landmark results on the ComputeBuffer (GPU) memory.
        material.SetBuffer("_worldVertices", detecter.worldLandmarkBuffer);
        // Set pose landmark counts.
        material.SetInt("_keypointCount", detecter.vertexCount);
        material.SetFloat("_humanExistThreshold", humanExistThreshold);
        material.SetVectorArray("_linePair", linePair);
        material.SetMatrix("_invViewMatrix", mainCamera.worldToCameraMatrix.inverse);

        // Draw 35 world body topology lines.
        material.SetPass(2);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 6, 35);
        //
        // Draw 33 world landmark points.
        material.SetPass(3);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 6, detecter.vertexCount);
    }
}
