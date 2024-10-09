
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseTest : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] Animator animator;
    [SerializeField] List<Transform> PosePositions  = new List<Transform>();

    

  //  [SerializeField] Transform Head;  // 0  머리 
   

    [SerializeField] Transform Rightarm;   // 12 14 16  오른팔 
    [SerializeField] Transform Rightforearm;
    [SerializeField] Transform Righthand;

    [SerializeField] Transform Leftarm;   // 11 13 15  왼팔
    [SerializeField] Transform Leftforearm;
    [SerializeField] Transform Lefthand;
 

    [SerializeField] Transform Rightupleg;  // 24 26 28  오른 다리 
    [SerializeField] Transform Rightleg;
    [SerializeField] Transform Rightfoot;


    [SerializeField] Transform Leftupleg;  // 23 25 27  왼다리
    [SerializeField] Transform Leftleg;
    [SerializeField] Transform Leftfoot;

    private Vector3[] previousPositions;

    private void Start()
    {
        StartCoroutine(GetChildTransform());
        StartCoroutine(ModelMotion());
      //  animator.GetBoneTransform;
    }

    IEnumerator ModelMotion() 
    {
        yield return new WaitForSeconds(5f);
        WaitForSeconds time = new WaitForSeconds(0.02f);

        previousPositions = new Vector3[PosePositions.Count];
        for (int i = 0; i < PosePositions.Count; i++)
        {
            previousPositions[i] = PosePositions[i].position;
        }
        
        while (true) 
        {   
            yield return time;
         
           // UpdatePartPosition(Head, PosePositions[0], previousPositions[0]);
            UpdatePartPosition(Rightarm, PosePositions[12], previousPositions[12]);
            UpdatePartPosition(Rightforearm, PosePositions[14], previousPositions[14]);
            UpdatePartPosition(Righthand, PosePositions[16], previousPositions[16]);

            UpdatePartPosition(Leftarm, PosePositions[11], previousPositions[11]);
            UpdatePartPosition(Leftforearm, PosePositions[13], previousPositions[13]);
            UpdatePartPosition(Lefthand, PosePositions[15], previousPositions[15]);

            UpdatePartPosition(Rightupleg, PosePositions[24], previousPositions[24]);
            UpdatePartPosition(Rightleg, PosePositions[26], previousPositions[26]);
            UpdatePartPosition(Rightfoot, PosePositions[28], previousPositions[28]);

            UpdatePartPosition(Leftupleg, PosePositions[23], previousPositions[23]);
            UpdatePartPosition(Leftleg, PosePositions[25], previousPositions[25]);
            UpdatePartPosition(Leftfoot, PosePositions[27], previousPositions[27]);

            for (int i = 0; i < PosePositions.Count; i++)
            {
                previousPositions[i] = PosePositions[i].position;
            }
        }

    }
    private void UpdatePartPosition(Transform part, Transform posePosition, Vector3 previousPosition)
    {
        Vector3 deltaPosition = posePosition.position - previousPosition; // 현재 위치와 이전 위치의 차이
        part.position += deltaPosition; // 파트를 이동
    }

    IEnumerator GetChildTransform()  // 랜드마크가 늦게 생성되서 약간 지연을 주지 않으면 값을 못받음 
    {
        Debug.Log("코루틴 시작");
        yield return  new WaitForSeconds(5f);
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            Transform childTransform = obj.transform.GetChild(i);
            PosePositions.Add(childTransform);
        }
        

    }
}
