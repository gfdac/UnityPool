# Object Pool

O Object Pool é um componente utilizado para melhorar o desempenho e a eficiência de jogos e aplicativos que fazem uso intensivo de criação e destruição de objetos. O objetivo do Object Pool é reutilizar objetos em vez de criar novas instâncias sempre que necessário, reduzindo assim o overhead de alocação de memória e otimizando o uso dos recursos do sistema.

## Funcionalidades

O Object Pool implementado neste projeto possui as seguintes funcionalidades:

- Criação inicial de um pool de objetos com um tamanho pré-definido.
- Reutilização de objetos inativos em vez de criar novas instâncias.
- Ativação e desativação de objetos do pool conforme a necessidade.
- Gerenciamento de objetos ativos e inativos.
- Suporte a diferentes tipos de objetos.
- Possibilidade de personalizar o tamanho do pool e outras configurações.

## Como usar

Para usar o Object Pool em seu projeto, siga as etapas abaixo:

1. Adicione o script `ObjectPool.cs` ao seu projeto Unity.
2. Crie uma classe para representar os objetos que serão gerenciados pelo pool, estendendo a classe `MonoBehaviour`.
3. No código da classe do objeto, adicione o campo `ObjectPool<T>` para referenciar o pool de objetos.
4. Implemente o método `Awake` da classe do objeto para inicializar o pool de objetos.
5. Utilize o método `GetObject()` para obter um objeto ativo do pool. Caso não haja objetos inativos disponíveis, o pool criará uma nova instância do objeto.
6. Utilize o método `ReturnObject(T obj)` para retornar um objeto ao pool quando ele não estiver mais em uso.

Exemplo de uso:

```csharp
public class Bullet : MonoBehaviour
{
    private ObjectPool<Bullet> bulletPool;

    public void SetObjectPool(ObjectPool<Bullet> pool)
    {
        bulletPool = pool;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Lógica de colisão da bala

        // Retorne a bala ao pool
        bulletPool.ReturnObject(this);
    }
}

public class BulletPoolManager : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int poolSize = 20;

    private ObjectPool<Bullet> bulletPool;

    private void Awake()
    {
        bulletPool = new ObjectPool<Bullet>(bulletPrefab.GetComponent<Bullet>(), poolSize);
    }

    public Bullet GetBullet()
    {
        Bullet bullet = bulletPool.GetObject();
        bullet.transform.position = transform.position;
        bullet.gameObject.SetActive(true);
        return bullet;
    }
}
```

## Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para abrir problemas (issues) e enviar pull requests com melhorias, correções de bugs ou novas funcionalidades.

## Licença

Este projeto está licenciado sob a Licença MIT. Consulte o arquivo [LICENSE](LICENSE) para obter mais informações.
