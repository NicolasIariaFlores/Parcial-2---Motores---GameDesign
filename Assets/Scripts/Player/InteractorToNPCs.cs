using UnityEngine;

public class InteractorToNPCs : MonoBehaviour
{
    private FollowPlayer[] allNpcs;

    void Start()
    {
        allNpcs = FindObjectsOfType<FollowPlayer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            foreach (var npc in allNpcs)
            {
                if (npc.CanSeePlayerCircular())
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
}