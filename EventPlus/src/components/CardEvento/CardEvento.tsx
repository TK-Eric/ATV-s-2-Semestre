import "./CardEvento.css";
import imagemPadrao from "../../assets/banner-1.png";
import { Link } from "react-router-dom";

interface CardEventoProps {
    id: string;
    imagem?: string | null;
    categoria?: string;
    titulo: string;
    descricao: string;
}

function CardEvento({ id, imagem, categoria, titulo, descricao }: CardEventoProps) {
    return (
        <article className="home-evento-card">
            <img src={imagem || imagemPadrao} alt={`imagem do evento ${titulo}`} />
            <span>{categoria || "Evento"}</span>
            <div className="home-evento-card-info">
                <h3>{titulo}</h3>
                <p>{descricao}</p>
                <Link to={`/Eventos/${id}`} className="home-evento-card-info-botao">
                    Ver Evento
                </Link>
            </div>
        </article>
    ); 
}

export default CardEvento;