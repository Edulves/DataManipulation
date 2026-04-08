/*
    Seja um arquivo com músicas em formato CSV (Comma Separated Values). 

    Implemente as funções abaixo:
    //     [x] Leia-o como uma coleção de músicas
    //     [x] Filtre a coleção por artista (por ex. Coldplay, Metallica, AC/DC)
    //     [x] Filtre a coleção por gênero (por ex. rock)
    //     [x] Filtre a coleção por duração (por ex. maiores que 5 minutos)
    //     [x] Ordene a coleção por artista
    //     [x] Ordene a coleção por artista e em seguida por músicas com duração crescente
    //     [x] Crie uma coleção de artistas e suas músicas
    //     [x] Informe a duração média das músicas da coleção
    //     [x] Informe a duração total das músicas da coleção
    //     [x] Informe qual artista tem mais músicas na coleção
    //     [x] Artistas com pelo menos uma música acima de 8 minutos (480 seg)
    //     [x] Artistas com pelo menos uma música de reggae
    //     [x] Existem músicas de Jazz na coleção?
 
*/


/*

    Fluxo Padrão: Estágio 1 (Origem Dados) > Estágio 2 > ... > Estágio N

    LINQ - Categoria de operações para manipulação de coleções
    ========================================================================================
    | Filtro (+)      | coleção c/ tam menor/igual atendendo condição | Where, DIstinct    |
    | Projeção (+)    | coleção transformanda, do mesmo tipo ou não   | Select, SelectMany |
    | Ordenação (*)   | coleção ordenada pela expressão lambda        | OrderBy, ThenBy    |
    | Agregação (*)   | valor único a partir de operação de acúmulo   | Sum, Min, Max      |
    | Agrupamento (+) | coleção de grupos onde a chave é o argumento  | GroupBy            |
    | Elementos (*)   | elemento único T a partir do argumento        | First, Last, MinBy |
    | Existência (*)  | Booleano a partir da operação e argumento     | All, Any, Contains |
    | Conversão (*)   | coleção em outra estrutura                    | ToList, ToArray    |
    ========================================================================================
     
      + operações avaliadas sob demanda (yield)
      * operaçãoes avaliadas imediatamente
*/

using var arquivo = new FileStream("musicas.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(arquivo);

OperacoesDeVerificacaoDeExistencia(stream);

void OperacoesDeVerificacaoDeExistencia(StreamReader stream)
{
    var musicas = ObterMusicas(stream).ToList();

    var artistas = musicas
        .GroupBy(m => m.Artista)
        .Where(g => g.Any(m => m.Duracao >= 480));
    Console.WriteLine($"\nArtistas com músicas acima de 8 minutos.");
    foreach(var artista in artistas)
    {
        Console.WriteLine($"\t - {artista.Key}");
    }


    var reggae = musicas
        .GroupBy(m => m.Artista)
        .Where(g => g.Any(m => m.Generos.Contains("Reggae")));
    Console.WriteLine($"\nArtistas com músicas de Reggae.");
    foreach (var musica in reggae)
    {
        Console.WriteLine($"\t - {musica.Key}");
    }
}

void ArtistaComMaiorQtde(StreamReader stream)
{
    var artistaComMaiorQtdeMusicas = ObterMusicas(stream)
    .GroupBy(m => m.Artista)
    .Select(g => new { Artista = g.Key, musica = g, Total = g.Count() })
    .MaxBy(a => a.Total);

    if (artistaComMaiorQtdeMusicas is not null)
        Console.WriteLine($"O artista com maior qtde de músicas é o {artistaComMaiorQtdeMusicas.Artista} com {artistaComMaiorQtdeMusicas.Total}!");
}
void OperacoesDeObtencaiDeElementos(StreamReader stream)
{
    var musicas = ObterMusicas(stream);

    var primeiraMusica = musicas.First();
    Console.WriteLine($"A primeira música é {primeiraMusica.Titulo}");

    var maiorDuracao = musicas.MaxBy(m => m.Duracao);
    if(maiorDuracao is not null)
        Console.WriteLine($"A música com maior duração é {maiorDuracao.Titulo} com {maiorDuracao.Duracao} segundos.");
}


void OpercaoesDeAgrupamento(StreamReader stream)
{
    var artistas = ObterMusicas(stream)
    .GroupBy(m => m.Artista);

    Console.WriteLine($"\nExibindo as músicas de cada artista:");
    foreach (var artista in artistas.Take(5))
    {
        Console.WriteLine($"Artista: {artista.Key} com {artista.Count()} músicas");
        foreach (var musica in artista)
        {
            Console.WriteLine($"\t - {musica.Titulo}");
        }
    }
}

void EstatisticasDeMusicas(StreamReader stream)
{
    var musicas = ObterMusicas(stream).ToList();

    Console.WriteLine($"\nExistem {musicas.Count()} músicas na coleção.");
    Console.WriteLine($"\nExistem {musicas.Count(m => m.Duracao >= 600)} músicas com mais do que 10 minutos de duração na coleção.");
    Console.WriteLine($"\nA música com menor duração da coleção tem {musicas.Min(m => m.Duracao)} segundos.");
    Console.WriteLine($"\nA música com maior duração da coleção tem {musicas.Max(m => m.Duracao)} segudnos.");
    Console.WriteLine($"\nA duração média das músicas da coleção é {musicas.Average(m => m.Duracao).ToString("F2")} segundos.");
    Console.WriteLine($"\nVocê vai levar {musicas.Sum(m => m.Duracao)/(60 * 60 * 24)} dias para ouvir toda a coleção.");
}

void OperacoesDeProjecao2(StreamReader stream)
{
    var generos = ObterMusicas(stream)
        .SelectMany(m => m.Generos)
        .Distinct()
        .OrderBy(g => g);

    foreach (var genero in generos)
    {
        Console.WriteLine(genero);
    }
}

void OperacoesDeProjecao(StreamReader stream)
{
    var artistas = ObterMusicas(stream)
    .Select(m => m.Artista)
    .Distinct()
    .OrderBy(a => a);

    foreach (var artist in artistas)
    {
        Console.WriteLine(artist);
    }
}


void OperacoesDeFiltroEOrdenacao(StreamReader stream)
{
    var musicasDoColdplay =
        (ObterMusicas(stream))
        .Where(musica => musica.Artista == "Coldplay")
        .OrderBy(musica => musica.Titulo)
        .Skip(5 * 2)
        .Take(5);

    ExibirMusicas(musicasDoColdplay);
}

void ExibirMusicas(IEnumerable<Musica> musicas) 
{
    var contador = 1;
    Console.WriteLine($"\nExibindo as músicas");
    foreach (var musica in musicas)
    {
        Console.WriteLine($"\t - {musica.Titulo} ({musica.Artista}) - {musica.Duracao} seg");
        contador++;
        if (contador > 10)
            break;
    }
}



IEnumerable<Musica> ObterMusicas(StreamReader stream)
{
    var linha = stream.ReadLine();

    while (linha is not null)
    {
        var partes = linha.Split(';');
        var musica = new Musica()
        {
            Titulo = partes[0],
            Artista = partes[1],
            Duracao = int.Parse(partes[2]),
            Generos = partes[3].Split(",").Select(g => g.Trim())
        };
        yield return musica;
        linha = stream.ReadLine();
    }
}

class Musica
{
    public string Titulo { get; set; }
    public string Artista { get; set; }
    public int Duracao { get; set; }
    public IEnumerable<string> Generos { get; set; }
}