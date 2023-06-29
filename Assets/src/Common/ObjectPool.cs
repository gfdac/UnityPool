using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private T prefab; // Prefab do objeto
    private int poolSize; // Tamanho do pool
    private List<T> activeObjects; // Lista de objetos ativos
    private Queue<T> inactiveObjects; // Fila de objetos inativos
    private List<Vector3> spawnLocations; // Lista de locais de spawn

    public ObjectPool(T prefab, int poolSize, List<Vector3> spawnLocations)
    {
        this.prefab = prefab;
        this.poolSize = poolSize;
        this.spawnLocations = spawnLocations;
        activeObjects = new List<T>();
        inactiveObjects = new Queue<T>(poolSize);
        InitializePool(); // Inicializa o pool de objetos
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            T obj = Object.Instantiate(prefab); // Instancia o objeto a partir do prefab
            obj.gameObject.SetActive(false); // Desativa o objeto
            inactiveObjects.Enqueue(obj); // Adiciona o objeto à fila de objetos inativos
        }
    }

    public T GetObject()
    {
        if (inactiveObjects.Count > 0)
        {
            T obj = inactiveObjects.Dequeue(); // Remove o objeto da fila de objetos inativos
            obj.transform.position = GetRandomSpawnLocation(); // Define a posição do objeto como um local de spawn aleatório
            obj.gameObject.SetActive(true); // Ativa o objeto
            activeObjects.Add(obj); // Adiciona o objeto à lista de objetos ativos
            return obj;
        }
        else
        {
            T obj = Object.Instantiate(prefab); // Instancia um novo objeto a partir do prefab
            obj.transform.position = GetRandomSpawnLocation(); // Define a posição do objeto como um local de spawn aleatório
            obj.gameObject.SetActive(true); // Ativa o objeto
            activeObjects.Add(obj); // Adiciona o objeto à lista de objetos ativos
            return obj;
        }
    }

    private Vector3 GetRandomSpawnLocation()
    {
        int index = Random.Range(0, spawnLocations.Count); // Seleciona um índice aleatório dentro do intervalo da lista de locais de spawn
        return spawnLocations[index]; // Retorna o local de spawn correspondente ao índice selecionado
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false); // Desativa o objeto
        activeObjects.Remove(obj); // Remove o objeto da lista de objetos ativos
        inactiveObjects.Enqueue(obj); // Adiciona o objeto à fila de objetos inativos
    }
}
