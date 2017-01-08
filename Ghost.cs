using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    enum State { Pursuit, Scatter, Frightened }
    State currentState;

    Transform target;
    NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerController>().transform;
        ToPursuit();
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Pursuit:
                {
                    navMeshAgent.SetDestination(target.position);
                    break;
                }
            case State.Scatter:
                {
                    
                    break;
                }
            case State.Frightened:
                {
                    
                    break;
                }
            default:
                break;
        }
    }

    public void ToFrightenedState()
    {
        navMeshAgent.speed = 2;
        currentState = State.Frightened;
        Invoke("ToPursuit", 5);
    }

    public void ToPursuit()
    {
        navMeshAgent.speed = 3;
        currentState = State.Pursuit;
    }

    public void SetDisableAfterEatingFruit()
    {
        gameObject.SetActive(false);
        Invoke("SetEnable", 0.4f);
    }

    private void SetEnable()
    {
        gameObject.SetActive(true);
    }
}
