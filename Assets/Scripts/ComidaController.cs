using UnityEngine;

public class ComidaController : MonoBehaviour
{
    private FogaoController fogaoOrigem;
    private bool estaNoBalcao = false;

    private string nomePrato = "Prato";
    private int unidades = 1;

    public void Configurar(FogaoController fogao)
    {
        fogaoOrigem = fogao;
        estaNoBalcao = false;
    }

    public void ConfigurarDadosDoPrato(string nome, int qtdUnidades)
    {
        nomePrato = nome;
        unidades = qtdUnidades;
    }

    public string GetNomePrato()
    {
        return nomePrato;
    }

    public int GetUnidades()
    {
        return unidades;
    }

    public void DefinirUnidades(int novaQuantidade)
    {
        unidades = novaQuantidade;
    }

    public void MarcarComoNoBalcao()
    {
        estaNoBalcao = true;
        fogaoOrigem = null;
    }

    private void OnMouseDown()
    {
        if (estaNoBalcao)
            return;

        if (fogaoOrigem != null && fogaoOrigem.ComidaEstaPronta())
        {
            fogaoOrigem.EnviarComidaParaBalcao();
        }
    }

    private void OnMouseEnter()
    {
        MostrarOuAtualizarBalao();
    }

    private void OnMouseOver()
    {
        MostrarOuAtualizarBalao();
    }

    private void OnMouseExit()
    {
        if (BalaoInfoCursor.Instance != null)
            BalaoInfoCursor.Instance.Esconder();
    }
    public void MostrarAvisoSemBalcao()
    {
        if (BalaoInfoCursor.Instance != null)
        {
            BalaoInfoCursor.Instance.Mostrar("Não há Balcões Disponíveis");
        }
    }

    private void MostrarOuAtualizarBalao()
    {
        if (BalaoInfoCursor.Instance == null)
            return;

        if (estaNoBalcao)
        {
            BalaoInfoCursor.Instance.Mostrar(
                nomePrato + "\nDisponível: " + unidades + " un."
            );
            return;
        }

        if (fogaoOrigem == null)
            return;

        if (fogaoOrigem.EstaPreparando())
        {
            BalaoInfoCursor.Instance.Mostrar(
                fogaoOrigem.GetNomeReceitaAtual() +
                "\nPreparando: " +
                Mathf.CeilToInt(fogaoOrigem.GetTempoRestante()) +
                "s"
            );
            return;
        }

        if (fogaoOrigem.ComidaEstaPronta())
        {
            BalaoInfoCursor.Instance.Mostrar(
                fogaoOrigem.GetNomeReceitaAtual() +
                "\nPronto pra servir"
            );
        }
    }
}