using var arquivo = new FileStream("musicas.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(arquivo);


var musicas = ObterMusicas(stream)
    .Take(20);

ExibirMusicasEmTabela(musicas);

void interning()
{
    var artista1 = "Coldplay"; // interning - string literal
    var artista2 = "Coldplay";
    var artista3 = new string("Coldplay"); // não faz interning
    var artista4 = "COLDPLAY";
    var artista5 = string.Intern(artista1.ToUpper()); // HEAP

    Console.WriteLine(artista1 == artista2); // True
    Console.WriteLine(ReferenceEquals(artista1, artista3)); //True - pool de strings
    Console.WriteLine(ReferenceEquals(artista1, artista4));
    Console.WriteLine(ReferenceEquals(artista4, artista5));

}

void ComparandoString(StreamReader stream)
{
    var musica = ObterMusicas(stream)
    .Where(musica => musica.Artista.Equals("COLDPLAY", StringComparison.OrdinalIgnoreCase))
    //.Where(m => m.Artista.ToUpper() == "COLDPLAY")
    .Take(20);

    // métodos que utilizam StringComparison
    "Coldplay".Equals("coldplay", StringComparison.OrdinalIgnoreCase);
    "Coldplay".StartsWith("coldplay", StringComparison.OrdinalIgnoreCase);
    "Coldplay".EndsWith("coldplay", StringComparison.OrdinalIgnoreCase);
    "Coldplay".IndexOf("coldplay", StringComparison.OrdinalIgnoreCase);
    "Coldplay".Contains("OLD", StringComparison.OrdinalIgnoreCase);
    "Coldplay".Replace("cold", "warm", StringComparison.OrdinalIgnoreCase);

    ExibirMusicasEmTabela(musica);
}

void AlterandoOTitulo(StreamReader stream)
{
    var musica = ObterMusicas(stream)
    .Where(m => m.Titulo.StartsWith('T'))
    .FirstOrDefault();


    if (musica is not null)
    {
        //Console.WriteLine("Título da música: " + musica.Titulo);
        Console.WriteLine($"Título da música: {musica.Titulo}");

        musica.Titulo = musica.Titulo.Replace("The ", "");
        //musica.Titulo = musica.Titulo.ToUpper();

        Console.WriteLine($"Título da música: {musica.Titulo}");
    }
}

void ValidandoSenha()
{
    //char[] letras;

    //var titulo = "Músicas do arquivo";
    //foreach(var letra in titulo) Console.WriteLine(letra);

    var senha = "Daniel123@";
    /*
        Senha será forte se:
        0. possui pelo menos 8 caracteres
        1. possui alguma letra maiúscula
        2. possui alguma letra minúscula
        3. possui algum número
        4. possui algum símbolo
    */
    var totalCaracteres = senha.Length;
    var totalLetrasMaiusculas = senha.Count(c => char.IsUpper(c));
    var totalLetrasMinusculas = senha.Count(c => char.IsLower(c));
    var totalNumero = senha.Count(c => char.IsDigit(c));
    var totalSimbolos = senha.Count(c => !char.IsLetterOrDigit(c));

    if (totalCaracteres < 8 ||
        totalLetrasMaiusculas == 0 ||
        totalLetrasMinusculas == 0 ||
        totalNumero == 0 ||
        totalSimbolos == 0)
    {
        Console.WriteLine("A senha digitada é fraca!");
    }
    else
    {
        Console.WriteLine("A senha digitada é forte");
    }
}


void ExibirMusicas(IEnumerable<Musica> musicas)
{
    var titulo = $"\nExibindo as músicas"; // string literal
                                           //var titulo = new string($"\nExibindo as músicas");

    Console.WriteLine(titulo);
    foreach (var musica in musicas)
    {
        var linha = $"\t - {musica.Titulo} ({musica.Artista}) - {musica.Duracao}s [{musica.Lancamento.ToString("dd/MM/yyyy")}]";
        Console.WriteLine(linha);
    }
}

void ExibirMusicasEmTabela(IEnumerable<Musica> musicas)
{
    var titulo = $"\nExibindo as músicas"; // string literal
    Console.WriteLine(titulo);

    var colunaTitulo = "Título".PadRight(40);
    var colunaArtista = "Artista".PadRight(30);
    var colunaDuracao = "Duração".PadRight(10);
    var colunaLancamento = "Lançada Em".PadRight(15);
    Console.WriteLine($"{colunaTitulo}{colunaArtista}{colunaDuracao}{colunaLancamento}");
    var borda = "".PadRight(92, '=');
    Console.WriteLine(borda);

    foreach (var musica in musicas)
    {
        var duracao = string.Format("{0, -10:F3}", musica.Duracao/60.0);
        var linha = $"{musica.Titulo, -40}{musica.Artista, -30}{duracao}{musica.Lancamento, -15:dd/MM/yyyy}";
        Console.WriteLine(linha);
    }
}

IEnumerable<Musica> ObterMusicas(StreamReader stream)
{
    var linha = stream.ReadLine();
    while (linha is not null)
    {
        var partes = linha.Split(';');
        if(partes.Length == 5)
        {
            var musica = new Musica()
            {
                Titulo = string.IsNullOrWhiteSpace(partes[0]) ? "N/A" : partes[0],
                Artista = string.IsNullOrWhiteSpace(partes[1]) ? "N/A" : partes[1],
                Duracao = int.TryParse(partes[2], out int duracao) ? duracao : 350,
                Generos = partes[3].Split(",", StringSplitOptions.TrimEntries),
                Lancamento = DateTime.TryParse(partes[4], out var data) ? data : DateTime.Today
            };
            yield return musica;
        }
        linha = stream.ReadLine();
    }
}

class Musica
{
    public string Titulo { get; set; }
    public string Artista { get; set; }
    public int Duracao { get; set; }
    public IEnumerable<string> Generos { get; set; }
    public DateTime Lancamento { get; set; }

    public override string ToString()
    {
        return $"{Titulo,-40} ({Artista,-30}) - {Duracao}s [{Lancamento}]";
    }
}