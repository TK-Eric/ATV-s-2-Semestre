// Plugin Local Figma — Adequação Completa do Projeto OurMusic
// Atualiza os 8 frames existentes e cria os 4 novos frames mantendo 100% da UX/UI original.

figma.showUI(__html__, { width: 440, height: 480 });

function sendStatus(msg, isError) {
  figma.ui.postMessage({
    type: isError ? 'error' : 'progress',
    text: msg,
    message: msg
  });
}

async function getNode(id) {
  if (typeof figma.getNodeByIdAsync === 'function') {
    return await figma.getNodeByIdAsync(id);
  }
  return figma.getNodeById(id);
}

function findChildByName(parent, name) {
  if (!parent || !parent.children) return null;
  for (var i = 0; i < parent.children.length; i++) {
    if (parent.children[i].name === name) {
      return parent.children[i];
    }
  }
  return null;
}

function findAllTextNodes(root, list) {
  if (!list) list = [];
  if (!root) return list;
  if (root.type === 'TEXT') {
    list.push(root);
  } else if (root.children) {
    for (var i = 0; i < root.children.length; i++) {
      findAllTextNodes(root.children[i], list);
    }
  }
  return list;
}

async function ensureNodeFontsLoaded(textNode) {
  if (!textNode || textNode.type !== 'TEXT') return;
  try {
    if (textNode.fontName !== figma.mixed && textNode.fontName) {
      await figma.loadFontAsync(textNode.fontName);
    } else if (typeof textNode.getStyledTextSegments === 'function') {
      var segs = textNode.getStyledTextSegments(['fontName']);
      for (var i = 0; i < segs.length; i++) {
        if (segs[i].fontName) {
          await figma.loadFontAsync(segs[i].fontName);
        }
      }
    }
  } catch (e) {
    // fallback
  }
}

async function preloadAllPageFonts() {
  var commonFonts = [
    { family: 'Inter', style: 'Light' },
    { family: 'Inter', style: 'Regular' },
    { family: 'Inter', style: 'Medium' },
    { family: 'Inter', style: 'Semi Bold' },
    { family: 'Inter', style: 'Bold' }
  ];
  for (var i = 0; i < commonFonts.length; i++) {
    try {
      await figma.loadFontAsync(commonFonts[i]);
    } catch (e) {}
  }

  var allTexts = findAllTextNodes(figma.currentPage, []);
  var seen = {};
  for (var j = 0; j < allTexts.length; j++) {
    var t = allTexts[j];
    if (t.fontName && t.fontName !== figma.mixed) {
      var key = t.fontName.family + '::' + t.fontName.style;
      if (!seen[key]) {
        seen[key] = true;
        try {
          await figma.loadFontAsync(t.fontName);
        } catch (e) {}
      }
    }
  }
}

async function setText(node, newText, mode) {
  if (!node || node.type !== 'TEXT') return;
  await ensureNodeFontsLoaded(node);
  var prevWidth = node.width;
  var prevHeight = node.height;
  node.characters = newText;
  if (mode === 'preserveWidth') {
    node.textAutoResize = 'HEIGHT';
    node.resize(prevWidth, node.height);
  } else if (mode === 'autoWidth') {
    node.textAutoResize = 'WIDTH_AND_HEIGHT';
  } else if (mode === 'keepBox') {
    node.resize(prevWidth, prevHeight);
  }
}

async function setTextById(id, newText, mode) {
  var node = await getNode(id);
  if (node && node.type === 'TEXT') {
    await setText(node, newText, mode);
  }
  return node;
}

async function hideById(id) {
  var node = await getNode(id);
  if (node) {
    node.visible = false;
  }
  return node;
}

// Helper para adicionar itens no Menu Lateral se ainda não tiverem sido adicionados
async function expandSidebarMenu(menuFrame, itemsToAdd, accessNoteFrame, accessNoteNewY) {
  if (!menuFrame || !menuFrame.children) return;
  // Evita duplicar se o plugin for rodado duas vezes
  if (menuFrame.children.length >= 5) return;

  var templateItem = menuFrame.children[menuFrame.children.length - 1];
  if (!templateItem) return;

  var isAuto = menuFrame.layoutMode && menuFrame.layoutMode !== 'NONE';
  var nextY = 133;
  for (var i = 0; i < itemsToAdd.length; i++) {
    var clone = templateItem.clone();
    clone.name = 'Item de navegação';
    menuFrame.appendChild(clone);
    if (!isAuto) {
      clone.x = 0;
      clone.y = nextY;
    }
    var texts = findAllTextNodes(clone, []);
    if (texts.length > 0) {
      await setText(texts[0], itemsToAdd[i], 'autoWidth');
    }
    nextY += 54;
  }
  if (!isAuto) {
    menuFrame.resize(menuFrame.width, nextY - 10);
    if (accessNoteFrame && (!accessNoteFrame.parent || accessNoteFrame.parent.layoutMode === 'NONE')) {
      accessNoteFrame.y = accessNoteNewY;
    }
  }
}

// Reposiciona chips de gênero horizontalmente com espaçamento uniforme
async function updateGenreChips(containerFrame, labels) {
  if (!containerFrame || !containerFrame.children) return;
  var curX = 0;
  var isContainerAuto = containerFrame.layoutMode && containerFrame.layoutMode !== 'NONE';
  for (var i = 0; i < containerFrame.children.length; i++) {
    var chip = containerFrame.children[i];
    if (!chip.visible) continue;
    if (i < labels.length) {
      var texts = findAllTextNodes(chip, []);
      if (texts.length > 0) {
        await setText(texts[0], labels[i], 'autoWidth');
        var isChipAuto = chip.layoutMode && chip.layoutMode !== 'NONE';
        if (!isChipAuto) {
          var padX = texts[0].x || 14;
          var newW = Math.ceil(texts[0].width + padX * 2);
          chip.resize(newW, chip.height);
        }
      }
      if (!isContainerAuto) {
        chip.x = curX;
        curX += chip.width + 8;
      }
    }
  }
}

// ============================================================================
// 1. CRIAR AS 4 TELAS NOVAS A PARTIR DE CLONES DOS FRAMES ORIGINAIS
// ============================================================================

