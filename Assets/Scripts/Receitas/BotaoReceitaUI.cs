using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BotaoReceitaUI : MonoBehaviour
{
    [Header("UI")]
    public Image icone;
    public TMP_Text textoNome;
    public TMP_Text textoPreco;
    public TMP_Text textoTempo;
    public TMP_Text textoUN;
    public TMP_Text textoXP;

    private ReceitaData receita;
    private LivroReceitasController livro;

    public void Configurar(ReceitaData novaReceita, LivroReceitasController livroReceitas)
    {
        receita = novaReceita;
        livro = livroReceitas;

        textoNome.text = receita.nomeReceita;

        textoPreco.text = "$ " + receita.valorParaProduzir;

        textoTempo.text = receita.tempoPreparo + "s";

        textoUN.text = receita.unidadesGeradas + " Unidades";

        textoXP.text = receita.xpAoVender + " XP";

        if (receita.icone != null)
        {
            icone.sprite = receita.icone;
        }

        GetComponent<Button>().onClick.RemoveAllListeners();

        GetComponent<Button>().onClick.AddListener(SelecionarReceita);
    }

    void SelecionarReceita()
    {
        livro.SelecionarReceita(receita);
    }
}