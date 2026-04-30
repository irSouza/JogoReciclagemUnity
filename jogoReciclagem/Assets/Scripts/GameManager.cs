using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float tempoTotal = 60f;
    private float tempoRestante;
    private bool jogoAcabou = false;

    public TextMeshProUGUI textoTimerUI;
    public GameObject painelFimDeJogo;
    public TextMeshProUGUI textoPontosFinalUI;
    
    // Substituímos o texto por essas imagens (GameObjects)
    public GameObject estrela1;
    public GameObject estrela2;
    public GameObject estrela3;

    private InventarioPlayer inventarioPlayer;

    void Start()
    {
        tempoRestante = tempoTotal;
        painelFimDeJogo.SetActive(false);
        Time.timeScale = 1f;
        inventarioPlayer = FindObjectOfType<InventarioPlayer>();
    }

    void Update()
    {
        if (jogoAcabou) return;
        
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
            int segundos = Mathf.CeilToInt(tempoRestante);
            textoTimerUI.text = "Tempo: " + segundos + "s";
            if (segundos <= 10) textoTimerUI.color = Color.red; 
            else textoTimerUI.color = Color.white;
        }
    }

    public static int CalcularEstrelas(int pontos)
    {
        if (pontos <= 0) return 0;
        if (pontos < 50) return 1;
        if (pontos < 100) return 2;
        return 3;
    }

    private void FinalizarJogo()
    {
        jogoAcabou = true;
        Time.timeScale = 0f;
        
        int pontos = (inventarioPlayer != null) ? inventarioPlayer.pontos : 0;
        
        painelFimDeJogo.SetActive(true);
        
        if (textoPontosFinalUI != null) textoPontosFinalUI.text = "Pontuação: " + Mathf.Max(0, pontos);

        int estrelas = CalcularEstrelas(pontos);

        // Lógica visual das Estrelas com Imagens!
        if (estrela1 != null) estrela1.SetActive(estrelas >= 1);
        if (estrela2 != null) estrela2.SetActive(estrelas >= 2);
        if (estrela3 != null) estrela3.SetActive(estrelas >= 3);
    }

    public void BotaoTentarNovamente()
    {
        // Avisa a transição para carregar a mesma cena que estamos!
        TransicaoManager.Instancia.CarregarCena(SceneManager.GetActiveScene().name);
    }

    public void BotaoMenuPrincipal()
    {
        // Avisa a transição para ir pro Menu
        TransicaoManager.Instancia.CarregarCena("MenuScene"); 
    }
}