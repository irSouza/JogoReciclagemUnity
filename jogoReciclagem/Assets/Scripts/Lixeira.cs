using UnityEngine;

public class Lixeira : MonoBehaviour
{
    [Header("Configuração")]
    [Tooltip("Qual lixo essa lixeira aceita? (Organico ou Reciclavel)")]
    public TipoLixo.TipoDeLixo lixoAceito;

}