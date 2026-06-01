using UnityEngine;

public class CadeiraCliente : MonoBehaviour
{
    private bool ocupada = false;

    public bool EstaLivre()
    {
        return !ocupada;
    }

    public void Reservar()
    {
        ocupada = true;
    }

    public void Liberar()
    {
        ocupada = false;
    }
}