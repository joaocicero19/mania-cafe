using UnityEngine;
using System.Collections.Generic;

public class AreaCafeManager : MonoBehaviour
{
    public static AreaCafeManager instancia;

    [Header("Tamanho inicial do café")]
    public int largura = 5;
    public int profundidade = 5;

    [Header("Origem da área do café")]
    public Vector2Int origem = Vector2Int.zero;
    private HashSet<Vector2Int> gridsExtrasLiberados = new HashSet<Vector2Int>();

    private void Awake()
    {
        instancia = this;
    }

    public bool EstaDentroDaArea(Vector3 posicaoMundo)
    {
        Vector2Int grid = MundoParaGrid(posicaoMundo);
        return EstaDentroDaArea(grid);
    }

    public bool EstaDentroDaArea(Vector2Int grid)
    {
        int minX = origem.x;
        int maxX = origem.x + largura - 1;

        int minZ = origem.y;
        int maxZ = origem.y + profundidade - 1;

        bool dentroAreaInicial =
            grid.x >= minX &&
            grid.x <= maxX &&
            grid.y >= minZ &&
            grid.y <= maxZ;

        if (dentroAreaInicial)
            return true;

        return gridsExtrasLiberados.Contains(grid);
    }

    public Vector2Int MundoParaGrid(Vector3 posicaoMundo)
    {
        int x = Mathf.RoundToInt(posicaoMundo.x);
        int z = Mathf.RoundToInt(posicaoMundo.z);

        return new Vector2Int(x, z);
    }
    public void LiberarGridExtra(Vector2Int grid)
    {
        if (!gridsExtrasLiberados.Contains(grid))
        {
            gridsExtrasLiberados.Add(grid);
            Debug.Log("Grid extra liberado para edição: " + grid);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for (int x = origem.x; x < origem.x + largura; x++)
        {
            for (int z = origem.y; z < origem.y + profundidade; z++)
            {
                Vector3 centro = new Vector3(x, 0.05f, z);
                Gizmos.DrawWireCube(centro, new Vector3(1f, 0.05f, 1f));
            }
        }
    }
}