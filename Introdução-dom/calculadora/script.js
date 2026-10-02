function calcularResultado(operacao) {
    const input1 = document.querySelector("#Numero1");
    const input2 = document.querySelector("#Numero2");
    const listaResultado = document.querySelector("#listaResultado");
    const mensagem = document.querySelector("#mensagem");

    // Converte os valores para números
    const num1 = parseFloat(input1.value);
    const num2 = parseFloat(input2.value);

    // Valida se os campos foram preenchidos
    if (isNaN(num1) || isNaN(num2)) {
        if (mensagem) mensagem.textContent = "Digite os dois números!";
        return;
    }

    // Evita divisão por zero
    if (operacao === "/" && num2 === 0) {
        if (mensagem) mensagem.textContent = "Não dá para dividir por zero!";
        return;
    }

    if (mensagem) mensagem.textContent = "";

    // Cálculo usando IF simples
    let resultado;
    if (operacao === "+") resultado = num1 + num2;
    if (operacao === "-") resultado = num1 - num2;
    if (operacao === "*") resultado = num1 * num2;
    if (operacao === "/") resultado = num1 / num2;

    // Adiciona na lista
    const item = document.createElement("li");
    item.textContent = `${num1} ${operacao} ${num2} = ${resultado} `;

    // Botão apagar
    const botaoExcluir = document.createElement("button");
    botaoExcluir.textContent = "Apagar";
    botaoExcluir.onclick = () => item.remove();

    item.appendChild(botaoExcluir);
    listaResultado.appendChild(item);

    // Limpa os campos
    input1.value = "";
    input2.value = "";
}