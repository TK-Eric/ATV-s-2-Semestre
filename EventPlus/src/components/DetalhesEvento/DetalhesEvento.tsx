import { Link, useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { eventoService, presencaService } from "../../services/eventPlusService";
import type { Presenca, Evento } from "../../types/api";
import { useAuth } from "../../context/Auth";

import imagemPadrao from "../../assets/banner-1.png";
import relogio from "../../assets/icons/relogio.svg";
import predio from "../../assets/icons/predio.svg";
import filtro from "../../assets/icons/filtro.svg";

import Header from "../Header/Header";
import Footer from "../Footer/footer";
import Botao from "../Botao/Botao";
import Modal from "../Modal/Modal";

import "./DetalhesEvento.css";

function DetalhesEvento() {
    const { id = "" } = useParams();
    const navigate = useNavigate();
    const { usuario, autenticado } = useAuth();

    const [evento, setEvento] = useState<Evento | null>(null);
    const [secaoComentarios, setSecaoComentarios] = useState<boolean>(false);
    const [presenca, setPresenca] = useState<Presenca | null>(null);
    const [carregandoPresenca, setCarregandoPresenca] = useState(false);
    const [enviandoInscricao, setEnviandoInscricao] = useState(false);
    const [confirmandoCancelamento, setConfirmandoCancelamento] = useState(false);

    useEffect(() => {
        async function carregar() {
            if (!id) return;
            try {
                const dados = await eventoService.buscar(id);
                setEvento(dados);
            } catch (error) {
                console.error("Erro ao carregar detalhes do evento:", error);
            }
        }

        void carregar();
    }, [id]);

    useEffect(() => {
        async function verificarPresenca() {
            if (!usuario?.idUsuario || !id) return;
            setCarregandoPresenca(true);
            try {
                const presencas = await presencaService.listarPorUsuario(usuario.idUsuario);
                const atual = presencas.find((p) => p.idEvento === id && p.situacao);
                setPresenca(atual ?? null);
            } catch {
                // Silêncio em caso de erro ao carregar presenças
            } finally {
                setCarregandoPresenca(false);
            }
        }

        void verificarPresenca();
    }, [id, usuario?.idUsuario]);

    async function handleAcaoInscricao() {
        if (!autenticado) {
            navigate("/login", { state: { from: `/eventos/${id}` } });
            return;
        }

        if (presenca) {
            setConfirmandoCancelamento(true);
            return;
        }

        try {
            setEnviandoInscricao(true);
            const novaPresenca = await presencaService.cadastrar({
                idEvento: id,
                idUsuario: usuario?.idUsuario ?? null,
                situacao: true,
            });
            setPresenca(novaPresenca);
        } catch (error) {
            console.error("Erro ao se inscrever no evento:", error);
        } finally {
            setEnviandoInscricao(false);
        }
    }

    async function handleConfirmarCancelamento() {
        if (!presenca) return;
        try {
            setEnviandoInscricao(true);
            await presencaService.excluir(presenca.idPresenca);
            setPresenca(null);
            setConfirmandoCancelamento(false);
        } catch (error) {
            console.error("Erro ao cancelar inscrição:", error);
        } finally {
            setEnviandoInscricao(false);
        }
    }

    const textoBotao = carregandoPresenca
        ? "Verificando inscrição..."
        : enviandoInscricao
        ? "Processando..."
        : presenca
        ? "Cancelar Inscrição"
        : "Se inscrever";

    const dataFormatada = evento?.dataEvento
        ? new Date(evento.dataEvento).toLocaleDateString("pt-BR")
        : "A definir";

    const categoriaEvento =
        evento?.idTipoEventoNavigation?.titulo ||
        evento?.idTipoEventoNavigation?.tituloTipoEvento ||
        "Tecnologia & Inovação";

    return (
        <div className="pagina-detalhes-evento">
            <Header />

            <main className="detalhes-evento-container">
                <div className="detalhes-evento-principal">
                    <Link to="/eventos" className="detalhes-evento-voltar">
                        ← Voltar para Eventos
                    </Link>

                    <div className="detalhes-evento-banner">
                        <img
                            src={evento?.imagemUrl || imagemPadrao}
                            alt={evento?.nomeEvento || "Banner do evento"}
                        />
                    </div>

                    <h1 className="detalhes-evento-titulo">
                        {evento?.nomeEvento || "Detalhes do Evento"}
                    </h1>

                    <div className="detalhes-evento-abas">
                        <button
                            type="button"
                            className={`detalhes-evento-aba ${!secaoComentarios ? "ativa" : ""}`}
                            onClick={() => setSecaoComentarios(false)}
                        >
                            Sobre o Evento
                        </button>
                        <button
                            type="button"
                            className={`detalhes-evento-aba ${secaoComentarios ? "ativa" : ""}`}
                            onClick={() => setSecaoComentarios(true)}
                        >
                            Comentários
                        </button>
                    </div>

                    <div className="detalhes-evento-conteudo-aba">
                        {secaoComentarios ? (
                            <div className="detalhes-evento-comentarios">
                                <h2>Comentários do Evento</h2>
                                <p>
                                    Participe do evento e compartilhe seu feedback com os
                                    organizadores e participantes.
                                </p>
                            </div>
                        ) : (
                            <p className="detalhes-evento-descricao">
                                {evento?.descricao ||
                                    "Confira as informações completas sobre a programação, palestrantes e atividades deste evento."}
                            </p>
                        )}
                    </div>
                </div>

                <aside className="detalhes-evento-card-info">
                    <h2>Informações principais</h2>
                    <hr />

                    <div className="detalhes-evento-info-lista">
                        <div className="detalhes-evento-info-item">
                            <img src={relogio} alt="" />
                            <div>
                                <strong>Data</strong>
                                <span>{dataFormatada}</span>
                            </div>
                        </div>

                        <div className="detalhes-evento-info-item">
                            <img src={predio} alt="" />
                            <div>
                                <strong>Instituição</strong>
                                <span>
                                    {evento?.idInstituicaoNavigation?.nomeFantasia ||
                                        "SENAI São Caetano do Sul"}
                                </span>
                            </div>
                        </div>

                        <div className="detalhes-evento-info-item">
                            <img src={filtro} alt="" />
                            <div>
                                <strong>Categoria</strong>
                                <span>{categoriaEvento}</span>
                            </div>
                        </div>
                    </div>

                    <Botao
                        className={`detalhes-evento-botao-acao ${
                            presenca ? "detalhes-evento-cancelar" : ""
                        }`}
                        texto={textoBotao}
                        type="button"
                        disabled={carregandoPresenca || enviandoInscricao}
                        onClick={() => void handleAcaoInscricao()}
                    />
                </aside>
            </main>

            <Modal
                aberto={confirmandoCancelamento}
                titulo="Confirmar cancelamento"
                textoConfirmar="Sim, cancelar"
                textoCancelar="Voltar"
                carregando={enviandoInscricao}
                aoConfirmar={() => void handleConfirmarCancelamento()}
                aoFechar={() => setConfirmandoCancelamento(false)}
            >
                <p>Tem certeza de que deseja cancelar sua inscrição neste evento?</p>
            </Modal>

            <Footer />
        </div>
    );
}

export default DetalhesEvento;
