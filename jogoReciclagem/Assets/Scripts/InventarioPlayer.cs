using System.Collections.Generic;
using UnityEngine;
using TMPro; // Usado para acessar os textos da UI!

public class InventarioPlayer : MonoBehaviour
{
    [Header("Interface (UI)")]
    [Tooltip("Arraste o objeto TextoPontuacao do Canvas para cá")]
    public TextMeshProUGUI textoPontosUI;
    [Tooltip("Arraste o objeto TextoMochila do Canvas para cá")]
    public TextMeshProUGUI textoMochilaUI;
    [Tooltip("Arraste um novo Texto do Canvas para avisos (ex: Mochila Cheia)")]
    public TextMeshProUGUI textoAvisoUI;

    [Header("Configurações do Inventário")]
    [Tooltip("Máximo de lixo ORGÂNICO que o jogador pode carregar")]
    public int limiteOrganico = 3;
    [Tooltip("Máximo de lixo RECICLÁVEL que o jogador pode carregar")]
    public int limiteReciclavel = 3;

    [Header("Informações Atuais (Apenas Leitura)")]
    public int qtdOrganicoAtual = 0;
    public int qtdReciclavelAtual = 0;

    [Header("Pontuação Atual")]
    public int pontos = 0;

    // Guarda a lixeira que estamos encostando no momento
    private Lixeira lixeiraProxima = null;

    private void Start()
    {
        // Atualiza os textos na tela assim que o jogo começa
        AtualizarTextosNaTela();
        if (textoAvisoUI != null) textoAvisoUI.gameObject.SetActive(false); // Esconde o aviso no começo
    }

    private void Update()
    {
        // A checagem de botões deve ser sempre feita no Update!
        // Se estamos perto de uma lixeira, verificamos se o jogador quer descartar algo
        if (lixeiraProxima != null)
        {
            // Botão 1: Tenta jogar lixo ORGÂNICO na lixeira que estiver na frente
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                TentarDescartar(TipoLixo.TipoDeLixo.Organico, lixeiraProxima);
            }

            // Botão 2: Tenta jogar lixo RECICLÁVEL na lixeira que estiver na frente
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                TentarDescartar(TipoLixo.TipoDeLixo.Reciclavel, lixeiraProxima);
            }
        }
    }

    private void MostrarAviso(string mensagem)
    {
        if (textoAvisoUI != null)
        {
            textoAvisoUI.text = mensagem;
            textoAvisoUI.gameObject.SetActive(true);

            // Chama a função EsconderAviso depois de 2 segundos
            CancelInvoke("EsconderAviso");
            Invoke("EsconderAviso", 2f);
        }
    }

    private void EsconderAviso()
    {
        if (textoAvisoUI != null)
        {
            textoAvisoUI.gameObject.SetActive(false);
        }
    }

    // Função que muda o que está escrito nos textos
    private void AtualizarTextosNaTela()
    {
        if (textoPontosUI != null)
        {
            textoPontosUI.text = "Pontos: " + pontos;
        }

        if (textoMochilaUI != null)
        {
            textoMochilaUI.text = "Mochila:\n" +
                                  "Orgânico: " + qtdOrganicoAtual + "/" + limiteOrganico + "\n" +
                                  "Reciclável: " + qtdReciclavelAtual + "/" + limiteReciclavel;
        }
    }

    // Esta função do Unity roda quando o jogador "passa por cima" de um Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que batemos tem o script TipoLixo (ou seja, é um lixo)
        TipoLixo lixoEncontrado = collision.GetComponent<TipoLixo>();

        // Se realmente for um lixo...
        if (lixoEncontrado != null)
        {
            bool coletou = false;

            // Verifica qual o tipo e se tem espaço para ELE
            if (lixoEncontrado.tipo == TipoLixo.TipoDeLixo.Organico && qtdOrganicoAtual < limiteOrganico)
            {
                qtdOrganicoAtual++;
                coletou = true;
            }
            else if (lixoEncontrado.tipo == TipoLixo.TipoDeLixo.Reciclavel && qtdReciclavelAtual < limiteReciclavel)
            {
                qtdReciclavelAtual++;
                coletou = true;
            }

            if (coletou)
            {
                Destroy(collision.gameObject);
                AtualizarTextosNaTela();
            }
            else
            {
                MostrarAviso("Mochila cheia para este tipo de lixo!");
            }
            return; // Para a função aqui, pois já resolvemos a coleta
        }

        // Se não foi um lixo, verifica se encostamos numa Lixeira
        Lixeira lixeiraEncontrada = collision.GetComponent<Lixeira>();
        if (lixeiraEncontrada != null)
        {
            // Guarda a lixeira para sabermos que estamos perto dela no Update
            lixeiraProxima = lixeiraEncontrada;
        }
    }

    // Esta função roda quando o jogador SAI de cima de um Trigger
    private void OnTriggerExit2D(Collider2D collision)
    {
        Lixeira lixeiraSaindo = collision.GetComponent<Lixeira>();
        if (lixeiraSaindo != null && lixeiraSaindo == lixeiraProxima)
        {
            // Quando saímos de perto da lixeira, esquecemos dela
            lixeiraProxima = null;
        }
    }

    private void TentarDescartar(TipoLixo.TipoDeLixo tipoEscolhidoPeloJogador, Lixeira lixeira)
    {
        // 1. O jogador tem esse lixo na mochila?
        if (tipoEscolhidoPeloJogador == TipoLixo.TipoDeLixo.Organico && qtdOrganicoAtual <= 0) return;
        if (tipoEscolhidoPeloJogador == TipoLixo.TipoDeLixo.Reciclavel && qtdReciclavelAtual <= 0) return;

        // 2. A lixeira aceita esse lixo?
        if (lixeira.lixoAceito == tipoEscolhidoPeloJogador)
        {
            // ACERTOU!
            pontos += 10;
            if (tipoEscolhidoPeloJogador == TipoLixo.TipoDeLixo.Organico) qtdOrganicoAtual--;
            else qtdReciclavelAtual--;

            Debug.Log("Descarte Correto! +10 pontos.");
        }
        else
        {
            // ERROU DE LIXEIRA! (Penalidade opcional)
            pontos -= 5;
            if (pontos < 0) pontos = 0; // Não deixa a pontuação ficar negativa

            // O lixo é jogado fora de qualquer jeito (esvazia a mochila), mas ele perde pontos por jogar errado
            if (tipoEscolhidoPeloJogador == TipoLixo.TipoDeLixo.Organico) qtdOrganicoAtual--;
            else qtdReciclavelAtual--;

            Debug.Log("Lixeira Errada! -5 pontos de penalidade.");
        }

        AtualizarTextosNaTela();
    }
}
