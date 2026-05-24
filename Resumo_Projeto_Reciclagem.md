# Resumo do Projeto: Jogo 2D Educativo de Reciclagem

Este documento serve como um registro do estado atual do projeto do jogo educativo sobre reciclagem feito em Unity (C#). Ele detalha o que já foi construído, como a cena está estruturada e como os scripts interagem entre si, garantindo que nenhum contexto seja perdido.

## 1. Visão Geral do Jogo
- **Gênero:** Jogo 2D Top-Down educativo.
- **Público-alvo:** Crianças.
- **Objetivo:** Coletar lixos espalhados pelo mapa e descartá-los nas lixeiras corretas (Orgânico ou Reciclável) antes que o tempo acabe, acumulando pontos para ganhar de 1 a 3 estrelas.

## 2. Estrutura da Cena Atual (`SampleScene`)
A hierarquia atual do Unity está configurada da seguinte forma:
- **Main Camera & CinemachineCamera:** Câmeras responsáveis por seguir o jogador suavemente pelo mapa.
- **Global Light 2D:** Iluminação global para o cenário 2D.
- **Player:** O personagem principal controlado pelo jogador.
- **MapManager / GeradorDeBlocos:** O sistema responsável por criar o cenário infinitamente ou em blocos.
- **Canvas:** Interface de usuário (UI) contendo:
  - `TextoPontuacao`: Mostra a pontuação atual.
  - `TextoTimer`: Mostra o tempo restante.
  - `TextoMochila`: Mostra a quantidade de lixo coletado (inventário).
  - `TextoAviso`: Alertas para o jogador (ex: mochila cheia).
  - `PainelFim`: Tela de Game Over desativada por padrão, contendo o `TextoResultado` (pontuação e estrelas).
- **EventSystem:** Sistema padrão do Unity para processar cliques em botões.
- **GameManager_Objeto:** Objeto vazio que guarda o script de controle geral do jogo.

## 3. Mecânicas e Scripts Implementados (Estado Base)

### 3.1 Movimentação (`PlayerMovement.cs`)
- O jogador se move nas 4 direções (WASD ou Setas).
- O movimento usa `Rigidbody2D` com `MovePosition` para lidar com colisões e prender o jogador dentro dos limites físicos do mapa (`Mathf.Clamp`).

### 3.2 Coleta e Descarte de Lixo (`InventarioPlayer.cs`)
- **A Mochila:** O jogador tem um limite de capacidade para carregar Lixo Orgânico (máx: 3) e Lixo Reciclável (máx: 3).
- **Coleta (Triggers):** Ao encostar em um Lixo (`OnTriggerEnter2D`), o script verifica de qual tipo ele é e o adiciona na mochila (se houver espaço), destruindo o objeto do chão em seguida.
- **Descarte e Pontuação:** Ao se aproximar de uma Lixeira, o jogador pressiona `1` (para descartar Orgânico) ou `2` (para descartar Reciclável).
  - *Acerto:* Se jogar o lixo na lixeira correta, ganha **10 pontos**.
  - *Erro:* Se jogar na lixeira errada, perde **5 pontos** (a pontuação não fica negativa).

### 3.3 Geração Procedural do Cenário (`GeradorDeBlocos.cs` e `BlocoMapa.cs`)
- O mapa é composto por "blocos" de chão.
- Cada bloco, ao ser criado, sorteia e instancia automaticamente lixos aleatórios (Orgânico e Reciclável) e obstáculos espalhados pelo chão de forma procedural.
- Há um sistema de "Safe Area" e verificação de colisões (`Physics2D.OverlapCircle`) para impedir que obstáculos e lixos apareçam um em cima do outro.

### 3.4 Controle da Partida (`GameManager.cs`)
- Controla um timer em contagem regressiva (padrão: 60 segundos). Quando atinge 10 segundos, o texto fica vermelho para dar urgência.
- Ao final do tempo, pausa o jogo (`Time.timeScale = 0f`) e ativa o `PainelFim`.
- Calcula o número de Estrelas do jogador com base na pontuação final:
  - 1 Estrela: < 50 pontos.
  - 2 Estrelas: >= 50 pontos.
  - 3 Estrelas: >= 100 pontos.

## 4. O Que Aconteceu / Último Problema Enfrentado
Houve uma tentativa de refatorar a interface de usuário (UI) e adicionar mecânicas de Menu, Animações e novos itens de cenário. Durante esse processo, a tela de fim de jogo exibiu textos duplicados e os layouts se sobrepuseram (estrelas, texto, botões e lixeiras do mapa pareciam "estragados").
Para preservar o trabalho sólido feito até aqui, optamos por realizar um **Rollback** (Reset do repositório) para o estado original e funcional.
