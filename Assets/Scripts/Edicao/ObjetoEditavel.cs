using UnityEngine;

public class ObjetoEditavel : MonoBehaviour
{
    [Header("Offset de encaixe no grid")]
    public Vector3 offsetGrid;

    [Header("Tamanho ocupado no grid")]
    public Vector2Int tamanhoGrid = Vector2Int.one;

    public Vector2Int PosicaoGrid()
    {
        Vector3 posicaoBase = transform.position - offsetGrid;

        int x = Mathf.RoundToInt(posicaoBase.x);
        int z = Mathf.RoundToInt(posicaoBase.z);

        return new Vector2Int(x, z);
    }

    public bool OcupaGrid(Vector2Int grid)
    {
        Vector2Int origem = PosicaoGrid();

        return grid.x >= origem.x &&
               grid.x < origem.x + tamanhoGrid.x &&
               grid.y >= origem.y &&
               grid.y < origem.y + tamanhoGrid.y;
    }
}