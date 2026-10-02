function adicionarTarefa() {

    // • const: É uma palavra-chave usada para criar uma variável cujo valor não pode ser reatribuído ao longo do código. Ou seja,
    //          a variável input sempre apontará para esse mesmo elemento da página.
    // • input: É o nome da variável que você escolheu. Você poderia dar qualquer outro nome, como campoTexto ou entradaUsuario.
    // • document: É o objeto que representa toda a sua página web (o DOM). É através dele que o JavaScript consegue acessar o HTML.
    // • .querySelector(): É um método (uma função) que busca no HTML o primeiro elemento que coincida com o seletor que você colocar entre parênteses.
    // • "#novaTarefa": É o seletor CSS. O símbolo de hashtag (#) significa que você está buscando um elemento pelo seu ID. Portanto,
    //                  ele está procurando um elemento no HTML que tenha id="novaTarefa" (provavelmente uma tag <input>).

    const input = document.querySelector("#novaTarefa");
    const mensagem = document.querySelector("#mensagem");
    const listaTarefas = document.querySelector("#listarTarefas");
    // • input: É a variável que você criou antes (que aponta para a caixinha de texto do HTML).
    // • .value: É a propriedade que guarda exatamente o que está escrito dentro dessa caixinha naquele instante.
    // • .trim(): É uma função que remove os espaços em branco do início e do fim do texto.
    //  Ela serve para evitar que o usuário engane o sistema digitando apenas espaços (ex: "  " vira "").

    if (input.value.trim() === "") {
        mensagem.textContent = "Digite uma tarefa.";
        return;
    } else {
        mensagem.textContent = "";
    }
    //     Se não existisse o else, a frase "Digite uma tarefa." continuaria travada na tela para sempre, mesmo você tendo digitado a tarefa certa!
    //     O else { mensagem.textContent = ""; } diz ao navegador: "Se o campo NÃO estiver vazio, apague qualquer aviso de erro que foi colocado lá antes".

    const tarefa = document.createElement("li");
    tarefa.textContent = input.value + " ";

    // • .value é a propriedade que espreme esse input e pega o texto exato que o usuário digitou dentro dele naquele momento.
    // • Se o usuário digitou "Comprar leite", input.value vira o texto "Comprar leite".

    // A linha tarefa.textContent = input.value + " "; define o texto que vai ficar dentro do seu <li>.
    // O + " " serve para concatenar (juntar) o texto digitado com um espaço em branco. Geralmente,
    // os programadores fazem isso porque logo em seguida vão criar um botão de "Excluir" ao lado do texto. 
    // Sem esse espaço em branco, o botão de excluir ficaria colado na palavra, assim: Comprar leite[X]. 
    // Com o espaço, fica mais organizado: Comprar leite [X].

    const botaoExcluir = document.createElement("button");
    botaoExcluir.textContent = "apagar";

    botaoExcluir.onclick = function () {
        tarefa.remove();
    };

    tarefa.appendChild(botaoExcluir);

    listaTarefas.appendChild(tarefa);

    input.value = "";

//     • Quando você usa o sinal de igual sozinho(=), você está fazendo uma atribuição(mudando o valor).
//     • As aspas vazias "" significam "texto nenhum".
//     • Ao fazer input.value = "";, você está limpando o campo de texto.
//         Fazemos isso por pura elegância e experiência do usuário: depois que a pessoa clica em "Adicionar" e a tarefa vai para a lista,
//         a caixinha de texto limpa automaticamente para ela poder digitar a próxima tarefa sem ter que apagar o que escreveu antes.

}
