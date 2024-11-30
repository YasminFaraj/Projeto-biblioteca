import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axios from "axios";
import { Emprestimo } from "../../../models/Emprestimo";

function EmprestimoAlterar() {
    const { id } = useParams<{ id: string }>();
    const [emprestimo, setEmprestimo] = useState<Emprestimo | null>(null);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchEmprestimo = async () => {
            try {
                const response = await axios.get<Emprestimo>(`http://localhost:5274/biblioteca/emprestimo/${id}`);
                setEmprestimo(response.data);
            } catch (error) {
                console.error("Erro ao buscar empréstimo:", error);
            }
        };

        if (id) {
            fetchEmprestimo();
        }
    }, [id]);

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        if (emprestimo) {
            try {
                await axios.put(`http://localhost:5274/biblioteca/emprestimo/alterar/${id}`, emprestimo);
                navigate("/components/emprestimo");
            } catch (error) {
                console.error("Erro ao atualizar empréstimo:", error);
            }
        }
    };

    if (!emprestimo) return <div>Carregando...</div>;

    return (
        <div className="container">
            <h1>Alterar Empréstimo</h1>
            <form onSubmit={handleSubmit}>
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
                <button type="submit">Salvar Alterações</button>
            </form>
        </div>
    );
}

export default EmprestimoAlterar;