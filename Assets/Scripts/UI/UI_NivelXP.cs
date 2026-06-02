using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_NivelXP : MonoBehaviour
{
    [Header("Textos")]
    public TextMeshProUGUI textoNivel;
    public TextMeshProUGUI textoXP;

    [Header("Barra")]
    public Slider barraXP;

    private void Start()
    {
        if (SistemaNivelXP.instancia != null)
        {
            SistemaNivelXP.instancia.AoXPAlterado += AtualizarUI;
            SistemaNivelXP.instancia.AoSubirNivel += MostrarSubiuNivel;

            AtualizarUI(
                SistemaNivelXP.instancia.nivelAtual,
                SistemaNivelXP.instancia.xpAtual,
                SistemaNivelXP.instancia.xpParaProximoNivel
            );
        }
    }

    private void OnDestroy()
    {
        if (SistemaNivelXP.instancia != null)
        {
            SistemaNivelXP.instancia.AoXPAlterado -= AtualizarUI;
            SistemaNivelXP.instancia.AoSubirNivel -= MostrarSubiuNivel;
        }
    }

    private void AtualizarUI(int nivel, int xpAtual, int xpParaProximo)
    {
        if (textoNivel != null)
            textoNivel.text = "Nível " + nivel;

        if (textoXP != null)
            textoXP.text = xpAtual + " / " + xpParaProximo + " XP";

        if (barraXP != null)
        {
            barraXP.maxValue = xpParaProximo;
            barraXP.value = xpAtual;
        }
    }

    private void MostrarSubiuNivel(int novoNivel)
    {
        Debug.Log("Parabéns! Você chegou ao nível " + novoNivel);
    }
}