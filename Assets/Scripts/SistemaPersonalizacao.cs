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
    }

    public void SelecionarPiso(Material material)
    {
        materialSelecionado = material;
        tipoSelecionado = "Piso";

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

        painelLoja.SetActive(false);

        Debug.Log("Piso selecionado");
    }

    public void CancelarSelecao()
    {
        materialSelecionado = null;
        tipoSelecionado = "";

        imagemPreview.gameObject.SetActive(false);
    }
}