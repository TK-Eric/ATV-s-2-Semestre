import { Link, useParams } from "react-router-dom"

import Header from "../Header/Header"
import Footer from "../footer/footer"

import "./DetalhesEvento.css"

function DetalhesEvento() {
const {id = ""} = useParams();

return(
    <>
    <Header/>
    <main>
        <Link to="/eventos"> Voltar aos Eventos</Link>
        <h1>Detalhes do evento</h1>
        <p>ID recebido: {id}</p>
    </main>
    <footer/>
    </>
)

}

export default DetalhesEvento;