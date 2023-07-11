using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_2 : MonoBehaviour {
    public enum State
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }
    

    public State state = State.PATROL;
    
    private Transform playerTr;
    private Transform enemyTr;
    private Animator animator;
    public float attackDist = 5.0f;
    public float traceDist = 10.0f;
    Damage damage;
    public bool isDie = false;
    
    private WaitForSeconds ws,ws2;

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
    {
        while (!isDie)
        {
            if (state == State.DIE)
            {
                damage.killHo = 1;
                damage.HoScore += damage.killHo * 200;
                yield break;
            }

            float dist = Vector3.Distance(playerTr.position, enemyTr.position);

            if (dist <= attackDist)
            {
                state = State.ATTACK;
                yield return ws2;
                damage.currHp -= 20.0f;
            }
            else if (dist <= traceDist)
            {
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
    {
        while (!isDie)
        {
            yield return ws;

            switch (state)
            {
                case State.PATROL:
                    moveAgent.patrolling = true;
                    GetComponent<Animation>().Play("walk");
                    break;
                case State.TRACE:
                    moveAgent.traceTarget = playerTr.position;
                    GetComponent<Animation>().Play("run");
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    GetComponent<Animation>().Play("attack02");
                    
                    break;
                case State.DIE:
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
