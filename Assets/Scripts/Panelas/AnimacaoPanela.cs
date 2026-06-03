using UnityEngine;

public class AnimacaoPanela : MonoBehaviour
{
    [Header("Rotação")]
    public float velocidadeRotacao = 4f;
    public float intensidadeRotacao = 3f;

    [Header("Movimento Vertical")]
    public float velocidadeSubida = 2f;
    public float intensidadeSubida = 0.02f;

    private Vector3 posicaoInicial;
    private Quaternion rotacaoInicial;

    private bool animando = true;

    private void Start()
    {
        posicaoInicial = transform.localPosition;
        rotacaoInicial = transform.localRotation;
    }

    private void Update()
    {
        if (!animando)
            return;

        float rotacaoZ = Mathf.Sin(Time.time * velocidadeRotacao) * intensidadeRotacao;
        Quaternion rotacaoAnimada = Quaternion.Euler(0f, 0f, rotacaoZ);

        transform.localRotation = rotacaoInicial * rotacaoAnimada;

        float subida = Mathf.Sin(Time.time * velocidadeSubida) * intensidadeSubida;
        transform.localPosition = posicaoInicial + new Vector3(0f, subida, 0f);
    }

    public void PararAnimacao()
    {
        animando = false;

        transform.localRotation = rotacaoInicial;
        transform.localPosition = posicaoInicial;
    }
}