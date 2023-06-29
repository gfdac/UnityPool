using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class MachineGun : MonoBehaviour
{
    public float fireRate = 0.1f; // Taxa de disparo em segundos
    public int maxAmmo = 30; // Quantidade máxima de balas na arma
    public float reloadTime = 1.5f; // Tempo de recarregamento em segundos

    public BulletPool bulletPool; // Referência para o pool de balas

    private bool canShoot = true; // Flag para verificar se pode disparar
    private int currentAmmo; // Quantidade atual de munição
    private bool isReloading = false; // Flag para verificar se está recarregando

    private void Awake()
    {
        currentAmmo = maxAmmo; // Define a quantidade inicial de munição
    }

    private void Update()
    {
        if (isReloading)
            return;

        if (Input.GetMouseButton(0) && canShoot && currentAmmo > 0)
        {
            Shoot(); // Dispara a bala
            currentAmmo--; // Diminui a quantidade de munição
            canShoot = false; // Desativa a capacidade de disparar temporariamente
            StartCoroutine(ResetShot()); // Reinicia a capacidade de disparar após um tempo
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload()); // Inicia o processo de recarregamento se a munição estiver baixa
        }
    }

    public async Task Shoot()
    {
        GameObject bullet = bulletPool.GetBullet(); // Obtém uma bala do pool
        bullet.transform.position = transform.position; // Define a posição inicial da bala como a posição da metralhadora

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        if (bulletRigidbody == null)
        {
            bulletRigidbody = bullet.AddComponent<Rigidbody>(); // Adiciona um componente Rigidbody caso não exista
        }

        bulletRigidbody.velocity = Vector3.zero; // Zera a velocidade inicial da bala

        Vector3 shootDirection = transform.forward; // Define a direção do tiro como a frente da metralhadora
        bulletRigidbody.AddForce(shootDirection * 30f, ForceMode.VelocityChange); // Aplica uma força para impulsionar a bala na direção desejada

        await Task.Delay(0); // Aguarda um frame para evitar problemas de sincronização
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        bulletPool.ReturnBullet(bullet); // Retorna a bala ao pool
    }

    private IEnumerator ResetShot()
    {
        yield return new WaitForSeconds(fireRate); // Aguarda o tempo de recarga antes de permitir o próximo disparo
        canShoot = true; // Ativa a capacidade de disparar novamente
    }

    private IEnumerator Reload()
    {
        isReloading = true; // Inicia o processo de recarregamento
        yield return new WaitForSeconds(reloadTime); // Aguarda o tempo de recarregamento
        currentAmmo = maxAmmo; // Recarrega a munição
        isReloading = false; // Finaliza o processo de recarregamento
    }
}
