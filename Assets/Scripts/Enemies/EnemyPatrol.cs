using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{   
    [SerializeField] private float waitTime; 
    [SerializeField] private float patrolTime;

    [SerializeField] private Vector2 patrolDirection = Vector2.right;
    [SerializeField] private float patrolSpeed;

    [SerializeField] private PlayerDetector player;

    [SerializeField] private Vector2 escapeDirection = Vector2.left;
    [SerializeField] private float escapeSpeed;
    [SerializeField] private float escapeTime;

    private Vector3 firstPosition;
    private float timer; 
    private State currentState; 
    private State previous;
    private bool detectionPause = false; 
    private bool isEscaping = false; 
    private enum State { WaitingToPatrol, Patrol, WaitingToReturn, Return, Escape}

    void Start()
    {
        firstPosition = transform.position;
        timer = 0f;
        currentState = State.WaitingToPatrol;
    }

    void Update()
    {
        if (player.PlayerInRange())
        {
            if (!detectionPause)
            {
                detectionPause = true;
                previous = currentState;
            }

            return;
        }

        if (detectionPause)
        {
            detectionPause = false;
            currentState = previous;
        }

        EnemyMovement();
    }

    public void Escape()
    {
        if (!isEscaping)
        {
            isEscaping = true;
            timer = 0f;
            currentState = State.Escape;
        }
    }

    private void EnemyMovement()
    {
        switch (currentState)
        {
            case State.WaitingToPatrol:
                Debug.Log("Waiting to patrol"); 
                timer += Time.deltaTime;
                if (timer >= waitTime)
                {
                    timer = 0f;
                    currentState = State.Patrol; 
                }
                break;

            case State.Patrol:
                Debug.Log("Patroling");
                transform.Translate(patrolDirection.normalized * patrolSpeed * Time.deltaTime);
                timer += Time.deltaTime;
                if (timer >= patrolTime)
                {
                    timer = 0f;
                    currentState = State.WaitingToReturn;
                }
                break;

            case State.WaitingToReturn:
                Debug.Log("Waiting to return");
                timer += Time.deltaTime;
                if (timer >= waitTime)
                {
                    timer = 0f;
                    currentState = State.Return;
                }
                break;

            case State.Return:
                Debug.Log("Returning");
                Vector3 oppositeDirection = (firstPosition - transform.position).normalized;
                transform.position += oppositeDirection * patrolSpeed * Time.deltaTime;

                if (Vector3.Distance(transform.position, firstPosition) < 0.1f)
                {
                    transform.position = firstPosition;
                    timer = 0f;
                    currentState = State.WaitingToPatrol;
                }
                break;
            case State.Escape:
                Debug.Log("Escaping!");
                transform.Translate(escapeDirection.normalized * escapeSpeed * Time.deltaTime);
                timer += Time.deltaTime;
                if (timer >= escapeTime)
                {
                    timer = 0f;
                    isEscaping = false;
                    currentState = State.WaitingToPatrol;
                }
                break;
        }
    }
}
