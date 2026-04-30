using UnityEngine;
using System.Collections;

public class JanelaPopup : MonoBehaviour
{
    public GameObject painelParaDesligar;
    public GameObject painelParaLigar;
    public float duracaoAnimacao = 0.3f;

    void OnEnable()
    {
        transform.localScale = Vector3.zero;
        if (painelParaLigar != null) painelParaLigar.SetActive(true);
        StartCoroutine(AnimarEscala(Vector3.one));
    }

    public void FecharPopup()
    {
        StartCoroutine(FecharAnimacao());
    }

    private IEnumerator FecharAnimacao()
    {
        yield return StartCoroutine(AnimarEscala(Vector3.zero));

        if (painelParaDesligar != null) painelParaDesligar.SetActive(false);
        if (painelParaLigar != null) painelParaLigar.SetActive(true);
    }

    private IEnumerator AnimarEscala(Vector3 escalaFinal)
    {
        Vector3 escalaInicial = transform.localScale;
        float tempo = 0;

        while (tempo < duracaoAnimacao)
        {
            tempo += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, tempo / duracaoAnimacao);
            yield return null;
        }

        transform.localScale = escalaFinal;
    }
}
