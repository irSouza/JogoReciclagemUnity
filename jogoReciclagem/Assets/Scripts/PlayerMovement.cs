using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [Tooltip("Velocidade de movimento do jogador")]
    public float velocidade = 5f;

    private Rigidbody2D rb;
    private Vector2 movimento;

    void Start()
    {
        // Pegamos a referência do Rigidbody2D que está no mesmo GameObject
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Coleta o input do jogador (WASD ou Setas)
        // Horizontal: A/D ou Seta Esquerda/Seta Direita (retorna -1 a 1)
        // Vertical: W/S ou Seta Cima/Seta Baixo (retorna -1 a 1)
        movimento.x = Input.GetAxisRaw("Horizontal");
        movimento.y = Input.GetAxisRaw("Vertical");

        // Normalizamos o vetor para que o jogador não ande mais rápido na diagonal
        movimento = movimento.normalized;
    }

    void FixedUpdate()
    {
        // Movemos o Rigidbody2D usando a velocidade escolhida
        // Usamos FixedUpdate porque estamos lidando com física (Rigidbody)
        rb.MovePosition(rb.position + movimento * velocidade * Time.fixedDeltaTime);
    }
}
