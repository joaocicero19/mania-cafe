using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SistemaPersonalizacao : MonoBehaviour
{
    public static SistemaPersonalizacao instancia;

    public Material materialSelecionado;
    public string tipoSelecionado;

    public Image imagemPreview;
    public GameObject painelLoja;

    private void Awake()
    {
        instancia = this;
    }

    private void Start()
    {
        if (imagemPreview != null)
            imagemPreview.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (imagemPreview == null)
            return;

        if (imagemPreview.gameObject.activeSelf)
        {
            imagemPreview.transform.position =
                Mouse.current.position.ReadValue();
        }
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            CancelarSelecao();
        }
    }

    public void SelecionarPiso(Material material)
    {
        materialSelecionado = material;
        tipoSelecionado = "Piso";

        MenuLoja.EstaPosicionandoItem = true;

        Texture2D textura = material.mainTexture as Texture2D;

        if (textura != null)
        {
            Sprite spritePreview = Sprite.Create(
                textura,
                new Rect(0, 0, textura.width, textura.height),
                new Vector2(0.5f, 0.5f)
            );

            imagemPreview.sprite = spritePreview;
            imagemPreview.color = Color.white;
        }

        imagemPreview.gameObject.SetActive(true);
        imagemPreview.rectTransform.rotation =
        Quaternion.Euler(0, 0, 45f);

        painelLoja.SetActive(false);

        MenuLoja.LojaAberta = false;

        Debug.Log("Piso selecionado");
    }

    public void CancelarSelecao()
    {
        materialSelecionado = null;
        tipoSelecionado = "";

        MenuLoja.EstaPosicionandoItem = false;

        imagemPreview.gameObject.SetActive(false);
    }
}