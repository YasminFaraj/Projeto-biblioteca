import { useEffect, useState } from "react";
import { Livro } from "../../../models/Livro";
import { Autor } from "../../../models/Autor";
import styles from "./LivroLista.module.css";
import { Link } from "react-router-dom";
import axios from "axios";

function LivroLista(){
    const [livros, setLivros] = useState<Livro[]>([]);

    useEffect(() => {
        fetch("http://localhost:5274/biblioteca/livro/listar")
            .then((resposta) => {
                return resposta.json();
            })
            .then((livros) => {
                setLivros(livros);
            });
    })

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
            <table className={styles.table}>
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Título</th>
                        <th>Autor</th>
                        <th>Editora</th>
                        <th>Criado Em</th>
                        <th>Deletar</th>
                        <th>Alterar</th>
                    </tr>
                </thead>
                <tbody>
                    {livros.map((livro) => (
                        <tr key={livro.id}>
                            <td>{livro.id}</td>
                            <td>{livro.titulo}</td>
                            <td>{livro.editora}</td>
                            <td>{livro.autor?.nome}</td>
                            <td>{livro.criadoEm}</td>
                            <td>
                                <button onClick={() => deletar(livro.id!)}>
                                    Deletar
                                </button>
                            </td>
                            <td>
                                <Link to={`/pages/livro/alterar/${livro.id}`}>
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