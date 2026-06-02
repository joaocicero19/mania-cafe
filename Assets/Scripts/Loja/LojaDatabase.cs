using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LojaDatabase", menuName = "Mania Cafe/Loja/Database")]
public class LojaDatabase : ScriptableObject
{
    public List<ItemLojaData> itens = new List<ItemLojaData>();
}