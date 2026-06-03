using UnityEngine;

public class TampaPanelaController : MonoBehaviour
{
    [Header("Referência")]
    public GameObject tampa;

    public void MostrarTampa()
    {
        Debug.Log("Mostrar tampa chamado");

        if (tampa != null)
        {
            tampa.SetActive(true);
        }
    }

    public void EsconderTampa()
    {
        if (tampa != null)
            tampa.SetActive(false);
    }
}