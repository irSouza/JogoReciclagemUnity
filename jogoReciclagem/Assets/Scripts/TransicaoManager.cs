using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransicaoManager : MonoBehaviour
{
    public static TransicaoManager Instancia;
    public Image telaPreta;
    public float tempoDeFade = 1f;

    void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
            
            // Garante que o Canvas fique na frente de tudo
            Canvas canvas = GetComponentInChildren<Canvas>();
            if(canvas != null) canvas.sortingOrder = 999;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this == Instancia) StartCoroutine(ClarearTela());
    }

    void Start() 
    { 
        StartCoroutine(ClarearTela()); 
    }

    public void CarregarCena(string nomeCena)
    {
        StartCoroutine(EscurecerMudarCena(nomeCena));
    }

    private IEnumerator EscurecerMudarCena(string nomeCena)
    {
        // 1. Liga a tela preta
        telaPreta.gameObject.SetActive(true);
        telaPreta.raycastTarget = true;
        
        // 2. Escurece aos poucos
        Color cor = telaPreta.color;
        cor.a = 0f;
        while (cor.a < 1f)
        {
            cor.a += Time.unscaledDeltaTime / tempoDeFade;
            telaPreta.color = cor;
            yield return null;
        }
        
        // 3. Garante tela 100% preta e destrava o tempo
        cor.a = 1f;
        telaPreta.color = cor;
        Time.timeScale = 1f; 
        
        // 4. Muda de cena!
        SceneManager.LoadScene(nomeCena);
    }

    private IEnumerator ClarearTela()
    {
        // Liga a tela preta para começar clareando
        telaPreta.gameObject.SetActive(true);
        telaPreta.raycastTarget = true;

        Color cor = telaPreta.color;
        cor.a = 1f; 

        // Clareia aos poucos
        while (cor.a > 0f)
        {
            cor.a -= Time.unscaledDeltaTime / tempoDeFade;
            telaPreta.color = cor;
            yield return null;
        }

        // Esconde a tela
        cor.a = 0f;
        telaPreta.color = cor;
        telaPreta.gameObject.SetActive(false);
    }
}