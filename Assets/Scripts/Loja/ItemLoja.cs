using UnityEngine;

[System.Serializable]
public class ItemLoja
{
    [Header("Informações")]
    public string nome;

    [Header("Prefab")]
    public GameObject prefab;

    [Header("Posicionamento")]
    public Vector3 offsetPosicionamento;

    [Header("Tamanho no Grid")]
    public Vector2Int tamanhoGrid = Vector2Int.one;

    [Header("Tipo de colocação")]
    public bool ehParede;

    [Header("Personalização")]
    public Material materialPersonalizacao;
}