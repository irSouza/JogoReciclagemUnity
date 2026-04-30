using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class BotaoAnimado : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 escalaOriginal;
    public float fatorAumento = 1.1f;
    public float fatorClique = 0.9f;
    public float velocidadeAnimacao = 0.1f;

    void Start()
    {
        escalaOriginal = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(AnimarEscala(escalaOriginal * fatorAumento));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(AnimarEscala(escalaOriginal));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(AnimarEscala(escalaOriginal * fatorClique));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(AnimarEscala(escalaOriginal * fatorAumento));
    }

    private IEnumerator AnimarEscala(Vector3 escalaAlvo)
    {
        float tempo = 0;
        Vector3 escalaAtual = transform.localScale;

        while (tempo < velocidadeAnimacao)
        {
            tempo += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(escalaAtual, escalaAlvo, tempo / velocidadeAnimacao);
            yield return null;
        }
        transform.localScale = escalaAlvo;
    }
}
