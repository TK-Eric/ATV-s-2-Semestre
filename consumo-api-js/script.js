async function carregarPokemons() {
    const lista = document.querySelector("#listaPokemon");
    lista.textContent = "Iniciando a busca...";

    try {
        const resposta = await fetch("https://pokeapi.co/api/v2/pokemon/charizard");

        if (!resposta.ok) {
            throw new Error(`Erro na requisição: ${resposta.status}`);
        }

        const pokemon = await resposta.json();

        lista.textContent = "";

        const cartao = document.createElement("article");

        const name = document.createElement("h2");
        name.textContent = pokemon.name;

        const weight = document.createElement("p");
        weight.textContent = `Peso: ${pokemon.weight}`;

        const height = document.createElement("p");
        height.textContent = `Altura: ${pokemon.height}`;

        const base_experience = document.createElement("p");
        base_experience.textContent = `Experiência base: ${pokemon.base_experience}`;

        const order = document.createElement("p");
        order.textContent = `Ordem: ${pokemon.order}`;

        cartao.appendChild(name);
        cartao.appendChild(weight);
        cartao.appendChild(height);
        cartao.appendChild(base_experience);
        cartao.appendChild(order);

        lista.appendChild(cartao);
    } catch (erro) {
        lista.textContent = "Falha ao carregar os dados do Pokémon.";
        console.error(erro);
    }
} 