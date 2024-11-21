import React from "react";
import LivroLista from "./components/pages/livro/LivroLista"
import LivroCadastro from "./components/pages/livro/LivroCadastro"
import LivroAlterar from "./components/pages/livro/LivroAlterar"
import { BrowserRouter, Link, Route, Routes } from "react-router-dom";

function App() {
  return (
    <div id="app">
      <BrowserRouter>
        <nav>
          <ul>
            <li>
              <Link to="/">Home</Link>
            </li>
            <li>
              <Link to="/pages/livro/listar">Listar Livros</Link>
            </li>
            <li>
              <Link to="/pages/livro/cadastrar">Cadastrar Livros</Link>
            </li>
          </ul>
        </nav>
        <Routes>
          <Route path="/" element={<LivroLista />} />
          <Route path="/pages/livro/listar" element={<LivroLista />} />
          <Route path="/pages/livro/cadastrar" element={<LivroCadastro />} />
          <Route path="/pages/livro/alterar/:id" element={<LivroAlterar />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;

