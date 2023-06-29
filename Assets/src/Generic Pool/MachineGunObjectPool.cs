using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunObjectPool : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab da bala
    public int poolSize = 20; // Tamanho do pool de balas
    public List<Vector3> spawnLocations; // Lista de locais de spawn
    public float fireRate = 0.1f; // Taxa de disparo em segundos
    public int maxAmmo = 30; // Quantidade máxima de balas na arma
    public float reloadTime = 1.5f; // Tempo de recarregamento em segundos

    private ObjectPool<Bullet> bulletPool; // Pool de objetos do tipo Bullet

    private bool canShoot = true; // Flag para verificar se pode disparar
    private int currentAmmo; // Quantidade atual de munição
    private bool isReloading = false; // Flag para verificar se está recarregando

    private void Awake()
    {
        InitializePool();
        currentAmmo = maxAmmo; // Define a quantidade inicial de munição
    }

    private void Update()
    {
        if (isReloading)
            return;

        if (Input.GetMouseButton(0) && canShoot && currentAmmo > 0)
        {
            Shoot();
            currentAmmo--;
            canShoot = false;
            StartCoroutine(ResetShot());
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    private void InitializePool()
    {
        // Inicializa o pool de balas com base no prefab, tamanho do pool e locais de spawn
        bulletPool = new ObjectPool<Bullet>(bulletPrefab.GetComponent<Bullet>(), poolSize, spawnLocations);
    }

    private void Shoot()
    {
        // Obtém uma bala do pool
        Bullet bullet = bulletPool.GetObject();

        // Define a posição inicial da bala como a posição da metralhadora
        bullet.transform.position = transform.position;

        // Obtém o Rigidbody da bala ou adiciona um se não existir
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        if (bulletRigidbody == null)
        {
            bulletRigidbody = bullet.gameObject.AddComponent<Rigidbody>();
        }

        // Zera a velocidade inicial da bala
        bulletRigidbody.velocity = Vector3.zero;

        // Define a direção do tiro como a frente da metralhadora
        Vector3 shootDirection = transform.forward;

        // Aplica uma força para impulsionar a bala na direção desejada
        bulletRigidbody.AddForce(shootDirection * 30f, ForceMode.VelocityChange);
    }

    public void ReturnBulletToPool(Bullet bullet)
    {
        // Retorna a bala ao pool
        bulletPool.ReturnObject(bullet);
    }

    private IEnumerator ResetShot()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
