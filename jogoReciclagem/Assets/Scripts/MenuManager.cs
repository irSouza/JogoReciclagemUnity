using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject painelPrincipal;
    public GameObject painelComandos;

    void Start()
    {
        MostrarMenuPrincipal();
    }

    public void IniciarJogo()
    {
        if (TransicaoManager.Instancia != null)
        {
            TransicaoManager.Instancia.MudarCena("SampleScene");
        }
        else
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void AbrirComandos()
    {
        painelPrincipal.SetActive(false);
        painelComandos.SetActive(true);
    }

    public void MostrarMenuPrincipal()
    {
        painelPrincipal.SetActive(true);
        painelComandos.SetActive(false);
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }
}
