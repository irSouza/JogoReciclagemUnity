using UnityEngine;

public class Lixeira : MonoBehaviour
{
    [Header("Configuração")]
    [Tooltip("Qual lixo essa lixeira aceita? (Organico ou Reciclavel)")]
    public TipoLixo.TipoDeLixo lixoAceito;

    void Awake()
    {
        // Garante que a lixeira seja um Trigger, senão o jogador vai trombar nela
        // e o script de descartar o lixo (OnTriggerStay2D) não vai funcionar!
        Collider2D colisor = GetComponent<Collider2D>();
        if (colisor != null)
        {
            colisor.isTrigger = true;
        }
    }
}
