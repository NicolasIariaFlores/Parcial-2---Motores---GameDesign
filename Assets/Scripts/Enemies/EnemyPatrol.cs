using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Collider2D patrolArea;

    [SerializeField] private float patrolRadius; 
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float waitTime; 

    //ELIMINAR?
    //[SerializeField] private float patrolTime;
    //[SerializeField] private Vector2 patrolDirection = Vector2.right;
    //ELIMINAR??

    [SerializeField] private PlayerDetector player;
    [SerializeField] private Vector2 escapeDirection = Vector2.left;
    [SerializeField] private float escapeSpeed;
    [SerializeField] private float escapeTime;

    private Vector3 firstPosition;
    private Vector3 movingTarget;
    private float timer; 
    //ELIMINAR
    //private State currentState; 
    private State previous;
    //ELIMINAR

    private State state = State.Waiting; 
    private bool detectionPause = false; 
    //private bool isEscaping = false; 
    private enum State { WaitingToPatrol, Patrol, WaitingToReturn, Return, Escape, Moving, Waiting}

    void Start()
    {
        firstPosition = transform.position;
        timer = 0f;
        //currentState = State.WaitingToPatrol;
    }

    void Update()
    {
        if (player.PlayerInRange())
        {
            detectionPause = true; 
            /*if (!detectionPause)
            {
                detectionPause = true;
                previous = currentState;
            }*/

            return;
        }

        if (detectionPause)
        {
            detectionPause = false;
            timer = 0f;
            state = State.Waiting; 
            //currentState = previous;
        }

        EnemyMovement();
    }

    public void Escape()
    {
        /* if (!isEscaping)
         {
             isEscaping = true;
             timer = 0f;
             currentState = State.Escape;
         }*/

        state = State.Escape;
        timer = 0f;
    }

    private void EnemyMovement()
    {
        switch (state )//currentState)
        {
            case State.Waiting:
                timer += Time.deltaTime;
                if (timer >= waitTime)
                {
                    timer = 0f;
                    state = State.Moving;
                }
                break;

            case State.Moving:
                MoveTo(movingTarget, patrolSpeed);

                if (Vector2.Distance(transform.position, movingTarget) < 0.1f)
                {
                    NewTarget();
                    state = State.Waiting;
                    timer = 0f;
                }
                break;

            case State.Escape:
                MoveTo(transform.position + (Vector3)escapeDirection, escapeSpeed);
                timer += Time.deltaTime;
                if (timer >= escapeTime)
                {
                    state = State.Waiting;
                    timer = 0f;
                }
                break;

                /*case State.WaitingToPatrol:
                   // Debug.Log("Waiting to patrol"); 
                    timer += Time.deltaTime;
                    if (timer >= waitTime)
                    {
                        timer = 0f;
                        currentState = State.Patrol; 
                    }
                    break;

                case State.Patrol:
                    //Debug.Log("Patroling");
                    transform.Translate(patrolDirection.normalized * patrolSpeed * Time.deltaTime);
                    timer += Time.deltaTime;
                    if (timer >= patrolTime)
                    {
                        timer = 0f;
                        currentState = State.WaitingToReturn;
                    }
                    break;

                case State.WaitingToReturn:
                    //Debug.Log("Waiting to return");
                    timer += Time.deltaTime;
                    if (timer >= waitTime)
                    {
                        timer = 0f;
                        currentState = State.Return;
                    }
                    break;

                case State.Return:
                    //Debug.Log("Returning");
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
                    //Debug.Log("Escaping!");
                    transform.Translate(escapeDirection.normalized * escapeSpeed * Time.deltaTime);
                    timer += Time.deltaTime;
                    if (timer >= escapeTime)
                    {
                        timer = 0f;
                        isEscaping = false;
                        currentState = State.WaitingToPatrol;
                    }
                    break;*/
        }
    }

    private void MoveTo(Vector3 target, float speed)
    {
        Vector3 dir = (target - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime; 
    }

    private void NewTarget()
    {
        Vector2 newPoint;
        int maxTries = 10;
        int tries = 0;

        do
        {
            Vector2 randomOffset = Random.insideUnitCircle * patrolRadius;
            newPoint = firstPosition + (Vector3)randomOffset;
            tries++;
        }
        while (!patrolArea.OverlapPoint(newPoint) && tries < maxTries);

        movingTarget = newPoint;
        /*Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
        movingTarget = firstPosition + new Vector3(randomCircle.x, randomCircle.y, 0f);*/
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}
