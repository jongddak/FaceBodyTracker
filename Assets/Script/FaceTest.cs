//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using UnityEngine;

//public class FaceTest : MonoBehaviour
//{
//    [SerializeField] FaceTest faceManager;
//    [SerializeField] List<GameObject> cubes = new List<GameObject>(468);
//    [SerializeField] GameObject cubePrefab;

//    ARFace face;
//    private void Awake()
//    {

//    }

//    public void ChangeMater(Material material) // ���͸��� ���� 
//    {
//        face.GetComponent<Renderer>().material = material;
//    }

//    private void OnEnable()
//    {
//        faceManager.facesChanged += OnFaceChange;
//    }
//    private void OnDisable()
//    {
//        faceManager.facesChanged -= OnFaceChange;
//    }

//    private void OnFaceChange(ARFacesChangedEventArgs args)
//    {
//        if (args.updated.Count > 0)  // Ʈ��ŷ ���� �󱼿� ��������� ���� �� 
//        {
//            face = args.updated[0]; // ar ���̽��� �����ͼ� 
//            for (int i = 0; i < face.vertices.Length; i++)  // ��� ���鿡 
//            {
//                Vector3 vertPos = face.transform.TransformPoint(face.vertices[i]); // ���� ��ġ�� ��ȯ 
//                cubes[i].transform.position = vertPos;  // ������ ť�긦 ���� ��ġ�� �̵�
//            }
//        }
//    }
//    //}
//    //using System.Collections;
//    //using System.Collections.Generic;
//    //using UnityEngine;
//    //using UnityEngine.XR.ARFoundation;

//    //public class FaceController : MonoBehaviour
//    //{
//    //    [SerializeField] ARFaceManager faceManager;

//    //    [SerializeField] List<GameObject> cubes = new List<GameObject>(468);

//    //    private void Awake()
//    //    {
//    //        for (int i = 0; i < 468; i++)
//    //        {
//    //            GameObject cube = Instantiate(eyePrefab);
//    //            cubes.Add(cube);
//    //        }
//    //    }

//    //    private void OnEnable()
//    //    {
//    //        faceManager.facesChanged += OnFaceChange;
//    //    }

//    //    private void OnDisable()
//    //    {
//    //        faceManager.facesChanged -= OnFaceChange;
//    //    }

//    //    private void OnFaceChange(ARFacesChangedEventArgs args)
//    //    {
//    //        // �������� �󱼿� �������(��ġ, ȸ��)�� ���� ��
//    //        if (args.updated.Count > 0)     // ����� �� �ϳ��� �����ϴ� ��
//    //        {
//    //            // ARFace�� �����ͼ�
//    //            ARFace face = args.updated[0];

//    //            // �󱼿� �ִ� ��� ����
//    //            for (int i = 0; i < face.vertices.Length; i++)
//    //            {
//    //                // �� ������ ��ġ�� ������ġ�� ��ȯ
//    //                Vector3 vertPos = face.transform.TransformPoint(face.vertices[i]);

//    //                // ������ ť����� ������ ��ġ�� �̵�
//    //                cubes[i].transform.position = vertPos;
//    //            }
//    //        }
//    //    }
//    //}
//}
