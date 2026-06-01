using UnityEngine;

public class PortaEntrada : MonoBehaviour
{
    public static PortaEntrada portaAtual;

    [Header("Pontos dos NPCs")]
    public Transform pontoExterno;
    public Transform pontoInterno;

    private void Awake()
    {
        portaAtual = this;
    }

    public Vector3 PosicaoExterna()
    {
        return pontoExterno.position;
    }

    public Vector3 PosicaoInterna()
    {
        return pontoInterno.position;
    }
}