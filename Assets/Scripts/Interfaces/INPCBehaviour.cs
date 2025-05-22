public interface INPCBehavior
//Es una iterface para cuando implementemos diferentes aliados, nos facilite organizarlos.
{
    void Interact();     // Llamado cuando el jugador presiona F
    void UpdateBehavior(); // Se llama cada frame (seguir, esperar, etc.)
}