using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MenuLoja : MonoBehaviour
{
    public static bool LojaAberta = false;
    public static bool EstaPosicionandoItem = false;

    private Vector3 posicaoMouseAoClicar;
    private bool iniciouCliqueColocacao = false;
    public float limiteArrastoClique = 10f;

    [Header("UI")]
    public GameObject painelLoja;

    [Header("Sistema")]
    public Camera cameraPrincipal;
    public LayerMask camadaChao;

    [Header("Itens da Loja")]
    public List<ItemLoja> itensLoja = new List<ItemLoja>();

    private GameObject previewObjeto;
    private ItemLoja itemAtual;

    private bool colocandoItem = false;
    private bool posicaoValida = true;

    private Renderer[] renderersPreview;
    private MaterialPropertyBlock blocoCor;

    private void Start()
    {
        if (painelLoja != null)
            painelLoja.SetActive(false);

        blocoCor = new MaterialPropertyBlock();
    }

    private void Update()
    {
        if (!colocandoItem || previewObjeto == null)
            return;

        AtualizarPreview();

        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            posicaoMouseAoClicar = Input.mousePosition;
            iniciouCliqueColocacao = true;
        }

        if (Input.GetMouseButtonUp(0) && iniciouCliqueColocacao)
        {
            float distanciaArrasto = Vector3.Distance(posicaoMouseAoClicar, Input.mousePosition);

            if (distanciaArrasto <= limiteArrastoClique)
            {
                ConfirmarColocacao();
            }

            iniciouCliqueColocacao = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            CancelarColocacao();
        }
    }

    public void AbrirFecharLoja()
    {
        if (painelLoja != null)
        {
            bool novoEstado = !painelLoja.activeSelf;

            painelLoja.SetActive(novoEstado);

            LojaAberta = novoEstado;
        }
    }

    // BOTÕES DA LOJA
    public void SelecionarItemPorIndice(int indice)
    {
        if (indice < 0 || indice >= itensLoja.Count)
        {
            Debug.LogWarning("Índice inválido na loja.");
            return;
        }

        SelecionarItem(itensLoja[indice]);
    }

    public void SelecionarItem(ItemLoja item)
    {
        if (item == null || item.prefab == null)
        {
            Debug.LogWarning("Item inválido.");
            return;
        }

        itemAtual = item;

        colocandoItem = true;
        EstaPosicionandoItem = true;

        if (painelLoja != null)
        {
            painelLoja.SetActive(false);
            LojaAberta = false;
        }

        if (previewObjeto != null)
            Destroy(previewObjeto);

        previewObjeto = Instantiate(item.prefab);

        renderersPreview = previewObjeto.GetComponentsInChildren<Renderer>();

        DesativarColisoes(previewObjeto);

        AplicarObjetoEditavelAutomaticamente();
    }
    public void SelecionarItemNovo(ItemLojaData itemData)
    {
        if (itemData == null)
        {
            Debug.LogWarning("ItemLojaData inválido.");
            return;
        }

        // PERSONALIZAÇÃO (PISO/PAREDE)
        if (itemData.ehPersonalizacao)
        {
            SistemaPersonalizacao sistema =
                FindObjectOfType<SistemaPersonalizacao>();

            if (sistema != null)
            {
                sistema.SelecionarPiso(
                    itemData.materialPersonalizacao
                );
            }

            if (painelLoja != null)
            {
                painelLoja.SetActive(false);
                LojaAberta = false;
            }

            return;
        }

        // OBJETOS NORMAIS
        if (itemData.prefabObjeto == null)
        {
            Debug.LogWarning("Prefab do item é nulo.");
            return;
        }

        ItemLoja itemConvertido = new ItemLoja
        {
            nome = itemData.nomeItem,
            prefab = itemData.prefabObjeto,
            offsetPosicionamento = itemData.offsetPosicionamento,
            tamanhoGrid = itemData.tamanhoGrid
        };

        SelecionarItem(itemConvertido);
    }

    private void AtualizarPreview()
    {
        Ray ray = cameraPrincipal.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, camadaChao))
            return;

        Vector3 posicao = hit.point;

        posicao.x = Mathf.Round(posicao.x);
        posicao.z = Mathf.Round(posicao.z);
        posicao.y = 0f;

        posicao += itemAtual.offsetPosicionamento;

        previewObjeto.transform.position = posicao;

        posicaoValida = VerificarPosicaoValida();

        if (posicaoValida)
            LimparCorPreview();
        else
            PintarVermelhoPreview();
    }

    private bool VerificarPosicaoValida()
    {
        if (previewObjeto == null)
            return false;

        ObjetoEditavel editavel = previewObjeto.GetComponent<ObjetoEditavel>();

        if (editavel == null)
            return true;

        Vector2Int gridSelecionado = editavel.PosicaoGrid();

        // ÁREA DO CAFÉ
        if (AreaCafeManager.instancia != null)
        {
            if (!AreaCafeManager.instancia.EstaDentroDaArea(gridSelecionado))
            {
                return false;
            }
        }

        // OBJETOS EXISTENTES
        ObjetoEditavel[] objetos = FindObjectsOfType<ObjetoEditavel>();

        foreach (ObjetoEditavel outro in objetos)
        {
            if (outro == null)
                continue;

            if (outro.gameObject == previewObjeto)
                continue;

            if (outro.OcupaGrid(gridSelecionado))
            {
                return false;
            }
        }

        return true;
    }

    private void ConfirmarColocacao()
    {
        if (!posicaoValida)
        {
            Debug.Log("Posição inválida.");
            return;
        }

        GameObject novoObjeto = Instantiate(
            itemAtual.prefab,
            previewObjeto.transform.position,
            previewObjeto.transform.rotation
        );

        Collider[] colliders = novoObjeto.GetComponentsInChildren<Collider>();

        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }

        AtualizarPreview();
    }

    private void CancelarColocacao()
    {
        if (previewObjeto != null)
        {
            Destroy(previewObjeto);
        }

        previewObjeto = null;
        itemAtual = null;

        colocandoItem = false;
        EstaPosicionandoItem = false;
    }

    private void DesativarColisoes(GameObject obj)
    {
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();

        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }

    private void AplicarObjetoEditavelAutomaticamente()
    {
        ObjetoEditavel editavel = previewObjeto.GetComponent<ObjetoEditavel>();

        if (editavel == null)
        {
            editavel = previewObjeto.AddComponent<ObjetoEditavel>();
        }

        editavel.offsetGrid = itemAtual.offsetPosicionamento;
        editavel.tamanhoGrid = itemAtual.tamanhoGrid;
    }

    private void PintarVermelhoPreview()
    {
        if (renderersPreview == null)
            return;

        foreach (Renderer r in renderersPreview)
        {
            blocoCor.Clear();
            blocoCor.SetColor("_Color", Color.red);
            r.SetPropertyBlock(blocoCor);
        }
    }

    private void LimparCorPreview()
    {
        if (renderersPreview == null)
            return;

        foreach (Renderer r in renderersPreview)
        {
            r.SetPropertyBlock(null);
        }
    }
}