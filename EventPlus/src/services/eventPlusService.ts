import { api } from "./api";
import type { Evento } from "../types/api";

export const eventoService = {
  listar: async () => {
    const resposta = await api.get<Evento[]>('/Event'); 
    return resposta.data;
  },
  
  listaProximos: async () => {
    const resposta = await api.get<Evento[]>('/Event'); 
    return resposta.data;
  },

  buscar: async (id: string) => {
    const resposta = await api.get<Evento>(`/Event/${id}`);
    return resposta.data;
  }
};