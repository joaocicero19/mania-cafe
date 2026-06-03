using UnityEngine;

[CreateAssetMenu(fileName = "NovoItemLoja", menuName = "Mania Cafe/Loja/Item da Loja")]
public class ItemLojaData : ScriptableObject
{
    [Header("Informações")]
    public string nomeItem;
    public string descricao;
    public int preco;

    [Header("Categoria")]
    public CategoriaLoja categoria;

    [Header("Visual")]
    public Sprite icone;

    [Header("Prefab")]
    public GameObject prefabObjeto;

    [Header("Personalização")]
    public Material materialPersonalizacao;
    public bool ehPersonalizacao;

    [Header("Posicionamento")]
    public Vector3 offsetPosicionamento;

    [Header("Tamanho Grid")]
    public Vector2Int tamanhoGrid = Vector2Int.one;

    [Header("Configurações")]
    public bool usaPreview = true;
    public bool precisaAreaCafe = true;
}
public enum TipoPersonalizacao
{
    Nenhum,
    Piso,
    Parede
}