using UnityEngine;
using UnityEngine.EventSystems;

public class BotaoAnimado : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 tamanhoOriginal;
    public float tamanhoHover = 1.1f;
    public float velocidade = 15f; // Aumentei um pouquinho para ficar mais ágil

    private Vector3 tamanhoAlvo;

    void Start()
    {
        tamanhoOriginal = transform.localScale;
        tamanhoAlvo = tamanhoOriginal;
    }

    void Update()
    {
        // Aqui está o segredo: Time.unscaledDeltaTime ignora o pause (Time.timeScale = 0)!
        transform.localScale = Vector3.Lerp(transform.localScale, tamanhoAlvo, Time.unscaledDeltaTime * velocidade);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tamanhoAlvo = tamanhoOriginal * tamanhoHover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tamanhoAlvo = tamanhoOriginal;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        tamanhoAlvo = tamanhoOriginal * 0.9f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        tamanhoAlvo = tamanhoOriginal * tamanhoHover;
    }
}