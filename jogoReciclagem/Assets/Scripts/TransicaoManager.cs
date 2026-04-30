using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TransicaoManager : MonoBehaviour
{
    public static TransicaoManager Instancia { get; private set; }

    public Image telaPreta;
    public float tempoTransicao = 0.5f;

    void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (telaPreta != null)
        {
            StartCoroutine(EfeitoFade(1f, 0f));
        }
    }

    public void MudarCena(string nomeCena)
    {
        StartCoroutine(Transitar(nomeCena));
    }

    private IEnumerator Transitar(string nomeCena)
    {
        if (telaPreta != null)
        {
            telaPreta.gameObject.SetActive(true);
            yield return StartCoroutine(EfeitoFade(0f, 1f));
        }
        SceneManager.LoadScene(nomeCena);
        if (telaPreta != null)
        {
            yield return StartCoroutine(EfeitoFade(1f, 0f));
        }
    }

    private IEnumerator EfeitoFade(float alfaInicial, float alfaFinal)
    {
        float tempo = 0;
        Color cor = telaPreta.color;

        while (tempo < tempoTransicao)
        {
            tempo += Time.unscaledDeltaTime;
            cor.a = Mathf.Lerp(alfaInicial, alfaFinal, tempo / tempoTransicao);
            telaPreta.color = cor;
            yield return null;
        }

        cor.a = alfaFinal;
        telaPreta.color = cor;

        if (alfaFinal == 0f)
        {
            telaPreta.gameObject.SetActive(false);
        }
    }
}
