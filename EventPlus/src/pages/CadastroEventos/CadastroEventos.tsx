import { useCallback, useEffect, useRef, useState, type FormEvent } from "react";
import Footer from "../../components/Footer/footer";
import Header from "../../components/Header/Header";
import Botao from "../../components/Botao/Botao";
import type { Instituicao, Evento, TipoEvento } from "../../types/api";
import {
    eventoService,
    tipoEventoService,
    instituicaoService,
} from "../../services/eventPlusService";
import { getApiErrorMessage } from "../../services/api";

import lapis from "../../assets/icons/lapis.svg";
import lixeira from "../../assets/icons/lixeira.svg";
import visaoImg from "../../assets/visao-img.png";

import "./CadastroEventos.css";

interface FormularioEvento {
    nome: string;
    dataEvento: string;
    descricao: string;
    idTipoEvento: string;
    idInstituicao: string;
    arquivoImagem: File | null;
}

const inicial: FormularioEvento = {
    nome: "",
    dataEvento: "",
    descricao: "",
    idTipoEvento: "",
    idInstituicao: "",
    arquivoImagem: null,
};

function CadastroEvento() {
    const fotoInputRef = useRef<HTMLInputElement>(null);
    const [formulario, setFormulario] = useState<FormularioEvento>(inicial);
    const [tipos, setTipos] = useState<TipoEvento[]>([]);
    const [instituicoes, setInstituicoes] = useState<Instituicao[]>([]);
    const [eventos, setEventos] = useState<Evento[]>([]);
    const [idEdicao, setIdEdicao] = useState<string | null>(null);
    const [carregando, setCarregando] = useState(true);
    const [enviando, setEnviando] = useState(false);
    const [mensagem, setMensagem] = useState("");
    const [tipoMensagem, setTipoMensagem] = useState<"sucesso" | "erro">("sucesso");

    const carregar = useCallback(async () => {
        try {
            const [tiposApi, instituicoesApi, eventosApi] = await Promise.all([
                tipoEventoService.listar(),
                instituicaoService.listar(),
                eventoService.listar(),
            ]);

            setTipos(tiposApi);
            setInstituicoes(instituicoesApi);
            setEventos(eventosApi);
            setFormulario((value) => ({
                ...value,
                idTipoEvento: value.idTipoEvento || tiposApi[0]?.idTipoEvento || "",
                idInstituicao: value.idInstituicao || instituicoesApi[0]?.idInstituicao || "",
            }));
        } catch (error) {
            setTipoMensagem("erro");
            setMensagem(getApiErrorMessage(error));
        } finally {
            setCarregando(false);
        }
    }, []);

    useEffect(() => {
        let ativo = true;

        async function carregarInicial() {
            try {
                const [tiposApi, instituicoesApi, eventosApi] = await Promise.all([
                    tipoEventoService.listar(),
                    instituicaoService.listar(),
                    eventoService.listar(),
                ]);

                if (!ativo) return;

                setTipos(tiposApi);
                setInstituicoes(instituicoesApi);
                setEventos(eventosApi);
                setFormulario((value) => ({
                    ...value,
                    idTipoEvento: value.idTipoEvento || tiposApi[0]?.idTipoEvento || "",
                    idInstituicao: value.idInstituicao || instituicoesApi[0]?.idInstituicao || "",
                }));
            } catch (error) {
                if (ativo) {
                    setTipoMensagem("erro");
                    setMensagem(getApiErrorMessage(error));
                }
            } finally {
                if (ativo) {
                    setCarregando(false);
                }
            }
        }

        void carregarInicial();

        return () => {
            ativo = false;
        };
    }, []);

    function limparFormulario() {
        setIdEdicao(null);
        setFormulario({
            ...inicial,
            idTipoEvento: tipos[0]?.idTipoEvento || "",
            idInstituicao: instituicoes[0]?.idInstituicao || "",
        });
        if (fotoInputRef.current) {
            fotoInputRef.current.value = "";
        }
    }

    async function salvarEvento(e: FormEvent<HTMLFormElement>) {
        e.preventDefault();
        setEnviando(true);
        setMensagem("");

        const payload = {
            nomeEvento: formulario.nome,
            dataEvento: formulario.dataEvento,
            descricao: formulario.descricao,
            idTipoEvento: formulario.idTipoEvento,
            idInstituicao: formulario.idInstituicao,
            imagem: formulario.arquivoImagem,
        };

        try {
            if (idEdicao) {
                await eventoService.atualizar(idEdicao, payload);
                setTipoMensagem("sucesso");
                setMensagem("Evento atualizado com sucesso!");
            } else {
                await eventoService.cadastrar(payload);
                setTipoMensagem("sucesso");
                setMensagem("Evento cadastrado com sucesso!");
            }
            limparFormulario();
            await carregar();
        } catch (error) {
            setTipoMensagem("erro");
            setMensagem(getApiErrorMessage(error));
        } finally {
            setEnviando(false);
        }
    }

    function iniciarEdicao(evento: Evento) {
        setIdEdicao(evento.idEvento);
        setMensagem("");
        setFormulario({
            nome: evento.nomeEvento,
            dataEvento: evento.dataEvento ? evento.dataEvento.slice(0, 10) : "",
            descricao: evento.descricao,
            idTipoEvento: evento.idTipoEvento || tipos[0]?.idTipoEvento || "",
            idInstituicao: evento.idInstituicao || instituicoes[0]?.idInstituicao || "",
            arquivoImagem: null,
        });
    }

    async function removerEvento(evento: Evento) {
        if (!window.confirm(`Deseja realmente excluir o evento "${evento.nomeEvento}"?`)) {
            return;
        }

        try {
            await eventoService.excluir(evento.idEvento);
            if (idEdicao === evento.idEvento) {
                limparFormulario();
            }
            setTipoMensagem("sucesso");
            setMensagem("Evento excluído com sucesso!");
            await carregar();
        } catch (error) {
            setTipoMensagem("erro");
            setMensagem(getApiErrorMessage(error));
        }
    }

    return (
        <div className="pagina-cadastro-eventos">
            <Header />

            <main className="cadastro-eventos-main">
                <section className="cadastro-eventos-secao">
                    <div className="cadastro-eventos-container">
                        <h1 className="cadastro-eventos-titulo">
                            {idEdicao ? "EDITAR EVENTO" : "CADASTRO DE EVENTOS"}
                        </h1>

                        {mensagem && (
                            <p
                                className={`cadastro-eventos-alerta cadastro-eventos-alerta--${tipoMensagem}`}
                                role="alert"
                            >
                                {mensagem}
                            </p>
                        )}

                        <div className="cadastro-eventos-conteudo">
                            <div className="cadastro-eventos-ilustracao">
                                <img src={visaoImg} alt="Ilustração Cadastro de Eventos" />
                            </div>

                            <form className="cadastro-eventos-form" onSubmit={salvarEvento}>
                                <div className="cadastro-eventos-campo-grupo">
                                    <label htmlFor="nome-evento">Nome do Evento</label>
                                    <input
                                        id="nome-evento"
                                        className="campo"
                                        type="text"
                                        value={formulario.nome}
                                        onChange={(e) =>
                                            setFormulario({ ...formulario, nome: e.target.value })
                                        }
                                        placeholder="Nome do evento"
                                        required
                                    />
                                </div>

                                <div className="cadastro-eventos-linha-dupla">
                                    <div className="cadastro-eventos-campo-grupo">
                                        <label htmlFor="data-evento">Data do Evento</label>
                                        <input
                                            id="data-evento"
                                            className="campo"
                                            type="date"
                                            value={formulario.dataEvento}
                                            onChange={(e) =>
                                                setFormulario({
                                                    ...formulario,
                                                    dataEvento: e.target.value,
                                                })
                                            }
                                            required
                                        />
                                    </div>

                                    <div className="cadastro-eventos-campo-grupo">
                                        <label htmlFor="tipo-evento">Tipo de Evento</label>
                                        <select
                                            id="tipo-evento"
                                            className="campo"
                                            value={formulario.idTipoEvento}
                                            onChange={(e) =>
                                                setFormulario({
                                                    ...formulario,
                                                    idTipoEvento: e.target.value,
                                                })
                                            }
                                            required
                                        >
                                            <option value="">Selecione um tipo</option>
                                            {tipos.map((t) => (
                                                <option key={t.idTipoEvento} value={t.idTipoEvento}>
                                                    {t.titulo || t.tituloTipoEvento}
                                                </option>
                                            ))}
                                        </select>
                                    </div>
                                </div>

                                <div className="cadastro-eventos-campo-grupo">
                                    <label htmlFor="instituicao-evento">Instituição</label>
                                    <select
                                        id="instituicao-evento"
                                        className="campo"
                                        value={formulario.idInstituicao}
                                        onChange={(e) =>
                                            setFormulario({
                                                ...formulario,
                                                idInstituicao: e.target.value,
                                            })
                                        }
                                        required
                                    >
                                        <option value="">Selecione uma instituição</option>
                                        {instituicoes.map((inst) => (
                                            <option
                                                key={inst.idInstituicao}
                                                value={inst.idInstituicao}
                                            >
                                                {inst.nomeFantasia}
                                            </option>
                                        ))}
                                    </select>
                                </div>

                                <div className="cadastro-eventos-campo-grupo">
                                    <label htmlFor="descricao-evento">Descrição</label>
                                    <textarea
                                        id="descricao-evento"
                                        className="campo cadastro-eventos-textarea"
                                        value={formulario.descricao}
                                        onChange={(e) =>
                                            setFormulario({
                                                ...formulario,
                                                descricao: e.target.value,
                                            })
                                        }
                                        placeholder="Descrição do evento"
                                        rows={3}
                                        required
                                    />
                                </div>

                                <div className="cadastro-eventos-campo-grupo">
                                    <label htmlFor="imagem-evento">Imagem do Evento</label>
                                    <input
                                        ref={fotoInputRef}
                                        id="imagem-evento"
                                        className="cadastro-eventos-input-arquivo"
                                        type="file"
                                        accept="image/*"
                                        onChange={(e) =>
                                            setFormulario({
                                                ...formulario,
                                                arquivoImagem: e.target.files?.[0] || null,
                                            })
                                        }
                                    />
                                </div>

                                <div className="cadastro-eventos-acoes">
                                    <Botao
                                        type="submit"
                                        className="cadastro-eventos-botao-salvar"
                                        texto={
                                            enviando
                                                ? "Salvando..."
                                                : idEdicao
                                                ? "Atualizar"
                                                : "Cadastrar"
                                        }
                                        disabled={enviando}
                                    />
                                    {idEdicao && (
                                        <button
                                            type="button"
                                            className="cadastro-eventos-botao-cancelar"
                                            onClick={limparFormulario}
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
                        <h2>LISTA DE EVENTOS</h2>

                        <div className="lista-tipos-tabela-wrapper">
                            <table className="lista-tipos-tabela">
                                <thead>
                                    <tr>
                                        <th>Evento</th>
                                        <th>Data</th>
                                        <th>Tipo Evento</th>
                                        <th>Descrição</th>
                                        <th className="lista-tipos-col-acao">Editar</th>
                                        <th className="lista-tipos-col-acao">Excluir</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {carregando ? (
                                        <tr>
                                            <td colSpan={6} className="lista-tipos-tabela-vazia">
                                                Carregando eventos...
                                            </td>
                                        </tr>
                                    ) : eventos.length === 0 ? (
                                        <tr>
                                            <td colSpan={6} className="lista-tipos-tabela-vazia">
                                                Nenhum evento cadastrado.
                                            </td>
                                        </tr>
                                    ) : (
                                        eventos.map((ev) => (
                                            <tr
                                                key={ev.idEvento}
                                                className={
                                                    idEdicao === ev.idEvento
                                                        ? "lista-tipos-linha-ativa"
                                                        : ""
                                                }
                                            >
                                                <td>{ev.nomeEvento}</td>
                                                <td>
                                                    {ev.dataEvento
                                                        ? new Date(ev.dataEvento).toLocaleDateString(
                                                              "pt-BR"
                                                          )
                                                        : "-"}
                                                </td>
                                                <td>
                                                    {ev.idTipoEventoNavigation?.titulo ||
                                                        ev.idTipoEventoNavigation
                                                            ?.tituloTipoEvento ||
                                                        "-"}
                                                </td>
                                                <td>{ev.descricao}</td>
                                                <td className="lista-tipos-col-acao">
                                                    <button
                                                        type="button"
                                                        className="lista-tipos-acao"
                                                        aria-label={`Editar ${ev.nomeEvento}`}
                                                        onClick={() => iniciarEdicao(ev)}
                                                    >
                                                        <img src={lapis} alt="Editar" />
                                                    </button>
                                                </td>
                                                <td className="lista-tipos-col-acao">
                                                    <button
                                                        type="button"
                                                        className="lista-tipos-acao"
                                                        aria-label={`Excluir ${ev.nomeEvento}`}
                                                        onClick={() => void removerEvento(ev)}
                                                    >
                                                        <img src={lixeira} alt="Excluir" />
                                                    </button>
                                                </td>
                                            </tr>
                                        ))
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

export default CadastroEvento;
