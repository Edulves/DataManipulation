using System.Text.RegularExpressions;

using var arquivo = new FileStream("musicas.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(arquivo);


/*
    [x] encontrando artistas com caracteres especiais
    [x] encontrando títulos com duas palavras
    [x] encontrando títulos que começão e terminam com a mesma palavra
    [x] encontrando títulos com letras repetidas
    [ ] encontrando títulos com números romanos
 */

TitulosComNumerosRomanos();

void TitulosComNumerosRomanos()
{
    var regex = new Regex(@"\b[IVXLCDM]+\b");
    var musicas = ObterMusicas(stream)
        .Where(m => regex.IsMatch(m.Titulo))
        .Take(20);

    ExibirMusicasEmTabela(musicas);
}

void musicasComLetrasRepetidas()
{
    var regex = new Regex(@"\w*(\w)\1{1,}");
    var musicas = ObterMusicas(stream)
        .Where(m => regex.IsMatch(m.Titulo))
        .Take(20);

    ExibirMusicasEmTabela(musicas);
}

void musicasQueComecaoETerminamComAMesmaPalavra()
{
    var regex = new Regex(@"^(\w+).*\1$");
    var musicas = ObterMusicas(stream)
        .Where(m => regex.IsMatch(m.Titulo))
        .Take(20);

    ExibirMusicasEmTabela(musicas);
}

void MusicasComDuasPalavras()
{
    var regex = new Regex(@"^\w+\s+\w+$");
    var musicas = ObterMusicas(stream)
        .Where(m => regex.IsMatch(m.Titulo))
        .Take(20);

    ExibirMusicasEmTabela(musicas);
}

void ArtistasComCaracteresEspeciais()
{
    var regex = new Regex(@"[^a-zA-Z0-9 ]");

    var artistas = ObterMusicas(stream)
        .Where(m => regex.IsMatch(m.Artista))
        .Select(m => m.Artista)
        .Distinct()
        .OrderBy(a => a);

    foreach (var artista in artistas)
        Console.WriteLine(artista);
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
        var duracao = string.Format("{0, -10:F3}", musica.Duracao / 60.0);
        var linha = $"{musica.Titulo,-40}{musica.Artista,-30}{duracao}{musica.Lancamento,-15:dd/MM/yyyy}";
        Console.WriteLine(linha);
    }
}

IEnumerable<Musica> ObterMusicas(StreamReader stream)
{
    var linha = stream.ReadLine();
    while (linha is not null)
    {
        var partes = linha.Split(';');


        // 0:00
        int duracao = 350;
        var match = Regex.Match(linha, @"(\d?\d):(\d\d)");
        if (match.Success)
        {
            var minutos = int.Parse(match.Groups[1].Value);
            var segundos = int.Parse(match.Groups[2].Value);
            duracao = (minutos * 60) + segundos;
        }

        if (partes.Length == 5)
        {
            var musica = new Musica()
            {
                Titulo = string.IsNullOrWhiteSpace(partes[0]) ? "N/A" : partes[0],
                Artista = string.IsNullOrWhiteSpace(partes[1]) ? "N/A" : partes[1],
                Duracao = duracao,
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