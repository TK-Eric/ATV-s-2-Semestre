import { useCallback, useEffect, useState } from "react";

export function useCarrossel(total: number, intervaloMs: number) {
    const [indiceAtivo, setIndiceAtivo] = useState(0);
    const indice = total > 0 ? indiceAtivo % total : 0;

    useEffect(() => {
        if (total <= 1) return;
        const timer = window.setTimeout(() => {
            setIndiceAtivo((atual) => ((atual % total) + 1) % total);
        }, intervaloMs);
        return () => window.clearTimeout(timer);
    }, [indiceAtivo, total, intervaloMs]);

    const anterior = useCallback(() => {
        if (total <= 1) return;
        setIndiceAtivo((atual) => ((atual % total) - 1 + total) % total);
    }, [total]);

    const proximo = useCallback(() => {
        if (total <= 1) return;
        setIndiceAtivo((atual) => ((atual % total) + 1) % total);
    }, [total]);

    return { indice, anterior, proximo };
}
