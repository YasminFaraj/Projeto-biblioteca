import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import axios from "axios";
import { Emprestimo } from "../../../models/Emprestimo";

function EmprestimoLista() {
    const [emprestimos, setEmprestimos] = useState<Emprestimo[]>([]);

    useEffect(() => {
        const fetchEmprestimos = async () => {
            try {
                const response = await axios.get<Emprestimo[]>("http://localhost:5274/biblioteca/emprestimo/listar");
                const emprestimosData = response.data.map((emprestimo) => ({
                    ...emprestimo,
                    dataEmprestimo: new Date(emprestimo.dataEmprestimo),
                    prazoDevolucao: new Date(emprestimo.prazoDevolucao),
                }));
                setEmprestimos(emprestimosData);
            } catch (error) {
                console.error("Erro ao buscar empréstimos:", error);
            }
        };

        fetchEmprestimos();
    }, []);

    const deletar = async (id: string) => {
        try {
            await axios.delete(`http://localhost:5274/biblioteca/emprestimo/deletar/${id}`);
            setEmprestimos(emprestimos.filter(emprestimo => emprestimo.emprestimoId !== id));
        } catch (error) {
            console.error("Erro ao deletar empréstimo:", error);
        }
    };

    return (
        <div className="container">
            <h1>Lista de Empréstimos</h1>
            <table>
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Leitor</th>
                        <th>Livro</th>
                        <th>Data Empréstimo</th>
                        <th>Prazo Devolução</th>
                        <th>Status</th>
                        <th>Devolver</th>
                        <th>Deletar</th>
                    </tr>
                </thead>
                <tbody>
                    {emprestimos.length > 0 ? (
                        emprestimos.map((emprestimo) => (
                            <tr key={emprestimo.emprestimoId}>
                                <td>{emprestimo.emprestimoId}</td>
                                <td>{emprestimo.leitor.nome}</td>
                                <td>{emprestimo.livro.titulo}</td>
                                <td>{emprestimo.dataEmprestimo.toLocaleDateString()}</td>
                                <td>{emprestimo.prazoDevolucao.toLocaleDateString()}</td>
                                <td>{emprestimo.ativo ? "Ativo" : "Devolvido"}</td>
                                <td>
                                    {emprestimo.ativo && (
                                        <Link to={`/components/emprestimo/devolucao/${emprestimo.emprestimoId}`}>
                                            Devolver
                                        </Link>
                                    )}
                                </td>
                                <td>
                                    <button onClick={() => deletar(emprestimo.emprestimoId)}>Deletar</button>
                                </td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan={8}>Nenhum empréstimo encontrado.</td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
}

export default EmprestimoLista;
