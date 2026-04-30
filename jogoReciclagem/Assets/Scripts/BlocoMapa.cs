using System.Collections.Generic;
using UnityEngine;

public class BlocoMapa : MonoBehaviour
{
    [Header("Configurações do Fundo (Gramado)")]
    [Tooltip("A imagem (Sprite) que servirá como chão do bloco. Um quadrado branco simples serve!")]
    public Sprite spriteDeFundo;
    [Tooltip("A cor que queremos pintar o fundo (ex: Verde grama)")]
    public Color corDoFundo = new Color(0.1f, 0.6f, 0.1f); // Um verde padrão
    [Tooltip("Tamanho EXATO do bloco inteiro (para a imagem cobrir tudo)")]
    public float larguraBloco = 18f;
    public float alturaBloco = 10f;

    [Header("Configurações do Bloco (Safe Area)")]
    [Tooltip("Área onde itens nascem (menor que a tela inteira para evitar bordas)")]
    public float larguraSafeArea = 14f; 
    public float alturaSafeArea = 6f;   

    [Header("O que vamos gerar? (Prefabs)")]
    public GameObject prefabLixoOrganico;
    public GameObject prefabLixoReciclavel;
    public GameObject[] prefabsObstaculos;

    [Header("Quantidades")]
    public int minLixos = 2;
    public int maxLixos = 5;
    public int minObstaculos = 1;
    public int maxObstaculos = 3;

    [Header("Configurações de Otimização (Grid)")]
    [Tooltip("Tamanho de cada célula do grid. Deve ser maior que o diâmetro dos objetos (ex: 2.0).")]
    public float tamanhoCelula = 2.0f;
    [Tooltip("Raio de verificação para garantir que não haja sobreposição com objetos externos.")]
    public float raioColisao = 0.8f;

    private struct CelulaGrid
    {
        public int ColunaX;
        public Vector2 Centro;

        public CelulaGrid(int colunaX, Vector2 centro)
        {
            ColunaX = colunaX;
            Centro = centro;
        }
    }

    private List<CelulaGrid> celulasLivres;
    private int[] obstaculosNaColuna;
    private int totalLinhas;

    void Awake()
    {
        // Awake roda no momento exato em que o objeto é criado (Instantiate), 
        // antes de qualquer script ser desativado pelo GeradorDeBlocos!
        CriarChao();
    }

    void Start()
    {
        InicializarGrid();
        GerarItens();
    }

    private void CriarChao()
    {
        // Só cria o chão se você colocar um Sprite de referência!
        if (spriteDeFundo != null)
        {
            // Cria um novo objeto vazio "Chao" dentro do nosso bloco
            GameObject objChao = new GameObject("Chao_Visual");
            objChao.transform.SetParent(this.transform);
            
            // Coloca ele exatamente no centro do bloco, mas um pouco para trás no eixo Z
            // para não ficar na frente do lixo e do jogador!
            objChao.transform.localPosition = new Vector3(0, 0, 10f);

            // Adiciona o componente de desenhar imagem
            SpriteRenderer renderizador = objChao.AddComponent<SpriteRenderer>();
            renderizador.sprite = spriteDeFundo;
            renderizador.color = corDoFundo;

            // Ajusta o tamanho da imagem para ficar do tamanho exato da "larguraBloco" e "alturaBloco".
            // A matemática aqui divide o tamanho que queremos pelo tamanho original da imagem para achar a escala certa.
            float tamanhoXReal = renderizador.sprite.bounds.size.x;
            float tamanhoYReal = renderizador.sprite.bounds.size.y;
            
            objChao.transform.localScale = new Vector3(
                larguraBloco / tamanhoXReal, 
                alturaBloco / tamanhoYReal, 
                1f
            );
        }
    }

