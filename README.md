# Dungeon Eclipse

Jogo educacional 2D de exploração e estratégia desenvolvido em Unity como protótipo para a disciplina de **Desenvolvimento de Jogos e Realidade Virtual** do Instituto Federal de Educação, Ciência e Tecnologia do Piauí (IFPI) — Curso Superior Tecnólogo em Análise e Desenvolvimento de Sistemas.

> **Grupo 06:** Roger, Elinne, Maria Fernanda e Fabricyo
> **Professor:** Denylson Melo

---

## Sobre o jogo

**Dungeon Eclipse** é um jogo educacional 2D no estilo **Metroidvania** em que o jogador controla **Kael**, um aventureiro que desperta preso nas profundezas de um enorme labirinto criado por uma civilização antiga para proteger o equilíbrio entre duas forças fundamentais: **Luz e Escuridão**.

Os **Núcleos de Equilíbrio** que mantêm a dungeon estável estão sendo destruídos e o labirinto entra em colapso — salas desaparecem, criaturas são corrompidas e áreas inteiras ficam inacessíveis. Para escapar e impedir a destruição total, Kael precisa avançar andar a andar, restaurando cada região até alcançar o **Coração Eclipse** e reconstruir o núcleo central.

O jogo une **exploração progressiva** (Metroidvania), **combate tático** e **gestão de áreas** em um tabuleiro conectado de salas. Tem como objetivo educacional estimular **raciocínio lógico, resolução de problemas, planejamento estratégico e tomada de decisão**, sendo voltado a estudantes do ensino fundamental II, ensino médio e jogadores casuais.

- **Gênero:** Metroidvania 2D
- **Tema:** Dark and Light (Luz e Escuridão)
- **Forma:** Board game (tabuleiro conectado de salas)
- **Interação principal:** Destroy (destruir cristais e obstáculos)

---

## Mecânicas principais

O loop de gameplay combina quatro mecânicas centrais:

- **Destroy** — destruir cristais corrompidos e obstáculos que bloqueiam caminhos e liberam novas rotas.
- **Building** — construir mecanismos antigos como pontes, elevadores e plataformas para abrir caminhos.
- **Combat** — enfrentar monstros e guardiões corrompidos que protegem as salas.
- **Capture** — capturar salas especiais para restaurar o equilíbrio e desbloquear novas áreas do tabuleiro.

**Loop de jogo:** explorar a região → destruir o que bloqueia o caminho → combater inimigos → construir os mecanismos necessários → capturar a sala e restaurar o equilíbrio.

Recursos adicionais previstos: backtracking típico de Metroidvania (áreas antigas reabrem com novas habilidades), recursos limitados que exigem priorização, checkpoints por andar, minimapa, barra de vida e feedback visual/sonoro imediato.

### Estrutura das fases

| Andar | Nome | Foco |
|-------|------|------|
| 1 | Prisão Abandonada | Tutorial: movimento, exploração e destruição básica |
| 2 | Minas Escuras | Building: construção de mecanismos |
| 3 | Jardins Subterrâneos | Captura de salas |
| 4 | Fortaleza Perdida | Combate avançado contra guardiões |
| Final | Coração Eclipse | Reconstrução do núcleo central e batalha final |

> No escopo do protótipo (Trabalho 04), priorizam-se os **Andares 1 e 2**, um confronto com guardião simples, telas de vitória/derrota e tutorial integrado.

---

## Estrutura do projeto

```
dungeon-eclipse/
├── Assets/                 # Conteúdo do jogo (cenas, scripts, sprites, áudio, prefabs)
│   ├── Scenes/             # Cenas do Unity (ex.: SampleScene)
│   └── Plugins/            # Bibliotecas de terceiros (NuGet)
├── Packages/               # Dependências do Unity Package Manager (manifest.json)
├── ProjectSettings/        # Configurações do projeto Unity
├── .vsconfig               # Componentes recomendados do Visual Studio
├── .gitignore              # Arquivos ignorados pelo versionamento
└── README.md
```

> As pastas `Library/`, `Temp/`, `Obj/`, `Logs/` e `UserSettings/` são geradas automaticamente pelo Unity e **não** são versionadas (ver `.gitignore`).

---

## Tecnologias utilizadas

- **Unity** `2022.3.62f3` (LTS) — motor de jogo
- **C#** — linguagem de programação dos scripts
- **Unity 2D Feature** (`com.unity.feature.2d`) — ferramentas de desenvolvimento 2D (Tilemap, Sprite, etc.)
- **TextMeshPro** — renderização de texto/UI
- **Timeline** — sequências e cutscenes
- **Unity Test Framework** — testes automatizados
- **Visual Scripting** — lógica visual
- IDEs suportadas: **Visual Studio** e **JetBrains Rider**

---

## Como abrir o projeto

### Pré-requisitos

- [Unity Hub](https://unity.com/download)
- **Unity Editor 2022.3.62f3** (versão LTS — instale exatamente esta versão pelo Unity Hub para evitar upgrades automáticos)
- Git
- Uma IDE C#: Visual Studio 2022 ou JetBrains Rider

### Passos

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/FabricyoSilva/dungeon-eclipse.git
   ```

2. **Abra no Unity Hub:**
   - Abra o **Unity Hub** → aba **Projects** → **Add** → **Add project from disk**.
   - Selecione a pasta `dungeon-eclipse` que você clonou.

3. **Garanta a versão correta do Editor:**
   - O Unity Hub indicará a versão necessária (`2022.3.62f3`). Se não estiver instalada, instale-a pelo Hub antes de abrir.

4. **Abra o projeto:**
   - Clique no projeto na lista. O Unity irá importar os assets e reconstruir a pasta `Library/` na primeira abertura (pode levar alguns minutos).

5. **Rode a cena:**
   - No painel **Project**, abra uma cena em `Assets/Scenes/`.
   - Pressione **Play** (▶) na barra superior do Editor para jogar.
