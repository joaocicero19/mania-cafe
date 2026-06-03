using UnityEngine;

public class BalcaoController : MonoBehaviour
{
    [Header("Referências")]
    public Transform socketComidaPronta;

    private GameObject comidaAtual;
    private ComidaController comidaAtualController;

    public bool EstaVazio()
    {
        return comidaAtualController == null;
    }

    public bool TemMesmoPrato(string nomePrato)
    {
        if (comidaAtualController == null)
            return false;

        if (string.IsNullOrEmpty(nomePrato))
            return false;

        return comidaAtualController.GetNomePrato().Equals(nomePrato);
    }

    public bool TemMesmoPrato(ComidaController comida)
    {
        if (comida == null)
            return false;

        return TemMesmoPrato(comida.GetNomePrato());
    }

    public bool ReceberComidaPronta(ReceitaData receita)
    {
        if (receita == null)
            return false;

        if (socketComidaPronta == null)
        {
            Debug.LogWarning("SocketComidaPronta do balcão não definido.");
            return false;
        }

        if (receita.prefabComidaPronta == null)
        {
            Debug.LogWarning("A receita não tem Prefab Comida Pronta configurado.");
            return false;
        }

        if (comidaAtualController != null)
        {
            if (TemMesmoPrato(receita.nomeReceita))
            {
                int totalUnidades = comidaAtualController.GetUnidades() + receita.unidadesGeradas;
                comidaAtualController.DefinirUnidades(totalUnidades);
                return true;
            }

            return false;
        }

        comidaAtual = Instantiate(receita.prefabComidaPronta, socketComidaPronta.position, socketComidaPronta.rotation);
        comidaAtual.transform.SetParent(socketComidaPronta);
        comidaAtual.transform.localPosition = Vector3.zero;
        comidaAtual.transform.localRotation = Quaternion.identity;

        comidaAtualController = comidaAtual.GetComponent<ComidaController>();

        if (comidaAtualController == null)
        {
            Debug.LogWarning("O Prefab Comida Pronta não tem ComidaController.");
            Destroy(comidaAtual);
            comidaAtual = null;
            return false;
        }

        comidaAtualController.ConfigurarDadosDoPrato(receita.nomeReceita, receita.unidadesGeradas);
        comidaAtualController.MarcarComoNoBalcao();

        return true;
    }

    public bool TemComidaDisponivel()
    {
        if (comidaAtualController == null)
            return false;

        return comidaAtualController.GetUnidades() > 0;
    }

    public bool RetirarUmaUnidade()
    {
        if (comidaAtualController == null)
            return false;

        int unidadesAtuais = comidaAtualController.GetUnidades();

        if (unidadesAtuais <= 0)
            return false;

        unidadesAtuais--;
        comidaAtualController.DefinirUnidades(unidadesAtuais);

        if (unidadesAtuais <= 0)
        {
            Destroy(comidaAtual);
            comidaAtual = null;
            comidaAtualController = null;
        }

        return true;
    }
}