using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCEntradaTeste : MonoBehaviour
{
    private NavMeshAgent agente;

    [Header("Configuração")]
    public float velocidade = 1.2f;
    public float distanciaChegada = 0.2f;
    public float tempoComendo = 15f;

    private int areaRua;
    private int areaCafe;

    private BalcaoController balcaoEscolhido;
    private CadeiraCliente cadeiraEscolhida;

    private void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        agente.speed = velocidade;

        areaRua = NavMesh.GetAreaFromName("AreaRua");
        areaCafe = NavMesh.GetAreaFromName("AreaCafe");

        agente.areaMask = 1 << areaRua;

        if (PortaEntrada.portaAtual == null)
        {
            Debug.LogError("Nenhuma PortaEntrada encontrada.");
            return;
        }

        StartCoroutine(RotinaNPC());
    }

    IEnumerator RotinaNPC()
    {
        Vector3 pontoExterno = PortaEntrada.portaAtual.PosicaoExterna();
        Vector3 pontoInterno = PortaEntrada.portaAtual.PosicaoInterna();

        // ENTRAR
        agente.SetDestination(pontoExterno);
        yield return EsperarChegar();

        agente.areaMask = (1 << areaRua) | (1 << areaCafe);

        agente.SetDestination(pontoInterno);
        yield return EsperarChegar();

        // ESCOLHER BALCÃO
        balcaoEscolhido = EncontrarBalcaoDisponivel();

        if (balcaoEscolhido == null)
        {
            Debug.Log("NPC não encontrou comida.");

            yield return new WaitForSeconds(5f);

            yield return SairDoCafe();

            yield break;
        }

        // ESCOLHER CADEIRA
        cadeiraEscolhida = EncontrarCadeiraLivre();

        if (cadeiraEscolhida == null)
        {
            Debug.Log("Nenhuma cadeira livre.");

            yield return SairDoCafe();

            yield break;
        }

        cadeiraEscolhida.Reservar();

        // IR ATÉ BALCÃO
        agente.SetDestination(balcaoEscolhido.transform.position);
        yield return EsperarChegar();

        bool pegouComida = balcaoEscolhido.RetirarUmaUnidade();

        if (!pegouComida)
        {
            cadeiraEscolhida.Liberar();

            yield return SairDoCafe();

            yield break;
        }

        // IR ATÉ CADEIRA
        agente.SetDestination(cadeiraEscolhida.transform.position);
        yield return EsperarChegar();

        Debug.Log("NPC começou a comer.");

        yield return new WaitForSeconds(tempoComendo);

        Debug.Log("NPC terminou de comer.");

        cadeiraEscolhida.Liberar();

        // SAIR
        yield return SairDoCafe();
    }

    IEnumerator SairDoCafe()
    {
        Vector3 pontoInterno = PortaEntrada.portaAtual.PosicaoInterna();
        Vector3 pontoExterno = PortaEntrada.portaAtual.PosicaoExterna();

        agente.SetDestination(pontoInterno);
        yield return EsperarChegar();

        agente.areaMask = 1 << areaRua;

        agente.SetDestination(pontoExterno);
        yield return EsperarChegar();

        Vector3 destinoRua = pontoExterno + (PortaEntrada.portaAtual.transform.forward * -20f);

        agente.SetDestination(destinoRua);

        while (agente.pathPending || agente.remainingDistance > distanciaChegada)
        {
            yield return null;
        }

        Destroy(gameObject);
    }

    IEnumerator EsperarChegar()
    {
        while (agente.pathPending || agente.remainingDistance > distanciaChegada)
        {
            yield return null;
        }
    }

    BalcaoController EncontrarBalcaoDisponivel()
    {
        BalcaoController[] balcoes = FindObjectsByType<BalcaoController>(FindObjectsSortMode.None);

        foreach (BalcaoController balcao in balcoes)
        {
            if (balcao.TemComidaDisponivel())
                return balcao;
        }

        return null;
    }

    CadeiraCliente EncontrarCadeiraLivre()
    {
        CadeiraCliente[] cadeiras = FindObjectsByType<CadeiraCliente>(FindObjectsSortMode.None);

        foreach (CadeiraCliente cadeira in cadeiras)
        {
            if (cadeira.EstaLivre())
                return cadeira;
        }

        return null;
    }
}