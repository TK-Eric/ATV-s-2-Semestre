console.log("JavaScript Carregado!")

const titulo = document.getElementById("titulo")
console.log(titulo.textContent);
titulo.textContent = "JavaScript alterou este titulo"

const descricao = document.getElementById("descricao")
console.log(descricao.textContent)
descricao.textContent ="o conteudo foi alterado pelo DOM"

const titulo1 = document.querySelector("#titulo")
    console.log(titulo1)

    const mensagem = document.querySelector(".mensagem")
    console.log(mensagem)

    const botao = document.querySelector("button")
   
const mensagems = document.querySelectorAll(".mensagems")

console.log(mensagems)

console.log(mensagems[0])
console.log(mensagems[1])

console.log(mensagems.length)

for(let cont =0; cont < mensagems.length; cont++)
{
    console.log(mensagems[cont].textContent)
}

titulo.style.color = "blue"
titulo.style.backgroundColor = "lightgray"
titulo.style.padding = "20px"

descricao.style.fontSize

titulo.classList.add("destque")
titulo.classList.remove("destaque")
titulo.classList.toggle("destaque")

console.log(titulo.classList.contains("destaque"))

function mostrarMensagem() {
console.log("o botao foi clicado")
}

function mostrarMensagem() {
titulo.textContent = "BOTAO CLICADOOOOOO UUHUUU"
}
function mostrarMensagem() {
titulo.classList.toggle("destaque");
}

function mostrarNome() {
const inputNome = document.querySelector(`#nome`)
const resultado = document.querySelector(`#resultado`)
resultado.textContent = `Ola, ${inputNome.value}!`
}


