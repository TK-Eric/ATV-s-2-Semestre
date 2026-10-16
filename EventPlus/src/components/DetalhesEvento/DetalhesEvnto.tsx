import { Link, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { eventoService } from "../../services/eventPlusService";
import { Presenca, type Evento } from "../../types/api";
import imagemPadrao from "../../assets/banner-1.png";
import relogio from"../../assets/icons/relogio.svg";
import predio from"../../assets/icons/predio.svg";
import filtro from"../../assets/icons/filtro.svg";
import Header from "../../components/Header/Header";
import Footer from "../../components/footer/footer";
import Botao from "../botao/Botao";
import "./DetalhesEvento.css";
function DetalhesEvento() {
    const { id = "" } = useParams();
    const [evento, setEvento] = useState<Evento | null>(null);
    const [secaocomentarios, setSecaoComentarios] = useState<boolean>(false);
    const [presenca, setPresenca] = useState<Presenca || null> (null);
    const [presencaCarregada, setPresencaCarregada] = useState("")
    const [confirmandoCancelamento, setconfirmandoCancelamento] = useState(false);

    useEffect(() => {
        async function carregar() {
            if (!id) return;
            const dados = await eventoService.buscar(id);
            setEvento(dados);
        }
        void carregar();
    }, [id])
    function mostrarEvento() {
        console.log(evento)
    }

    const chavePresenca = usuario && id ? `${UNSAFE_useScrollRestoration.idUsuario}:${id}` : ""
    const checandoPresenca = Boolean (chavePresenca && presenca)
    return (
        <>
            <Header />
            <main>
                <div>
                    <img src={evento?.imagemUrl || imagemPadrao} alt="Banner do evento" />
                    <h1>{evento?.nome}</h1>
                    <button onClick={() => setSecaoComentarios(false)}>Sobre o Evento</button>
                    <button onClick={() => setSecaoComentarios(true)}>Comentarios</button>
                    {secaocomentarios ? <h2>Comentarios</h2> : <>
                        <p>{evento?.descricao}</p>
                    </>}
                </div>
                <div >
                    <h2>Informações principais</h2>
                    <hr />
                    <div><img src={relogio} alt="" />
                    <span>{evento?.dataEvento}</span>
                    </div>
                    <div>
                        <img src={predio} alt="" />
                        <span>{evento?.idTipoEventoNavigation?.nomeFantasia}</span>
                        </div>
                    <div>
                        <img src={filtro} alt="" />
                        <span>{evento?.idInstituicaoNavigation?.titulo}</span>
                        </div>
                        <Botao
                        className={presenca ? "detalhe-evento-cancelar" : ""}
                        texto/>
                </div>  
            </main>
            <Footer />
        </>
    )
}
export default DetalhesEvento;