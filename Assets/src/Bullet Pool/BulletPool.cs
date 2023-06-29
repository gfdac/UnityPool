using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab da bala
    public int poolSize = 20; // Tamanho do pool de balas

    private List<GameObject> activeBullets; // Lista de balas ativas
    private Queue<GameObject> inactiveBullets; // Fila de balas inativas

    private void Awake()
    {
        InitializePool().ConfigureAwait(false);
    }

    private async Task InitializePool()
    {
        activeBullets = new List<GameObject>();
        inactiveBullets = new Queue<GameObject>(poolSize);

        // Inicializa o pool de balas
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            inactiveBullets.Enqueue(bullet);
        }

        // Realize outras tarefas de inicialização assíncrona aqui
    }

    public async Task GetBulletAsync(Vector3 position, Vector3 direction)
    {
        GameObject bullet;

        if (inactiveBullets.Count > 0)
        {
            bullet = inactiveBullets.Dequeue();
            bullet.transform.position = position;
            bullet.transform.rotation = Quaternion.LookRotation(direction);
            bullet.SetActive(true);
            activeBullets.Add(bullet);
        }
        else
        {
            // Caso o pool esteja vazio, criar uma nova bala
            bullet = Instantiate(bulletPrefab);
            bullet.transform.position = position;
            bullet.transform.rotation = Quaternion.LookRotation(direction);
            bullet.SetActive(true);
            activeBullets.Add(bullet);
        }

        // Obter a referência para o componente ObjectLifetime
        ObjectLifetime objectLifetime = bullet.GetComponent<ObjectLifetime>();

        // Esperar um tempo antes de desativar a bala e retorná-la ao pool
        await Task.Delay((int)(objectLifetime.lifetime * 1000f));
        ReturnBullet(bullet);
    }

    public GameObject GetBullet()
    {
        if (inactiveBullets.Count > 0)
        {
            GameObject bullet = inactiveBullets.Dequeue();
            bullet.SetActive(true);
            activeBullets.Add(bullet);
            return bullet;
        }
        else
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(true);
            activeBullets.Add(bullet);
            return bullet;
        }
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        activeBullets.Remove(bullet);
        inactiveBullets.Enqueue(bullet);
    }
}
