using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

    public Transform target;  //추적할 대상
    public float moveDamping = 15.0f;  //이동 속도 계수
    public float rotateDmaping = 10.0f;  //회전속도 계수
    public float distance = 5.0f;  //추적 대상과의 거리
    public float height = 4.0f;  //추적 대상과의 높이
    public float targetOffset = 2.0f;  //추적 좌표의 오프셋

    [Header("Wall Obstacle Setting")]
    public float heightAboveWall = 7.0f;  //카메라가 올라갈 높이
    public float colliderRadius = 1.8f;  //충돌체의 반지름
    public float overDamping = 5.0f;  //이동 속도 계수
    private float originHeight;  //최초 높이를 보관할 변수

    private Transform tr;
    
	void Start () {
        tr = GetComponent<Transform>();
        originHeight = height; //최초 카메라의 높이를 저장
	}

    void Update()
    {
        if (Physics.CheckSphere(tr.position, colliderRadius))
        {//구체 형태의 충돌체로 충돌 여부를 검사
            height = Mathf.Lerp(height, heightAboveWall, Time.deltaTime * overDamping);
            //보간함수를 사용해 카메라의 높이를 상승
        }
        else
        {
            height = Mathf.Lerp(height, originHeight, Time.deltaTime * overDamping);
            //보간함수를 사용해 카메라의 높이를 하강
        }
    }

    void LateUpdate()
    {
        var camPos = target.position - (target.forward * distance) + (target.up * height);  //카메라의 높이와 거리를 계산

        tr.position = Vector3.Slerp(tr.position, camPos, Time.deltaTime * moveDamping); //이동할 때의 속도 계수 적용

        tr.rotation = Quaternion.Slerp(tr.rotation, target.rotation, Time.deltaTime * rotateDmaping);  //회전할 때의 속도 계수 적용

        tr.LookAt(target.position + (target.up * targetOffset));  //카메라를 추적 대상으로 z축을 회전시킴
    }
}