async function createNewFramesFromClones() {
  var generatedNames = [
    'Gerenciamento de gêneros',
    'Cadastro e edição de usuário',
    'Meu perfil',
    'Confirmação de desativação de usuário'
  ];

  // Remove versões anteriores das 4 telas geradas caso o usuário execute novamente
  var pageChildren = figma.currentPage.children.slice();
  for (var i = 0; i < pageChildren.length; i++) {
    var ch = pageChildren[i];
    if (generatedNames.indexOf(ch.name) !== -1) {
      ch.remove();
    }
  }

  var frameAdminMusicas = await getNode('2:6687');
  var frameFormMusica = await getNode('2:6892');
  var frameCatalogo = await getNode('2:6421');
  var frameModalExclusao = await getNode('2:7303');

  // --------------------------------------------------------------------------
  // TELA NOVA 1: Gerenciamento de gêneros (RF11 / RN11)
  // --------------------------------------------------------------------------
  if (frameAdminMusicas) {
    sendStatus('Criando tela: Gerenciamento de gêneros (RF11)...', false);
    var genFrame = frameAdminMusicas.clone();
    genFrame.name = 'Gerenciamento de gêneros';
    figma.currentPage.appendChild(genFrame);
    genFrame.x = -681;
    genFrame.y = 4667;

    var navLat = genFrame.children[0];
    var menu = findChildByName(navLat, 'Menu');
    var notaAcesso = findChildByName(navLat, 'Nota de acesso');
    var rodapeLat = findChildByName(navLat, 'Rodapé lateral');
    await expandSidebarMenu(menu, ['Gêneros', 'Meu perfil'], notaAcesso, 360);
    if (rodapeLat) {
      var rodTexts = findAllTextNodes(rodapeLat, []);
      if (rodTexts.length > 0) await setText(rodTexts[0], 'Ir para catálogo · Sair', 'autoWidth');
    }

    var areaConteudo = genFrame.children[1];
    var barraSup = findChildByName(areaConteudo, 'Barra superior');
    if (barraSup) {
      var caminho = findChildByName(barraSup, 'Caminho');
      var camTexts = findAllTextNodes(caminho, []);
      if (camTexts.length >= 3) {
        await setText(camTexts[2], 'Gêneros', 'autoWidth');
      }
    }

    var conteudo = findChildByName(areaConteudo, 'Conteúdo');
    var cabSecao = findChildByName(conteudo, 'Cabeçalho da seção');
    if (cabSecao) {
      var apres = findChildByName(cabSecao, 'Apresentação');
      var apTexts = findAllTextNodes(apres, []);
      if (apTexts.length >= 3) {
        await setText(apTexts[0], 'GESTÃO DE GÊNEROS · RF11 E RN11', 'autoWidth');
        await setText(apTexts[1], 'Gêneros musicais', 'preserveWidth');
        await setText(apTexts[2], 'Cadastre novos gêneros e edite os nomes existentes vinculados às músicas do catálogo.', 'preserveWidth');
      }
      var btnAcao = findChildByName(cabSecao, 'Ação');
      var btnTexts = findAllTextNodes(btnAcao, []);
      if (btnTexts.length > 0) {
        await setText(btnTexts[0], 'Novo gênero', 'autoWidth');
      }
    }

    var visaoGeral = findChildByName(conteudo, 'Visão geral');
    if (visaoGeral && visaoGeral.children.length >= 3) {
      var c0 = findAllTextNodes(visaoGeral.children[0], []);
      if (c0.length >= 2) {
        await setText(c0[0], '12', 'autoWidth');
        await setText(c0[1], 'Gêneros cadastrados', 'autoWidth');
      }
      var c1 = findAllTextNodes(visaoGeral.children[1], []);
      if (c1.length >= 2) {
        await setText(c1[0], '148', 'autoWidth');
        await setText(c1[1], 'Vínculos em MusicaGenero', 'autoWidth');
      }
      var c2 = findAllTextNodes(visaoGeral.children[2], []);
      if (c2.length >= 2) {
        await setText(c2[0], '100%', 'autoWidth');
        await setText(c2[1], 'Nomes únicos (RN11)', 'autoWidth');
      }
    }

    var gestao = findChildByName(conteudo, 'Gestão de músicas');
    if (gestao) {
      gestao.name = 'Gestão de gêneros';
      var pesq = findChildByName(gestao, 'Pesquisa e filtros');
      if (pesq && pesq.children.length >= 2) {
        var busca = pesq.children[0];
        var bTexts = findAllTextNodes(busca, []);
        if (bTexts.length > 0) {
          await setText(bTexts[0], 'Digite o nome do gênero para buscar ou cadastrar rapidamente (RN11: nome único)...', 'autoWidth');
        }
        busca.resize(860, 44);
        var filtro1 = pesq.children[1];
        filtro1.x = 872;
        filtro1.resize(200, 44);
        var f1Texts = findAllTextNodes(filtro1, []);
        if (f1Texts.length > 0) {
          await setText(f1Texts[0], 'Salvar gênero (RF11)', 'autoWidth');
        }
        if (pesq.children[2]) {
          pesq.children[2].visible = false;
        }
      }

      var tabela = findChildByName(gestao, 'Tabela de músicas');
      if (tabela) {
        tabela.name = 'Tabela de gêneros';
        var tabHeader = findChildByName(tabela, 'Cabeçalho da tabela');
        if (tabHeader) {
          var hTexts = findAllTextNodes(tabHeader, []);
          if (hTexts.length >= 6) {
            await setText(hTexts[0], 'GÊNERO MUSICAL (TABELA GENERO)', 'autoWidth');
            await setText(hTexts[1], 'ID (IDGENERO)', 'autoWidth');
            hTexts[1].x = 562;
            await setText(hTexts[2], 'MÚSICAS VINCULADAS', 'autoWidth');
            hTexts[2].x = 690;
            await setText(hTexts[3], 'UNICIDADE (RN11)', 'autoWidth');
            hTexts[3].x = 835;
            hTexts[4].visible = false;
            await setText(hTexts[5], 'AÇÕES (RF11)', 'autoWidth');
            hTexts[5].x = 955;
          }
        }

        var genreRowsData = [
          { name: 'Maid Core', sub: 'Gênero inicial do banco OurMusicDB.sql', id: '#01', count: '24 músicas', rule: 'Nome único validado' },
          { name: 'Tribal', sub: 'Gênero inicial do banco OurMusicDB.sql', id: '#02', count: '19 músicas', rule: 'Nome único validado' },
          { name: 'Sertanejo', sub: 'Gênero inicial do banco OurMusicDB.sql', id: '#03', count: '32 músicas', rule: 'Nome único validado' },
          { name: 'Eletrônica', sub: 'Cadastrado pelo Administrador', id: '#04', count: '28 músicas', rule: 'Nome único validado' },
          { name: 'Indie / Alternativo', sub: 'Cadastrado pelo Administrador', id: '#05', count: '25 músicas', rule: 'Nome único validado' },
          { name: 'MPB Contemporânea', sub: 'Cadastrado pelo Administrador', id: '#06', count: '20 músicas', rule: 'Nome único validado' }
        ];

        var rowIdx = 0;
        for (var r = 0; r < tabela.children.length; r++) {
          var row = tabela.children[r];
          if (row.name === 'Linha de música' && rowIdx < genreRowsData.length) {
            row.name = 'Linha de gênero';
            var d = genreRowsData[rowIdx];
            var musArt = findChildByName(row, 'Música e artista');
            if (musArt) {
              var ident = findChildByName(musArt, 'Identificação');
              var idTexts = findAllTextNodes(ident, []);
              if (idTexts.length >= 2) {
                await setText(idTexts[0], d.name, 'autoWidth');
                await setText(idTexts[1], d.sub, 'autoWidth');
              }
            }
            var gText = findChildByName(row, 'Gênero');
            if (gText) {
              gText.x = 562;
              await setText(gText, d.id, 'autoWidth');
            }
            var anoText = findChildByName(row, 'Ano');
            if (anoText) {
              anoText.x = 690;
              await setText(anoText, d.count, 'autoWidth');
            }
            var durText = findChildByName(row, 'Duração');
            if (durText) {
              durText.x = 835;
              await setText(durText, d.rule, 'autoWidth');
            }
            var pubFrame = findChildByName(row, 'Publicação');
            if (pubFrame) pubFrame.visible = false;

            var acoes = findChildByName(row, 'Ações da música');
            if (acoes) {
              acoes.x = 955;
              // RF11 prevê apenas Cadastrar e Editar gêneros (esconde olho e lixeira, mantém lápis de edição)
              if (acoes.children.length >= 3) {
                acoes.children[0].visible = false;
                acoes.children[1].x = 0;
                acoes.children[2].visible = false;
              }
            }
            rowIdx++;
          }
        }

        var rodTabela = findChildByName(tabela, 'Rodapé da tabela');
        if (rodTabela) {
          var rtTexts = findAllTextNodes(rodTabela, []);
          if (rtTexts.length > 0) {
            await setText(rtTexts[0], 'Mostrando 1–6 de 12 gêneros', 'autoWidth');
          }
        }
      }

      var leg = findChildByName(gestao, 'Legenda de ações');
      if (leg) {
        var legTexts = findAllTextNodes(leg, []);
        if (legTexts.length >= 2) {
          await setText(legTexts[0], 'RF11 e RN11: Apenas Administradores cadastram e editam gêneros (UQ_Genero_Nome)', 'autoWidth');
          await setText(legTexts[1], 'Clique no ícone de lápis para renomear', 'autoWidth');
        }
      }
    }
  }

  // --------------------------------------------------------------------------
  // TELA NOVA 2: Cadastro e edição de usuário (RF13, RF14, RF15 / RN01, RN03, RN04)
  // --------------------------------------------------------------------------
  if (frameFormMusica) {
    sendStatus('Criando tela: Cadastro e edição de usuário (RF13–RF15)...', false);
    var userFormFrame = frameFormMusica.clone();
    userFormFrame.name = 'Cadastro e edição de usuário';
    figma.currentPage.appendChild(userFormFrame);
    userFormFrame.x = 799;
    userFormFrame.y = 4667;

    var uNav = userFormFrame.children[0];
    var uMenu = findChildByName(uNav, 'Menu');
    var uNota = findChildByName(uNav, 'Nota de acesso');
    var uRodape = findChildByName(uNav, 'Rodapé lateral');
    await expandSidebarMenu(uMenu, ['Gêneros', 'Meu perfil'], uNota, 360);

    var uArea = userFormFrame.children[1];
    var uTop = findChildByName(uArea, 'Barra superior');
    if (uTop) {
      var uCam = findChildByName(uTop, 'Caminho');
      var uCamTexts = findAllTextNodes(uCam, []);
      if (uCamTexts.length >= 3) {
        await setText(uCamTexts[2], 'Usuários', 'autoWidth');
      }
    }

    var uCont = findChildByName(uArea, 'Conteúdo');
    var uRet = findChildByName(uCont, 'Retornar à lista');
    if (uRet) {
      var uRetTexts = findAllTextNodes(uRet, []);
      if (uRetTexts.length > 0) await setText(uRetTexts[0], 'Voltar para lista de usuários', 'autoWidth');
    }

    var uCab = findChildByName(uCont, 'Cabeçalho da seção');
    if (uCab) {
      var uApres = findChildByName(uCab, 'Apresentação');
      var uApTexts = findAllTextNodes(uApres, []);
      if (uApTexts.length >= 2) {
        await setText(uApTexts[0], 'Cadastro e edição de usuário', 'preserveWidth');
        await setText(uApTexts[1], 'Exclusivo para Administradores (RN01): cadastre contas, defina TipoUsuario e controle o status Ativo (RN04).', 'preserveWidth');
      }
      var uInd = findChildByName(uCab, 'Indicador');
      if (uInd) uInd.visible = true;
      var uIndTexts = findAllTextNodes(uInd, []);
      if (uIndTexts.length > 0) {
        await setText(uIndTexts[0], 'Ativo (1)', 'autoWidth');
      }
    }

    var uForm = findChildByName(uCont, 'Formulário de música');
    if (uForm) {
      uForm.name = 'Formulário de usuário';
      var uEsq = findChildByName(uForm, 'Capa e publicação');
      if (uEsq) {
        uEsq.name = 'Identidade e status';
        var cardImg = findChildByName(uEsq, 'Imagem do álbum');
        if (cardImg) {
          cardImg.name = 'Identificação do usuário';
          var ciTexts = findAllTextNodes(cardImg, []);
          if (ciTexts.length >= 3) {
            await setText(ciTexts[0], 'Identificação da conta', 'autoWidth');
            await setText(ciTexts[1], 'Tipo: Usuario / Admin', 'autoWidth');
            await setText(ciTexts[2], 'RN01: Não existe auto-cadastro público. Apenas o Admin cria contas.', 'preserveWidth');
          }
        }
        var cardVis = findChildByName(uEsq, 'Visibilidade');
        if (cardVis) {
          cardVis.visible = true;
          cardVis.name = 'Status Ativo (RN04)';
          var cvTexts = findAllTextNodes(cardVis, []);
          if (cvTexts.length >= 3) {
            await setText(cvTexts[0], 'Status (Ativo)', 'autoWidth');
            await setText(cvTexts[1], 'Conta ativa (Ativo = 1)', 'autoWidth');
            await setText(cvTexts[2], 'RN04: Desativar executa exclusão lógica (Ativo = 0 via trigger).', 'preserveWidth');
          }
        }
      }

      var uDir = findChildByName(uForm, 'Dados da música');
      if (uDir) {
        uDir.name = 'Dados do usuário';
        var infoPrinc = findChildByName(uDir, 'Informações principais');
        var credLanc = findChildByName(uDir, 'Créditos e lançamento');
        if (credLanc) {
          credLanc.visible = false;
        }
        if (infoPrinc) {
          var ipTexts = findAllTextNodes(infoPrinc, []);
          // Estrutura de textos em Informações principais:
          // [0] Título do grupo, [1] Campos obrigatórios
          // [2] Rótulo 1, [3] Valor 1
          // [4] Rótulo 2 (Artista), [5] Valor 2
          // [6] Rótulo 3 (Álbum), [7] Valor 3
          // [8] Rótulo 4 (Gênero), [9] Valor 4
          // [10] Rótulo 5 (Ano), [11] Valor 5
          // [12] Rótulo 6 (Duração), [13] Valor 6, [14] Ajuda 6
          // [15] Rótulo 7 (Descrição), [16] Valor 7, [17] Ajuda 7
          if (ipTexts.length >= 18) {
            await setText(ipTexts[0], 'Dados da tabela Usuario', 'autoWidth');
            await setText(ipTexts[1], '* Campos obrigatórios', 'autoWidth');
            await setText(ipTexts[2], 'Nome completo (Nome) *', 'autoWidth');
            await setText(ipTexts[3], 'Marina Costa', 'autoWidth');
            await setText(ipTexts[4], 'E-mail único (Email) *', 'autoWidth');
            await setText(ipTexts[5], 'marina@ourmusic.com', 'autoWidth');
            await setText(ipTexts[6], 'Senha (Senha — SHA2_256) *', 'autoWidth');
            await setText(ipTexts[7], '•••••••••••• (Hash SHA-256 no banco)', 'autoWidth');
            await setText(ipTexts[8], 'Tipo de usuário *', 'autoWidth');
            await setText(ipTexts[9], 'Usuario', 'autoWidth');
            await setText(ipTexts[10], 'Status (Ativo) *', 'autoWidth');
            await setText(ipTexts[11], '1 — Ativo', 'autoWidth');
            await setText(ipTexts[12], 'ID (IdUsuario)', 'autoWidth');
            await setText(ipTexts[13], '#01', 'autoWidth');
            await setText(ipTexts[14], 'PK IDENTITY(1,1)', 'autoWidth');
            await setText(ipTexts[15], 'Resumo de permissões atribuídas (RN01 / RN02 / RN09)', 'autoWidth');
            await setText(ipTexts[16], 'Perfil Usuario: explora o catálogo, cadastra músicas, exclui apenas as próprias músicas e edita seu próprio perfil.', 'preserveWidth');
            await setText(ipTexts[17], 'RN03: O e-mail informado deve ser único no sistema (UQ_Usuario_Email).', 'preserveWidth');
          }
        }
        uDir.resize(748, 615);
      }
      uForm.resize(1072, 615);
    }

    var uAcoes = findChildByName(uCont, 'Ações do formulário');
    if (uAcoes) {
      uAcoes.y = 815;
      var uaTexts = findAllTextNodes(uAcoes, []);
      if (uaTexts.length >= 3) {
        await setText(uaTexts[0], 'Desativar conta', 'autoWidth');
        await setText(uaTexts[1], 'Cancelar', 'autoWidth');
        await setText(uaTexts[2], 'Salvar usuário', 'autoWidth');
      }
    }

    var uUlt = findChildByName(uCont, 'Última alteração');
    if (uUlt) {
      uUlt.y = 910;
      await setText(uUlt, 'RF13 a RF15 · RN01, RN03 e RN04: Senha criptografada via HASHBYTES SHA2_256 no SQL Server.', 'autoWidth');
    }

    uCont.resize(1136, 997);
    uArea.resize(1136, 1077);
    uNav.resize(224, 1077);
    if (uRodape) uRodape.y = 1002;
    userFormFrame.resize(1360, 1077);
  }

  // --------------------------------------------------------------------------
  // TELA NOVA 3: Meu perfil (RF07 / RN09 / RF02 — Todos os usuários)
  // --------------------------------------------------------------------------
  if (frameFormMusica && frameCatalogo) {
    sendStatus('Criando tela: Meu perfil (RF07 / RN09)...', false);
    var profileFrame = frameFormMusica.clone();
    profileFrame.name = 'Meu perfil';
    figma.currentPage.appendChild(profileFrame);
    profileFrame.x = -681;
    profileFrame.y = 5864;

    // Substitui a navegação lateral pela navegação do Ouvinte (Catálogo) para refletir Todos os Usuários
    var catNavClone = frameCatalogo.children[0].clone();
    profileFrame.children[0].remove();
    profileFrame.insertChild(0, catNavClone);
    catNavClone.x = 0;
    catNavClone.y = 0;
    catNavClone.resize(224, 1020);

    var pMenu = findChildByName(catNavClone, 'Menu');
    var pNota = findChildByName(catNavClone, 'Nota de acesso');
    var pRod = findChildByName(catNavClone, 'Rodapé lateral');
    await expandSidebarMenu(pMenu, ['Cadastrar música', 'Meu perfil'], pNota, 360);
    if (pNota) {
      var pnTexts = findAllTextNodes(pNota, []);
      if (pnTexts.length >= 2) {
        await setText(pnTexts[0], 'Perfil: Usuário (Ouvinte)', 'autoWidth');
        await setText(pnTexts[1], 'Edite seus dados (RF07) e gerencie suas músicas.', 'preserveWidth');
      }
    }
    if (pRod) {
      pRod.y = 945;
      var prTexts = findAllTextNodes(pRod, []);
      if (prTexts.length > 0) await setText(prTexts[0], 'Encerrar sessão (RF02)', 'autoWidth');
    }

    var pArea = profileFrame.children[1];
    var pTop = findChildByName(pArea, 'Barra superior');
    if (pTop) {
      var pCam = findChildByName(pTop, 'Caminho');
      var pCamTexts = findAllTextNodes(pCam, []);
      if (pCamTexts.length >= 3) {
        await setText(pCamTexts[2], 'Meu perfil', 'autoWidth');
      }
    }

    var pCont = findChildByName(pArea, 'Conteúdo');
    var pRet = findChildByName(pCont, 'Retornar à lista');
    if (pRet) {
      var pRetTexts = findAllTextNodes(pRet, []);
      if (pRetTexts.length > 0) await setText(pRetTexts[0], 'Voltar para o catálogo', 'autoWidth');
    }

    var pCab = findChildByName(pCont, 'Cabeçalho da seção');
    if (pCab) {
      var pApres = findChildByName(pCab, 'Apresentação');
      var pApTexts = findAllTextNodes(pApres, []);
      if (pApTexts.length >= 2) {
        await setText(pApTexts[0], 'Meu perfil', 'preserveWidth');
        await setText(pApTexts[1], 'Atualize suas informações pessoais e senha de acesso (RF07 / RN09) ou encerre sua sessão (RF02).', 'preserveWidth');
      }
      var pInd = findChildByName(pCab, 'Indicador');
      if (pInd) pInd.visible = true;
      var pIndTexts = findAllTextNodes(pInd, []);
      if (pIndTexts.length > 0) {
        await setText(pIndTexts[0], 'Conta ativa', 'autoWidth');
      }
    }

    var pForm = findChildByName(pCont, 'Formulário de música');
    if (pForm) {
      pForm.name = 'Formulário de perfil';
      var pEsq = findChildByName(pForm, 'Capa e publicação');
      if (pEsq) {
        var pCardImg = findChildByName(pEsq, 'Imagem do álbum');
        if (pCardImg) {
          var pciTexts = findAllTextNodes(pCardImg, []);
          if (pciTexts.length >= 3) {
            await setText(pciTexts[0], 'Minha conta', 'autoWidth');
            await setText(pciTexts[1], 'Encerrar sessão (RF02)', 'autoWidth');
            await setText(pciTexts[2], 'RF02: Clique para sair da conta atual e voltar à tela de login.', 'preserveWidth');
          }
        }
        var pCardVis = findChildByName(pEsq, 'Visibilidade');
        if (pCardVis) pCardVis.visible = false;
        pEsq.resize(300, 552);
      }

      var pDir = findChildByName(pForm, 'Dados da música');
      if (pDir) {
        var pInfo = findChildByName(pDir, 'Informações principais');
        var pCred = findChildByName(pDir, 'Créditos e lançamento');
        if (pCred) pCred.visible = false;
        if (pInfo) {
          var piTexts = findAllTextNodes(pInfo, []);
          if (piTexts.length >= 18) {
            await setText(piTexts[0], 'Meus dados pessoais (RF07)', 'autoWidth');
            await setText(piTexts[1], '* Campos obrigatórios', 'autoWidth');
            await setText(piTexts[2], 'Meu nome completo (Nome) *', 'autoWidth');
            await setText(piTexts[3], 'Marina Costa', 'autoWidth');
            await setText(piTexts[4], 'Meu e-mail de acesso (Email) *', 'autoWidth');
            await setText(piTexts[5], 'marina@ourmusic.com', 'autoWidth');
            await setText(piTexts[6], 'Nova senha (Senha — opcional)', 'autoWidth');
            await setText(piTexts[7], '••••••••••••', 'autoWidth');
            await setText(piTexts[8], 'Confirmar senha', 'autoWidth');
            await setText(piTexts[9], '••••••••••••', 'autoWidth');
            await setText(piTexts[10], 'Tipo de perfil (RN09)', 'autoWidth');
            await setText(piTexts[11], 'Usuario (Somente leitura)', 'autoWidth');
            await setText(piTexts[12], 'Minhas músicas', 'autoWidth');
            await setText(piTexts[13], '4 cadastradas', 'autoWidth');
            await setText(piTexts[14], 'Você pode excluir as suas', 'autoWidth');
            await setText(piTexts[15], 'Regras de privacidade e escopo do perfil (RN09)', 'autoWidth');
            await setText(piTexts[16], 'Você pode editar livremente seu Nome, E-mail e Senha. Você também pode cadastrar novas músicas e excluir apenas as músicas que você mesmo cadastrou.', 'preserveWidth');
            await setText(piTexts[17], 'RN09: Um usuário não pode editar informações de outros usuários.', 'preserveWidth');
          }
        }
        pDir.resize(748, 552);
      }
      pForm.resize(1072, 552);
    }

    var pAcoes = findChildByName(pCont, 'Ações do formulário');
    if (pAcoes) {
      pAcoes.y = 755;
      var paTexts = findAllTextNodes(pAcoes, []);
      if (paTexts.length >= 3) {
        await setText(paTexts[0], 'Sair da conta', 'autoWidth');
        await setText(paTexts[1], 'Cancelar', 'autoWidth');
        await setText(paTexts[2], 'Salvar alterações', 'autoWidth');
      }
    }

    var pUlt = findChildByName(pCont, 'Última alteração');
    if (pUlt) {
      pUlt.y = 850;
      await setText(pUlt, 'RF07 e RN09: Alterações salvas atualizam seu registro na tabela Usuario.', 'autoWidth');
    }

    pCont.resize(1136, 940);
    pArea.resize(1136, 1020);
    profileFrame.resize(1360, 1020);
  }

  // --------------------------------------------------------------------------
  // TELA NOVA 4: Confirmação de desativação de usuário (RF15 / RN04 — Soft Delete)
  // --------------------------------------------------------------------------
  if (frameModalExclusao) {
    sendStatus('Criando modal: Confirmação de desativação de usuário (RN04)...', false);
    var deactModal = frameModalExclusao.clone();
    deactModal.name = 'Confirmação de desativação de usuário';
    figma.currentPage.appendChild(deactModal);
    deactModal.x = 1369;
    deactModal.y = 3275;

    var dmTexts = findAllTextNodes(deactModal, []);
    // [0] Título, [1] Consequência, [2] Item Título, [3] Item Sub, [4] Aviso permanente, [5] Cancelar, [6] Sim, excluir música
    if (dmTexts.length >= 7) {
      await setText(dmTexts[0], 'Desativar este usuário?', 'preserveWidth');
      await setText(dmTexts[1], 'O usuário terá seu campo Ativo alterado para 0 (Inativo) e não poderá autenticar-se, preservando o histórico no banco (RN04).', 'preserveWidth');
      await setText(dmTexts[2], 'Marina Costa', 'autoWidth');
      await setText(dmTexts[3], 'marina@ourmusic.com · Perfil: Usuario (ID #01)', 'autoWidth');
      await setText(dmTexts[4], 'Exclusão lógica via trigger TR_UsuarioSoftDelete (Ativo = 0).', 'autoWidth');
      await setText(dmTexts[5], 'Cancelar', 'autoWidth');
      await setText(dmTexts[6], 'Sim, desativar', 'autoWidth');
    }
  }
}

