import { useEffect, useState } from "react";
import visao from "../../assets/visao-img.png";

import bannerPadrao1 from "../../assets/banner-1.png";
import bannerPadrao2 from "../../assets/banner-2.png";
import bannerPadrao3 from "../../assets/banner-3.png";

// Imports da API
import { eventoService } from "../../services/eventPlusService";
import type { Evento } from "../../types/api";

import Header from "../../components/Header/Header";
import Footer from "../../components/Footer/footer";
import Carrossel from "../../components/Carrossel/Carrossel";
import CardEvento from "../../components/CardEvento/CardEvento";

import "./Home.css";

function Home() {
    // 1. Estado para guardar os eventos da API
    const [eventos, setEventos] = useState<Evento[]>([]);

    // 2. Chamada à API ao carregar a página
    useEffect(() => {
        async function carregarProximos() {
            try {
                const dados = await eventoService.listaProximos();
                if (dados && dados.length > 0) {
                    setEventos(dados);
                }
            } catch (error) {
                console.error("Erro ao carregar próximos eventos:", error);
            }
        }
        void carregarProximos();
    }, []);

    // Se a API não estiver conectada, usa os 3 cards padrão do layout Figma
    const eventosExibicao: Evento[] =
        eventos.length > 0
            ? eventos
            : [
                  {
                      idEvento: "1",
                      nomeEvento: "Título do Evento",
                      dataEvento: "2026-12-03",
                      descricao: "Breve descrição do evento, pode ser um parágrafo pequeno",
                      imagemUrl: bannerPadrao1,
                      idTipoEventoNavigation: { idTipoEvento: "1", titulo: "Categoria" },
                  },
                  {
                      idEvento: "2",
                      nomeEvento: "Título do Evento",
                      dataEvento: "2026-11-08",
                      descricao: "Breve descrição do evento, pode ser um parágrafo pequeno",
                      imagemUrl: bannerPadrao2,
                      idTipoEventoNavigation: { idTipoEvento: "2", titulo: "Categoria" },
                  },
                  {
                      idEvento: "3",
                      nomeEvento: "Título do Evento",
                      dataEvento: "2026-10-22",
                      descricao: "Breve descrição do evento, pode ser um parágrafo pequeno",
                      imagemUrl: bannerPadrao3,
                      idTipoEventoNavigation: { idTipoEvento: "3", titulo: "Categoria" },
                  },
              ];

    return (
        <div className="home-pagina">
            <Header />

            <main>
                {/* 1. Carrossel de Banners */}
                <section id="inicio" className="home-banner">
                    <Carrossel />
                </section>

                {/* 2. Seção Visão */}
                <section className="home-visao">
                    <div className="home-visao-container">
                        <div className="home-visao-circulo">
                            <img
                                src={visao}
                                alt="Ilustração da visão da Event Plus"
                            />
                        </div>

                        <div className="home-visao-texto">
                            <h2>VISÃO</h2>
                            <p>
                                A EventPlus organiza eventos e informações em uma
                                interface direta, com foco em facilitar a consulta
                                da agenda, o cadastro e a navegação entre as tela
                                principais
                            </p>
                        </div>
                    </div>
                </section>

                {/* 3. Seção Próximos Eventos */}
                <section id="eventos" className="home-eventos">
                    <div className="home-eventos-titulo">
                        <h2>PRÓXIMOS EVENTOS</h2>
                        <div className="linha-destaque" />
                    </div>

                    <div className="home-eventos-lista">
                        {eventosExibicao.map((e) => (
                            <CardEvento
                                key={e.idEvento}
                                id={e.idEvento}
                                titulo={e.nomeEvento}
                                imagem={e.imagemUrl || ""}
                                descricao={e.descricao}
                                categoria={
                                    e.idTipoEventoNavigation?.titulo ||
                                    e.idTipoEventoNavigation?.tituloTipoEvento ||
                                    "Categoria"
                                }
                            />
                        ))}
                    </div>
                </section>

                {/* 4. Seção Contato */}
                <section id="contato" className="home-contato">
                    <div className="home-contato-titulo">
                        <h2>CONTATO</h2>
                        <div className="linha-destaque-branca" />
                    </div>

                    <div className="home-contato-container">
                        <div className="home-contato-dados">
                            <p>Rua Niterói, 180 – Centro</p>
                            <p>São Caetano do Sul – SP</p>
                            <a href="tel:1142252000" className="home-contato-telefone">
                                (11) 4225-2000
                            </a>
                        </div>

                        <div className="home-contato-mapa">
                            <iframe
                                title="Mapa de localização do SENAI São Caetano do Sul"
                                src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3655.6990374600537!2d-46.573299222839175!3d-23.615124263572987!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x94ce5d11c031c57f%3A0x8f79f71018065a16!2sSENAI%20S%C3%A3o%20Caetano%20do%20Sul%20-%20Cyber%20e%20IA!5e0!3m2!1spt-BR!2sbr!4v1791222612716!5m2!1spt-BR!2sbr"
                                loading="lazy"
                            />
                        </div>
                    </div>
                </section>
            </main>

            <Footer />
        </div>
    );
}

export default Home;
