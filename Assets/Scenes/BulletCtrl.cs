using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour {

    public float damage = 20.0f;  //총알의 데미지
    public float speed = 750.0f;  //총알의 속도

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        Destroy(gameObject,0.35f);
	}
}
