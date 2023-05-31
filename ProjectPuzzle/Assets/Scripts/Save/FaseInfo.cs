[System.Serializable]
public class FaseInfo
{
    public bool concluida;
    public int ?pontosUtilizados;

    public FaseInfo(bool concluida, int ?pontosUtilizados)
    {
        this.concluida = concluida;
        this.pontosUtilizados = pontosUtilizados;
    }
}