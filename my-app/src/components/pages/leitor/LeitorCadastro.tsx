import { useState } from "react";
import { Leitor } from "../../../models/Leitor";

function LeitorCadastro() {
    const [nome, setNome] = useState("");
    const [sobrenome, setSobrenome] = useState("");
    const [telefone, setTelefone] = useState("");
    const [email, setEmail] = useState("");
    const [cpf, setCPF] = useState(0);

    function enviarLeitor(e: any) {
        e.preventDefault();

        const leitor: Leitor = {
            nome: nome,
            sobrenome: sobrenome,
            telefone: telefone,
            email: email,
            cpf: cpf,
        };

        fetch("http://localhost:5274/biblioteca/leitor/cadastrar", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(leitor),
        })
        .then((resposta) => {
            return resposta.json();
        })
        .then((leitor) => {
            console.log("Leitor cadastrado", leitor);
        });
    }

    return(
        <div id="cadastrar_leitor" className="container">
            <h1>Cadastrar Leitor</h1>
            <form onSubmit={enviarLeitor}>
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
                    <label htmlFor="telefone">Telefone</label>
                    <input 
                        type="text" 
                        name="telefone" 
                        id="telefone"
                        value={telefone}
                        required
                        onChange={(e: any) => setTelefone(e.target.value)} 
                    />
                </div>

                <div>
                    <label htmlFor="email">Email</label>
                    <input 
                        type="text" 
                        name="email" 
                        id="email"
                        value={email}
                        required
                        onChange={(e: any) => setEmail(e.target.value)} 
                    />
                </div>

                <div>
                    <label htmlFor="cpf">CPF</label>
                    <input 
                        type="number" 
                        name="cpf" 
                        id="cpf"
                        value={cpf}
                        required
                        onChange={(e: any) => setCPF(e.target.value)} 
                    />
                </div>

                <button type="submit">Cadastrar Leitor</button>
            </form>
        </div>
    );
}

export default LeitorCadastro;