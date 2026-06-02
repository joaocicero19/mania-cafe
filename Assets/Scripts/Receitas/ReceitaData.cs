using UnityEngine;

[CreateAssetMenu(fileName = "NovaReceita", menuName = "Café Mania/Receita")]
public class ReceitaData : ScriptableObject
{
    public string nomeReceita;

    [Header("Tempo")]
    public float tempoPreparo = 10f;

    [Header("Unidades")]
    public int unidadesGeradas = 5;

    [Header("Visual temporário")]
    public GameObject prefabComida;

    [Header("Recompensas")]
    public int xpAoVender = 10;
}