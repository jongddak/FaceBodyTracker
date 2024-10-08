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

//    public void ChangeMater(Material material) // 머터리얼 변경 
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
//        if (args.updated.Count > 0)  // 트래킹 중인 얼굴에 변경사항이 있을 때 
//        {
//            face = args.updated[0]; // ar 페이스를 가져와서 
//            for (int i = 0; i < face.vertices.Length; i++)  // 모든 점들에 
//            {
//                Vector3 vertPos = face.transform.TransformPoint(face.vertices[i]); // 월드 위치로 변환 
//                cubes[i].transform.position = vertPos;  // 생성한 큐브를 기준 위치로 이동
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
//    //        // 추적중인 얼굴에 변경사항(위치, 회전)이 있을 때
//    //        if (args.updated.Count > 0)     // 현재는 얼굴 하나만 적용하는 중
//    //        {
//    //            // ARFace를 가져와서
//    //            ARFace face = args.updated[0];

//    //            // 얼굴에 있는 모든 점을
//    //            for (int i = 0; i < face.vertices.Length; i++)
//    //            {
//    //                // 얼굴 기준의 위치를 월드위치로 변환
//    //                Vector3 vertPos = face.transform.TransformPoint(face.vertices[i]);

//    //                // 생성한 큐브들을 기준의 위치로 이동
//    //                cubes[i].transform.position = vertPos;
//    //            }
//    //        }
//    //    }
//    //}
//}
