import { useEffect, useState } from "react";

import { eventoService } from "../../services/eventPlusService";
import type { Evento } from "../../types/api";

import Header from "../../components/Header/Header";
import Footer from "../../components/footer/footer";
import CardEvento from "../../components/CardEvento/CardEvento";

import "./Eventos.css";

export function Eventos() {
  const [eventos, setEventos] = useState<Evento[]>([]);
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
        setEventos(dados);
      } catch (error) {
        console.error("Erro ao carregar eventos:", error);
      }
    }

    carregarEventos();
  }, []);

  return (
    <>
      <Header />
      <main>
        <h1>Catálogo de eventos</h1>

        <div>
          {eventos.map((e) => (
            <CardEvento
              key={e.idEvento}
              id={e.idEvento}
              imagem={e.imagemUrl || ""}
              titulo={e.nome}
              categoria={e.idTipoEventoNavigation?.titulo || "Sem categoria"}
              descricao={e.descricao}
            />
          ))}
        </div>

        <p>{data.toLocaleString()}</p>
      </main>
      <Footer />
    </>
  );
}

export default Eventos;