import { useEffect, useState } from "react";
import { Livro } from "../../../models/Livro";
import { Autor } from "../../../models/Autor";
import axios from "axios";
import { useParams } from "react-router-dom";

function LivroCadastro() {
    const { id } = useParams();
    const [autores, setAutores] = useState<Autor[]>([]);
    const [titulo, setTitulo] = useState("");
    const [genero, setGenero] = useState("");
    const [qtdExemplares, setQtdExemplares] = useState(0);
    const [anoLancamento, setAnoLancamento] = useState(0);
    const [editora, setEditora] = useState("");
    const [autorId, setAutorId] = useState(0);

    useEffect(() => {
        axios
            .get<Autor[]>("http://localhost:5274/biblioteca/autor/listar")
            .then((resposta) => {
                setAutores(resposta.data);
            })
    });

    function enviarLivro(e: any) {
        e.preventDefault();

        const livro: Livro = {
            titulo: titulo,
            genero: genero,
            qtdExemplares: Number(qtdExemplares),
            anoLancamento: Number(anoLancamento),
            editora: editora,
            autorId: autorId,
        };

        fetch("http://localhost:5274/biblioteca/livro/cadastrar", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(livro),
        })
        .then((resposta) => {
            return resposta.json();
        })
        .then((livro) => {
            console.log("Livro cadastrado", livro);
        });
    }

    return(
        <div id="cadastrar_livro" className="container">
            <h1>Cadastrar Livro</h1>
            <form onSubmit={enviarLivro}>
                <div>
                    <label htmlFor="titulo">Título</label>
                    <input 
                        type="text" 
                        id="titulo" 
                        name="titulo" 
                        value={titulo} 
                        required
                        onChange={(e: any) => setTitulo(e.target.value)}
                    />
                </div>

                <div>
                    <label htmlFor="genero">Gênero</label>
                    <input 
                        type="text" 
                        name="genero" 
                        id="genero"
                        value={genero}
                        required
                        onChange={(e: any) => setGenero(e.target.value)} 
                    />
                </div>

                <div>
                    <label htmlFor="qtdExemplares">Quantidade de Exemplares</label>
                    <input 
                        type="number"
                        id="qtdExemplares" 
                        name="qtdExemplares"
                        value={qtdExemplares}
                        required
                        onChange={(e: any) => setQtdExemplares(e.target.value)}
                    />
                </div>

                <div>
                    <label htmlFor="anoLancamento">Ano de Lançamento</label>
                    <input 
                        type="number" 
                        id="anoLancamento"
                        name="anoLancamento"
                        value={anoLancamento}
                        onChange={(e: any) => setAnoLancamento(e.target.value)}
                    />
                </div>

                <div>
                    <label htmlFor="editora">Editora</label>
                    <input 
                        type="text" 
                        name="editora" 
                        id="editora"
                        value={editora}
                        onChange={(e: any) => setEditora(e.target.value)} 
                    />
                </div>

                <div>
                    <label htmlFor="autor">Autores</label>
                    <select onChange={(e: any) => setAutorId(e.target.value)}>
                        {autores.map((autor) => (
                            <option 
                                value={autor.autorId}
                                key={autor.autorId}
                            >
                                {autor.nome}
                            </option>
                        ))}
                    </select>
                </div>

                <button type="submit">Cadastrar Livro</button>
            </form>
        </div>
    );
}

export default LivroCadastro;