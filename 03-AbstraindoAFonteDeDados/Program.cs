/*
    Seja um arquivo com músicas em formato CSV (Comma Separated Values). 

    Implemente as funções abaixo:
    //     [x] Leia-o como uma coleção de músicas
    //     [x] Filtre a coleção por artista (por ex. Coldplay, Metallica, AC/DC)
    //     [ ] Filtre a coleção por gênero (por ex. rock)
    //     [ ] Filtre a coleção por duração (por ex. maiores que 5 minutos)
    //     [ ] Ordene a coleção por artista
    //     [ ] Ordene a coleção por artista e em seguida por músicas com duração crescente
    //     [ ] Crie uma coleção de artistas e suas músicas
    //     [ ] Informe a duração média das músicas da coleção
    //     [ ] Informe a duração total das músicas da coleção
    //     [ ] Informe qual artista tem mais músicas na coleção
 
*/

using var arquivo = new FileStream("musicas.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(arquivo);

var musicasDoColdplay =
    (ObterMusicas(stream))                  // 1. obtenção de dados
    .Where(musica => musica.Titulo.StartsWith('C'))        // 2. filtragem por artista
    .Where(m => m.Duracao < 350);      // 3. filtragem por duração

ExibirMusicas(musicasDoColdplay);


void ExibirMusicas(IEnumerable<Musica> musicas)
{
    var contador = 1;
    Console.WriteLine($"\nExibindo as músicas");
    foreach(var musica in musicas)
    {
        Console.WriteLine($"\t - {musica.Titulo} ({musica.Artista}) - {musica.Duracao} seg");
        contador++;
        if (contador > 10) break;
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
            Duracao = int.Parse(partes[2])
        };
        yield return musica;
        linha = stream.ReadLine();
    }
}

bool FiltrarPorArtista(Musica musica) => musica.Artista == "Coldplay";

bool FiltrarMaisLongasQue(Musica musica) => musica.Duracao >= 400;

bool FiltrarPorMetallica(Musica musica) => musica.Artista == "Metallica";
bool FiltrarPorColdplay(Musica musica) => musica.Artista == "ColdPlay";
bool FiltrarPorTituloQueComecaComA(Musica musica) => musica.Titulo.StartsWith('A');

Func<Musica, bool> condicao = FiltrarMaisLongasQue; // delegate = tipos que representam métodos com a mesma assinatura

static class MusicasExtensions
{
    public static IEnumerable<T> FiltrarPor<T>(this IEnumerable<T> colecao, Func<T, bool> condicao)
    {
        foreach (var elemento in colecao)
        {
            if (condicao(elemento)) yield return elemento;
        }
    }
}

class Musica
{
    public string Titulo { get; set; }
    public string Artista { get; set; }
    public int Duracao { get; set; }
}