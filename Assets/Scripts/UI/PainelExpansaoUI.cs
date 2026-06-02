using UnityEngine;

public class PainelExpansaoUI : MonoBehaviour
{
    [Header("Painel")]
    public GameObject painelExpansao;

    public void AbrirPainel()
    {
        painelExpansao.SetActive(true);

        Time.timeScale = 0f;
    }

    public void FecharPainel()
    {
        painelExpansao.SetActive(false);

        Time.timeScale = 1f;
    }
}