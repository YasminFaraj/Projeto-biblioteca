import internal from "stream"
import { Autor } from "./Autor"

export interface Livro {
    livroId?: string;
    titulo: string;
    genero: string;
    qtdExemplares: number;
    anoLancamento: number;
    criadoEm?: string;
    editora: string
    autorId: string;
    autor?: Autor; 
}