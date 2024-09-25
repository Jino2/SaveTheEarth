using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    public float patrolRadius = 10f;   // 순찰 범위
    public float patrolTime = 5f;      // 다음 목표로 이동하는 시간 간격
    public float initialWaitTime = 2f; // 시작 후 대기 시간
    private float patrolTimer;         // 시간 누적을 위한 타이머
    private float initialTimer;        // 처음 대기 시간을 위한 타이머
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        patrolTimer = patrolTime;
        initialTimer = initialWaitTime;
    }

    void Update()
    {
        initialTimer -= Time.deltaTime;

        // 처음 대기 시간이 끝난 후 순찰 시작
        if (initialTimer <= 0f)
        {
            patrolTimer += Time.deltaTime;

            // 일정 시간 간격으로 새로운 랜덤 목표 지점을 설정
            if (patrolTimer >= patrolTime)
            {
                Vector3 newTarget = RandomNavSphere(transform.position, patrolRadius, -1);
                agent.SetDestination(newTarget);
                patrolTimer = 0;
            }

            // 현재 목적지까지 도착했는지 확인
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                patrolTimer = patrolTime; // 도착 후 즉시 새로운 목표 설정
            }
        }
    }

    // 랜덤 위치를 찾아주는 함수
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}