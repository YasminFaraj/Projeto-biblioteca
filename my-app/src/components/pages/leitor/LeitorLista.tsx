import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import axios from "axios";
import { Leitor } from "../../../models/Leitor";

function LeitorLista(){
    const [leitores, setLeitores] = useState<Leitor[]>([]);

    useEffect(() => {
        fetch("http://localhost:5274/biblioteca/leitor/listar", {
            method: 'GET', 
        })
            .then((resposta) => {
                return resposta.json();
            })
            .then((leitores) => {
                setLeitores(leitores);
            });
    }, []);

    function deletar(id: string) {
        axios
            .delete(`http://localhost:5274/biblioteca/leitor/deletar/${id}`)
            .then((resposta) => {
                console.log(resposta.data);
            });
    }

    return (
        <div className="container">
            <h1>Lista de Leitores</h1>
            <table>
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Nome</th>
                        <th>Sobrenome</th>
                        <th>Telefone</th>
                        <th>E-mail</th>
                        <th>CPF</th>
                        <th>Deletar</th>
                        <th>Alterar</th>
                    </tr>
                </thead>
                <tbody>
                    {leitores.map((leitor) => (
                        <tr key={leitor.leitorId}>
                            <td>{leitor.leitorId}</td>
                            <td>{leitor.nome}</td>
                            <td>{leitor.sobrenome}</td>
                            <td>{leitor.telefone}</td>
                            <td>{leitor.email}</td>
                            <td>{leitor.cpf}</td>
                            <td>
                                <button onClick={() => deletar(leitor.leitorId!)}>
                                    Deletar
                                </button> 
                            </td>
                            <td>
                                <Link to={`/pages/leitor/alterar/${leitor.leitorId}`}>
                                    Alterar
                                </Link>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

export default LeitorLista;