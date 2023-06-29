using UnityEngine;

public class ObjectLifetime : MonoBehaviour
{
    private float spawnTime; // Tempo de criação do objeto
    public float lifetime = 5f; // Tempo de vida do objeto em segundos

    private void OnEnable()
    {
        spawnTime = Time.time; // Armazena o tempo de criação do objeto
    }

    public bool HasExpired()
    {
        // Verifica se o tempo de vida do objeto foi atingido
        return Time.time >= spawnTime + lifetime;
    }
}
