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

function normalizarTipoEvento(item: TipoEvento): TipoEvento {
  const titulo = item.titulo || item.tituloTipoEvento || "";
  return {
    ...item,
    titulo,
    tituloTipoEvento: item.tituloTipoEvento || titulo,
  };
}

function normalizarTipoUsuario(item: TipoUsuario): TipoUsuario {
  const titulo = item.titulo || item.tituloTipoUsuario || "";
  return {
    ...item,
    titulo,
    tituloTipoUsuario: item.tituloTipoUsuario || titulo,
  };
}

function normalizarUsuario(usuario: Usuario): Usuario {
  return {
    ...usuario,
    idTipoUsuarioNavigation: usuario.idTipoUsuarioNavigation
      ? normalizarTipoUsuario(usuario.idTipoUsuarioNavigation)
      : null,
  };
}

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
    return data.map(normalizarTipoEvento);
  },

  buscar: async (id: string) => {
    const { data } = await axiosInstance.get<TipoEvento>(`/TipoEvento/${id}`);
    return normalizarTipoEvento(data);
  },

  cadastrar: async (titulo: string) => {
    const { data } = await axiosInstance.post<TipoEvento>("/TipoEvento", {
      titulo,
      tituloTipoEvento: titulo,
    });
    return normalizarTipoEvento(data);
  },

  atualizar: async (id: string, titulo: string) => {
    const { data } = await axiosInstance.put<TipoEvento>(`/TipoEvento/${id}`, {
      titulo,
      tituloTipoEvento: titulo,
    });
    return data ? normalizarTipoEvento(data) : undefined;
  },

  excluir: async (id: string) => {
    await axiosInstance.delete(`/TipoEvento/${id}`);
  },
};

export const tipoUsuarioService = {
  listar: async () => {
    const { data } = await axiosInstance.get<TipoUsuario[]>("/TipoUsuario");
    return data.map(normalizarTipoUsuario);
  },

  buscar: async (id: string) => {
    const { data } = await axiosInstance.get<TipoUsuario>(`/TipoUsuario/${id}`);
    return normalizarTipoUsuario(data);
  },

  cadastrar: async (titulo: string) => {
    const { data } = await axiosInstance.post<TipoUsuario>("/TipoUsuario", {
      titulo,
      tituloTipoUsuario: titulo,
    });
    return normalizarTipoUsuario(data);
  },

  atualizar: async (id: string, titulo: string) => {
    const { data } = await axiosInstance.put<TipoUsuario>(`/TipoUsuario/${id}`, {
      titulo,
      tituloTipoUsuario: titulo,
    });
    return data ? normalizarTipoUsuario(data) : undefined;
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
    return normalizarUsuario(data);
  },

  async listar() {
    const { data } = await axiosInstance.get<Usuario[]>("/Usuario");
    return data.map(normalizarUsuario);
  },

  async cadastrar(usuario: Partial<Usuario>) {
    const { data } = await axiosInstance.post<Usuario>("/Usuario", usuario);
    return normalizarUsuario(data);
  },

  async atualizar(id: string, usuario: Partial<Usuario>) {
    const { data } = await axiosInstance.put<Usuario>(`/Usuario/${id}`, usuario);
    return data ? normalizarUsuario(data) : undefined;
  },

  async excluir(id: string) {
    await axiosInstance.delete(`/Usuario/${id}`);
  },
};
