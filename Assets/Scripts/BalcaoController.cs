using UnityEngine;

public class BalcaoController : MonoBehaviour
{
    public Transform socketComida;

    private GameObject comidaAtual;
    private ComidaController comidaAtualController;

    public bool EstaVazio()
    {
        return comidaAtualController == null;
    }

    public bool TemMesmoPrato(ComidaController comida)
    {
        if (comidaAtualController == null)
            return false;

        if (comida == null)
            return false;

        string pratoAtual = comidaAtualController.GetNomePrato();
        string pratoNovo = comida.GetNomePrato();

        Debug.Log("Comparando pratos:");
        Debug.Log("Atual: " + pratoAtual);
        Debug.Log("Novo: " + pratoNovo);

        return pratoAtual.Equals(pratoNovo);
    }

    public bool ReceberObjetoComida(GameObject comida)
    {
        if (comida == null)
            return false;

        if (socketComida == null)
        {
            Debug.LogWarning("Socket_Comida do balcão não definido.");
            return false;
        }

        ComidaController novaComidaController = comida.GetComponent<ComidaController>();

        if (novaComidaController == null)
        {
            Debug.LogWarning("A comida recebida não tem ComidaController.");
            return false;
        }

        if (comidaAtualController != null)
        {
            if (TemMesmoPrato(novaComidaController))
            {
                int totalUnidades =
                    comidaAtualController.GetUnidades() + novaComidaController.GetUnidades();

                comidaAtualController.DefinirUnidades(totalUnidades);

                Destroy(comida);

                Debug.Log("Unidades acumuladas no balcão: " + totalUnidades);

                return true;
            }

            return false;
        }


        comidaAtual = comida;
        comidaAtualController = novaComidaController;

        comida.transform.SetParent(socketComida);
        comida.transform.localPosition = Vector3.zero;
        comida.transform.localRotation = Quaternion.identity;

        comidaAtualController.MarcarComoNoBalcao();

        Debug.Log("Nova comida colocada no balcão: " + comidaAtualController.GetNomePrato());

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

        Debug.Log("NPC pegou 1 unidade. Restam: " + unidadesAtuais);

        // acabou a comida
        if (unidadesAtuais <= 0)
        {
            Destroy(comidaAtual);

            comidaAtual = null;
            comidaAtualController = null;
        }

        return true;
    }
}