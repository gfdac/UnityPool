using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private ObjectPool<Enemy> objectPool;

    public void SetObjectPool(ObjectPool<Enemy> pool)
    {
        objectPool = pool;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Realize as ações desejadas quando o inimigo colidir com uma bala

            // Desative o objeto do inimigo e retorne-o ao pool
            gameObject.SetActive(false);
            objectPool.ReturnObject(this);
        }
    }

}
