using System.Collections;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(MovePlayerToSpawn());
    }

    private IEnumerator MovePlayerToSpawn()
    {
        while (GameManager.Instance == null || GameObject.FindGameObjectWithTag("Player") == null)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        GameObject playerChild = GameObject.FindGameObjectWithTag("Player");
        GameObject player = playerChild.transform.root.gameObject;

        string spawnPointName = GameManager.Instance.spawnPoint;
        Transform spawn = GameObject.Find(spawnPointName)?.transform;

        if (spawn != null)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.position = spawn.position; 
            }
            else
            {
                player.transform.position = spawn.position;
            }

            Debug.Log("Jugador movido a: " + spawn.position);
        }
        else
        {
            Debug.LogWarning("Spawn point no encontrado: " + spawnPointName);
        }
    }
}
