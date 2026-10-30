import axios from "axios";

export const SESSION_STORAGE_KEY = "eventplus:session";
export const UNAUTHORIZED_EVENT = "eventplus:unauthorized";

export class ApiError extends Error {
  status?: number;
  validationErrors?: Record<string, string[]>;
  constructor(
    message: string,
    status?: number,
    validationErrors?: Record<string, string[]>
  ) {
    super(message);
    this.name = "ApiError";
    this.status = status;
    this.validationErrors = validationErrors;
  }
}

export const axiosInstance = axios.create({
  baseURL: "https://localhost:7021/api/",
  timeout: 15000,
  headers: {
    "Content-Type": "application/json",
    Accept: "application/json",
  },
});

axiosInstance.interceptors.request.use((config) => {
  try {
    const rawSession = localStorage.getItem(SESSION_STORAGE_KEY);
    if (rawSession) {
      const session = JSON.parse(rawSession);
      if (session?.token) {
        config.headers.Authorization = `Bearer ${session.token}`;
      }
    }
  } catch {
    return config;
  }
  return config;
});

axiosInstance.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem(SESSION_STORAGE_KEY);
      window.dispatchEvent(new Event(UNAUTHORIZED_EVENT));
    }
    return Promise.reject(error);
  }
);

export function getApiErrorMessage(error: unknown): string {
  if (error instanceof ApiError) {
    return error.message;
  }
  if (axios.isAxiosError(error)) {
    const serverMessage =
      error.response?.data?.mensagem ||
      error.response?.data?.message ||
      (typeof error.response?.data === "string" ? error.response.data : undefined);

    if (serverMessage) return serverMessage;
    if (error.response?.status === 401) return "Sessão expirada ou credenciais inválidas.";
    if (error.response?.status === 403) return "Acesso negado.";
    if (error.response?.status === 404) return "Recurso não encontrado.";
    if (error.message) return error.message;
  }
  if (error instanceof Error) {
    return error.message;
  }
  return "Ocorreu um erro inesperado. Tente novamente.";
}
