using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour {

    public List<Transform> wayPoints;  //순찰 지점들을 저장하기 위한 List 타입 변수

    public int nextIdx; //다음 순찰 지점의 배열의 index

    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;

    private NavMeshAgent agent;

    private bool _patrolling;  //순찰 여부를 판단하는 변수

    public bool patrolling
    {
        get { return _patrolling; }
        set {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed;
                MoveWayPoint();
            }
        }
    }

    private Vector3 _traceTarget;  //추적 대상의 위치를 저장하는 변수
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set {
            _traceTarget = value;
            agent.speed = traceSpeed;
            TraceTarget(_traceTarget);
        }
    }

    public float speed
    {
        get { return agent.velocity.magnitude; }
    }
    
	void Start () {

        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.speed = patrolSpeed;
        var group = GameObject.Find("WayPointGroup");
        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoints);
            wayPoints.RemoveAt(0);

            nextIdx = Random.Range(0, wayPoints.Count);
        }

        MoveWayPoint();
	}

    void MoveWayPoint()
    {  //다음 목적지까지 이동 명령을 내리는 함수
        if (agent.isPathStale) return;

        agent.destination = wayPoints[nextIdx].position;
        agent.isStopped = false;
    }

    void TraceTarget(Vector3 pos)
    {  //주인공을 추적할 때 이동시키는 함수
        if (agent.isPathStale) return;

        agent.destination = pos;
        agent.isStopped = false;
    }

    public void Stop()
    {  //순찰 및 추적을 정지시키는 함수
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }
    
	void Update () {
        if (!_patrolling) return;
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.5f)
        {  //NavMeshAgent가 이동하고 있고 목적지에 도착했는지 계산
            nextIdx = Random.Range(0,wayPoints.Count);  //다음 목적지의 배열 첨자 계산
            MoveWayPoint();  //다음 목적지로 이동
        }
	}
}
