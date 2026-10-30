export interface TipoEvento {
    idTipoEvento: string;
    titulo: string;
    tituloTipoEvento?: string;
}

export interface TipoUsuario {
    idTipoUsuario: string;
    titulo: string;
    tituloTipoUsuario?: string;
}

export interface Instituicao {
    idInstituicao: string;
    cnpj: string;
    nomeFantasia: string;
    endereco?: string;
    endereço?: string;
}

export interface Usuario {
    idUsuario: string;
    nome: string;
    email: string;
    idTipoUsuario: string;
    idTipoUsuarioNavigation?: TipoUsuario | null;
}

export interface Evento {
    idEvento: string;
    nomeEvento: string;
    descricao: string;
    dataEvento: string;
    imagemUrl?: string | null;
    idTipoEvento?: string | null;
    idInstituicao?: string | null;
    idTipoEventoNavigation?: TipoEvento | null;
    idInstituicaoNavigation?: Instituicao | null;
}

export interface EventoPayload {
    nomeEvento: string;
    dataEvento: string;
    descricao: string;
    idTipoEvento: string;
    idInstituicao: string;
    imagem?: File | null;
}

export interface Presenca {
    idPresenca: string;
    situacao: boolean;
    idEvento: string;
    idUsuario: string | null;
    idEventoNavigation?: Evento | null;
}

export interface LoginPayload {
    email: string;
    senha: string;
}

export interface LoginResponse {
    token: string;
    expiracao: string;
    perfil?: string;
    usuario: Usuario;
}

export type AuthSession = LoginResponse;

export interface PresencaPayload {
    situacao: boolean;
    idEvento: string;
    idUsuario: string | null;
}
