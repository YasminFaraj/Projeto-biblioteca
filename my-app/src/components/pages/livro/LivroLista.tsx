import { useEffect, useState } from "react";
import { Livro } from "../../../models/Livro";
import { Link } from "react-router-dom";
import axios from "axios";

function LivroLista(){
    const [livros, setLivros] = useState<Livro[]>([]);

    useEffect(() => {
        fetch("http://localhost:5274/biblioteca/livro/listar", {
            method: 'GET', 
        })
            .then((resposta) => {
                return resposta.json();
            })
            .then((livros) => {
                setLivros(livros);
            });
    }, []);

    function deletar(id: string) {
        axios
            .delete(`http://localhost:5274/biblioteca/livro/deletar/${id}`)
            .then((resposta) => {
                console.log(resposta.data);
            });
    }

    return (
        <div className="container">
            <h1>Lista de Livros</h1>
            <table>
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Título</th>
                        <th>Autor</th>
                        <th>Gênero</th>
                        <th>Editora</th>
                        <th>Ano de Lançamento</th>
                        <th>Quantidade de Exemplares</th>
                        <th>Criado Em</th>
                        <th>Deletar</th>
                        <th>Alterar</th>
                    </tr>
                </thead>
                <tbody>
                    {livros.map((livro) => (
                        <tr key={livro.livroId}>
                            <td>{livro.livroId}</td>
                            <td>{livro.titulo}</td>
                            <td>{livro.autor?.nome + ' ' + livro.autor?.sobrenome}</td>
                            <td>{livro.genero}</td>
                            <td>{livro.editora}</td>
                            <td>{livro.anoLancamento}</td>
                            <td>{livro.qtdExemplares}</td>
                            <td>{livro.criadoEm}</td>
                            <td>
                                <button onClick={() => deletar(livro.livroId!)}>
                                    Deletar
                                </button> 
                            </td>
                            <td>
                                <Link to={`/pages/livro/alterar/${livro.livroId}`}>
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

export default LivroLista;