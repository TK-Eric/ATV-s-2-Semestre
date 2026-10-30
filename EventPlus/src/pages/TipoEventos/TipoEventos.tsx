import { useCallback, useEffect, useState, type FormEvent } from "react";
import Header from "../../components/Header/Header";
import Footer from "../../components/Footer/footer";
import Campo from "../../components/Campo/Campo";
import Botao from "../../components/Botao/Botao";
import { tipoEventoService } from "../../services/eventPlusService";
import { getApiErrorMessage } from "../../services/api";
import type { TipoEvento } from "../../types/api";

import lapis from "../../assets/icons/lapis.svg";
import lixeira from "../../assets/icons/lixeira.svg";
import visaoImg from "../../assets/visao-img.png";

import "./TipoEventos.css";

function TipoEventos() {
    const [tiposEventos, setTiposEventos] = useState<TipoEvento[]>([]);
    const [titulo, setTitulo] = useState("");
    const [idEmEdicao, setIdEmEdicao] = useState<string | null>(null);
    const [carregando, setCarregando] = useState(true);
    const [enviando, setEnviando] = useState(false);
    const [mensagem, setMensagem] = useState("");
    const [tipoMensagem, setTipoMensagem] = useState<"sucesso" | "erro">("sucesso");

    const carregarTiposEventos = useCallback(async () => {
        try {
            const dados = await tipoEventoService.listar();
            setTiposEventos(dados);
        } catch (error) {
            setTipoMensagem("erro");
            setMensagem(getApiErrorMessage(error));
        } finally {
            setCarregando(false);
        }
    }, []);

    useEffect(() => {
        async function carregarDados() {
            try {
                const dados = await tipoEventoService.listar();
                setTiposEventos(dados);
            } catch (error) {
                setTipoMensagem("erro");
                setMensagem(getApiErrorMessage(error));
            } finally {
                setCarregando(false);
            }
        }

        carregarDados();
    }, []);

    function cancelarEdicao() {
        setIdEmEdicao(null);
        setTitulo("");
    }

    function iniciarEdicao(tipo: TipoEvento) {
        setIdEmEdicao(tipo.idTipoEvento);
        setTitulo(tipo.titulo || tipo.tituloTipoEvento || "");
        setMensagem("");
    }

    async function salvarTipoEvento(e: FormEvent<HTMLFormElement>) {
        e.preventDefault();
        const tituloLimpo = titulo.trim();

        if (!tituloLimpo) {
            setTipoMensagem("erro");
            setMensagem("Informe o título do tipo de evento.");
            return;
        }

        setEnviando(true);
        setMensagem("");

        try {
            if (idEmEdicao) {
                await tipoEventoService.atualizar(idEmEdicao, tituloLimpo);
                setTipoMensagem("sucesso");
                setMensagem("Tipo de evento atualizado com sucesso!");
            } else {
                await tipoEventoService.cadastrar(tituloLimpo);
                setTipoMensagem("sucesso");
                setMensagem("Tipo de evento cadastrado com sucesso!");
            }

            cancelarEdicao();
            await carregarTiposEventos();
        } catch (error) {
            setTipoMensagem("erro");
            setMensagem(getApiErrorMessage(error));
        } finally {
            setEnviando(false);
        }
    }

    async function excluirTipoEvento(tipo: TipoEvento) {
        const nomeExibicao = tipo.titulo || tipo.tituloTipoEvento || "este tipo de evento";
        if (!window.confirm(`Deseja realmente excluir "${nomeExibicao}"?`)) {
            return;
        }

        setMensagem("");
        try {
            await tipoEventoService.excluir(tipo.idTipoEvento);
            if (idEmEdicao === tipo.idTipoEvento) {
                cancelarEdicao();
            }
            setTipoMensagem("sucesso");
            setMensagem("Tipo de evento excluído com sucesso!");
            await carregarTiposEventos();
        } catch (error) {
            setTipoMensagem("erro");
            setMensagem(getApiErrorMessage(error));
        }
    }

    return (
        <div className="pagina-tipo-eventos">
            <Header />

            <main className="tipo-eventos-main">
                <section className="tipo-eventos-cadastro">
                    <div className="tipo-eventos-container">
                        <h1 className="tipo-eventos-titulo">
                            {idEmEdicao ? "EDITAR TIPO DE EVENTO" : "CADASTRO DE TIPO DE EVENTOS"}
                        </h1>

                        {mensagem && (
                            <p
                                className={`tipo-eventos-alerta tipo-eventos-alerta--${tipoMensagem}`}
                            >
                                {mensagem}
                            </p>
                        )}

                        <div className="tipo-eventos-conteudo">
                            <div className="tipo-eventos-ilustracao">
                                <img src={visaoImg} alt="Cadastro de Tipo de Eventos" />
                            </div>

                            <form className="tipo-eventos-form" onSubmit={salvarTipoEvento}>
                                <div className="tipo-eventos-campo-grupo">
                                    <label htmlFor="titulo-tipo-evento">Título</label>
                                    <Campo
                                        id="titulo-tipo-evento"
                                        name="titulo"
                                        placeholder="Título"
                                        value={titulo}
                                        onChange={(e) => setTitulo(e.target.value)}
                                        maxLength={100}
                                        required
                                    />
                                </div>

                                <div className="tipo-eventos-acoes">
                                    <Botao
                                        type="submit"
                                        className="tipo-eventos-botao-salvar"
                                        texto={
                                            enviando
                                                ? "Salvando..."
                                                : idEmEdicao
                                                ? "Atualizar"
                                                : "Cadastrar"
                                        }
                                        disabled={enviando}
                                    />

                                    {idEmEdicao && (
                                        <button
                                            type="button"
                                            className="tipo-eventos-botao-cancelar"
                                            onClick={cancelarEdicao}
                                        >
                                            Cancelar
                                        </button>
                                    )}
                                </div>
                            </form>
                        </div>
                    </div>
                </section>

                <section className="lista-tipos">
                    <div className="lista-tipos-conteudo">
                        <h2>LISTA DE TIPO DE EVENTOS</h2>

                        <div className="lista-tipos-tabela-wrapper">
                            <table className="lista-tipos-tabela">
                                <thead>
                                    <tr>
                                        <th>Título</th>
                                        <th className="lista-tipos-col-acao">Editar</th>
                                        <th className="lista-tipos-col-acao">Excluir</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {carregando ? (
                                        <tr>
                                            <td colSpan={3} className="lista-tipos-tabela-vazia">
                                                Carregando...
                                            </td>
                                        </tr>
                                    ) : tiposEventos.length === 0 ? (
                                        <tr>
                                            <td colSpan={3} className="lista-tipos-tabela-vazia">
                                                Nenhum tipo de evento cadastrado.
                                            </td>
                                        </tr>
                                    ) : (
                                        tiposEventos.map((tipo) => {
                                            const nomeTipo =
                                                tipo.titulo || tipo.tituloTipoEvento || "Sem título";
                                            return (
                                                <tr
                                                    key={tipo.idTipoEvento}
                                                    className={
                                                        idEmEdicao === tipo.idTipoEvento
                                                            ? "lista-tipos-linha-ativa"
                                                            : ""
                                                    }
                                                >
                                                    <td>{nomeTipo}</td>
                                                    <td className="lista-tipos-col-acao">
                                                        <button
                                                            type="button"
                                                            className="lista-tipos-acao"
                                                            onClick={() => iniciarEdicao(tipo)}
                                                        >
                                                            <img src={lapis} alt="Editar" />
                                                        </button>
                                                    </td>
                                                    <td className="lista-tipos-col-acao">
                                                        <button
                                                            type="button"
                                                            className="lista-tipos-acao"
                                                            onClick={() => excluirTipoEvento(tipo)}
                                                        >
                                                            <img src={lixeira} alt="Excluir" />
                                                        </button>
                                                    </td>
                                                </tr>
                                            );
                                        })
                                    )}
                                </tbody>
                            </table>
                        </div>
                    </div>
                </section>
            </main>

            <Footer />
        </div>
    );
}

export default TipoEventos;
