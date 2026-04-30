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

    void Awake()
    {
        // Awake roda no momento exato em que o objeto é criado (Instantiate),
        // antes de qualquer script ser desativado pelo GeradorDeBlocos!
        CriarChao();
    }

    void Start()
    {
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

    private void GerarItens()
    {
        // 1. Gera Obstáculos
        if (prefabsObstaculos != null && prefabsObstaculos.Length > 0)
        {
            int qtdObstaculos = Random.Range(minObstaculos, maxObstaculos + 1);
            for (int i = 0; i < qtdObstaculos; i++)
            {
                int indice = Random.Range(0, prefabsObstaculos.Length);
                Instantiate(prefabsObstaculos[indice], SortearPosicaoLivre(), Quaternion.identity, transform);
            }
        }

        // 2. Gera Lixos
        int qtdLixos = Random.Range(minLixos, maxLixos + 1);
        for (int i = 0; i < qtdLixos; i++)
        {
            int tipo = Random.Range(0, 2);
            GameObject prefabLixo = (tipo == 0) ? prefabLixoOrganico : prefabLixoReciclavel;
            Instantiate(prefabLixo, SortearPosicaoLivre(), Quaternion.identity, transform);
        }
    }

    private Vector2 SortearPosicaoLivre()
    {
        Vector2 posicaoSorteada = Vector2.zero;
        bool achouLugarVazio = false;
        int tentativas = 0;

        while (!achouLugarVazio && tentativas < 30)
        {
            float x = transform.position.x + Random.Range(-larguraSafeArea / 2f, larguraSafeArea / 2f);
            float y = transform.position.y + Random.Range(-alturaSafeArea / 2f, alturaSafeArea / 2f);
            posicaoSorteada = new Vector2(x, y);

            Collider2D colisao = Physics2D.OverlapCircle(posicaoSorteada, 0.8f);
            if (colisao == null)
            {
                achouLugarVazio = true;
            }
            tentativas++;
        }

        return posicaoSorteada;
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
