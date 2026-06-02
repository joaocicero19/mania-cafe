using UnityEngine;

public class LojaUIManager : MonoBehaviour
{
    [Header("Database da Loja")]
    public LojaDatabase lojaDatabase;

    [Header("UI")]
    public Transform containerItens;
    public GameObject prefabBotaoItem;

    private void OnEnable()
    {
        LimparItens();
    }

    public void MostrarCategoriaCadeiras()
    {
        MostrarItensDaCategoria(CategoriaLoja.Cadeiras);
    }

    public void MostrarCategoriaFogoes()
    {
        MostrarItensDaCategoria(CategoriaLoja.Fogoes);
    }

    public void MostrarCategoriaBalcoes()
    {
        MostrarItensDaCategoria(CategoriaLoja.Balcoes);
    }

    public void MostrarCategoriaPisos()
    {
        MostrarItensDaCategoria(CategoriaLoja.Pisos);
    }

    public void MostrarCategoriaParedes()
    {
        MostrarItensDaCategoria(CategoriaLoja.Paredes);
    }

    public void MostrarCategoriaDecoracoes()
    {
        MostrarItensDaCategoria(CategoriaLoja.Decoracoes);
    }

    private void MostrarItensDaCategoria(CategoriaLoja categoria)
    {
        if (lojaDatabase == null)
        {
            Debug.LogError("LojaDatabase não foi atribuída.");
            return;
        }

        if (containerItens == null)
        {
            Debug.LogError("ContainerItens não foi atribuído.");
            return;
        }

        if (prefabBotaoItem == null)
        {
            Debug.LogError("PrefabBotaoItem não foi atribuído.");
            return;
        }

        LimparItens();

        foreach (ItemLojaData item in lojaDatabase.itens)
        {
            if (item.categoria != categoria)
                continue;

            GameObject novoBotao = Instantiate(prefabBotaoItem, containerItens);

            BotaoItemLoja botao = novoBotao.GetComponent<BotaoItemLoja>();

            if (botao != null)
            {
                botao.Configurar(item);
            }
        }
    }

    private void LimparItens()
    {
        foreach (Transform filho in containerItens)
        {
            Destroy(filho.gameObject);
        }
    }
}