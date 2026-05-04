using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public float tempoTotal = 60f;
    private float tempoRestante;
    private bool jogoAcabou = false;

    public TextMeshProUGUI textoTimerUI;
    public GameObject painelFimDeJogo;
    public TextMeshProUGUI textoResultadoUI;

    // Array para os objetos das estrelas na UI
    public GameObject[] estrelasUI;

    private InventarioPlayer inventarioPlayer;

    void Start()
    {
        tempoRestante = tempoTotal;
        painelFimDeJogo.SetActive(false);
        Time.timeScale = 1f;

        // Esconde as estrelas no começo
        if (estrelasUI != null)
        {
            foreach(GameObject estrela in estrelasUI)
            {
                if (estrela != null) estrela.SetActive(false);
            }
        }

        inventarioPlayer = Object.FindFirstObjectByType<InventarioPlayer>();
    }

    void Update()
    {
        if (jogoAcabou) return;
        tempoRestante -= Time.deltaTime;
        if (tempoRestante <= 0) { tempoRestante = 0; FinalizarJogo(); }
        AtualizarTimerNaTela();
    }

    private void AtualizarTimerNaTela()
    {
        if (textoTimerUI != null)
        {
            int segundos = Mathf.CeilToInt(tempoRestante);
            textoTimerUI.text = "Tempo: " + segundos + "s";
            if (segundos <= 10) textoTimerUI.color = Color.red; else textoTimerUI.color = Color.white;
        }
    }

    public static int CalcularEstrelas(int pontos)
    {
        if (pontos >= 100) return 3;
        if (pontos >= 50) return 2;
        return 1;
    }

    private void FinalizarJogo()
    {
        jogoAcabou = true;
        Time.timeScale = 0f; // Pausa o jogo, mas para a coroutine rodar, precisamos usar unscaledTime se animarmos.

        int pontos = (inventarioPlayer != null) ? inventarioPlayer.pontos : 0;
        int estrelas = CalcularEstrelas(pontos);

        painelFimDeJogo.SetActive(true);
        if (textoResultadoUI != null)
        {
            textoResultadoUI.text = "FIM DE JOGO!\nVocê fez: " + pontos + " Pontos\n\nNota Final: " + estrelas + " Estrelas!";
        }

        // Ativa as estrelas animadas correspondentes
        if (estrelasUI != null)
        {
            for (int i = 0; i < estrelas; i++)
            {
                if (i < estrelasUI.Length && estrelasUI[i] != null)
                {
                    estrelasUI[i].SetActive(true);
                }
            }
        }
    }

    public void BotaoTentarNovamente()
    {
        Time.timeScale = 1f;
        if (TransicaoManager.Instancia != null)
        {
            TransicaoManager.Instancia.MudarCena("SampleScene");
        }
        else
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void BotaoMenuPrincipal()
    {
        Time.timeScale = 1f;
        if (TransicaoManager.Instancia != null)
        {
            TransicaoManager.Instancia.MudarCena("MenuScene");
        }
        else
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}
