using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour {
    public GameObject expeffect;  //폭발 효과 프리팹을 저장할 변수
    public Mesh[] meshes;  //찌그러진 드럼통의 메쉬를 저장할 배열
    public Texture[] textures;  //드럼통의 텍스처를 저장할 배열

    private int hitcount = 0;  //총알에 맞은 횟수
    
    private MeshFilter meshFilter;  //meshFilter 컴포넌트를 저장할 변수

    private MeshRenderer _renderer;  //meshrenderer 컴포넌트를 저장할 변수

    private AudioSource _audio;

    public float expRadius = 10.0f;  //폭발 반경

    public AudioClip expSfx;

    Damage damage;

    // Use this for initialization
    void Start () {
        meshFilter = GetComponent<MeshFilter>(); //meshFilter 컴포넌트를 추출해 저장

        _renderer = GetComponent<MeshRenderer>();  //meshrenderer 컴포넌트를 추출해 저장

        _audio = GetComponent<AudioSource>();

        _renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
        //난수를 발생시켜 불규칙적인 텍스처를 적용

        damage = GameObject.Find("Player").GetComponent<Damage>();
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("BULLET"))
        { //충돌한 게임오브젝트의 태그를 비교
            if (++hitcount == 3)  //총알의 충돌 횟수를 증가시킨다
            {
                Expbarrel();  //3발 이상 맞을 시 폭발
            }
        }
    } 

    void Expbarrel()
    {  //폭발 효과를 처리할 함수
        GameObject effect = Instantiate(expeffect, transform.position, Quaternion.identity);
        
        damage.baScore += 30;  //점수에 30점 추가
        Destroy(effect, 2.0f);
        
        IndirectDamage(transform.position);

        int idx = Random.Range(0, meshes.Length);  //난수를 발생
        meshFilter.sharedMesh = meshes[idx];  //찌그러진 메쉬를 적용
        GetComponent<MeshCollider>().sharedMesh = meshes[idx];

        _audio.PlayOneShot(expSfx, 1.0f);
        
    }

    void IndirectDamage(Vector3 pos)
    {//폭발력을 주변에 전달하는 함수
        Collider[] colls = Physics.OverlapSphere(pos, expRadius, 1 << 8);

        foreach (var coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>();

            _rb.mass = 1.0f;

            _rb.AddExplosionForce(1200.0f, pos, expRadius, 1000.0f);
        }
    }
}
