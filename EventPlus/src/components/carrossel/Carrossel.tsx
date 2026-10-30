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

    useEffect(() => {
        const timer = window.setTimeout(proximo, 10000);
        return () => window.clearTimeout(timer);
    }, [indiceAtivo, proximo]);

    return (
        <section className="carrossel">
            <div className="carrossel-track">
                {banners.map((banner, index) => (
                    <div
                        key={index}
                        className={`carrossel-slide ${index === indiceAtivo ? "carrossel-slide--ativo" : ""}`}
                    >
                        <img src={banner.src} alt={banner.alt} />
                    </div>
                ))}
            </div>

            <button
                type="button"
                className="carrossel-seta carrossel-seta--esq"
                onClick={anterior}
                aria-label="Banner anterior"
            >
                ‹
            </button>

            <button
                type="button"
                className="carrossel-seta carrossel-seta--dir"
                onClick={proximo}
                aria-label="Próximo banner"
            >
                ›
            </button>

            <div className="carrossel-indicadores">
                {banners.map((_, index) => (
                    <button
                        key={index}
                        type="button"
                        className={`carrossel-bolinha ${index === indiceAtivo ? "carrossel-bolinha--ativa" : ""}`}
                        onClick={() => setIndiceAtivo(index)}
                        aria-label={`Slide ${index + 1}`}
                    />
                ))}
            </div>
        </section>
    );
}

export default Carrossel;
