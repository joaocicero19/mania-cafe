using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class SistemaExpansaoPorPisos : MonoBehaviour
{
    [Header("Pisos")]
    public Transform containerPisos;
    public Material materialAreaComprada;

    [Header("Layer liberada")]
    public string layerAreaCafe = "AreaCafe";

    [Header("Limites atuais da área café")]
    public int minX = -3;
    public int maxX = 3;
    public int minZ = -3;
    public int maxZ = 1;

    [Header("Controle da linha de baixo")]
    public int minXCompravel = -2;

    [Header("Configuração")]
    public float tamanhoBloco = 1f;

    [Header("NavMesh")]
    public NavMeshSurface navMeshSurface;

    public void ComprarExpansao()
    {
        int novoMaxX = maxX + 1;
        int novoMinZ = minZ - 1;

        for (int z = novoMinZ; z <= maxZ; z++)
        {
            LiberarPiso(novoMaxX, z);
        }

        for (int x = minXCompravel; x <= novoMaxX; x++)
        {
            LiberarPiso(x, novoMinZ);
        }

        maxX = novoMaxX;
        minZ = novoMinZ;

        if (navMeshSurface != null)
            navMeshSurface.BuildNavMesh();

        Debug.Log("Expansão comprada. Novo limite: X " + minX + " até " + maxX + " | Z " + minZ + " até " + maxZ);
    }

    private void LiberarPiso(int x, int z)
    {
        Transform piso = EncontrarPisoNaPosicao(x, z);

        if (piso == null)
        {
            Debug.LogWarning("Não encontrei piso na posição X=" + x + " Z=" + z);
            return;
        }

        // MUDA A LAYER DO PISO
        int layerCafe = LayerMask.NameToLayer("AreaCafe");

        if (layerCafe != -1)
        {
            piso.gameObject.layer = layerCafe;
        }

        // MUDA O NAVMESH MODIFIER PARA AREACAFE
        NavMeshModifier modifier = piso.GetComponent<NavMeshModifier>();

        if (modifier != null)
        {
            int areaCafe = NavMesh.GetAreaFromName("AreaCafe");

            if (areaCafe != -1)
            {
                modifier.overrideArea = true;
                modifier.area = areaCafe;
            }
        }

        // MUDA O MATERIAL VISUAL DO PISO COMPRADO
        Renderer renderer = piso.GetComponent<Renderer>();

        if (renderer != null && materialAreaComprada != null)
        {
            renderer.material = materialAreaComprada;
        }

        // LIBERA O GRID NO MODO EDIÇÃO
        if (AreaCafeManager.instancia != null)
        {
            AreaCafeManager.instancia.LiberarGridExtra(new Vector2Int(x, z));
        }

        Debug.Log("Piso liberado: " + piso.name + " | X=" + x + " Z=" + z);
    }

    private Transform EncontrarPisoNaPosicao(int x, int z)
    {
        if (containerPisos == null)
        {
            Debug.LogWarning("ContainerPisos não configurado.");
            return null;
        }

        foreach (Transform piso in containerPisos)
        {
            int pisoX = Mathf.RoundToInt(piso.position.x / tamanhoBloco);
            int pisoZ = Mathf.RoundToInt(piso.position.z / tamanhoBloco);

            if (pisoX == x && pisoZ == z)
            {
                return piso;
            }
        }

        return null;
    }
}