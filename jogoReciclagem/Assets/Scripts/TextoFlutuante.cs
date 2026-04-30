using UnityEngine;
using TMPro;

public class TextoFlutuante : MonoBehaviour
{
    public float velocidadeSubida = 1f;
    public float tempoDeVida = 1.5f;
    private TextMeshPro textoMesh;

    void Awake()
    {
        textoMesh = GetComponent<TextMeshPro>();
        Destroy(gameObject, tempoDeVida);
    }

    void Update()
    {
        transform.position += Vector3.up * velocidadeSubida * Time.deltaTime;
    }

    public void ConfigurarTexto(string texto, Color cor)
    {
        if (textoMesh != null)
        {
            textoMesh.text = texto;
            textoMesh.color = cor;
        }
    }
}
