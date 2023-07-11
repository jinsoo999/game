using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour {

    private const string bulletTag = "BULLET";

    private float hp = 100.0f;  //적 HP

    public GameObject bloodEffect; //피격 시 사용할 혈흔 효과

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == bulletTag)
        {
            ShowBloodEffect(coll);

            Destroy(coll.gameObject); //총알 삭제

            hp -= coll.gameObject.GetComponent<BulletCtrl>().damage;
            if (hp <= 0.0f)
            {
                GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
            }
        }
    }
    void ShowBloodEffect(Collision coll)
    {//혈흔 효과를 생성하는 함수
        Vector3 pos = coll.contacts[0].point;  //총알이 충돌한 지점 산출

        Vector3 _normal = coll.contacts[0].normal;  //총알이 충돌했을 때의 법선 벡터

        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);  //총알의 충돌 시 방향 벡터의 회전값 계산

        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot); //혈흔 효과 생성
        Destroy(blood, 1.0f);
    }
}
