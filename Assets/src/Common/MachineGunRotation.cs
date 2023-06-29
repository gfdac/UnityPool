using UnityEngine;

public class MachineGunRotation : MonoBehaviour
{
    public float rotationSpeed = 180f; // Velocidade de rotação em graus por segundo

    private bool isRotating = false; // Flag para verificar se está ocorrendo a rotação
    private float rotationDirection = 1f; // Direção da rotação (1 para sentido horário, -1 para sentido anti-horário)

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isRotating = true; // Inicia a rotação quando o botão do mouse é pressionado
        }

        if (Input.GetMouseButtonUp(0))
        {
            isRotating = false; // Para a rotação quando o botão do mouse é solto
        }
    }

    private void FixedUpdate()
    {
        if (isRotating)
        {
            float rotationAmount = rotationDirection * rotationSpeed * Time.fixedDeltaTime; // Calcula o valor da rotação a ser aplicada
            transform.Rotate(Vector3.forward, rotationAmount); // Aplica a rotação ao objeto no eixo Z (sentido horário ou anti-horário)
        }
    }
}