// ============================================================================
// 2. ATUALIZAR AS 8 TELAS EXISTENTES PARA SEGUIR ESTRITAMENTE O ESCOPO OURMUSIC
// ============================================================================

async function updateExistingFrames() {
  // --------------------------------------------------------------------------
  // 1) Autenticação (2:6362) — RF01 / RN01
  // --------------------------------------------------------------------------
  sendStatus('Ajustando tela 1/8: Autenticação (RF01 / RN01)...', false);
  await hideById('2:6383'); // "Precisa de ajuda?"
  await hideById('2:6402'); // "Continuar conectado" / "Esqueci minha senha"
  await hideById('2:6414'); // "Ainda não tem uma conta? Solicite acesso"

  await setTextById('2:6388', 'ACESSO À PLATAFORMA · RF01', 'autoWidth');
  await setTextById('2:6390', 'Informe seu e-mail e senha cadastrados pelo administrador.', 'preserveWidth');

  var btnLogin = await getNode('2:6408');
  if (btnLogin) btnLogin.y = 330;

  var segFrame = await getNode('2:6411');
  if (segFrame) segFrame.y = 398;
  var lockIcon = await getNode('2:7344');
  if (lockIcon) lockIcon.x = 52;
  var segText = await setTextById('2:6413', 'Cadastro exclusivo pelo Administrador (RN01 · SHA2_256)', 'autoWidth');
  if (segText) segText.x = 74;

  // --------------------------------------------------------------------------
  // 2) Catálogo de músicas (2:6421) — RF02, RF03, RF04, RF05, RF07
  // --------------------------------------------------------------------------
  sendStatus('Ajustando tela 2/8: Catálogo de músicas (RF03–RF05)...', false);
  var catMenu = await getNode('2:6427');
  var catNota = await getNode('2:6435');
  await expandSidebarMenu(catMenu, ['Cadastrar música', 'Meu perfil'], catNota, 360);

  await setTextById('2:6437', 'Perfil: Usuário (Ouvinte)', 'autoWidth');
  await setTextById('2:6438', 'Explora catálogo, cadastra músicas e exclui as próprias.', 'preserveWidth');
  await setTextById('2:6442', 'Encerrar sessão (RF02)', 'autoWidth');

  await setTextById('2:6465', 'DESTAQUE DO CATÁLOGO', 'autoWidth');
  await setTextById('2:6467', 'Explore as músicas disponíveis, filtre por gênero musical, pesquise pelo título e acesse os detalhes completos.', 'preserveWidth');

  // Remove botão extra "Filtros" e expande a busca por título (RF05)
  await hideById('2:6482');
  var catBusca = await getNode('2:6479');
  if (catBusca) catBusca.resize(1072, 44);
  await setTextById('2:6481', 'Buscar músicas pelo título (RF05)...', 'autoWidth');

  // Atualiza os chips de gênero (RF04) incluindo os gêneros do banco OurMusicDB.sql
  var catGenres = await getNode('2:6485');
  await updateGenreChips(catGenres, [
    'Todos',
    'Maid Core',
    'Tribal',
    'Sertanejo',
    'Eletrônica',
    'Indie',
    'MPB',
    'Ambiente'
  ]);

  // Remove ordenação fora do escopo
  await hideById('2:6507');

  // Atualiza metadados dos cards para exibir Gênero · Duração
  await setTextById('2:6519', 'Maid Core · 03:48', 'autoWidth');
  await setTextById('2:6528', 'Tribal · 04:12', 'autoWidth');
  await setTextById('2:6537', 'Sertanejo · 02:56', 'autoWidth');

  // --------------------------------------------------------------------------
  // 3) Detalhes da música (2:6575) — RF06 + Ações do Usuário na própria música
  // --------------------------------------------------------------------------
  sendStatus('Ajustando tela 3/8: Detalhes da música (RF06)...', false);
  var detMenu = await getNode('2:6581');
  var detNota = await getNode('2:6589');
  await expandSidebarMenu(detMenu, ['Cadastrar música', 'Meu perfil'], detNota, 360);

  await setTextById('2:6591', 'Perfil: Usuário (Ouvinte)', 'autoWidth');
  await setTextById('2:6592', 'Explora catálogo, cadastra músicas e exclui as próprias.', 'preserveWidth');
  await setTextById('2:6596', 'Encerrar sessão (RF02)', 'autoWidth');

  // Remove menção a Álbum e mostra DataLancamento + Duracao
  await setTextById('2:6620', '2025', 'autoWidth');
  var detBadge2 = await getNode('2:6621');
  if (detBadge2) detBadge2.resize(145, 25);
  await setTextById('2:6622', 'Maid Core · Eletrônica', 'autoWidth');
  await setTextById('2:6626', 'Lançamento: 14/03/2025  ·  Duração: 03:48 (228s)', 'autoWidth');

  // Substitui "Ver álbum" e "Compartilhar" por "Editar música" e "Excluir minha música"
  var btnDet1 = await getNode('2:6628');
  if (btnDet1) btnDet1.resize(145, 44);
  await setTextById('2:6630', 'Editar música', 'autoWidth');

  var btnDet2 = await getNode('2:6631');
  if (btnDet2) {
    btnDet2.x = 155;
    btnDet2.resize(195, 44);
  }
  await setTextById('2:6633', 'Excluir minha música', 'autoWidth');
  var detActions = await getNode('2:6627');
  if (detActions) detActions.resize(350, 44);

  // Seção Comentário e Letra (colunas Comentario e Letra da tabela Musica)
  await setTextById('2:6636', 'Comentário e Letra da música', 'autoWidth');
  await setTextById(
    '2:6637',
    'Comentário (Comentario): Faixa com sintetizadores analógicos e atmosfera noturna cadastrada no acervo OurMusic.',
    'preserveWidth'
  );
  await setTextById(
    '2:6638',
    'Letra (Letra): "Luzes da avenida refletem no asfalto molhado / O pulso acelera enquanto a cidade adormece / Entre ecos e frequências encontramos nosso lugar..."',
    'preserveWidth'
  );
  var detTags = await getNode('2:6639');
  await updateGenreChips(detTags, ['Maid Core', 'Eletrônica', 'Letra disponível']);

  // Ficha técnica -> Dados reais da tabela Musica + MusicaGenero (sem Álbum/Composição/Idioma/Gravadora)
  await setTextById('2:6647', 'Dados do banco', 'autoWidth');
  var fichaRows = [
    { labelId: '2:6649', valId: '2:6650', label: 'ID da música', val: '#01 (IdMusica)' },
    { labelId: '2:6652', valId: '2:6653', label: 'Artista', val: 'Lumen Vale' },
    { labelId: '2:6655', valId: '2:6656', label: 'Lançamento', val: '14/03/2025' },
    { labelId: '2:6658', valId: '2:6659', label: 'Duração', val: '228 seg (03:48)' },
    { labelId: '2:6661', valId: '2:6662', label: 'Gênero(s)', val: 'Maid Core, Eletrônica' }
  ];
  for (var f = 0; f < fichaRows.length; f++) {
    await setTextById(fichaRows[f].labelId, fichaRows[f].label, 'autoWidth');
    var vNode = await setTextById(fichaRows[f].valId, fichaRows[f].val, 'autoWidth');
    if (vNode) {
      vNode.x = 328 - vNode.width;
    }
  }

  // --------------------------------------------------------------------------
  // 4) Administração de músicas (2:6687) — RF08, RF09, RF10 (Sem Álbum e sem Status/Rascunho)
  // --------------------------------------------------------------------------
  sendStatus('Ajustando tela 4/8: Administração de músicas (RF08–RF10)...', false);
  var admMenu = await getNode('2:6693');
  var admNota = await getNode('2:6701');
  await expandSidebarMenu(admMenu, ['Gêneros', 'Meu perfil'], admNota, 360);
  await setTextById('2:6708', 'Ir para catálogo · Sair', 'autoWidth');

  await setTextById('2:6727', 'GESTÃO DE MÚSICAS · RF08 A RF10', 'autoWidth');
  await setTextById(
    '2:6729',
    'Gerencie as músicas do catálogo (Admin gerencia todas; Usuário comum adiciona e exclui apenas as próprias).',
    'preserveWidth'
  );

  // Cards de resumo: substitui "Publicadas" e "Rascunhos" por métricas reais do banco
  await setTextById('2:6741', '12', 'autoWidth');
  await setTextById('2:6742', 'Gêneros cadastrados', 'autoWidth');
  await setTextById('2:6746', '148', 'autoWidth');
  await setTextById('2:6747', 'Músicas com letra', 'autoWidth');

  // Remove filtro "Todos os status" e expande busca
  await hideById('2:6757');
  var admBusca = await getNode('2:6751');
  if (admBusca) admBusca.resize(886, 44);
  await setTextById('2:6753', 'Pesquisar música por título (RF05)...', 'autoWidth');
  var admFiltroGenero = await getNode('2:6754');
  if (admFiltroGenero) admFiltroGenero.x = 898;

  // Cabeçalho da tabela: remove Álbum e coluna STATUS
  await setTextById('2:6764', 'MÚSICA E ARTISTA', 'autoWidth');
  var colAno = await setTextById('2:6766', 'LANÇAMENTO', 'autoWidth');
  if (colAno) colAno.x = 700;
  var colDur = await setTextById('2:6767', 'DURAÇÃO', 'autoWidth');
  if (colDur) colDur.x = 830;
  await hideById('2:6768'); // STATUS header

  var songRowsSpec = [
    { subId: '2:6777', genId: '2:6778', yrId: '2:6779', durId: '2:6780', stId: '2:6781', sub: 'Lumen Vale · Cadastrada por Marina', gen: 'Maid Core', yr: '14/03/2025' },
    { subId: '2:6795', genId: '2:6796', yrId: '2:6797', durId: '2:6798', stId: '2:6799', sub: 'Kora Sol · Cadastrada por Lucas', gen: 'Tribal', yr: '02/11/2024' },
    { subId: '2:6813', genId: '2:6814', yrId: '2:6815', durId: '2:6816', stId: '2:6817', sub: 'Nexo Norte · Cadastrada por Marina', gen: 'Sertanejo', yr: '19/08/2024' },
    { subId: '2:6831', genId: '2:6832', yrId: '2:6833', durId: '2:6834', stId: '2:6835', sub: 'Clara Vésper · Cadastrada por Helena', gen: 'MPB', yr: '05/01/2025' },
    { subId: '2:6849', genId: '2:6850', yrId: '2:6851', durId: '2:6852', stId: '2:6853', sub: 'Deriva Azul · Cadastrada por Rafael', gen: 'Ambiente', yr: '11/05/2023' },
    { subId: '2:6867', genId: '2:6868', yrId: '2:6869', durId: '2:6870', stId: '2:6871', sub: 'Vetor Sul · Cadastrada por Marina', gen: 'Eletrônica', yr: '28/02/2025' }
  ];
  for (var s = 0; s < songRowsSpec.length; s++) {
    var sr = songRowsSpec[s];
    await setTextById(sr.subId, sr.sub, 'autoWidth');
    await setTextById(sr.genId, sr.gen, 'autoWidth');
    var yNode = await setTextById(sr.yrId, sr.yr, 'autoWidth');
    if (yNode) yNode.x = 700;
    var dNode = await getNode(sr.durId);
    if (dNode) dNode.x = 830;
    await hideById(sr.stId);
  }

  await setTextById('2:6889', 'Admin exclui qualquer música; Usuário exclui apenas as próprias', 'autoWidth');

  // --------------------------------------------------------------------------
  // 5) Cadastro e edição de música (2:6892) — Campos exatos de Musica e MusicaGenero
  // --------------------------------------------------------------------------
  sendStatus('Ajustando tela 5/8: Cadastro e edição de música (RF08–RF09)...', false);
  var formMenu = await getNode('2:6898');
  var formNota = await getNode('2:6906');
  await expandSidebarMenu(formMenu, ['Gêneros', 'Meu perfil'], formNota, 360);
  await setTextById('2:6913', 'Ir para catálogo · Sair', 'autoWidth');

  await setTextById(
    '2:6936',
    'Preencha os dados da tabela Musica (RF08, RF09 · RN05 a RN08). Disponível para Admin e Usuário.',
    'preserveWidth'
  );
  await hideById('2:6937'); // Badge "Publicada"
  await hideById('2:6949'); // Card "Visibilidade / Publicar imediatamente"

  await setTextById('2:6943', 'Capa da música', 'autoWidth');
  await setTextById('2:6947', 'Enviar capa', 'autoWidth');
  await setTextById('2:6948', 'Campo ImagemCapa (VARCHAR MAX). Opcional.', 'preserveWidth');

  await setTextById('2:6959', 'Dados da tabela Musica', 'autoWidth');
  await setTextById('2:6962', 'Título da música (Titulo) *', 'autoWidth');
  await setTextById('2:6967', 'Artista (Artista) *', 'autoWidth');

  // Substitui "Álbum *" por "URL da Capa (ImagemCapa)"
  await setTextById('2:6971', 'URL da Capa (ImagemCapa)', 'autoWidth');
  await setTextById('2:6973', 'https://cdn.ourmusic.com/capas/cidade-submersa.jpg', 'autoWidth');

  // Classificação: Gênero(s), Data de lançamento (1888–atual), Duração (> 0)
  await setTextById('2:6976', 'Gênero(s) *', 'autoWidth');
  await setTextById('2:6978', 'Maid Core', 'autoWidth');
  await setTextById('2:6981', 'Data de lançamento *', 'autoWidth');
  await setTextById('2:6983', '14/03/2025', 'autoWidth');
  await setTextById('2:6985', 'Duração (seg) *', 'autoWidth');
  await setTextById('2:6987', '228 (03:48)', 'autoWidth');
  await setTextById('2:6988', 'RN06: Maior que zero (Duracao > 0)', 'autoWidth');

  // Descrição -> Comentário (Comentario VARCHAR(255))
  await setTextById('2:6990', 'Comentário (Comentario — até 255 caracteres)', 'autoWidth');
  await setTextById(
    '2:6992',
    'Faixa com sintetizadores analógicos e atmosfera noturna cadastrada no acervo OurMusic.',
    'preserveWidth'
  );
  await setTextById('2:6993', 'Campo opcional Comentario VARCHAR(255) da tabela Musica.', 'preserveWidth');

  // Créditos e lançamento -> Letra da música (Letra VARCHAR(MAX)), removendo Composição, Idioma e Gravadora
  await setTextById('2:6995', 'Letra da música (Letra)', 'autoWidth');
  await hideById('2:7000'); // Esconde linha de Data duplicada, Idioma e Gravadora
  await setTextById('2:6997', 'Letra completa da música (Letra — VARCHAR MAX)', 'autoWidth');

  var letraField = await getNode('2:6996');
  if (letraField) letraField.resize(700, 168);
  var letraInput = await getNode('2:6998');
  if (letraInput) letraInput.resize(700, 140);
  await setTextById(
    '2:6999',
    'Luzes da avenida refletem no asfalto molhado\nO pulso acelera enquanto a cidade adormece\nEntre ecos e frequências encontramos nosso lugar...',
    'preserveWidth'
  );

  await setTextById(
    '2:7025',
    'RN05 a RN08: Título, Artista, Data de lançamento (1888 até o ano atual) e Duração (> 0) são obrigatórios.',
    'autoWidth'
  );

  // --------------------------------------------------------------------------
  // 6) Gerenciamento de usuários (2:7027) — RF12 a RF15 / RN01, RN03, RN04
  // --------------------------------------------------------------------------
  sendStatus('Ajustando tela 6/8: Gerenciamento de usuários (RF12–RF15)...', false);
  var usrMenu = await getNode('2:7033');
  var usrNota = await getNode('2:7041');
  await expandSidebarMenu(usrMenu, ['Gêneros', 'Meu perfil'], usrNota, 360);
  await setTextById('2:7048', 'Ir para catálogo · Sair', 'autoWidth');

  await setTextById('2:7067', 'CONTROLE DE ACESSO · RF12 A RF15', 'autoWidth');
  await setTextById(
    '2:7069',
    'Cadastre usuários (RN01), edite dados e desative contas com exclusão lógica (Ativo = 0 · RN04).',
    'preserveWidth'
  );

  await setTextById('2:7087', 'Pesquisar usuário por nome ou e-mail único (RN03)...', 'autoWidth');
  await setTextById('2:7090', 'Tipo: Todos', 'autoWidth');
  await setTextById('2:7093', 'Status: Todos', 'autoWidth');

  // Cabeçalho da tabela: remove ÚLTIMO ACESSO e alinha colunas com a tabela Usuario
  await setTextById('2:7096', 'USUÁRIO (NOME E E-MAIL)', 'autoWidth');
  var uColTipo = await setTextById('2:7097', 'TIPO (TIPOUSUARIO)', 'autoWidth');
  if (uColTipo) uColTipo.x = 520;
  var uColStatus = await setTextById('2:7098', 'STATUS (ATIVO)', 'autoWidth');
  if (uColStatus) uColStatus.x = 740;
  await hideById('2:7099'); // ÚLTIMO ACESSO header
  var uColAcoes = await setTextById('2:7100', 'AÇÕES', 'autoWidth');
  if (uColAcoes) uColAcoes.x = 940;

  var userRowsSpec = [
    { roleFrame: '2:7109', roleText: '2:7111', statusFrame: '2:7113', lastAccess: '2:7116', actions: '2:7117', role: 'Usuario' },
    { roleFrame: '2:7129', roleText: '2:7131', statusFrame: '2:7133', lastAccess: '2:7136', actions: '2:7137', role: 'Admin' },
    { roleFrame: '2:7148', roleText: '2:7150', statusFrame: '2:7152', lastAccess: '2:7155', actions: '2:7156', role: 'Admin' },
    { roleFrame: '2:7167', roleText: '2:7169', statusFrame: '2:7171', lastAccess: '2:7174', actions: '2:7175', role: 'Usuario' },
    { roleFrame: '2:7186', roleText: '2:7188', statusFrame: '2:7190', lastAccess: '2:7193', actions: '2:7194', role: 'Usuario' }
  ];
  for (var u = 0; u < userRowsSpec.length; u++) {
    var ur = userRowsSpec[u];
    var rf = await getNode(ur.roleFrame);
    if (rf) rf.x = 520;
    await setTextById(ur.roleText, ur.role, 'autoWidth');
    var sf = await getNode(ur.statusFrame);
    if (sf) sf.x = 740;
    await hideById(ur.lastAccess);
    var af = await getNode(ur.actions);
    if (af) af.x = 940;
  }

  // Guia de permissões atualizado conforme escolha do usuário e RF01–RF15
  await setTextById('2:7211', 'Usuario (Comum)', 'autoWidth');
  await setTextById(
    '2:7212',
    'Acessa o catálogo, filtra por gênero, busca por título, cadastra músicas, exclui apenas as próprias músicas e edita seu perfil (RN09).',
    'preserveWidth'
  );
  await setTextById('2:7216', 'Admin (Administrador)', 'autoWidth');
  await setTextById(
    '2:7217',
    'Possui todas as permissões: gerencia todas as músicas, cadastra/edita gêneros (RF11) e gerencia contas de usuários (RF12 a RF15).',
    'preserveWidth'
  );

  // --------------------------------------------------------------------------
  // 7) Catálogo mobile (2:7219) — RF03, RF04, RF05, RF07
  // --------------------------------------------------------------------------
  sendStatus('Ajustando tela 7/8: Catálogo mobile...', false);
  await setTextById('2:7243', 'Buscar música por título (RF05)...', 'autoWidth');
  await hideById('2:7253'); // Botão extra "Mais filtros"
  var mobGenres = await getNode('2:7244');
  await updateGenreChips(mobGenres, ['Todos', 'Maid Core', 'Tribal', 'Sertanejo']);
  await hideById('2:7260'); // Ordenação "Mais recentes"

  // Barra inferior mobile: Catálogo, Nova música, Meu perfil
  var mobTab1 = await setTextById('2:7295', 'Catálogo', 'autoWidth');
  if (mobTab1) mobTab1.x = Math.round((86 - mobTab1.width) / 2);
  var mobTab2 = await setTextById('2:7298', 'Nova música', 'autoWidth');
  if (mobTab2) mobTab2.x = Math.round((86 - mobTab2.width) / 2);
  var mobTab3 = await setTextById('2:7301', 'Meu perfil', 'autoWidth');
  if (mobTab3) mobTab3.x = Math.round((86 - mobTab3.width) / 2);

  // --------------------------------------------------------------------------
  // 8) Confirmação de exclusão de música (2:7303) — RF10
  // --------------------------------------------------------------------------
  sendStatus('Ajustando tela 8/8: Confirmação de exclusão (RF10)...', false);
  await setTextById(
    '2:7310',
    'Esta ação removerá a música e seus vínculos em MusicaGenero (Admin exclui qualquer música; Usuário exclui apenas as próprias).',
    'preserveWidth'
  );
  await setTextById('2:7315', 'Lumen Vale · Maid Core · 03:48', 'autoWidth');
}

figma.ui.onmessage = async function (msg) {
  if (msg.type === 'close') {
    figma.closePlugin();
    return;
  }
  if (msg.type === 'apply-ourmusic' || msg.type === 'apply-changes') {
    try {
      if (typeof figma.currentPage.loadAsync === 'function') {
        await figma.currentPage.loadAsync();
      }
      sendStatus('Carregando fontes do documento...', false);
      await preloadAllPageFonts();

      // 1. Cria os 4 novos frames clonando a estrutura original
      await createNewFramesFromClones();

      // 2. Ajusta os 8 frames existentes
      await updateExistingFrames();

      figma.notify('✅ OurMusic: 8 telas ajustadas e 4 novas telas criadas com sucesso!');
      figma.ui.postMessage({
        type: 'done',
        text: 'Concluído! 8 telas atualizadas e 4 novas telas criadas no canvas.',
        message: 'Concluído! 8 telas atualizadas e 4 novas telas criadas no canvas.'
      });
    } catch (err) {
      var errMsg = err && err.message ? err.message : String(err);
      figma.notify('❌ Erro: ' + errMsg, { error: true });
      sendStatus(errMsg, true);
    }
  }
};
