import logoEvent from "../../assets/logo-event.svg";
import bannerEvento1 from "../../assets/banner-1.png";
import bannerEvento2 from "../../assets/banner-2.png";
import bannerEvento3 from "../../assets/banner-3.png";
import visao from "../../assets/visao-img.png";

// 1. Import do componente Carrossel
import Carrossel from "../../components/carrossel/Carrossel";

import "./Home.css";

function Home() {
    return (
        <>
            <header className="home-header">
                <div className="home-header-conteudo">
                    <img src={logoEvent} alt="Logo Event+" />

                    <nav>
                        <a href="#inicio">Home</a>
                        <a href="#eventos">Eventos</a>
                        <a href="#contato">Contatos</a>
                    </nav>

                    <button type="button">Entrar</button>
                </div>
            </header>

            <main>
                {/* 2. Carrossel aplicado na secção do banner */}
                <section id="inicio" className="home-banner">
                    <Carrossel />
                </section>

                <section className="home-visao">
                    <div className="home-visao-container">
                        <img
                            src={visao}
                            alt="Ilustração representando a visão da Event Plus"
                        />

                        <div className="home-visao-texto">
                            <h1>Visão</h1>
                            <p>
                                A Event Plus organiza eventos e informações em uma
                                interface direta, facilitando a consulta da agenda e
                                a participação dos usuários.
                            </p>
                        </div>
                    </div>
                </section>

                <section id="eventos" className="home-eventos">
                    <div className="home-eventos-titulo">
                        <h2>Próximos Eventos</h2>
                        <hr />
                    </div>

                    <div className="home-eventos-lista">
                        <article className="home-evento-card">
                            <img
                                src={bannerEvento1}
                                alt="Evento de tecnologia"
                            />
                            <span>Tecnologia</span>

                            <div className="home-evento-card-info">
                                <h3>Evento de tecnologia</h3>
                                <p>
                                    Conheça novidades e tendências do setor.
                                </p>
                                <button type="button">Ver Evento</button>
                            </div>
                        </article>

                        <article className="home-evento-card">
                            <img
                                src={bannerEvento2}
                                alt="Workshop de tecnologia"
                            />
                            <span>Workshop</span>

                            <div className="home-evento-card-info">
                                <h3>Workshop de tecnologia</h3>
                                <p>
                                    Pratique desenvolvimento com atividades
                                    guiadas.
                                </p>
                                <button type="button">Ver Evento</button>
                            </div>
                        </article>

                        <article className="home-evento-card">
                            <img
                                src={bannerEvento3}
                                alt="Meetup de inteligência artificial"
                            />
                            <span>Meetup</span>

                            <div className="home-evento-card-info">
                                <h3>Meetup de inteligência</h3>
                                <p>
                                    Discussões sobre como usar IA.
                                </p>
                                <button type="button">Ver Evento</button>
                            </div>
                        </article>
                    </div>
                </section>

                <section id="contato" className="home-contato">
                    <div>
                        <h2>Contato</h2>
                        <p>Rua Niterói, 180 - Centro</p>
                        <p>São Caetano do Sul - SP</p>
                    </div>

                    <iframe
                        title="Mapa de localização do SENAI São Caetano do Sul"
                        src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3655.6990374600537!2d-46.573299222839175!3d-23.615124263572987!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x94ce5d11c031c57f%3A0x8f79f71018065a16!2sSENAI%20S%C3%A3o%20Caetano%20do%20Sul%20-%20Cyber%20e%20IA!5e0!3m2!1spt-BR!2sbr!4v1791222612716!5m2!1spt-BR!2sbr"
                        loading="lazy"
                    />
                </section>
            </main>

            <footer className="home-footer">
                <p>Escola Senai de Informática - 2026</p>
            </footer>
        </>
    );
}

export default Home;