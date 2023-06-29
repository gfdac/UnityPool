using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab do inimigo
    public int poolSize = 2; // Tamanho do pool de inimigos
    public List<Vector3> spawnLocations; // Lista de locais de spawn

    private ObjectPool<Enemy> enemyPool; // Pool de objetos do tipo Enemy

    private void Start()
    {
        // Inicializa o pool de inimigos com base no prefab, tamanho do pool e locais de spawn
        enemyPool = new ObjectPool<Enemy>(enemyPrefab.GetComponent<Enemy>(), poolSize, spawnLocations);
        SpawnEnemies(); // Invoca o método para spawnar os inimigos
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Enemy enemy = enemyPool.GetObject(); // Obtém um inimigo do pool
            enemy.transform.position = GetRandomSpawnLocation(); // Define a posição do inimigo como um local de spawn aleatório
            enemy.gameObject.SetActive(true); // Ativa o objeto do inimigo
            enemy.SetObjectPool(enemyPool); // Atribui o pool de objetos ao inimigo
        }
    }

    private Vector3 GetRandomSpawnLocation()
    {
        int index = Random.Range(0, spawnLocations.Count); // Seleciona um índice aleatório dentro do intervalo da lista de locais de spawn
        return spawnLocations[index]; // Retorna o local de spawn correspondente ao índice selecionado
    }
}
