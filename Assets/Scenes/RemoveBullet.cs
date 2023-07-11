using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour {

    public GameObject sparkEffect; //스파크 프리팹을 저장할 변수
    //충돌이 시작할 때 발생하는 이벤트
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == "BULLET")
        {//충돌한 게임오브젝트의 태그값 비교
            ShowEffect(coll);  //스파크 효과 함수 호출
            Destroy(coll.gameObject); //충돌한 게임오브젝트 삭제
        }
    }
    void ShowEffect(Collision coll)
    {
        ContactPoint contact = coll.contacts[0];  //충돌 지점의 정보를 추출
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal); 
        //법선 벡터가 이루는 회전각도를 추출
        GameObject spark = Instantiate(sparkEffect, contact.point + (-contact.normal * 0.05f), rot);
        spark.transform.SetParent(this.transform);//스파크 효과 생성
    }
}
