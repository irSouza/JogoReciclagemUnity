using UnityEngine;

public class JanelaPopup : MonoBehaviour
{
    public float velocidade = 15f;
    public GameObject painelParaDesligar; // Ex: Painel_Comandos
    public GameObject painelParaLigar;    // Ex: Painel_Principal

    private Vector3 tamanhoAlvo;
    private bool estaFechando = false;

    void OnEnable()
    {
        transform.localScale = Vector3.zero;
        tamanhoAlvo = Vector3.one; 
        estaFechando = false;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, tamanhoAlvo, Time.unscaledDeltaTime * velocidade);

        if (estaFechando && transform.localScale.x <= 0.05f)
        {
            // Quando terminar de encolher...
            // 1. Religa o menu de trás (Painel Principal)
            if (painelParaLigar != null) painelParaLigar.SetActive(true);
            
            // 2. Desliga o menu de comandos inteiro
            if (painelParaDesligar != null) painelParaDesligar.SetActive(false);
        }
    }

    public void FecharPopup()
    {
        tamanhoAlvo = Vector3.zero; // Manda encolher
        estaFechando = true; // Avisa que está fechando
    }
}