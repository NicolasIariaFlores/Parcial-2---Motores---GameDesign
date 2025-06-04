using UnityEngine;
using UnityEngine.AI;

public class InteractorToNPCs : MonoBehaviour
{
    private FollowPlayer[] allNpcs;
    private Transform player;

    void Start()
    {
        allNpcs = FindObjectsOfType<FollowPlayer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            foreach (var npc in allNpcs)
            {
                if (npc.CanSeePlayer())
                {
                    npc.Interact();
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.T))
        {
            foreach (var npc in allNpcs)
            {
                npc.SendToAttack();
            }
        }
    }

    private bool CanReachViaNavMesh(Vector3 from, Vector3 to)
    {
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path))
        {
            return path.status == NavMeshPathStatus.PathComplete;
        }
        return false;
    }
}