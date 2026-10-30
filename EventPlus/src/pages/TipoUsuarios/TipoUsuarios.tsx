import { useCallback, useEffect, useState, type FormEvent } from "react";
import Header from "../../components/Header/Header";
import Footer from "../../components/Footer/footer";
import Campo from "../../components/Campo/Campo";
import Botao from "../../components/Botao/Botao";
import { tipoUsuarioService } from "../../services/eventPlusService";
import { getApiErrorMessage } from "../../services/api";
import type { TipoUsuario } from "../../types/api";

import lapis from "../../assets/icons/lapis.svg";
import lixeira from "../../assets/icons/lixeira.svg";
import visaoImg from "../../assets/visao-img.png";

import "./TipoUsuarios.css";

function TipoUsuarios() {
    const [tiposUsuarios, setTiposUsuarios] = useState<TipoUsuario[]>([]);
    const [titulo, setTitulo] = useState("");
    const [idEmEdicao, setIdEmEdicao] = useState<string | null>(null);
    const [carregando, setCarregando] = useState(true);
    const [enviando, setEnviando] = useState(false);
    const [mensagem, setMensagem] = useState("");
    const [tipoMensagem, setTipoMensagem] = useState<"sucesso" | "erro">("sucesso");

    const carregarTiposUsuarios = useCallback(async () => {
        try {
            const dados = await tipoUsuarioService.listar();
            setTiposUsuarios(dados);
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
                const dados = await tipoUsuarioService.listar();
                if (!ativo) return;
                setTiposUsuarios(dados);
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

    function cancelarEdicao() {
        setIdEmEdicao(null);
        setTitulo("");
    }

    function iniciarEdicao(tipo: TipoUsuario) {
        setIdEmEdicao(tipo.idTipoUsuario);
        setTitulo(tipo.titulo || tipo.tituloTipoUsuario || "");
        setMensagem("");
    }

    async function salvarTipoUsuario(e: FormEvent<HTMLFormElement>) {
        e.preventDefault();
        const tituloLimpo = titulo.trim();

        if (!tituloLimpo) {
            setTipoMensagem("erro");
            setMensagem("Informe o título do tipo de usuário.");
            return;
        }

        setEnviando(true);
        setMensagem("");

        try {
            if (idEmEdicao) {
                await tipoUsuarioService.atualizar(idEmEdicao, tituloLimpo);
                setTipoMensagem("sucesso");
                setMensagem("Tipo de usuário atualizado com sucesso!");
            } else {
                await tipoUsuarioService.cadastrar(tituloLimpo);
                setTipoMensagem("sucesso");
                setMensagem("Tipo de usuário cadastrado com sucesso!");
            }

            cancelarEdicao();
            await carregarTiposUsuarios();
        } catch (error) {
            setTipoMensagem("erro");
            setMensagem(getApiErrorMessage(error));
        } finally {
            setEnviando(false);
        }
    }

    async function excluirTipoUsuario(tipo: TipoUsuario) {
        const nomeExibicao = tipo.titulo || tipo.tituloTipoUsuario || "este tipo de usuário";
        if (!window.confirm(`Deseja realmente excluir "${nomeExibicao}"?`)) {
            return;
        }

        setMensagem("");
        try {
            await tipoUsuarioService.excluir(tipo.idTipoUsuario);
            if (idEmEdicao === tipo.idTipoUsuario) {
                cancelarEdicao();
            }
            setTipoMensagem("sucesso");
            setMensagem("Tipo de usuário excluído com sucesso!");
            await carregarTiposUsuarios();
        } catch (error) {
            setTipoMensagem("erro");
            setMensagem(getApiErrorMessage(error));
        }
    }

    return (
        <div className="pagina-tipo-usuarios">
            <Header />

            <main className="tipo-usuarios-main">
                <section className="tipo-usuarios-cadastro">
                    <div className="tipo-usuarios-container">
                        <h1 className="tipo-usuarios-titulo">
                            {idEmEdicao ? "EDITAR TIPO DE USUÁRIO" : "CADASTRO DE TIPO DE USUÁRIO"}
                        </h1>

                        {mensagem && (
                            <p
                                className={`tipo-usuarios-alerta tipo-usuarios-alerta--${tipoMensagem}`}
                                role="alert"
                            >
                                {mensagem}
                            </p>
                        )}

                        <div className="tipo-usuarios-conteudo">
                            <div className="tipo-usuarios-ilustracao">
                                <img src={visaoImg} alt="Ilustração Cadastro de Tipo de Usuário" />
                            </div>

                            <form className="tipo-usuarios-form" onSubmit={salvarTipoUsuario}>
                                <div className="tipo-usuarios-campo-grupo">
                                    <label htmlFor="titulo-tipo-usuario">Título</label>
                                    <Campo
                                        id="titulo-tipo-usuario"
                                        name="titulo"
                                        placeholder="Título"
                                        value={titulo}
                                        onChange={(e) => setTitulo(e.target.value)}
                                        maxLength={100}
                                        required
                                    />
                                </div>

                                <div className="tipo-usuarios-acoes">
                                    <Botao
                                        type="submit"
                                        className="tipo-usuarios-botao-salvar"
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
                                            className="tipo-usuarios-botao-cancelar"
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
                        <h2>LISTA DE TIPO DE USUÁRIOS</h2>

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
                                                Carregando tipos de usuários...
                                            </td>
                                        </tr>
                                    ) : tiposUsuarios.length === 0 ? (
                                        <tr>
                                            <td colSpan={3} className="lista-tipos-tabela-vazia">
                                                Nenhum tipo de usuário cadastrado.
                                            </td>
                                        </tr>
                                    ) : (
                                        tiposUsuarios.map((tipo) => {
                                            const nomeTipo =
                                                tipo.titulo || tipo.tituloTipoUsuario || "Sem título";
                                            return (
                                                <tr
                                                    key={tipo.idTipoUsuario}
                                                    className={
                                                        idEmEdicao === tipo.idTipoUsuario
                                                            ? "lista-tipos-linha-ativa"
                                                            : ""
                                                    }
                                                >
                                                    <td>{nomeTipo}</td>
                                                    <td className="lista-tipos-col-acao">
                                                        <button
                                                            type="button"
                                                            className="lista-tipos-acao"
                                                            aria-label={`Editar ${nomeTipo}`}
                                                            onClick={() => iniciarEdicao(tipo)}
                                                        >
                                                            <img src={lapis} alt="Editar" />
                                                        </button>
                                                    </td>
                                                    <td className="lista-tipos-col-acao">
                                                        <button
                                                            type="button"
                                                            className="lista-tipos-acao"
                                                            aria-label={`Excluir ${nomeTipo}`}
                                                            onClick={() => void excluirTipoUsuario(tipo)}
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

export default TipoUsuarios;
