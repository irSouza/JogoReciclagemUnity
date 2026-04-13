using UnityEngine;

public class TipoLixo : MonoBehaviour
{
    [Header("Configurações do Lixo")]
    [Tooltip("Defina se este lixo é Orgânico ou Reciclável")]
    public TipoDeLixo tipo;

    public enum TipoDeLixo
    {
        Organico,
        Reciclavel
    }

    // Na próxima fase (Fase 4), colocaremos aqui a lógica para o jogador coletar o lixo!
}
