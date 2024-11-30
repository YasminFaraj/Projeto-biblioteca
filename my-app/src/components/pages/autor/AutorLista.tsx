import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import axios from "axios";
import { Autor } from "../../../models/Autor";

function AutorLista() {
    const [autores, setAutores] = useState<Autor[]>([]);

    useEffect(() => {
        fetch("http://localhost:5274/biblioteca/autor/listar", {
            method: 'GET',
        })
            .then((resposta) => resposta.json()) // Retorno do fetch é tratado como 'unknown'
            .then((autores: Autor[]) => { // Agora definimos explicitamente que 'autores' é do tipo Autor[]
                setAutores(autores); // Atualiza o estado corretamente
            })
            .catch((erro) => console.error("Erro ao carregar os autores:", erro));
    }, []);

    function deletar(id: string) {
        axios
            .delete(`http://localhost:5274/biblioteca/autor/deletar/${id}`)
            .then((resposta) => {
                console.log(resposta.data);
            })
            .catch((erro) => {
                console.error("Erro ao deletar o autor:", erro);
            });
    }

    return (
        <div className="container">
            <h1>Lista de Autores</h1>
            <table>
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Nome</th>
                        <th>Sobrenome</th>
                        <th>País</th>
                        <th>Deletar</th>
                        <th>Alterar</th>
                    </tr>
                </thead>
                <tbody>
                    {autores.map((autor) => (
                        <tr key={autor.autorId}>
                            <td>{autor.autorId}</td>
                            <td>{autor.nome}</td>
                            <td>{autor.sobrenome}</td>
                            <td>{autor.pais}</td>
                            <td>
                                <button onClick={() => deletar(autor.autorId!)}>Deletar</button>
                            </td>
                            <td>
                                <Link to={`/pages/autor/alterar/${autor.autorId}`}>Alterar</Link>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

export default AutorLista;
