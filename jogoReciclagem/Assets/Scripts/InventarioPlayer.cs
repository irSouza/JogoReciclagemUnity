using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventarioPlayer : MonoBehaviour
{
    [Header("Interface (UI)")]
    public TextMeshProUGUI textoPontosUI;
    public TextMeshProUGUI textoMochilaUI;
    public TextMeshProUGUI textoAvisoUI;

    [Header("Efeitos Visuais")]
    public GameObject prefabTextoFlutuante;
    public ParticleSystem particulaColeta; // Efeito de poeira ou estrelas ao pegar

    [Header("Configurações do Inventário")]
    public int limiteOrganico = 3;
    public int limiteReciclavel = 3;
    public int qtdOrganicoAtual = 0;
    public int qtdReciclavelAtual = 0;
    public int pontos = 0;

    private Lixeira lixeiraProxima = null;

    private void Start()
    {
        AtualizarTextosNaTela();
        if (textoAvisoUI != null) textoAvisoUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (lixeiraProxima != null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                TentarDescartar(TipoLixo.TipoDeLixo.Organico, lixeiraProxima);

            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                TentarDescartar(TipoLixo.TipoDeLixo.Reciclavel, lixeiraProxima);
        }
    }

    private void MostrarAviso(string mensagem)
    {
        if (textoAvisoUI != null)
        {
            textoAvisoUI.text = mensagem;
            textoAvisoUI.gameObject.SetActive(true);
            CancelInvoke("EsconderAviso");
            Invoke("EsconderAviso", 2f);
        }
    }

    private void EsconderAviso() { if (textoAvisoUI != null) textoAvisoUI.gameObject.SetActive(false); }

    private void AtualizarTextosNaTela()
    {
        if (textoPontosUI != null) textoPontosUI.text = "Pontos: " + pontos;
        if (textoMochilaUI != null)
        {
            textoMochilaUI.text = "Mochila:\nOrgânico: " + qtdOrganicoAtual + "/" + limiteOrganico + "\nReciclável: " + qtdReciclavelAtual + "/" + limiteReciclavel;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TipoLixo lixoEncontrado = collision.GetComponent<TipoLixo>();
        if (lixoEncontrado != null)
        {
            bool coletou = false;
            if (lixoEncontrado.tipo == TipoLixo.TipoDeLixo.Organico && qtdOrganicoAtual < limiteOrganico) { qtdOrganicoAtual++; coletou = true; }
            else if (lixoEncontrado.tipo == TipoLixo.TipoDeLixo.Reciclavel && qtdReciclavelAtual < limiteReciclavel) { qtdReciclavelAtual++; coletou = true; }

            if (coletou)
            {
                EfeitoColeta(collision.transform.position);
                Destroy(collision.gameObject);
                AtualizarTextosNaTela();
            }
            else MostrarAviso("Mochila cheia para este tipo de lixo!");
            return;
        }

        Lixeira lixeiraEncontrada = collision.GetComponent<Lixeira>();
        if (lixeiraEncontrada != null) lixeiraProxima = lixeiraEncontrada;
    }

    private void EfeitoColeta(Vector3 posicao)
    {
        MostrarTextoFlutuante("+1", Color.green, posicao);
        if (particulaColeta != null)
        {
            Instantiate(particulaColeta, posicao, Quaternion.identity);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Lixeira lixeiraSaindo = collision.GetComponent<Lixeira>();
        if (lixeiraSaindo != null && lixeiraSaindo == lixeiraProxima) lixeiraProxima = null;
    }

    public static bool CalcularDescarte(
        TipoLixo.TipoDeLixo tipoJogador,
        TipoLixo.TipoDeLixo tipoAceito,
        ref int qtdOrganico,
        ref int qtdReciclavel,
        ref int pontosAtuais,
        out bool sucesso)
    {
        if (tipoJogador == TipoLixo.TipoDeLixo.Organico && qtdOrganico <= 0)
        {
            sucesso = false;
            return false;
        }
        if (tipoJogador == TipoLixo.TipoDeLixo.Reciclavel && qtdReciclavel <= 0)
        {
            sucesso = false;
            return false;
        }

        if (tipoJogador == tipoAceito)
        {
            pontosAtuais += 10;
            sucesso = true;
        }
        else
        {
            pontosAtuais -= 5;
            if (pontosAtuais < 0) pontosAtuais = 0;
            sucesso = false;
        }

        if (tipoJogador == TipoLixo.TipoDeLixo.Organico) qtdOrganico--;
        else qtdReciclavel--;

        return true;
    }

    private void TentarDescartar(TipoLixo.TipoDeLixo tipoEscolhidoPeloJogador, Lixeira lixeira)
    {
        int pontosAnteriores = pontos;

        bool tentou = CalcularDescarte(
            tipoEscolhidoPeloJogador,
            lixeira.lixoAceito,
            ref qtdOrganicoAtual,
            ref qtdReciclavelAtual,
            ref pontos,
            out bool sucesso);

        if (tentou)
        {
            int diferencaDePontos = pontos - pontosAnteriores;

            if (sucesso)
            {
                MostrarTextoFlutuante("+" + diferencaDePontos, Color.green, lixeira.transform.position);
            }
            else
            {
                MostrarTextoFlutuante(diferencaDePontos.ToString(), Color.red, lixeira.transform.position);
            }

            AtualizarTextosNaTela();
        }
    }

    private void MostrarTextoFlutuante(string texto, Color cor, Vector3 posicao)
    {
        if (prefabTextoFlutuante != null)
        {
            GameObject textoObj = Instantiate(prefabTextoFlutuante, posicao, Quaternion.identity);
            TextoFlutuante scriptTexto = textoObj.GetComponent<TextoFlutuante>();
            if (scriptTexto != null)
            {
                scriptTexto.ConfigurarTexto(texto, cor);
            }
        }
    }
}
