# FaceBodyTracker
 AR 컨텐츠 


예시 gif (idel 애니메이션에 팔만 트래킹 )

![Animation2](https://github.com/user-attachments/assets/eec6d51d-2bdd-4c2a-ac23-7babd34f897d)

주제 : 실시간 모션 캡쳐 
목표 : 카메라로 촬영한 실제 사람의 움직임을 임의의 3d 모델로 트래킹 해보기
Arkit(ios) 에는 body tracker가 구현되어 있지만 
ARCore에는 아직 body tracker가 없음
하지만 이런 문제를 해결하기 위해 구글의 Mediapipe 로 
만들어진 유니티 플러그인을 이용해 body tracking이 가능함 

참고 깃헙
https://github.com/homuler/MediaPipeUnityPlugin
https://github.com/creativeIKEP/BlazePoseBarracuda


![blazeposepoint](https://github.com/user-attachments/assets/bd132184-7b09-4e80-a8d2-5be257c20b67)


목표 기능 
1.실시간으로 촬영된 사람과 동일한 포즈 구현
예상 기간 24.10.08 ~ 24.10.14
https://ai.google.dev/edge/mediapipe/solutions/vision/pose_landmarker?hl=ko
구글의 mediapipe에서 제공한  위 이미지에서 알 수 있듯이  목 , 허리 , 가슴 , 골반 등의 척추라인의 랜드마크가 없음 
따라서 가상의 점을 만들어서 트래킹 해야 함 .
제공하는 패키지의 기능에서 촬영된 이미지의 포즈를 예측해서
 3d 월드상의 좌표로(각각의 랜드마크로 변환) 제공해주기 때문에 
이를 이용해서 3d 아바타를 움직여볼 계획


2.표정 감지 ,손 동작 감지
표정(얼굴), 은 유니티의 FACE detection 을 사용해볼 계획
손 동작은 mediapipe에서  핸드 트래킹을 제공하기 때문에 이를 이용할 계획 


3.Ar로 촬영한 사람 위에 덮어씌우기 










사용할 에셋 및 참고 깃헙
https://github.com/homuler/MediaPipeUnityPlugin
https://github.com/creativeIKEP/BlazePoseBarracuda
https://assetstore.unity.com/packages/3d/characters/unity-chan-model-18705

