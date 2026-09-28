console.log("Java Funcionando")
console.log("Liso")

const nome = "Eric";
const idade = 20;
const cidade = "Sao Paulo"
const profissao = "Estudante";


console.log(nome)
console.log(idade)
console.log(profissao)
console.log(cidade)


console.log(`meu Nome é ${nome} e tenho ${idade} anos atuo como ${profissao} e moo na cidade de ${cidade}.`)

if(idade >= 18){
    console.log("Maior de idade")
}else{
    console.log("Menor de idade")
}



const Numero1 = 3
const Numero2 = 6

console.log(`soma: ${Numero1 + Numero2}`)
console.log(`subtracao: ${Numero1 - Numero2}`)
console.log(`multi: ${Numero1 * Numero2}`)
console.log(`divisao: ${Numero1 / Numero2}`)

const Idade = 25;
const meses = 12;

console.log(`essa pessoa tem aproximadamente ${idade * meses} meses de vida`)

const Nota1 = 6
const Nota2 = 10
const Nota3 = 2

console.log (`Media da Nota:${Nota1 + Nota2 + Nota3 / 3}`)

const IdadeOFC = 26
if ( idadeOFC >= 18) console.log ("E maior de idade")
    else console.log ("menor de idade")

const Valor = 2

if (Valor >= 0) console.log("E positivo")
else if (Valor == 0) console.log ("E igual a 0")
else if (Valor <= 0) console.log("ele e negativo") 
    
const Numero = 5

if (Numero/2 == 0) console.log("é par")
    else ("é impar")



console.log(nomes)

for(let cont =0; cont < nomes.length; cont++){
    console.log(nomes[cont]);
}

function calcularMedia(nota1, nota2){
    return(nota1 + nota2) / 2
}

const mediaau = calcularMedia (8, 7);

console.log(media)

if(media <= 5) console.log("reprovado")
else if(media > 5, media < 6.9) console.log("Recuperacao")
    else console.log ("Aprovado")    


const numero1 = 3
const numero2 = 8
const numero3 = 1

 if (numero1 > numero2) {
            console.log("O maior número é: " + numero1);
        } else if (numero2 > numero1) {
            console.log("O maior número é: " + numero2);
        } else {
            console.log("Os dois números são iguais.");
        }

 if (numero1 > numero2 && numero1 > numero3) 
    {console.log("O maior número é: " + numero1);} 
 
 else if (numero2 > numero1 && numero2 > numero3) 
    {console.log("O maior número é: " + numero2);} 
 
 else if (numero3 > numero2 && numero3 > numero1) 
            {console.log("O maior número é: " + numero3);}

 const Produto = 200

        if (Produto > 100.00) {
            console.log (`com desconto${Produto * 0.10}`);
        }

const nomesPeople = [
    "Fernanda",
    "Bruno",
    "Alex",
    "Leonardo",
    "Eric"
]


const listaprodutos = [
    "1 Sabao",
    "500g 5Presunto",
    "200 mil Pepinos",
    "100L de Coca",
    "7 Atendentes"
]

const Caio = "Caio 19 Anos"
const Pedro = "Pedro 10 Anos"
const Angelo = "Angelo 99 Anos"

console.log(`${Caio}, ${Pedro}, ${Angelo}`)

const Numerionho = 4

console.log(`O dobro do ${Numerionho} é: ${Numerionho * 2} `)

const Jadeu1 = 3
const Jadeu2 = 87

console.log(`A soma é: ${Jadeu1} + ${Jadeu2} = ${Jadeu1 + Jadeu2}`)

console.log(`a media de ${Jadeu1} e ${Jadeu2} é: ${Jadeu1 + Jadeu2 / 2}`)

console.log (`Ola ${nome} Bem vindo!`)

const tabuada = [
    1, 2, 3, 4, 5, 6, 7, 8 ,9, 10
]

const tabuada20 = [
    1, 2, 3, 4, 5, 6, 7, 8 ,9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20
]

const adaubat = [
    10, 9, 8, 7, 6, 5, 4, 3, 2, 1, "Fim!"
]

const paresate50 = [
    2, 4, 6, 8, 1012, 14, 16, 18, 2022, 24, 26, 28, 3032, 34, 36, 38, 4042, 44, 46, 48, 50
]

const numerionhos = [
     1, 2, 3, 4, 5
]

const soma = Arrays.stream(numerionhos).sum();

        console.log("A soma é: " + soma);


 const notaus = [7.5, 8.0, 6.5, 9.0, 7.0];

        const media = Arrays.stream(notaus / 5);

        console.log("A média das notas é: " + media);        

        const Listas = [
            Nomer = "Bolo",
            Preco = 29,
            Quantidade = 30
        ]
        if (Quantidade <= 0) console.log("Indisponivel")

            const Boletin = [
                NomeAluno = "Eric",
                NotaPortugues = 9,
                NotaMatematica = 7,                
            ]
            
            const mEdiazinha = NotaMatematica + NotaMatematica / 2

            console.log(`Media do ${NomeAluno} é ${NotaPortugues + NotaMatematica / 2} `)
            
            if(mEdiazinha <= 5) console.log("reprovado")
            else if(mEdiazinha > 5, mEdiazinha < 6.9) console.log("Recuperacao")
            else console.log ("Aprovado") 