using UnityEngine;

public enum CategoriaReceita
{
    Lanches,
    Massas,
    Carnes,
    Doces,
    Sopas,
    Saladas,
    Peixes,
}

[CreateAssetMenu(fileName = "NovaReceita", menuName = "Café Mania/Receita")]
public class ReceitaData : ScriptableObject
{
    [Header("Informações")]
    public string nomeReceita;
    public CategoriaReceita categoria;
    public Sprite icone;

    [Header("Economia")]
    public int valorParaProduzir = 10;
    public int valorPorUnidade = 5;

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