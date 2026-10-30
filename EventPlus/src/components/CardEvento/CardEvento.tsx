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
            <div className="home-evento-card-corpo">
                <span className="home-evento-card-tag">{categoria || "Categoria"}</span>
                <h3>{titulo}</h3>
                <p>{descricao}</p>
                <div className="home-evento-card-rodape">
                    <Link to={`/eventos/${id}`} className="home-evento-card-info-botao">
                        Ver Evento
                    </Link>
                </div>
            </div>
        </article>
    ); 
}

export default CardEvento;
