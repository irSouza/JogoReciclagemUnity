using UnityEngine;

public class AnimacaoUI : MonoBehaviour
{
    public float velocidade = 10f;
    public float atraso = 0f; // Útil para as estrelas!
    
    private Vector3 tamanhoAlvo;
    private float tempoAtivacao;
    private bool podeAnimar = false;

    void OnEnable()
    {
        // Quando o objeto liga, ele fica tamanho 0 e começa a contar o tempo
        transform.localScale = Vector3.zero;
        tamanhoAlvo = Vector3.one; // Tamanho 1 (tamanho normal)
        tempoAtivacao = Time.unscaledTime;
        podeAnimar = true;
    }

    void Update()
    {
        if (!podeAnimar) return;

        // Se já passou o tempo de atraso, ele começa a crescer
        if (Time.unscaledTime >= tempoAtivacao + atraso)
        {
            // Cresce ignorando o pause!
            transform.localScale = Vector3.Lerp(transform.localScale, tamanhoAlvo, Time.unscaledDeltaTime * velocidade);
            
            // Opcional: Se chegar muito perto, trava no tamanho exato
            if(Vector3.Distance(transform.localScale, tamanhoAlvo) < 0.01f)
            {
                transform.localScale = tamanhoAlvo;
                podeAnimar = false;
            }
        }
    }
}