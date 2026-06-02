using UnityEngine;
using System;

public class SistemaNivelXP : MonoBehaviour
{
    public static SistemaNivelXP instancia;

    [Header("Nível")]
    public int nivelAtual = 1;
    public int xpAtual = 0;
    public int xpParaProximoNivel = 100;

    [Header("Configuração")]
    public float multiplicadorXP = 1.35f;

    public Action<int, int, int> AoXPAlterado;
    public Action<int> AoSubirNivel;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GanharXP(int quantidade)
    {
        if (quantidade <= 0) return;

        xpAtual += quantidade;

        while (xpAtual >= xpParaProximoNivel)
        {
            xpAtual -= xpParaProximoNivel;
            SubirNivel();
        }

        AoXPAlterado?.Invoke(nivelAtual, xpAtual, xpParaProximoNivel);
    }

    private void SubirNivel()
    {
        nivelAtual++;

        xpParaProximoNivel = Mathf.RoundToInt(xpParaProximoNivel * multiplicadorXP);

        Debug.Log("Subiu para o nível " + nivelAtual);

        AoSubirNivel?.Invoke(nivelAtual);
    }
}