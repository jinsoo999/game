using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAnim
{
    public AnimationClip idle;
    public AnimationClip runF;
    public AnimationClip runB;
    public AnimationClip runL;
    public AnimationClip runR;
}

public class PlayerCtrl : MonoBehaviour {

    private float h = 0.0f;
    private float v = 0.0f;
    private float r = 0.0f;

    private Transform tr; //접근해야 하는 컴포넌트는 반드시 변수에 할당한 후 사용
    public float moveSpeed = 10.0f;  //이동 속도 변수
    public float rotSpeed = 80.0f;
    
    public PlayerAnim playerAnim;  //인스펙터 뷰에 표시할 애니메이션 클래스 변수
    public Animation anim;  //animation 컴포넌트를 저장하기 위한 변수
    

	void Start () {
        tr = GetComponent<Transform>(); //스크립트가 실행된 후 처음 수행되는 start 함수에서 Transform 컴포넌트를 할당

        anim = GetComponent<Animation>();  //animation 컴포넌트를 변수에 할당

        anim.clip = playerAnim.idle;  //animation 컴포넌트의 애니메이션 클립을 지정하고 실행
        anim.Play();
	}
	
	void Update () {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        Debug.Log("h=" + h.ToString());
        Debug.Log("v=" + v.ToString());

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h); //전후좌우 이동 방향 벡터 계산

        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);

        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);  //vector3.up 축을 기준으로 rotSpeed만큼의 속도로 회전

        if (v >= 0.1f)  //키보드 입력값을 기준으로 동작할 애니메이션 수행
        {
            anim.CrossFade(playerAnim.runF.name, 0.3f);  //전진
        }
        else if (v <= -0.1f)
        {
            anim.CrossFade(playerAnim.runB.name, 0.3f);  //후진
        }
        else if (h >= 0.1f)
        {
            anim.CrossFade(playerAnim.runR.name, 0.3f);  //오른쪽 이동
        }
        else if (h <= -0.1f)
        {
            anim.CrossFade(playerAnim.runL.name, 0.3f);  //왼쪽 이동
        }
        else
        {
            anim.CrossFade(playerAnim.idle.name, 0.3f);  //정지 시
        }
	}
}
