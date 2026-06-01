using UnityEngine;

public class FogaoController : MonoBehaviour
{
    [Header("Referências")]
    public Transform socketComida;

    private BalcaoController[] balcoes;

    [Header("Estado")]
    private ReceitaData receitaAtual;
    private GameObject comidaAtual;

    private bool preparando = false;
    private bool comidaPronta = false;
    private float tempoRestante = 0f;
    private ModoEdicaoController modoEdicaoController;
    private void Start()
    {
        balcoes = FindObjectsByType<BalcaoController>(FindObjectsSortMode.None);
        modoEdicaoController = FindFirstObjectByType<ModoEdicaoController>();
    }

    private void Update()
    {
        if (preparando)
        {
            tempoRestante -= Time.deltaTime;

            if (tempoRestante <= 0f)
            {
                tempoRestante = 0f;
                preparando = false;
                comidaPronta = true;

                Debug.Log("Comida pronta: " + GetNomeReceitaAtual());
            }
        }
    }

    private void OnMouseDown()
    {
        if (InteracaoBloqueada())
            return;

        if (!preparando && !comidaPronta && comidaAtual == null)
        {
            if (LivroReceitasController.Instance != null)
            {
                LivroReceitasController.Instance.Abrir(this);
            }
            else
            {
                Debug.LogWarning("Instance do LivroReceitasController não encontrada.");
            }
        }
    }

    public void IniciarPreparo(ReceitaData receita)
    {
        if (receita == null)
        {
            Debug.LogWarning("Receita está vazia.");
            return;
        }

        if (preparando || comidaPronta || comidaAtual != null)
        {
            Debug.Log("Fogão já está ocupado.");
            return;
        }

        receitaAtual = receita;
        tempoRestante = receita.tempoPreparo;
        preparando = true;
        comidaPronta = false;

        comidaAtual = Instantiate(receita.prefabComida, socketComida.position, socketComida.rotation);
        comidaAtual.transform.SetParent(socketComida);
        comidaAtual.transform.localPosition = Vector3.zero;
        comidaAtual.transform.localRotation = Quaternion.identity;

        ComidaController comidaController = comidaAtual.GetComponent<ComidaController>();

        if (comidaController != null)
        {
            comidaController.Configurar(this);
            comidaController.ConfigurarDadosDoPrato(receitaAtual.nomeReceita, receitaAtual.unidadesGeradas);
        }
        else
        {
            Debug.LogWarning("O prefab da comida não tem ComidaController.");
        }

        Debug.Log("Preparando: " + receitaAtual.nomeReceita);
    }
    private void LimparFogaoDepoisDeServir()
    {
        comidaAtual = null;
        receitaAtual = null;
        comidaPronta = false;
        preparando = false;
        tempoRestante = 0f;
    }
    private bool InteracaoBloqueada()
    {
        if (modoEdicaoController != null && modoEdicaoController.EstaEmModoEdicao())
            return true;

        if (LivroReceitasController.Instance != null &&
            LivroReceitasController.Instance.painelLivro.activeSelf)
            return true;

        MenuLoja menuLoja = FindFirstObjectByType<MenuLoja>();

        if (menuLoja != null && menuLoja.painelLoja.activeSelf)
            return true;

        return false;
    }

    private void MostrarAvisoSemBalcao()
    {
        if (BalaoInfoCursor.Instance != null)
        {
            BalaoInfoCursor.Instance.MostrarTemporario("Não há Balcões Disponíveis", 2f);
        }

        Debug.LogWarning("Não há balcões disponíveis.");
    }

    public void EnviarComidaParaBalcao()
    {
        if (!comidaPronta || comidaAtual == null)
            return;

        if (balcoes == null || balcoes.Length == 0)
        {
            MostrarAvisoSemBalcao();
            return;
        }

        ComidaController comidaController = comidaAtual.GetComponent<ComidaController>();

        if (comidaController == null)
        {
            Debug.LogWarning("Comida atual não tem ComidaController.");
            return;
        }

        // 1. Primeiro tenta acumular em balcão que já tem o mesmo prato
        foreach (BalcaoController balcao in balcoes)
        {
            if (balcao != null && balcao.TemMesmoPrato(comidaController))
            {
                bool recebeu = balcao.ReceberObjetoComida(comidaAtual);

                if (recebeu)
                {
                    LimparFogaoDepoisDeServir();
                    return;
                }
            }
        }

        // 2. Depois tenta colocar em um balcão vazio
        foreach (BalcaoController balcao in balcoes)
        {
            if (balcao != null && balcao.EstaVazio())
            {
                bool recebeu = balcao.ReceberObjetoComida(comidaAtual);

                if (recebeu)
                {
                    LimparFogaoDepoisDeServir();
                    return;
                }
            }
        }

        // 3. Se não achou lugar
        MostrarAvisoSemBalcao();
    }

    public bool EstaPreparando()
    {
        return preparando;
    }

    public bool ComidaEstaPronta()
    {
        return comidaPronta;
    }

    public float GetTempoRestante()
    {
        return tempoRestante;
    }

    public string GetNomeReceitaAtual()
    {
        if (receitaAtual == null)
            return "Prato";

        return receitaAtual.nomeReceita;
    }
    private void OnMouseEnter()
    {
        if (InteracaoBloqueada())
            return;

        if (preparando || comidaPronta)
            return;

        if (BalaoInfoCursor.Instance != null)
        {
            BalaoInfoCursor.Instance.Mostrar("Abrir Livro de Receitas");
        }
    }
    

    private void OnMouseExit()
    {

        if (BalaoInfoCursor.Instance != null)
        {
            BalaoInfoCursor.Instance.Esconder();
        }
    }
}