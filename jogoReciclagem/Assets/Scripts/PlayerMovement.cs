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
        // Sanitizamos o input com Mathf.Clamp para garantir que os valores não saiam do intervalo seguro de -1 a 1,
        // prevenindo comportamentos anômalos caso o sistema de input retorne valores inesperados.
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        movimento.x = Mathf.Clamp(inputX, -1f, 1f);
        movimento.y = Mathf.Clamp(inputY, -1f, 1f);

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
