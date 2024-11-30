export interface Emprestimo {
    emprestimoId: string;
    livroId: string;
    livro: {
        livroId: string;
        titulo: string;
    };
    leitorId: string;
    leitor: {
        leitorId: string;
        nome: string;
    };
    dataEmprestimo: Date;
    prazoDevolucao: Date;
    ativo: boolean;
}