    private void InicializarGrid()
    {
        celulasLivres = new List<CelulaGrid>();

        int colunas = Mathf.FloorToInt(larguraSafeArea / tamanhoCelula);
        totalLinhas = Mathf.FloorToInt(alturaSafeArea / tamanhoCelula);

        // Se as áreas forem menores que a célula, garante no mínimo 1 para não quebrar
        if (colunas <= 0) colunas = 1;
        if (totalLinhas <= 0) totalLinhas = 1;

        obstaculosNaColuna = new int[colunas];

        float startX = transform.position.x - (colunas * tamanhoCelula) / 2f + (tamanhoCelula / 2f);
        float startY = transform.position.y - (totalLinhas * tamanhoCelula) / 2f + (tamanhoCelula / 2f);

        for (int x = 0; x < colunas; x++)
        {
            for (int y = 0; y < totalLinhas; y++)
            {
                float posX = startX + (x * tamanhoCelula);
                float posY = startY + (y * tamanhoCelula);
                celulasLivres.Add(new CelulaGrid(x, new Vector2(posX, posY)));
            }
        }

        EmbaralharCelulas(celulasLivres);
    }

    private void EmbaralharCelulas(List<CelulaGrid> lista)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            CelulaGrid temp = lista[i];
            int randomIndex = Random.Range(i, lista.Count);
            lista[i] = lista[randomIndex];
            lista[randomIndex] = temp;
        }
    }

    private void GerarItens()
    {
        // 1. Gera Obstáculos
        if (prefabsObstaculos != null && prefabsObstaculos.Length > 0)
        {
            int qtdObstaculos = Random.Range(minObstaculos, maxObstaculos + 1);
            for (int i = 0; i < qtdObstaculos; i++)
            {
                int indice = Random.Range(0, prefabsObstaculos.Length);
                Vector2? posicao = SortearPosicaoLivre(true);
                if (posicao.HasValue)
                {
                    Instantiate(prefabsObstaculos[indice], posicao.Value, Quaternion.identity, transform);
                }
            }
        }

        // 2. Gera Lixos
        int qtdLixos = Random.Range(minLixos, maxLixos + 1);
        for (int i = 0; i < qtdLixos; i++)
        {
            int tipo = Random.Range(0, 2);
            GameObject prefabLixo = (tipo == 0) ? prefabLixoOrganico : prefabLixoReciclavel;
            Vector2? posicao = SortearPosicaoLivre(false);
            if (posicao.HasValue)
            {
                Instantiate(prefabLixo, posicao.Value, Quaternion.identity, transform);
            }
        }
    }

    private Vector2? SortearPosicaoLivre(bool ehObstaculo)
    {
        // Limite de variação dentro da célula (jitter) para parecer mais natural
        float variacaoMaxima = (tamanhoCelula / 2f) - raioColisao;
        if (variacaoMaxima < 0) variacaoMaxima = 0; // Prevenção se a célula for muito pequena

        while (celulasLivres.Count > 0)
        {
            int ultimoIndice = celulasLivres.Count - 1;
            CelulaGrid celula = celulasLivres[ultimoIndice];
            celulasLivres.RemoveAt(ultimoIndice); // Remove do final é O(1)

            // Regra Anti-Bloqueio para obstáculos
            if (ehObstaculo)
            {
                // Se adicionar mais um obstáculo nesta coluna preencheria todas as linhas (ou quase todas),
                // pulamos esta célula para garantir que sempre haverá passagem livre na vertical
                if (obstaculosNaColuna[celula.ColunaX] >= totalLinhas - 1)
                {
                    continue;
                }
            }

            // Aplica Jitter (desvio aleatório)
            float jitterX = Random.Range(-variacaoMaxima, variacaoMaxima);
            float jitterY = Random.Range(-variacaoMaxima, variacaoMaxima);
            Vector2 posicaoCandidata = new Vector2(celula.Centro.x + jitterX, celula.Centro.y + jitterY);

            // Apenas UMA checagem de física
            Collider2D colisao = Physics2D.OverlapCircle(posicaoCandidata, raioColisao);
            if (colisao == null)
            {
                if (ehObstaculo)
                {
                    obstaculosNaColuna[celula.ColunaX]++;
                }
                return posicaoCandidata;
            }
        }

        // Retorna null se não encontrar espaço livre
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        // Desenha a Safe Area em verde
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(larguraSafeArea, alturaSafeArea, 0));
        
        // Desenha o limite total do bloco em amarelo claro
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(larguraBloco, alturaBloco, 0));
    }
}
