using UnityEngine;
using TMPro;

public class ModoEdicaoController : MonoBehaviour
{
    [Header("Configuração")]
    public Camera cameraPrincipal;
    public LayerMask camadaChao;
    public LayerMask camadaObjetosEditaveis;

    [Header("UI")]
    public GameObject botaoGirar;

    [Header("Botão Editar")]
    public TextMeshProUGUI textoBotaoEditar;

    private bool modoEdicaoAtivo;
    private GameObject objetoSelecionado;
    private ObjetoEditavel objetoEditavel;

    private Collider[] collidersObjetoSelecionado;
    private Renderer[] renderersObjetoSelecionado;

    private Vector3 posicaoOriginal;
    private bool posicaoAtualValida = true;

    private MaterialPropertyBlock blocoCor;
    private bool estaVermelho = false;
    public bool EstaEmModoEdicao()
    {
        return modoEdicaoAtivo;
    }

    void Start()
    {
        botaoGirar.SetActive(false);
        blocoCor = new MaterialPropertyBlock();
    }

    void Update()
    {
        if (!modoEdicaoAtivo) return;

        if (UnityEngine.EventSystems.EventSystem.current != null &&
            UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            AtualizarBotaoGirar();
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cameraPrincipal.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, 100f, camadaObjetosEditaveis))
            {
                DeselecionarObjeto();
                return;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (objetoSelecionado == null)
            {
                TentarTrocarSelecao();
                AtualizarBotaoGirar();
                return;
            }
            else
            {
                ConfirmarMovimento();
                AtualizarBotaoGirar();
                return;
            }
        }

        if (objetoSelecionado != null)
        {
            AtualizarPreviewMovimento();
        }

        AtualizarBotaoGirar();
    }

    public void AlternarModoEdicao()
    {
        modoEdicaoAtivo = !modoEdicaoAtivo;

        if (modoEdicaoAtivo)
        {
            textoBotaoEditar.text = "Cancelar Edição";
        }
        else
        {
            textoBotaoEditar.text = "Modo Edição";
            DeselecionarObjeto();
            botaoGirar.SetActive(false);
        }
    }

    bool TentarTrocarSelecao()
    {
        Ray ray = cameraPrincipal.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, camadaObjetosEditaveis))
        {
            ObjetoEditavel novoObjetoEditavel = hit.collider.GetComponentInParent<ObjetoEditavel>();

            if (novoObjetoEditavel != null)
            {
                if (objetoSelecionado == novoObjetoEditavel.gameObject)
                    return true;

                SelecionarObjeto(novoObjetoEditavel);
                return true;
            }
        }

        return false;
    }

    void SelecionarObjeto(ObjetoEditavel novoObjetoEditavel)
    {
        LimparCorPreview();
        ReativarCollidersObjetoSelecionado();

        objetoEditavel = novoObjetoEditavel;
        objetoSelecionado = objetoEditavel.gameObject;

        posicaoOriginal = objetoSelecionado.transform.position;
        posicaoAtualValida = true;

        botaoGirar.SetActive(true);

        collidersObjetoSelecionado = objetoSelecionado.GetComponentsInChildren<Collider>();
        renderersObjetoSelecionado = objetoSelecionado.GetComponentsInChildren<Renderer>();

        foreach (Collider col in collidersObjetoSelecionado)
        {
            if (col != null)
                col.enabled = false;
        }

        estaVermelho = false;
    }

    void AtualizarPreviewMovimento()
    {
        Ray ray = cameraPrincipal.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, camadaChao))
            return;

        Vector3 posicao = hit.point;

        posicao.x = Mathf.Round(posicao.x);
        posicao.z = Mathf.Round(posicao.z);
        posicao.y = 0f;

        if (objetoEditavel != null)
            posicao += objetoEditavel.offsetGrid;

        objetoSelecionado.transform.position = posicao;

        posicaoAtualValida = VerificarPosicaoValida();

        if (posicaoAtualValida)
            LimparCorPreview();
        else
            PintarVermelhoPreview();
    }

    bool VerificarPosicaoValida()
    {
        if (objetoSelecionado == null || objetoEditavel == null)
            return true;

        Vector2Int gridSelecionado = objetoEditavel.PosicaoGrid();

        // 1. Verifica se está dentro da área do café
        if (AreaCafeManager.instancia != null)
        {
            if (!AreaCafeManager.instancia.EstaDentroDaArea(gridSelecionado))
            {
                Debug.Log("Posição inválida: fora da área do café.");
                return false;
            }
        }

        // 2. Verifica se outro objeto ocupa o mesmo bloco
        ObjetoEditavel[] todosObjetos = FindObjectsOfType<ObjetoEditavel>();

        foreach (ObjetoEditavel outro in todosObjetos)
        {
            if (outro == null)
                continue;

            if (outro == objetoEditavel)
                continue;

            if (outro.OcupaGrid(gridSelecionado))
            {
                Debug.Log("Posição inválida: bloco ocupado por " + outro.name);
                return false;
            }
        }

        return true;
    }

    void ConfirmarMovimento()
    {
        if (objetoSelecionado == null || objetoEditavel == null)
            return;

        posicaoAtualValida = VerificarPosicaoValida();

        if (posicaoAtualValida)
        {
            posicaoOriginal = objetoSelecionado.transform.position;

            LimparCorPreview();

            Debug.Log("Movimento confirmado.");

            DeselecionarObjeto();
        }
        else
        {
            objetoSelecionado.transform.position = posicaoOriginal;

            LimparCorPreview();

            Debug.Log("Movimento cancelado. Objeto voltou para posição original.");

            DeselecionarObjeto();
        }
    }

    void DeselecionarObjeto()
    {
        LimparCorPreview();
        ReativarCollidersObjetoSelecionado();

        objetoSelecionado = null;
        objetoEditavel = null;
        collidersObjetoSelecionado = null;
        renderersObjetoSelecionado = null;

        botaoGirar.SetActive(false);
        estaVermelho = false;
    }

    void ReativarCollidersObjetoSelecionado()
    {
        if (collidersObjetoSelecionado == null) return;

        foreach (Collider col in collidersObjetoSelecionado)
        {
            if (col != null)
                col.enabled = true;
        }
    }

    void PintarVermelhoPreview()
    {
        if (estaVermelho) return;
        if (renderersObjetoSelecionado == null) return;

        foreach (Renderer r in renderersObjetoSelecionado)
        {
            if (r == null) continue;

            blocoCor.Clear();
            blocoCor.SetColor("_Color", Color.red);
            r.SetPropertyBlock(blocoCor);
        }

        estaVermelho = true;
    }

    void LimparCorPreview()
    {
        if (renderersObjetoSelecionado == null) return;

        foreach (Renderer r in renderersObjetoSelecionado)
        {
            if (r == null) continue;

            r.SetPropertyBlock(null);
        }

        estaVermelho = false;
    }

    public void GirarObjetoSelecionado()
    {
        if (objetoSelecionado == null) return;

        objetoSelecionado.transform.Rotate(0f, 90f, 0f);

        posicaoAtualValida = VerificarPosicaoValida();

        if (posicaoAtualValida)
            LimparCorPreview();
        else
            PintarVermelhoPreview();
    }

    void AtualizarBotaoGirar()
    {
        if (objetoSelecionado == null)
        {
            botaoGirar.SetActive(false);
            return;
        }

        Vector3 posicaoMundo = objetoSelecionado.transform.position + new Vector3(0f, -0.3f, 0f);
        Vector3 posicaoTela = cameraPrincipal.WorldToScreenPoint(posicaoMundo);

        botaoGirar.transform.position = posicaoTela;
    }
}