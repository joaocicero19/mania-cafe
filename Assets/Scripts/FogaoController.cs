using UnityEngine;

public class FogaoController : MonoBehaviour
{
    [Header("Referências")]
    public Transform socketPanela;

    private BalcaoController[] balcoes;

    [Header("Estado")]
    private ReceitaData receitaAtual;
    private GameObject panelaAtual;

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

                if (panelaAtual != null)
                {
                    AnimacaoPanela animacao = panelaAtual.GetComponent<AnimacaoPanela>();

                    if (animacao != null)
                        animacao.PararAnimacao();

                    TampaPanelaController tampaController = panelaAtual.GetComponent<TampaPanelaController>();

                    if (tampaController != null)
                        tampaController.EsconderTampa();
                }

                Debug.Log("Comida pronta: " + GetNomeReceitaAtual());
            }
        }
    }

    private void OnMouseDown()
    {
        if (MenuLoja.LojaAberta)
            return;

        if (MenuLoja.EstaPosicionandoItem)
            return;

        if (InteracaoBloqueada())
            return;

        if (comidaPronta)
        {
            EnviarComidaParaBalcao();
            return;
        }

        if (!preparando && !comidaPronta && panelaAtual == null)
        {
            if (LivroReceitasController.Instance != null)
                LivroReceitasController.Instance.Abrir(this);
            else
                Debug.LogWarning("Instance do LivroReceitasController não encontrada.");
        }
    }

    public void IniciarPreparo(ReceitaData receita)
    {
        if (receita == null)
            return;

        if (preparando || comidaPronta || panelaAtual != null)
        {
            Debug.Log("Fogão já está ocupado.");
            return;
        }

        if (socketPanela == null)
        {
            Debug.LogWarning("SocketPanela não foi definido no FogaoController.");
            return;
        }

        if (receita.prefabPanelaPreparo == null)
        {
            Debug.LogWarning("A receita não tem Prefab Panela Preparo configurado.");
            return;
        }

        receitaAtual = receita;
        tempoRestante = receita.tempoPreparo;
        preparando = true;
        comidaPronta = false;

        panelaAtual = Instantiate(receita.prefabPanelaPreparo, socketPanela.position, socketPanela.rotation);
        panelaAtual.transform.SetParent(socketPanela);
        panelaAtual.transform.localPosition = receita.offsetPosicaoPanela;
        panelaAtual.transform.localRotation = Quaternion.Euler(receita.offsetRotacaoPanela);
        panelaAtual.transform.localScale = receita.escalaPanela;
        PanelaHoverInfo hoverInfo = panelaAtual.GetComponent<PanelaHoverInfo>();

        if (hoverInfo == null)
        {
            hoverInfo = panelaAtual.AddComponent<PanelaHoverInfo>();
        }

        hoverInfo.Configurar(this);

        TampaPanelaController tampaController = panelaAtual.GetComponent<TampaPanelaController>();

        if (tampaController != null)
        {
            tampaController.MostrarTampa();
        }

        Debug.Log("Preparando: " + receitaAtual.nomeReceita);
    }

    public void EnviarComidaParaBalcao()
    {
        if (!comidaPronta || receitaAtual == null)
            return;

        balcoes = FindObjectsByType<BalcaoController>(FindObjectsSortMode.None);

        if (balcoes == null || balcoes.Length == 0)
        {
            MostrarAvisoSemBalcao();
            return;
        }

        foreach (BalcaoController balcao in balcoes)
        {
            if (balcao != null && balcao.TemMesmoPrato(receitaAtual.nomeReceita))
            {
                if (balcao.ReceberComidaPronta(receitaAtual))
                {
                    GanharXPDaReceita();
                    LimparFogaoDepoisDeServir();
                    return;
                }
            }
        }

        foreach (BalcaoController balcao in balcoes)
        {
            if (balcao != null && balcao.EstaVazio())
            {
                if (balcao.ReceberComidaPronta(receitaAtual))
                {
                    GanharXPDaReceita();
                    LimparFogaoDepoisDeServir();
                    return;
                }
            }
        }

        MostrarAvisoSemBalcao();
    }

    private void LimparFogaoDepoisDeServir()
    {
        if (panelaAtual != null)
            Destroy(panelaAtual);

        panelaAtual = null;
        receitaAtual = null;
        comidaPronta = false;
        preparando = false;
        tempoRestante = 0f;
    }

    private void GanharXPDaReceita()
    {
        if (SistemaNivelXP.instancia != null && receitaAtual != null)
            SistemaNivelXP.instancia.GanharXP(receitaAtual.xpAoVender);
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
            BalaoInfoCursor.Instance.MostrarTemporario("Não há Balcões Disponíveis", 2f);

        Debug.LogWarning("Não há balcões disponíveis.");
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
        if (MenuLoja.LojaAberta)
            return;

        if (MenuLoja.EstaPosicionandoItem)
            return;

        if (InteracaoBloqueada())
            return;

        if (preparando)
        {
            if (BalaoInfoCursor.Instance != null)
                BalaoInfoCursor.Instance.Mostrar(GetNomeReceitaAtual() + " - " + Mathf.CeilToInt(tempoRestante) + "s");

            return;
        }

        if (comidaPronta)
        {
            if (BalaoInfoCursor.Instance != null)
                BalaoInfoCursor.Instance.Mostrar("Pronto para servir");

            return;
        }

        if (BalaoInfoCursor.Instance != null)
            BalaoInfoCursor.Instance.Mostrar("Abrir Livro de Receitas");
    }

    private void OnMouseOver()
    {
        if (preparando)
        {
            if (BalaoInfoCursor.Instance != null)
                BalaoInfoCursor.Instance.Mostrar(GetNomeReceitaAtual() + " - " + Mathf.CeilToInt(tempoRestante) + "s");
        }
    }

    private void OnMouseExit()
    {
        if (BalaoInfoCursor.Instance != null)
            BalaoInfoCursor.Instance.Esconder();
    }
}