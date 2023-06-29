using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 5f; // Tempo de vida da bala em segundos
    public float shootForce = 30f; // Força do disparo

    private float spawnTime; // Tempo de criação da bala
    private Rigidbody rb; // Rigidbody da bala
    private MachineGun machineGun; // Referência à metralhadora

    private void Awake()
    {
        // Obtém a referência à metralhadora no cenário
        machineGun = FindObjectOfType<MachineGun>();

        // Obtém o Rigidbody da bala
        rb = GetComponent<Rigidbody>();

        // Define o modo de detecção contínua de colisão para melhor precisão
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void OnEnable()
    {
        // Armazena o tempo de criação da bala
        spawnTime = Time.time;
    }

    private void Update()
    {
        // Verifica se o tempo de vida da bala foi atingido
        if (Time.time >= spawnTime + lifetime)
        {
            ReturnToPool(); // Retorna a bala ao pool
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Adicione aqui a lógica de colisão que você deseja aplicar à bala
        // Por exemplo, você pode destruir a bala quando ela colidir com um objeto:
        // if (collision.gameObject.CompareTag("Target"))
        // {
        //     ReturnToPool();
        // }

        // Verifica se a colisão ocorreu com outra bala
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Obtém o Rigidbody da outra bala
            Rigidbody otherRb = collision.gameObject.GetComponent<Rigidbody>();

            // Aplica a força de repulsão nas duas balas
            ApplyRepulsionForce(rb, otherRb, shootForce);
        }
    }

    private void ApplyRepulsionForce(Rigidbody rb1, Rigidbody rb2, float force)
    {
        // Obtém a direção entre as duas balas
        Vector3 repulsionDirection = (rb1.position - rb2.position).normalized;

        // Aplica a força de repulsão em ambas as direções
        rb1.AddForce(repulsionDirection * force, ForceMode.Impulse);
        rb2.AddForce(-repulsionDirection * force, ForceMode.Impulse);
    }

    private void ReturnToPool()
    {
        // Verifica se a referência à metralhadora é válida
        if (machineGun != null)
        {
            machineGun.ReturnBulletToPool(gameObject); // Retorna a bala ao pool através da metralhadora
        }
        else
        {
            Destroy(gameObject); // Destroi a bala caso a referência à metralhadora seja inválida
        }
    }
}
