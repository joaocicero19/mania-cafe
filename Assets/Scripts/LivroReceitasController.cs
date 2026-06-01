using UnityEngine;

public class LivroReceitasController : MonoBehaviour
{
    public static LivroReceitasController Instance;

    [Header("UI")]
    public GameObject painelLivro;

    [Header("Receitas")]
    public ReceitaData receitaHamburguer;
    public ReceitaData receitaCachorroQuente;

    private FogaoController fogaoSelecionado;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (painelLivro != null)
            painelLivro.SetActive(false);
    }

    public void Abrir(FogaoController fogao)
    {
        fogaoSelecionado = fogao;

        Debug.Log("Fogão selecionado: " + fogao.name);

        if (painelLivro != null)
            painelLivro.SetActive(true);
    }

    public void Fechar()
    {
        if (painelLivro != null)
            painelLivro.SetActive(false);
    }
    public void PrepararCachorroQuente()
    {
        if (fogaoSelecionado == null)
        {
            Debug.LogWarning("Nenhum fogão selecionado.");
            return;
        }

        if (receitaCachorroQuente == null)
        {
            Debug.LogWarning("Receita Cachorro Quente não foi definida no Inspector.");
            return;
        }

        fogaoSelecionado.IniciarPreparo(receitaCachorroQuente);
        Fechar();
    }

    public void PrepararHamburguer()
    {
        Debug.Log("PrepararHamburguer chamado no objeto: " + gameObject.name);

        if (fogaoSelecionado == null)
        {
            Debug.LogWarning("Nenhum fogão selecionado.");
            return;
        }

        if (receitaHamburguer == null)
        {
            Debug.LogWarning("Receita Hambúrguer não foi definida no Inspector.");
            return;
        }

        fogaoSelecionado.IniciarPreparo(receitaHamburguer);
        Fechar();
    }
}