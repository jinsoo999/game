using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]  //총알 발사와 재장전 오디오 클립을 저장할 구조체
public struct PlayerSfx
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}

public class FireCtrl : MonoBehaviour {

    public enum WeaponType
    {
        RIFLE=0
    }
    //주인공이 현재 들고 있는 무기를 저장할 변수
    public WeaponType currWeapon = WeaponType.RIFLE;

    public GameObject bullet;  //총알 프리팹
    public ParticleSystem catridge;  //탄피 추출 파티클
    private ParticleSystem muzzleFlash;  //총구 화염 파티클
    private AudioSource _audio;  //audiosource 컴포넌트를 저장할 변수

    public Transform firePos;  //총알 발사 좌표
    public PlayerSfx playerSfx;  //오디오 클립을 저장할 변수
    
	void Start () {
        //FirePos 하위에 있는 컴포넌트 추출
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();
        _audio = GetComponent<AudioSource>();
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
	}

    void Fire() 
    { 
        //Bullet 프리팹을 동적으로 생성
        Instantiate(bullet, firePos.position, firePos.rotation);

        catridge.Play();  //파티클 실행

        muzzleFlash.Play();  //총구 화염 파티클 실행

        FireSfx();
    }

    void FireSfx()
    {  //현재 들고 있는 무기의 오디오 클립을 가져옴
        var _sfx = playerSfx.fire[(int)currWeapon];
        _audio.PlayOneShot(_sfx, 1.0f);
    }
}


