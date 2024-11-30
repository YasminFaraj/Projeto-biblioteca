import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axios from "axios";
import { Emprestimo } from "../../../models/Emprestimo";

function EmprestimoDevolucao() {
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

    const handleDevolver = async () => {
        if (emprestimo) {
            try {
                await axios.put(`http://localhost:5274/biblioteca/emprestimo/devolver/${id}`, emprestimo);
                navigate("/components/emprestimo");
            } catch (error) {
                console.error("Erro ao devolver empréstimo:", error);
            }
        }
    };

    if (!emprestimo) return <div>Carregando...</div>;

    return (
        <div className="container">
            <h1>Devolver Empréstimo</h1>
            <p>Você tem certeza que deseja devolver este livro?</p>
            <button onClick={handleDevolver}>Confirmar Devolução</button>
        </div>
    );
}

export default EmprestimoDevolucao;