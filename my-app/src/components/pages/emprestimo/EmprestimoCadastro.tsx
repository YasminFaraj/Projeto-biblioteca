import { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { Emprestimo } from "../../../models/Emprestimo";
import { Livro } from "../../../models/Livro"; 
import { Leitor } from "../../../models/Leitor";

function EmprestimoCadastro() {
    const [emprestimo, setEmprestimo] = useState<Emprestimo>({
        emprestimoId: "",
        livroId: "",
        leitorId: "",
        livro: { livroId: "", titulo: "" },
        leitor: { leitorId: "", nome: "" },
        dataEmprestimo: new Date(),
        prazoDevolucao: new Date(),
        ativo: true
    });

    const [livros, setLivros] = useState<Livro[]>([]);
    const [leitores, setLeitores] = useState<Leitor[]>([]);

    const navigate = useNavigate();

    useEffect(() => {
        const fetchLivrosLeitores = async () => {
            try {
                const [livrosResponse, leitoresResponse] = await Promise.all([
                    axios.get<Livro[]>("http://localhost:5274/biblioteca/livro/listar"),
                    axios.get<Leitor[]>("http://localhost:5274/biblioteca/leitor/listar")
                ]);
                setLivros(livrosResponse.data);
                setLeitores(leitoresResponse.data);
            } catch (error) {
                console.error("Erro ao buscar livros e leitores:", error);
            }
        };

        fetchLivrosLeitores();
    }, []);

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        try {
            await axios.post("http://localhost:5274/biblioteca/emprestimo/cadastrar", emprestimo);
            navigate("/components/emprestimo");
        } catch (error) {
            console.error("Erro ao cadastrar empréstimo:", error);
        }
    };

    return (
        <div className="container">
            <h1>Cadastrar Empréstimo</h1>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Livro:</label>
                    <select onChange={(e) => setEmprestimo({ ...emprestimo, livroId: e.target.value })}>
                        {livros.map((livro) => (
                            <option key={livro.livroId} value={livro.livroId}>
                                {livro.titulo}
                            </option>
                        ))}
                    </select>
                </div>
                <div>
                    <label>Leitor:</label>
                    <select onChange={(e) => setEmprestimo({ ...emprestimo, leitorId: e.target.value })}>
                        {leitores.map((leitor) => (
                            <option key={leitor.leitorId} value={leitor.leitorId}>
                                {leitor.nome}
                            </option>
                        ))}
                    </select>
                </div>
                <div>
                    <label>Data Empréstimo:</label>
                    <input
                        type="date"
                        value={emprestimo.dataEmprestimo.toISOString().slice(0, 10)}
                        onChange={(e) => setEmprestimo({ ...emprestimo, dataEmprestimo: new Date(e.target.value) })}
                    />
                </div>
                <div>
                    <label>Prazo Devolução:</label>
                    <input
                        type="date"
                        value={emprestimo.prazoDevolucao.toISOString().slice(0, 10)}
                        onChange={(e) => setEmprestimo({ ...emprestimo, prazoDevolucao: new Date(e.target.value) })}
                    />
                </div>
                <button type="submit">Cadastrar</button>
            </form>
        </div>
    );
}

export default EmprestimoCadastro;
