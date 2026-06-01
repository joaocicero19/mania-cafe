using UnityEngine;

public class MenuLoja : MonoBehaviour
{
    [Header("UI")]
    public GameObject painelLoja;

    [Header("Sistema de Compra")]
    public Camera cameraPrincipal;
    public LayerMask camadaChao;

    [Header("Prefabs")]
    public ItemLoja cadeira;

    private GameObject previewObjeto;
    private GameObject prefabSelecionado;

    void Start()
    {
        painelLoja.SetActive(false);
    }

    void Update()
    {
        if (previewObjeto == null) return;

        AtualizarPreview();

        // Clique esquerdo = confirmar
        if (Input.GetMouseButtonDown(0))
        {
            ConfirmarColocacao();
        }

        // Clique direito = cancelar
        if (Input.GetMouseButtonDown(1))
        {
            CancelarColocacao();
        }
    }

    public void AbrirFecharLoja()
    {
        painelLoja.SetActive(!painelLoja.activeSelf);
    }

    // BOTÃO DA CADEIRA
    private ItemLoja itemAtual;
    public void ComprarCadeira()
    {
        painelLoja.SetActive(false);

        itemAtual = cadeira;

        IniciarColocacao(cadeira.prefab);
    }

    void IniciarColocacao(GameObject prefab)
    {
        prefabSelecionado = prefab;

        if (previewObjeto != null)
        {
            Destroy(previewObjeto);
        }

        previewObjeto = Instantiate(prefab);

        DesativarColisoes(previewObjeto);
    }

    void AtualizarPreview()
    {
        Ray ray = cameraPrincipal.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, camadaChao))
        {
            Vector3 posicao = hit.point;

            // SNAP GRID
            posicao.x = Mathf.Round(posicao.x);
            posicao.z = Mathf.Round(posicao.z);

            // OFFSET DA CADEIRA
            // OFFSET DO ITEM
            posicao += itemAtual.offsetPosicionamento;

            previewObjeto.transform.position = posicao;
        }
    }

    void ConfirmarColocacao()
    {
        Instantiate(
            prefabSelecionado,
            previewObjeto.transform.position,
            previewObjeto.transform.rotation
        );

        Destroy(previewObjeto);

        previewObjeto = null;
        prefabSelecionado = null;
    }

    void CancelarColocacao()
    {
        Destroy(previewObjeto);

        previewObjeto = null;
        prefabSelecionado = null;
    }

    void DesativarColisoes(GameObject obj)
    {
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();

        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }
}