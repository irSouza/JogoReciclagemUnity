using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [Tooltip("Aceleração do personagem.")]
    public float aceleracao = 50f;
    [Tooltip("Velocidade máxima permitida.")]
    public float velocidadeMaxima = 7f;

    private Rigidbody2D rb;
    private Vector2 movimento;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Configurações recomendadas para física orgânica
        rb.mass = 1.5f;
        rb.linearDamping = 10f;
    }

    void Update()
    {
        movimento.x = Input.GetAxisRaw("Horizontal");
        movimento.y = Input.GetAxisRaw("Vertical");
        movimento = movimento.normalized;
    }

    [Header("Limites do Mapa")]
    public float limiteX = 26f;
    public float limiteY = 14f;

    void FixedUpdate()
    {
        // Aplica força baseada no input
        rb.AddForce(movimento * aceleracao);

        // Limita a velocidade máxima
        if (rb.linearVelocity.magnitude > velocidadeMaxima)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * velocidadeMaxima;
        }

        // Prende a posição dentro dos limites
        Vector2 pos = rb.position;
        pos.x = Mathf.Clamp(pos.x, -limiteX, limiteX);
        pos.y = Mathf.Clamp(pos.y, -limiteY, limiteY);
        rb.position = pos;
    }
}
