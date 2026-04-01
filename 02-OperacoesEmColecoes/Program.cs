/*
   Seja um aplicativo de gerenciamento de músicas onde os usuários podem organizar suas faixas favoritas em playlists personalizadas. Para cada playlist, é essencial que o usuário tenha controle total sobre a sequência de reprodução das músicas, permitindo reordená-las livremente a qualquer momento. Além disso, o aplicativo precisa oferecer a funcionalidade de reprodução aleatória para uma playlist específica, proporcionando uma experiência de audição dinâmica e variada, sem, contudo, alterar a ordem original que o usuário definiu. O desafio é criar uma estrutura robusta que suporte a adição e remoção eficiente de músicas, a reordenação flexível dentro das playlists e a seleção de faixas tanto em modo sequencial quanto aleatório.

   Funções que vamos implementar:
   //     [x] Criar as classes para musicas e playlist
   //     [ ] Listar músicas da playlist
   //     [ ] Adicionar música à playlist
   //     [ ] Obter uma música específica da playlist
   //     [ ] Remover música da playlist
   //     [ ] Tocar músicas da playlist em modo aleatório 
   //     [ ] Reordenar músicas segundo alguma lógica específica (ex. duração)
   //     [ ] Uma playlist não pode ter músicas repetidas
   //     [ ] Exibir as 10 músicas mais tocadas em todas as playlists (ranking)
   //     [ ] Player de música com:
   //     [ ] - Fila de reprodução (para músicas avulsas e/ou playlists)
   //     [ ] - Histórico de reprodução

*/

using System.Collections;

var musica1 = new Musica { Titulo = "Que País é esse?", Artista = "Legião Urbana", Duracao = 350 };
var musica2 = new Musica { Titulo = "Tempo Perdido", Artista = "Legião Urbana", Duracao = 455 };
var musica3 = new Musica { Titulo = "Pro Dia Nascer Feliz", Artista = "Barão Vermelho", Duracao = 345 };
var musica4 = new Musica { Titulo = "Eduardo e Mônica", Artista = "Legião Urbana", Duracao = 530 };
var musica5 = new Musica { Titulo = "Geração Coca-Cola", Artista = "Legião Urbana", Duracao = 350 };

var rockNacional = new Playlist { Nome = "Rock Nacional" };
rockNacional.Add(musica1);
rockNacional.Add(musica2);
rockNacional.Add(musica3);
rockNacional.Add(musica4);
rockNacional.Add(musica5);

ExibirPlaylist(rockNacional);

void ExibirPlaylist(Playlist playlist)
{
    Console.WriteLine($"\n Tocando as músicas de {playlist.Nome}");
    foreach (var musica in playlist)
    {
        Console.WriteLine($"\t - {musica.Titulo}");
    }
}

class Musica
{
    public string Titulo { get; set; }
    public string Artista { get; set; }
    public int Duracao { get; set; }
}

class Playlist : ICollection<Musica>
{
    private List<Musica> lista = [];
    public string Nome { get; set; }

    public int Count => lista.Count;

    public bool IsReadOnly => false;

    public void Add(Musica musica)
    {
        lista.Add(musica);
    }

    public void Clear()
    {
        lista.Clear();
    }

    public bool Contains(Musica item)
    {
        return lista.Contains(item);
    }

    public void CopyTo(Musica[] array, int arrayIndex)
    {
        lista.CopyTo(array, arrayIndex);
    }

    public IEnumerator<Musica> GetEnumerator()
    {
        return lista.GetEnumerator();
    }

    public bool Remove(Musica item)
    {
        return lista.Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}