# Object Pool para Unity

![Unity](https://img.shields.io/badge/Unity-2021.3%2B-blue?style=for-the-badge&logo=unity)
![Language](https://img.shields.io/badge/Language-C%23-green?style=for-the-badge&logo=c-sharp)
![License](https://img.shields.io/badge/License-MIT-orange?style=for-the-badge)

Um componente de Object Pooling genérico e reutilizável para Unity, projetado para melhorar drasticamente o desempenho em jogos que instanciam e destroem objetos com frequência.

## 📝 Sobre o Projeto

O Object Pool é um padrão de projeto essencial para o desenvolvimento de jogos. Ele resolve um dos maiores gargalos de performance: a alocação e liberação constante de memória causadas pelas funções `Instantiate()` e `Destroy()`.

Em vez de criar novos objetos (como tiros, inimigos, efeitos visuais) e destruí-los depois, este sistema mantém um "pool" de objetos desativados. Quando um objeto é necessário, ele é retirado do pool e ativado. Quando não é mais preciso, ele é devolvido ao pool e desativado, pronto para ser reutilizado. Isso reduz o trabalho do Garbage Collector (Coletor de Lixo), resultando em um jogo mais fluido e com menos picos de lag (travamentos).

## ✨ Principais Funcionalidades

- **Pool Pré-aquecido:** Cria uma quantidade inicial de objetos para evitar picos de lag durante o jogo.
- **Reutilização Inteligente:** Reutiliza objetos inativos em vez de criar novas instâncias.
- **Crescimento Dinâmico:** Se o pool ficar vazio, ele pode criar novos objetos sob demanda (configurável).
- **Gerenciamento Simples:** Métodos claros para pegar (`GetObject`) e devolver (`ReturnObject`) objetos ao pool.
- **100% Genérico:** Funciona com qualquer `MonoBehaviour`, seja para tiros, inimigos, partículas, etc.
- **Fácil de Configurar:** Parâmetros públicos para definir o tamanho do pool e o prefab do objeto no Inspector da Unity.

## 🚀 Como Usar

### 1. Instalação

- Baixe o script `ObjectPool.cs` deste repositório.
- Adicione o script a uma pasta do seu projeto Unity (ex: `Assets/Scripts/Pooling`).

### 2. Configuração

Para usar o sistema, você precisa de dois scripts principais: um para o objeto que será "poolado" (ex: `Bullet.cs`) e um para gerenciar o pool (ex: `BulletPoolManager.cs`).

#### Passo 1: Script do Objeto (`Bullet.cs`)

Este script deve estar no Prefab do objeto que você quer reutilizar (sua bala, inimigo, etc.).

```csharp
// Bullet.cs
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Guarda uma referência ao pool de onde esta bala veio.
    private ObjectPool<Bullet> bulletPool;

    // Método para injetar a referência do pool no objeto.
    public void SetObjectPool(ObjectPool<Bullet> pool)
    {
        bulletPool = pool;
    }

    // Exemplo de como devolver o objeto ao pool após uma colisão.
    private void OnCollisionEnter(Collision collision)
    {
        // ... sua lógica de colisão aqui ...

        // Devolve o objeto ao pool para ser reutilizado.
        bulletPool.ReturnObject(this);
    }

    // Você também pode devolver ao pool depois de um tempo
    private void OnEnable()
    {
        // Exemplo: desativa a bala depois de 5 segundos
        Invoke("DisableBullet", 5f);
    }

    private void DisableBullet()
    {
        bulletPool.ReturnObject(this);
    }
}
