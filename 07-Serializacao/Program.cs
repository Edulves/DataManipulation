using System.Text.Json;
using System.Text.RegularExpressions;

using var arquivo = new FileStream("musicas.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(arquivo);

/*

    1. Crie uma coleção de artistas com sua músicas ordenadas por data de lançamento. Inclua o total de músicas em uma propriedade separada.
    2. Gere um arqivo no formato JSON com essa coleção

 */

var artistas = ObterMusicas(stream)
    .GroupBy(m => m.Artista)
    .Select(g => new { Artistas = g.Key, Total = g.Count(), Musicas = g.OrderBy(m => m.Lancamento) })
    .ToList();

var options = new JsonSerializerOptions
{
    WriteIndented = true,
};
var nomeArquivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "artistas.json");
using var arquivoJson = new FileStream(nomeArquivo, FileMode.Create, FileAccess.Write);
JsonSerializer.Serialize(arquivoJson, artistas, options);

Console.WriteLine("Serialização concluída!");

void ExibirMusicas(IEnumerable<Musica> musicas)
{
    var titulo = $"\nExibindo as músicas!"; // string literal
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