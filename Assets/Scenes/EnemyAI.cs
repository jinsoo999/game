using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    public enum State
    {//적 캐릭터의 상태를 표현하기 위한 열거형 변수 정의
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }
    public State state = State.PATROL;  //상태를 저장할 변수
    
    private Transform playerTr;  //주인공의 위치를 저장할 변수
    private Transform enemyTr;  //적 캐릭터의 위치를 저장할 변수
    private Animator animator;
    public float attackDist = 5.0f;  //공격 사정거리
    public float traceDist = 10.0f;  //추적 사정거리
    Damage damage;
    public bool isDie = false;  //사망여부를 판단할 변수
    
    private WaitForSeconds ws,ws2;  //지연시간 변수

    private MoveAgent moveAgent;
    
    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("PLAYER");
        if (player != null)
            playerTr = player.GetComponent<Transform>();

        enemyTr = GetComponent<Transform>();
        
        moveAgent = GetComponent<MoveAgent>();

        ws = new WaitForSeconds(0.3f);
    }

    void Start()
    {
        damage = GameObject.Find("Player").GetComponent<Damage>();
        ws2 = new WaitForSeconds(1.0f);
    }

    void OnEnable()
    {
        StartCoroutine(CheckState());

        StartCoroutine(Action());

        Damage.OnPlayerDie += this.OnPlayerDie;
    }

    void OnDisable()
    {
        Damage.OnPlayerDie -= this.OnPlayerDie;
    }
    
    IEnumerator CheckState()
    {//적 캐릭터의 상태 검사
        while (!isDie)  //사망하기 전까지 무한루프
        {
            if (state == State.DIE) //사망할 경우 함수 종료
            {
                damage.killGo = 1;
                damage.GoScore += damage.killGo * 100;  //고블린의 경우 100점 추가
                yield break;
            }
            float dist = Vector3.Distance(playerTr.position, enemyTr.position);
            //사용자와 적 캐릭터 간의 거리 계산

            if (dist <= attackDist) //공격 사정거리 이내인 경우
            {
                state = State.ATTACK;
                yield return ws2;
                damage.currHp -= 10.0f;
            }
            else if (dist <= traceDist)
            {//추적 사정거리 이내인 경우
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }
            yield return ws;
        }
    }

    IEnumerator Action()
    {//상태에 따라 적 캐릭터의 행동 처리
        while (!isDie)
        {  //적 캐릭터가 사망할 때까지 무한루프
            yield return ws;

            switch (state)
            {//상태에 따라 처리
                case State.PATROL: //순찰
                    moveAgent.patrolling = true;
                    GetComponent<Animation>().Play("walk");
                    break;
                case State.TRACE: //추적
                    moveAgent.traceTarget = playerTr.position;
                    GetComponent<Animation>().Play("run");
                    break;
                case State.ATTACK:  //공격
                    moveAgent.Stop();
                    GetComponent<Animation>().Play("attack01");
                    
                    break;
                case State.DIE:  //사망
                    moveAgent.Stop();
                    GetComponent<Animation>().Play("dead");
                    Destroy(gameObject, 2.0f);
                    
                    break;
            }
        }
    }
    public void OnPlayerDie()
    {
        moveAgent.Stop();
        StopAllCoroutines();
        GetComponent<Animation>().Play("idel01");
    }
}
