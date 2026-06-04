using UnityEngine;

public class LivroReceitasController : MonoBehaviour
{
    public static LivroReceitasController Instance;

    [Header("UI")]
    public GameObject painelLivro;

    [Header("Sistema Automático")]
    public ReceitaData[] receitasDisponiveis;
    public Transform gridReceitas;
    public GameObject botaoReceitaModelo;

    [Header("Categoria Atual")]
    public CategoriaReceita categoriaAtual = CategoriaReceita.Lanches;

    private FogaoController fogaoSelecionado;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (painelLivro != null)
            painelLivro.SetActive(false);

        GerarBotoesReceitas();
    }

    public void Abrir(FogaoController fogao)
    {
        fogaoSelecionado = fogao;

        if (painelLivro != null)
            painelLivro.SetActive(true);

        GerarBotoesReceitas();
    }

    public void Fechar()
    {
        if (painelLivro != null)
            painelLivro.SetActive(false);

        fogaoSelecionado = null;
    }

    public void SelecionarReceita(ReceitaData receita)
    {
        if (fogaoSelecionado == null)
        {
            Debug.LogWarning("Nenhum fogão selecionado.");
            return;
        }

        if (receita == null)
        {
            Debug.LogWarning("Receita não foi definida.");
            return;
        }

        fogaoSelecionado.IniciarPreparo(receita);
        Fechar();
    }

    private void GerarBotoesReceitas()
    {
        if (gridReceitas == null || botaoReceitaModelo == null)
        {
            Debug.LogWarning("GridReceitas ou BotaoReceitaModelo não configurado.");
            return;
        }

        foreach (Transform filho in gridReceitas)
        {
            if (filho.gameObject != botaoReceitaModelo)
                Destroy(filho.gameObject);
        }

        botaoReceitaModelo.SetActive(false);

        foreach (ReceitaData receita in receitasDisponiveis)
        {
            if (receita == null)
                continue;

            if (receita.categoria != categoriaAtual)
                continue;

            GameObject novoBotao = Instantiate(botaoReceitaModelo, gridReceitas, false);
            novoBotao.SetActive(true);

            BotaoReceitaUI botaoUI = novoBotao.GetComponent<BotaoReceitaUI>();

            if (botaoUI != null)
                botaoUI.Configurar(receita, this);
        }

    }

    public void MostrarLanches()
    {
        categoriaAtual = CategoriaReceita.Lanches;
        GerarBotoesReceitas();
    }

    public void MostrarMassas()
    {
        categoriaAtual = CategoriaReceita.Massas;
        GerarBotoesReceitas();
    }

    public void MostrarCarnes()
    {
        categoriaAtual = CategoriaReceita.Carnes;
        GerarBotoesReceitas();
    }

    public void MostrarDoces()
    {
        categoriaAtual = CategoriaReceita.Doces;
        GerarBotoesReceitas();
    }

    public void MostrarSopas()
    {
        categoriaAtual = CategoriaReceita.Sopas;
        GerarBotoesReceitas();
    }

    public void MostrarSaladas()
    {
        categoriaAtual = CategoriaReceita.Saladas;
        GerarBotoesReceitas();
    }

    public void MostrarPeixes()
    {
        categoriaAtual = CategoriaReceita.Peixes;
        GerarBotoesReceitas();
    }

}