using UnityEngine;

[CreateAssetMenu(fileName = "NovaReceita", menuName = "Café Mania/Receita")]
public class ReceitaData : ScriptableObject
{
    [Header("Informações")]
    public string nomeReceita;

    [Header("Tempo")]
    public float tempoPreparo = 10f;

    [Header("Unidades")]
    public int unidadesGeradas = 5;

    [Header("XP")]
    public int xpAoVender = 10;

    [Header("Visual do Preparo")]
    public GameObject prefabPanelaPreparo;
    public Vector3 offsetPosicaoPanela;
    public Vector3 offsetRotacaoPanela;
    public Vector3 escalaPanela = Vector3.one;

    [Header("Visual do Balcão")]
    public GameObject prefabComidaPronta;
}