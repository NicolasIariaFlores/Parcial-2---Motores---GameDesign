
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Collider2D patrolArea;

    [SerializeField] private float patrolRadius; 
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float waitTime; 

    [SerializeField] private PlayerDetector player;
    [SerializeField] private Vector2 escapeDirection = Vector2.left;
    [SerializeField] private float escapeSpeed;
    [SerializeField] private float escapeTime;

    private Vector3 firstPosition;
    private Vector3 movingTarget;
    private float timer; 

    private State state = State.Waiting; 
    private bool detectionPause = false; 

    private enum State {Escape, Moving, Waiting}

    void Start()
    {
        firstPosition = transform.position;
        timer = 0f;
        NewTarget();
        state = State.Waiting; 
    }

    void Update()
    {
        if (player.PlayerInRange())
        {
            detectionPause = true; 
            return;
        }

        if (detectionPause)
        {
            detectionPause = false;
            timer = 0f;
            state = State.Waiting; 
        }

        EnemyMovement();
    }

    public void Escape()
    {
        state = State.Escape;
        timer = 0f;
    }

    private void EnemyMovement()
    {
        switch (state )
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
        }
    }

    private void MoveTo(Vector3 target, float speed)
    {
        Vector3 dir = (target - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
        Debug.DrawLine(transform.position, target, Color.red);
    }

    private void NewTarget()
    {
        Vector2 newPoint;
        int maxTries = 30;
        int tries = 0;

        do
        {
            Vector2 randomOffset = Random.insideUnitCircle * patrolRadius;
            newPoint = firstPosition + (Vector3)randomOffset;
            tries++;
        }
        while (!patrolArea.OverlapPoint(newPoint) && tries < maxTries);

        movingTarget = newPoint;
    }
    
    public void SetPatrolArea(Collider2D area)
    {
        patrolArea = area; 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}
