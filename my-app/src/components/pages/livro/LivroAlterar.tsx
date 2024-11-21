import axios from "axios";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Livro } from "../../../models/Livro";
import { Autor } from "../../../models/Autor";

function LivroAlterar(){
    const { id } = useParams();
    const [autores, setAutores] = useState<Autor[]>([]);
    const [titulo, setTitulo] = useState("");
    const [genero, setGenero] = useState("");
    const [qtdExemplares, setQtdExemplares] = useState(0);
    const [anoLancamento, setAnoLancamento] = useState(0);
    const [editora, setEditora] = useState("");
    const [autorId, setAutorId] = useState(0);

    useEffect(() => {
        if(id) {
            axios
                .get<Livro>(
                    `http://localhost:5274/biblioteca/livro/buscar/${id}`
                )
                .then((resposta) => {
                    setTitulo(resposta.data.titulo);
                    setGenero(resposta.data.genero);
                    setQtdExemplares(resposta.data.qtdExemplares);
                    setAnoLancamento(resposta.data.anoLancamento);
                    setEditora(resposta.data.editora);
                    buscarAutores();
                })
        }
    }, []);

    function buscarAutores() {
        axios
            .get<Autor[]>("http://localhost:5274/biblioteca/autor/listar")
            .then((resposta) => {
                setAutores(resposta.data);
            })
    }

    function enviarLivro(e: any) {
        e.preventDefault();

        const livro : Livro = {
            titulo: titulo,
            genero: genero,
            qtdExemplares: Number(qtdExemplares),
            anoLancamento: Number(anoLancamento),
            editora: editora,
            autorId: autorId,
        };

        axios
            .put(`http://localhost:5274/biblioteca/livro/alterar/${id}`, livro)
            .then((resposta) =>{
                console.log(resposta.data);
            })
    }

    return (
        <div id="alterar_livro" className="container">
            <h1>Alterar Livro</h1>
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

                <button type="submit">Alterar Livro</button>
            </form>
        </div>
    );
}

export default LivroAlterar;