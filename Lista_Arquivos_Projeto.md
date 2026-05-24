# Lista de Arquivos, Scripts e Prefabs do Projeto

Com base no seu projeto do jogo educativo de reciclagem, aqui está o mapa completo de todos os arquivos, scripts, objetos 2D (prefabs) e elementos de interface que estão sendo usados. Isso te ajuda a saber exatamente onde cada coisa está caso você precise encontrar no Unity depois!

## 1. Cenas (Scenes)
- **SampleScene.unity:** A sua cena principal onde todo o jogo acontece e todos os objetos estão organizados.

## 2. Scripts C# (.cs)
Estes são todos os arquivos de código que dão vida ao jogo:
- **`PlayerMovement.cs`:** Cuida de andar com o boneco pelas setinhas/WASD e prende ele nos limites do mapa.
- **`InventarioPlayer.cs`:** A mochila do jogador. Cuida de encostar no lixo para coletar e de apertar 1 ou 2 perto da lixeira para descartar e ganhar pontos.
- **`GameManager.cs`:** O "chefe" da partida. Conta o cronômetro para baixo, pausa o jogo no final e calcula se o jogador ganha 1, 2 ou 3 estrelas.
- **`GeradorDeBlocos.cs`:** O script que fica lá no topo do mapa e é responsável por espalhar os quadrados (blocos) de chão formando a fase (Grade 3x3).
- **`BlocoMapa.cs`:** O script que vai colado em cada "pedacinho de chão". É ele quem espalha os lixos e as árvores aleatoriamente sem deixar baterem um no outro.
- **`Lixeira.cs`:** O código mais simples, só para avisar para o Unity "Ei, este objeto é uma Lixeira e ela aceita o tipo de lixo X".
- **`TipoLixo.cs`:** O código colado nas cascas de banana ou garrafas pet para dizer "Este lixo é Orgânico" ou "Este é Reciclável".

## 3. Lista de Prefabs (.prefab)
Os prefabs são os "carimbos" ou "clones" que o Unity pega da sua pasta de arquivos para espalhar pela tela automaticamente. De acordo com o nosso código, você tem (ou precisa ter) estes prefabs criados na sua pasta:
- `Prefab_LixoOrganico` (Ex: Casca de banana, Maçã mordida)
- `Prefab_LixoReciclavel` (Ex: Garrafa Pet, Papel)
- `Prefab_LixeiraOrganica`
- `Prefab_LixeiraReciclavel`
- `Prefab_BlocoMapa` (O quadradão verde do chão)
- `Prefab_Fonte` (A fonte de água que fica no centro do mapa)
- **Novos Prefabs que planejamos criar/adicionar:** Banco de Praça, Poste, Arbusto, Mesa de Piquenique e Balanço.

## 4. Elementos de Interface do Canvas (UI)
Estes são os textos e painéis que ficam desenhados na tela por cima do jogo:
- `TextoPontuacao`: Mostra os Pontos no canto da tela.
- `TextoTimer`: O cronômetro de 60 segundos.
- `TextoMochila`: Aquele textinho que mostra "Orgânico: 1/3" etc.
- `TextoAviso`: O aviso que pisca dizendo "Mochila cheia!".
- `PainelFim`: O grupão escondido que só aparece quando o tempo acaba.
  - `TextoResultado`: O texto do PainelFim que escreve "FIM DE JOGO!".
  - `Botao_TentarNovamente`: Para reiniciar a partida.
  - `Botao_VoltarMenu`: Para sair do jogo (futuro menu).
  - *Imagens das 3 Estrelas* (As imagens UI verdinhas que restauramos).

## 5. Arquivos de Animação (.anim)
- Como você mencionou no seu pedido de hoje ("O jogo não tem nenhuma animação"), atualmente não temos arquivos `.anim` nem `.controller` criados na pasta Assets. Esse será o nosso próximo grande passo!
