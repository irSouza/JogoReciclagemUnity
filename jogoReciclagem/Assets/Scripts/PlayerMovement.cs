using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [Tooltip("A velocidade com que o personagem vai se mover.")]
    public float velocidade = 5f;

    private Rigidbody2D rb;
    private Vector2 movimento;

    void Start()
    {
        // Pegamos a referência do componente Rigidbody2D que está no mesmo GameObject.
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Captura o input do jogador (WASD ou Setas)
        // Input.GetAxisRaw retorna -1, 0 ou 1, o que dá um movimento mais "seco" e preciso.
        movimento.x = Input.GetAxisRaw("Horizontal");
        movimento.y = Input.GetAxisRaw("Vertical");

        // Normalizamos o vetor para que o movimento na diagonal não seja mais rápido
        movimento = movimento.normalized;
    }

    void FixedUpdate()
    {
        // Movimentamos o jogador usando o Rigidbody2D.
        // Multiplicamos o vetor de movimento pela velocidade e pelo tempo fixo (Time.fixedDeltaTime)
        rb.MovePosition(rb.position + movimento * velocidade * Time.fixedDeltaTime);
    }
}
