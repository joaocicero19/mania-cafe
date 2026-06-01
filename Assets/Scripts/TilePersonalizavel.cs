using UnityEngine;

public class TilePersonalizavel : MonoBehaviour
{
    public string tipoDoTile = "Piso";

    private void OnMouseDown()
    {
        var sistema = SistemaPersonalizacao.instancia;

        if (sistema.materialSelecionado == null)
            return;

        if (sistema.tipoSelecionado != tipoDoTile)
            return;

        Renderer render = GetComponent<Renderer>();
        render.material = sistema.materialSelecionado;

        sistema.CancelarSelecao();
    }
}