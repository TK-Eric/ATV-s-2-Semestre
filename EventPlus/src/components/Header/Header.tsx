import { Link, NavLink } from "react-router-dom"
import "./Header.css"
import logoEvent from "../../assets/logo1.svg"

function Header() {
    return (
        <header className="header">
            <div className="header-conteudo">
                <Link to="/Home">
                    <img src={logoEvent} alt="Logo Event+" />
                </Link>

                <nav>
                    <NavLink to="/Home">Home</NavLink>
                    <NavLink to="/Eventos">Eventos</NavLink>
                    <NavLink to="/Usuarios">Usuarios</NavLink>
                    <Link to="#Contato">Contato</Link>
                </nav>

                <Link to="/Login" className="header-botao-login">Entrar</Link>
            </div>
        </header>
    )
}

export default Header;