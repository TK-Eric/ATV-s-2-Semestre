import { useEffect, useState } from "react";
import { eventoService } from "../../services/eventPlusService";
import type { Evento } from "../../types/api";

import bannerPadrao1 from "../../assets/banner-1.png";
import bannerPadrao2 from "../../assets/banner-2.png";
import bannerPadrao3 from "../../assets/banner-3.png";

import Header from "../../components/Header/Header";
import Footer from "../../components/Footer/footer";
import CardEvento from "../../components/CardEvento/CardEvento";

import "./Eventos.css";

export function Eventos() {
    const [eventos, setEventos] = useState<Evento[]>([]);
    const [busca, setBusca] = useState("");
    const [data, setData] = useState<Date>(new Date());

    useEffect(() => {
        const timerId = setInterval(() => {
            setData(new Date());
        }, 1000);

        return () => {
            clearInterval(timerId);
        };
    }, []);

    useEffect(() => {
        async function carregarEventos() {
            try {
                const dados = await eventoService.listar();
                if (dados && dados.length > 0) {
                    setEventos(dados);
                }
            } catch (error) {
                console.error("Erro ao carregar eventos:", error);
            }
        }

        void carregarEventos();
    }, []);

    const eventosBase: Evento[] =
        eventos.length > 0
            ? eventos
            : [
                  {
                      idEvento: "1",
                      nomeEvento: "NextGen Data Forum 2026",
                      dataEvento: "2026-12-03",
                      descricao: "Dados, cibersegurança e inteligência para novos negócios.",
                      imagemUrl: bannerPadrao1,
                      idTipoEventoNavigation: { idTipoEvento: "1", titulo: "Tecnologia" },
                  },
                  {
                      idEvento: "2",
                      nomeEvento: "CodeSphere Week 2026",
                      dataEvento: "2026-11-08",
                      descricao: "Desenvolvimento, cloud e experiências imersivas para devs.",
                      imagemUrl: bannerPadrao2,
                      idTipoEventoNavigation: { idTipoEvento: "2", titulo: "Desenvolvimento" },
                  },
                  {
                      idEvento: "3",
                      nomeEvento: "TechNova Expo 2026",
                      dataEvento: "2026-10-22",
                      descricao: "O encontro entre inovação, IA e futuro digital.",
                      imagemUrl: bannerPadrao3,
                      idTipoEventoNavigation: { idTipoEvento: "3", titulo: "Inovação & IA" },
                  },
              ];

    const termo = busca.trim().toLowerCase();
    const eventosFiltrados = termo
        ? eventosBase.filter(
              (ev) =>
                  ev.nomeEvento.toLowerCase().includes(termo) ||
                  ev.descricao.toLowerCase().includes(termo) ||
                  (
                      ev.idTipoEventoNavigation?.titulo ||
                      ev.idTipoEventoNavigation?.tituloTipoEvento ||
                      ""
                  )
                      .toLowerCase()
                      .includes(termo)
          )
        : eventosBase;

    return (
        <div className="pagina-eventos">
            <Header />

            <main className="eventos-main">
                <section className="eventos-cabecalho">
                    <div className="eventos-container">
                        <h1 className="eventos-titulo">CATÁLOGO DE EVENTOS</h1>

                        <div className="eventos-barra-topo">
                            <input
                                type="search"
                                className="eventos-busca"
                                placeholder="Buscar evento por nome, descrição ou categoria..."
                                value={busca}
                                onChange={(e) => setBusca(e.target.value)}
                            />
                            <span className="eventos-relogio">
                                {data.toLocaleString("pt-BR")}
                            </span>
                        </div>
                    </div>
                </section>

                <section className="eventos-catalogo">
                    <div className="eventos-container">
                        {eventosFiltrados.length === 0 ? (
                            <p className="eventos-vazio">
                                Nenhum evento encontrado para "{busca}".
                            </p>
                        ) : (
                            <div className="eventos-grid">
                                {eventosFiltrados.map((e) => (
                                    <CardEvento
                                        key={e.idEvento}
                                        id={e.idEvento}
                                        imagem={e.imagemUrl || ""}
                                        titulo={e.nomeEvento}
                                        categoria={
                                            e.idTipoEventoNavigation?.titulo ||
                                            e.idTipoEventoNavigation?.tituloTipoEvento ||
                                            "Sem categoria"
                                        }
                                        descricao={e.descricao}
                                    />
                                ))}
                            </div>
                        )}
                    </div>
                </section>
            </main>

            <Footer />
        </div>
    );
}

export default Eventos;
