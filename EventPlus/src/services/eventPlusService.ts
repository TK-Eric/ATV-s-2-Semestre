import { axiosInstance } from "./api";
import type { Evento } from "../types/api";

export const eventoService = {
  listar: async () => {
    const resposta = await axiosInstance.get<Evento[]>('/Event'); 
    return resposta.data;
  },
  
  listaProximos: async () => {
    const resposta = await axiosInstance.get<Evento[]>('/Event'); 
    return resposta.data;
  },

  buscar: async (id: string) => {
    const resposta = await axiosInstance.get<Evento>(`/Event/${id}`);
    return resposta.data;
  }
}

export const presencaService = {
  async listarPorUsuario(idUsuario: string){
    const { data } = await axiosInstance.get<Presenca[]>(`Presenca/usuario/${idUsuario}`);
    return data;
  },

  async cadastrar(payload: PresecaPayload) {
    const { data } = await axiosInstance.get<Presenca[]>("/Presenca", payload); 
  },

  async excluir(id: string) {
    await axiosInstance.delete(`/Presenca/${id}`); 
  },
}

