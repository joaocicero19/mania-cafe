using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BotaoItemLoja : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text textoNome;
    public Image icone;

    private ItemLojaData item;

    public void Configurar(ItemLojaData novoItem)
    {
        item = novoItem;

        textoNome.text = item.nomeItem;

        if (icone != null && item.icone != null)
        {
            icone.sprite = item.icone;
        }
    }

    public void AoClicar()
    {
        if (item == null)
        {
            Debug.LogWarning("Nenhum item configurado.");
            return;
        }

        MenuLoja menuLoja = FindObjectOfType<MenuLoja>();

        if (menuLoja == null)
        {
            Debug.LogError("MenuLoja não encontrado na cena.");
            return;
        }

        menuLoja.SelecionarItemNovo(item);
    }
}