import type { InitialEntry } from "react-router-dom";

export interface TipoEvento {
    idTipoEvento: string;
    titulo: string;
}

export interface instituicao {
    idInstituicao: string;
    cnpj: string;
    nomeFantasia: string;
    endereco: string;
}

export interface Evento {
    idEvento: string;
    nome: string;
    descricao: string;
    dataEvento: string;
    imagemUrl?: string | null;
    idTipoEvento?: string | null;
    idInstituicao?: string | null;
    idTipoEventoNavigation?: TipoEvento | null;
    idInstituicaoNavigation?: instituicao | null;
}


export interface Presenca {
    idPresenca: string;
    situacao: string;
    idEvento: string;
    idUsuario?: string | null;
    idEventoNavigation?: Evento | null;
}

export interface PresencaPayload {
    situacao: boolean;
    idEvento: string;
    idUsuario: string;
}