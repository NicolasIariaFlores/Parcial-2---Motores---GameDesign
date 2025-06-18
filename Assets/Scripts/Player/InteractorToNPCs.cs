using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class InteractorToNPCs : MonoBehaviour
{
    private List<FollowPlayer> allNpcs = new List<FollowPlayer>();
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        UpdateNPClist();

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

    private void UpdateNPClist()
    {
        allNpcs.Clear();
        allNpcs.AddRange(FindObjectsOfType<FollowPlayer>());
    }
}