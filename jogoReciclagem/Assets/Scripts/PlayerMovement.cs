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

    [Header("Limites do Mapa")]
    [Tooltip("Distância máxima que o jogador pode andar a partir do centro do mapa")]
    public float limiteX = 26f; // Limite lateral (1,5 tela para cada lado)
    public float limiteY = 14f; // Limite vertical (1,5 tela para cima/baixo)

    void FixedUpdate()
    {
        // Calcula a nova posição baseada no movimento
        Vector2 novaPosicao = rb.position + movimento * velocidade * Time.fixedDeltaTime;

        // "Clamp" (Prende) a posição do jogador para que ele não passe dos limites X e Y
        novaPosicao.x = Mathf.Clamp(novaPosicao.x, -limiteX, limiteX);
        novaPosicao.y = Mathf.Clamp(novaPosicao.y, -limiteY, limiteY);

        // Move o jogador para a nova posição (agora com limites)
        rb.MovePosition(novaPosicao);
    }
}
