using TMPro;
using UnityEngine;

public class BalaoInfoCursor : MonoBehaviour
{
    public static BalaoInfoCursor Instance;

    [Header("Referências")]
    public RectTransform painelBalao;
    public TextMeshProUGUI textoBalao;

    [Header("Configuração")]
    public Vector2 offset = new Vector2(20f, -20f);

    private bool visivel = false;
    private float tempoFixo = 0f;

    private void Awake()
    {
        Instance = this;
        Esconder();
    }

    private void Update()
    {
        if (DeveBloquearBalao())
        {
            Esconder();
            return;
        }

        if (visivel)
        {
            painelBalao.position = Input.mousePosition + new Vector3(offset.x, offset.y, 0f);
        }

        if (tempoFixo > 0f)
        {
            tempoFixo -= Time.deltaTime;

            if (tempoFixo <= 0f)
            {
                Esconder();
            }
        }
    }

    public void Mostrar(string texto)
    {
        if (DeveBloquearBalao())
        {
            Esconder();
            return;
        }

        if (tempoFixo > 0f)
            return;

        tempoFixo = 0f;
        textoBalao.text = texto;
        painelBalao.gameObject.SetActive(true);
        visivel = true;
    }

    public void MostrarTemporario(string texto, float duracao)
    {
        if (DeveBloquearBalao())
        {
            Esconder();
            return;
        }

        textoBalao.text = texto;
        painelBalao.gameObject.SetActive(true);
        visivel = true;
        tempoFixo = duracao;
    }

    public void Esconder()
    {
        if (painelBalao != null)
        {
            painelBalao.gameObject.SetActive(false);
        }

        visivel = false;
        tempoFixo = 0f;
    }

    private bool DeveBloquearBalao()
    {
        if (MenuLoja.LojaAberta)
            return true;

        if (LivroReceitasController.Instance != null &&
            LivroReceitasController.Instance.painelLivro != null &&
            LivroReceitasController.Instance.painelLivro.activeSelf)
            return true;

        ModoEdicaoController modoEdicao = FindFirstObjectByType<ModoEdicaoController>();

        if (modoEdicao != null && modoEdicao.EstaEmModoEdicao())
            return true;

        return false;
    }
}