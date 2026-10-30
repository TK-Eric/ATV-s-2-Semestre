import { axiosInstance } from "./api";
import type {
  Evento,
  EventoPayload,
  Instituicao,
  LoginPayload,
  LoginResponse,
  Presenca,
  PresencaPayload,
  TipoEvento,
  TipoUsuario,
  Usuario,
} from "../types/api";

function eventoFormData(payload: EventoPayload) {
  const formData = new FormData();
  formData.append("NomeEvento", payload.nomeEvento);
  formData.append("DataEvento", payload.dataEvento);
  formData.append("Descricao", payload.descricao);
  formData.append("IdTipoEvento", payload.idTipoEvento);
  formData.append("IdInstituicao", payload.idInstituicao);
  if (payload.imagem) {
    formData.append("ArquivoImagem", payload.imagem);
  }
  return formData;
}

export const eventoService = {
  listar: async () => {
    const resposta = await axiosInstance.get<Evento[]>("/Event");
    return resposta.data;
  },

  listaProximos: async () => {
    try {
      const resposta = await axiosInstance.get<Evento[]>("/Event/ListarProximos");
      return resposta.data;
    } catch {
      const resposta = await axiosInstance.get<Evento[]>("/Event");
      return resposta.data;
    }
  },

  buscar: async (id: string) => {
    const resposta = await axiosInstance.get<Evento>(`/Event/${id}`);
    return resposta.data;
  },

  cadastrar: async (payload: EventoPayload) => {
    const formData = eventoFormData(payload);
    const { data } = await axiosInstance.post<Evento>("/Event", formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    return data;
  },

  atualizar: async (id: string, payload: EventoPayload) => {
    const formData = eventoFormData(payload);
    const { data } = await axiosInstance.put<Evento>(`/Event/${id}`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    return data;
  },

  excluir: async (id: string) => {
    await axiosInstance.delete(`/Event/${id}`);
  },
};

export const tipoEventoService = {
  listar: async () => {
    const { data } = await axiosInstance.get<TipoEvento[]>("/TipoEvento");
    return data.map((item) => ({
      ...item,
      titulo: item.titulo || item.tituloTipoEvento || "",
    }));
  },

  buscar: async (id: string) => {
    const { data } = await axiosInstance.get<TipoEvento>(`/TipoEvento/${id}`);
    return {
      ...data,
      titulo: data.titulo || data.tituloTipoEvento || "",
    };
  },

  cadastrar: async (titulo: string) => {
    const { data } = await axiosInstance.post<TipoEvento>("/TipoEvento", {
      titulo,
      tituloTipoEvento: titulo,
    });
    return data;
  },

  atualizar: async (id: string, titulo: string) => {
    const { data } = await axiosInstance.put<TipoEvento>(`/TipoEvento/${id}`, {
      titulo,
      tituloTipoEvento: titulo,
    });
    return data;
  },

  excluir: async (id: string) => {
    await axiosInstance.delete(`/TipoEvento/${id}`);
  },
};

export const tipoUsuarioService = {
  listar: async () => {
    const { data } = await axiosInstance.get<TipoUsuario[]>("/TipoUsuario");
    return data.map((item) => ({
      ...item,
      titulo: item.titulo || item.tituloTipoUsuario || "",
    }));
  },

  buscar: async (id: string) => {
    const { data } = await axiosInstance.get<TipoUsuario>(`/TipoUsuario/${id}`);
    return {
      ...data,
      titulo: data.titulo || data.tituloTipoUsuario || "",
    };
  },

  cadastrar: async (titulo: string) => {
    const { data } = await axiosInstance.post<TipoUsuario>("/TipoUsuario", {
      titulo,
      tituloTipoUsuario: titulo,
    });
    return data;
  },

  atualizar: async (id: string, titulo: string) => {
    const { data } = await axiosInstance.put<TipoUsuario>(`/TipoUsuario/${id}`, {
      titulo,
      tituloTipoUsuario: titulo,
    });
    return data;
  },

  excluir: async (id: string) => {
    await axiosInstance.delete(`/TipoUsuario/${id}`);
  },
};

export const instituicaoService = {
  listar: async () => {
    const { data } = await axiosInstance.get<Instituicao[]>("/Instituicao");
    return data;
  },

  buscar: async (id: string) => {
    const { data } = await axiosInstance.get<Instituicao>(`/Instituicao/${id}`);
    return data;
  },

  cadastrar: async (instituicao: Partial<Instituicao>) => {
    const { data } = await axiosInstance.post<Instituicao>("/Instituicao", instituicao);
    return data;
  },

  atualizar: async (id: string, instituicao: Partial<Instituicao>) => {
    const { data } = await axiosInstance.put(`/Instituicao/${id}`, instituicao);
    return data;
  },

  excluir: async (id: string) => {
    await axiosInstance.delete(`/Instituicao/${id}`);
  },
};

export const presencaService = {
  async listarPorUsuario(idUsuario: string) {
    const { data } = await axiosInstance.get<Presenca[]>(`/Presenca/usuario/${idUsuario}`);
    return data;
  },

  async cadastrar(payload: PresencaPayload) {
    const { data } = await axiosInstance.post<Presenca>("/Presenca", payload);
    return data;
  },

  async excluir(id: string) {
    await axiosInstance.delete(`/Presenca/${id}`);
  },
};

export const authService = {
  async login(payload: LoginPayload) {
    const { data } = await axiosInstance.post<LoginResponse>("/Login", payload);
    return data;
  },
};

export const usuarioService = {
  async buscar(id: string) {
    const { data } = await axiosInstance.get<Usuario>(`/Usuario/${id}`);
    return data;
  },

  async listar() {
    const { data } = await axiosInstance.get<Usuario[]>("/Usuario");
    return data;
  },

  async cadastrar(usuario: Partial<Usuario>) {
    const { data } = await axiosInstance.post<Usuario>("/Usuario", usuario);
    return data;
  },

  async atualizar(id: string, usuario: Partial<Usuario>) {
    const { data } = await axiosInstance.put<Usuario>(`/Usuario/${id}`, usuario);
    return data;
  },

  async excluir(id: string) {
    await axiosInstance.delete(`/Usuario/${id}`);
  },
};
