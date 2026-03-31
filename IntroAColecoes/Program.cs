using System.Collections;

var diasDaSemana = new string[] { "Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado" };

var carrinho = new List<Produto>()
{
    new("Leite", 7.89),
    new("Manteiga",3.45)
    //"Domingo"
};

// diasDaSemana[0];
//carrinho[0];




PercorrendoComForEach();

void PercorrendoComFor()
{
    for (int i = 0; i < carrinho.Count; i++)
    {
        var produto = carrinho[i];
        Console.WriteLine($"Produto: {produto}");
    }
}

void PercorrendoComForEach()
{
    foreach (var produto in carrinho)
    {
        Console.WriteLine($"Produto: {produto.Nome}");
    }
}

class Produto(string nome, double preco)
{
    public string Nome { get; } = nome;
    public double Preco { get; } = preco;
}