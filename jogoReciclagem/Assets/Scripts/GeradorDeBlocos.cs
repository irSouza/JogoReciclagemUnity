using UnityEngine;

public class GeradorDeBlocos : MonoBehaviour
{
    [Header("Blocos do Mundo")]
    [Tooltip("Arraste o prefab do Bloco (Safe Area) aqui")]
    public GameObject prefabBlocoMapa;

    [Tooltip("Tamanho físico de cada Bloco (1920x1080 em pixels = ~18x10 em unidades do Unity)")]
    public float larguraBlocoX = 18f;
    public float alturaBlocoY = 10f;

    [Header("Centro (0,0)")]
    public GameObject prefabLixeiraOrganica;
    public GameObject prefabLixeiraReciclavel;
    public GameObject prefabFonte;

    void Start()
    {
        GerarGrade3x3();
    }

    private void GerarGrade3x3()
    {
        // 3x3 significa que os blocos vão de -1 a 1 em X e em Y (um grid de 9 espaços com o centro 0,0)
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // Calcula a posição do centro deste bloco na grade
                Vector2 posicaoBloco = new Vector2(x * larguraBlocoX, y * alturaBlocoY);

                // Instancia o bloco inteiro (com grama e tudo).
                // Todos os 9 blocos agora são instanciados para termos o fundo contínuo!
                GameObject blocoCriado = Instantiate(prefabBlocoMapa, posicaoBloco, Quaternion.identity);

                // Se for o centro (0,0), não queremos que nasça lixo espalhado ou árvore nele!
                if (x == 0 && y == 0)
                {
                    // Desliga o script BlocoMapa para ele não gerar lixo/obstáculos aleatórios
                    // Como mudamos o CriarChao() para o Awake() no BlocoMapa.cs, o chão será criado
                    // ANTES do script ser desativado aqui. Isso resolve o problema da tela inicial azul!
                    BlocoMapa scriptBloco = blocoCriado.GetComponent<BlocoMapa>();
                    if (scriptBloco != null)
                    {
                        scriptBloco.enabled = false;
                    }

                    // E colocamos as Lixeiras fixas e a Fonte no meio do mapa!
                    if (prefabFonte != null)
                        Instantiate(prefabFonte, posicaoBloco + new Vector2(0, 2f), Quaternion.identity);

                    if (prefabLixeiraOrganica != null)
                        Instantiate(prefabLixeiraOrganica, posicaoBloco + new Vector2(-3f, 0), Quaternion.identity);

                    if (prefabLixeiraReciclavel != null)
                        Instantiate(prefabLixeiraReciclavel, posicaoBloco + new Vector2(3f, 0), Quaternion.identity);
                }
            }
        }
    }
}
