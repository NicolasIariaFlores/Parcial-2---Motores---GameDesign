using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField] private Transform player; // tu jugador
    [SerializeField] private Transform target; // el destino
    [SerializeField] private RectTransform arrowUI; // el objeto UI (Image con flecha)
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float hideDistance = 3f; // distancia para ocultar

    private void Update()
    {
        Vector3 direction = target.position - player.position;
        float distance = direction.magnitude;

        // Ocultar si estás cerca
        arrowUI.gameObject.SetActive(distance > hideDistance);

        // Proyectar dirección a pantalla
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);

        // Si está fuera de la pantalla, quedate en el borde
        if (screenPos.z > 0)
        {
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Vector2 dir = ((Vector2)screenPos - screenCenter).normalized;

            // Apuntar la flecha
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arrowUI.rotation = Quaternion.Euler(0, 0, angle - 90); // -90 por orientación del sprite

            // Opcional: limitar la posición a un radio circular alrededor del centro
            float radius = 300f; // distancia del borde desde el centro
            arrowUI.position = screenCenter + dir * radius;
        }
        else
        {
            arrowUI.gameObject.SetActive(false); // detrás del jugador
        }
    }
}