using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage_3 : MonoBehaviour {

    private const string bulletTag = "BULLET";

    private float hp = 500.0f;

    public GameObject bloodEffect;
    
	void Start () {

	}

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == bulletTag)
        {
            ShowBloodEffect(coll);

            Destroy(coll.gameObject);

            hp -= coll.gameObject.GetComponent<BulletCtrl>().damage;
            if (hp <= 0.0f)
            {
                GetComponent<EnemyAI_3>().state = EnemyAI_3.State.DIE;
            }
        }
    }
    void ShowBloodEffect(Collision coll)
    {
        Vector3 pos = coll.contacts[0].point;

        Vector3 _normal = coll.contacts[0].normal;

        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);

        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot);
        Destroy(blood, 1.0f);
    }
}
