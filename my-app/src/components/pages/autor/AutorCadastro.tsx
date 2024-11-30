import { useState } from "react";
import { Autor } from "../../../models/Autor";

function AutorCadastro() {
    const [nome, setNome] = useState("");
    const [sobrenome, setSobrenome] = useState("");
    const [pais, setPais] = useState("");

    function enviarAutor(e: any) {
        e.preventDefault();

        const autor: Autor = {
            nome: nome,
            sobrenome: sobrenome,
            pais: pais,
        };

        fetch("http://localhost:5274/biblioteca/autor/cadastrar", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(autor),
        })
        .then((resposta) => {
            return resposta.json();
        })
        .then((autor) => {
            console.log("Autor cadastrado", autor);
        });
    }

    return(
        <div id="cadastrar_autor" className="container">
            <h1>Cadastrar Autor</h1>
            <form onSubmit={enviarAutor}>
                <div>
                    <label htmlFor="nome">Nome</label>
                    <input 
                        type="text" 
                        id="nome" 
                        name="nome" 
                        value={nome} 
                        required
                        onChange={(e: any) => setNome(e.target.value)}
                    />
                </div>

                <div>
                    <label htmlFor="sobrenome">Sobrenome</label>
                    <input 
                        type="text" 
                        name="sobrenome" 
                        id="sobrenome"
                        value={sobrenome}
                        required
                        onChange={(e: any) => setSobrenome(e.target.value)} 
                    />
                </div>

                <div>
                    <label htmlFor="pais">País</label>
                    <input 
                        type="text" 
                        name="pais" 
                        id="pais"
                        value={pais}
                        required
                        onChange={(e: any) => setPais(e.target.value)} 
                    />
                </div>

                <button type="submit">Cadastrar Autor</button>
            </form>
        </div>
    );
}

export default AutorCadastro;