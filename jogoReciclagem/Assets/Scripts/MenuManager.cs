using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Paineis do Menu")]
    public GameObject painelPrincipal;
    public GameObject painelComandos;

    void Start()
    {
        Time.timeScale = 1f;

        if (painelPrincipal != null) painelPrincipal.SetActive(true);
        if (painelComandos != null) painelComandos.SetActive(false);
    }

    public void IniciarJogo()
    {
        if (TransicaoManager.Instancia != null)
            TransicaoManager.Instancia.CarregarCena("SampleScene");
        else
            SceneManager.LoadScene("SampleScene"); 
    }

    public void AbrirComandos()
    {
        // Ao abrir os comandos, DESLIGAMOS o menu principal pra ele sumir!
        if (painelPrincipal != null) painelPrincipal.SetActive(false);
        
        if (painelComandos != null) painelComandos.SetActive(true);
    }

    public void SairDoJogo()
    {
        Debug.Log("O jogo foi fechado!"); 
        Application.Quit(); 
    }
}