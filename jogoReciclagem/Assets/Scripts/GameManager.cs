using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Configurações do Jogo")]
    [Tooltip("Tempo em segundos que o jogador tem para jogar (ex: 60 = 1 minuto)")]
    public float tempoTotal = 60f;
    private float tempoRestante;
    private bool jogoAcabou = false;

    [Header("Interface (UI)")]
    public TextMeshProUGUI textoTimerUI;

    [Tooltip("O Painel que vai aparecer quando o tempo acabar")]
    public GameObject painelFimDeJogo;
    public TextMeshProUGUI textoResultadoUI;

    private InventarioPlayer inventarioPlayer;

    void Start()
    {
        // Começamos o jogo com o tempo cheio e a tela final escondida
        tempoRestante = tempoTotal;
        painelFimDeJogo.SetActive(false);

        // Despausamos o jogo, caso estivesse pausado antes
        Time.timeScale = 1f;

        // Achamos o jogador para ler os pontos dele depois
        inventarioPlayer = FindObjectOfType<InventarioPlayer>();
    }

    void Update()
    {
        if (jogoAcabou) return;

        // O timer vai descendo de acordo com o relógio da vida real
        tempoRestante -= Time.deltaTime;

        if (tempoRestante <= 0)
        {
            tempoRestante = 0;
            FinalizarJogo();
        }

        AtualizarTimerNaTela();
    }

    private void AtualizarTimerNaTela()
    {
        if (textoTimerUI != null)
        {
            // Converte os segundos quebrados (ex: 59.94) num número inteiro para ficar bonito na tela
            int segundos = Mathf.CeilToInt(tempoRestante);
            textoTimerUI.text = "Tempo: " + segundos + "s";

            // Fica vermelho nos últimos 10 segundos
            if (segundos <= 10)
                textoTimerUI.color = Color.red;
            else
                textoTimerUI.color = Color.white;
        }
    }

    private void FinalizarJogo()
    {
        jogoAcabou = true;

        // Pausa o mundo todo (nada mais se move)
        Time.timeScale = 0f;

        // Calcula as estrelas
        int pontos = (inventarioPlayer != null) ? inventarioPlayer.pontos : 0;
        int estrelas = 1; // Pelo menos 1 estrela por tentar

        if (pontos >= 50) estrelas = 2;  // 5 acertos
        if (pontos >= 100) estrelas = 3; // 10 acertos

        // Mostra a tela de fim de jogo
        painelFimDeJogo.SetActive(true);

        if (textoResultadoUI != null)
        {
            textoResultadoUI.text = "FIM DE JOGO!\n" +
                                    "Você fez: " + pontos + " Pontos\n\n" +
                                    "Nota Final: " + estrelas + " Estrelas!";
        }
    }
}
