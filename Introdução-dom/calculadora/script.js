function calcularResultado(operacao) {
    const input1 = document.querySelector("#Numero1");
    const input2 = document.querySelector("#Numero2");
    const listaResultado = document.querySelector("#listaResultado");
    const mensagem = document.querySelector("#mensagem");

    const num1 = parseFloat(input1.value);
    const num2 = parseFloat(input2.value);

  if (num1 === "" || num2 === "") {
    mensagem.textContent = "FAZ DIREITO C$@%&%O!";
    return;
}

mensagem.textContent = "";

if (operacao === "+") resultado = num1 + num2;
else if (operacao === "-") resultado = num1 - num2;
else if (operacao === "*") resultado = num1 * num2;
else if (operacao === "/") resultado = num1 / num2;

    // Adiciona na lista
    const item = document.createElement("li");
    item.textContent = `${num1} ${operacao} ${num2} = ${resultado} `;

    // Botão apagar
    const botaoExcluir = document.createElement("button");
    botaoExcluir.textContent = "oBLITERAR FEZES NO ELEVADOR";
    botaoExcluir.onclick = () => item.remove();
    
    item.appendChild(botaoExcluir);
    listaResultado.appendChild(item);

    // Limpa os campos
    input1.value = "";
    input2.value = "";
}
