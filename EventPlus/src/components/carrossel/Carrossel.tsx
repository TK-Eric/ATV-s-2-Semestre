import { useCallback, useEffect, useState } from "react";
import "./Carrossel.css";
import bannerEvento1 from "../../assets/banner-1.png";
import bannerEvento2 from "../../assets/banner-2.png";
import bannerEvento3 from "../../assets/banner-3.png";

const banners = [
    { src: bannerEvento1, alt: "NextGen Data Forum 2026" },
    { src: bannerEvento2, alt: "CodeSphere Week 2026" },
    { src: bannerEvento3, alt: "TechNova Expo 2026" },
];

function Carrossel() {
    const [indiceAtivo, setIndiceAtivo] = useState(0);

    const proximo = useCallback(() => {
        setIndiceAtivo((atual) => (atual + 1) % banners.length);
    }, []);

    const anterior = useCallback(() => {
        setIndiceAtivo((atual) => (atual - 1 + banners.length) % banners.length);
    }, []);

    // Auto-avanço a cada 10 segundos (reinicia o timer se o usuário trocar manualmente)
    useEffect(() => {
        const timer = window.setTimeout(proximo, 10000);
        return () => window.clearTimeout(timer);
    }, [indiceAtivo, proximo]);

    return (
        <section className="carrossel" aria-roledescription="carrossel" aria-label="Banners em destaque">
            {/* Área dos slides */}
            <div className="carrossel-track">
                {banners.map((banner, index) => (
                    <div
                        key={index}
                        className={`carrossel-slide ${index === indiceAtivo ? "carrossel-slide--ativo" : ""}`}
                        aria-hidden={index !== indiceAtivo}
                    >
                        <img src={banner.src} alt={banner.alt} />
                    </div>
                ))}
            </div>

            {/* Seta esquerda quase translúcida */}
            <button
                type="button"
                className="carrossel-seta carrossel-seta--esq"
                onClick={anterior}
                aria-label="Banner anterior"
            >
                ‹
            </button>

            {/* Seta direita quase translúcida */}
            <button
                type="button"
                className="carrossel-seta carrossel-seta--dir"
                onClick={proximo}
                aria-label="Próximo banner"
            >
                ›
            </button>

            {/* Indicadores inferiores (3 bolinhas) */}
            <div className="carrossel-indicadores">
                {banners.map((_, index) => (
                    <button
                        key={index}
                        type="button"
                        className={`carrossel-bolinha ${index === indiceAtivo ? "carrossel-bolinha--ativa" : ""}`}
                        onClick={() => setIndiceAtivo(index)}
                        aria-label={`Ir para slide ${index + 1}`}
                        aria-current={index === indiceAtivo ? "true" : undefined}
                    />
                ))}
            </div>
        </section>
    );
}

export default Carrossel;
