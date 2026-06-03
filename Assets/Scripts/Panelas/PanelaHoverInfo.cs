using UnityEngine;

public class PanelaHoverInfo : MonoBehaviour
{
    private FogaoController fogao;

    public void Configurar(FogaoController fogaoOrigem)
    {
        fogao = fogaoOrigem;
    }

    private void OnMouseEnter()
    {
        MostrarInformacao();
    }

    private void OnMouseOver()
    {
        MostrarInformacao();
    }

    private void OnMouseExit()
    {
        if (BalaoInfoCursor.Instance != null)
        {
            BalaoInfoCursor.Instance.Esconder();
        }
    }

    private void MostrarInformacao()
    {
        if (fogao == null)
            return;

        if (MenuLoja.LojaAberta)
            return;

        if (MenuLoja.EstaPosicionandoItem)
            return;

        if (fogao.EstaPreparando())
        {
            BalaoInfoCursor.Instance.Mostrar(
                fogao.GetNomeReceitaAtual() +
                " - " +
                Mathf.CeilToInt(fogao.GetTempoRestante()) +
                "s"
            );

            return;
        }

        if (fogao.ComidaEstaPronta())
        {
            BalaoInfoCursor.Instance.Mostrar("Pronto para servir");
        }
    }
    private void OnMouseDown()
    {
        if (fogao == null)
            return;

        if (MenuLoja.LojaAberta)
            return;

        if (MenuLoja.EstaPosicionandoItem)
            return;

        if (fogao.ComidaEstaPronta())
        {
            fogao.EnviarComidaParaBalcao();
        }
    }
}