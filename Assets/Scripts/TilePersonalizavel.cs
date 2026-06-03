using UnityEngine;
using UnityEngine.InputSystem;

public class TilePersonalizavel : MonoBehaviour
{
    public string tipoDoTile = "Piso";

    private Vector3 posicaoMouseAoClicar;
    private bool iniciouCliqueAplicacao = false;
    public float limiteArrastoClique = 10f;

    private void OnMouseDown()
    {
        SistemaPersonalizacao sistema = SistemaPersonalizacao.instancia;

        if (sistema == null)
            return;

        if (sistema.materialSelecionado == null)
            return;

        posicaoMouseAoClicar = Mouse.current.position.ReadValue();
        iniciouCliqueAplicacao = true;
    }

    private void OnMouseUp()
    {
        SistemaPersonalizacao sistema = SistemaPersonalizacao.instancia;

        if (sistema == null)
            return;

        if (!iniciouCliqueAplicacao)
            return;

        float distanciaArrasto = Vector2.Distance(
            posicaoMouseAoClicar,
            Mouse.current.position.ReadValue()
        );

        if (distanciaArrasto > limiteArrastoClique)
        {
            iniciouCliqueAplicacao = false;
            return;
        }

        if (sistema.materialSelecionado == null)
            return;

        if (sistema.tipoSelecionado != tipoDoTile)
            return;

        Renderer render = GetComponent<Renderer>();
        render.material = sistema.materialSelecionado;

        iniciouCliqueAplicacao = false;
    }
}