import { Link, NavLink } from "react-router-dom";
import "./Header.css";
import logoEvent from "../../assets/logo-event.svg";
import { useAuth, perfilEhAdministrador } from "../../context/Auth";

function Header() {
    const { autenticado, usuario, perfil, logout } = useAuth();
    const ehAdmin =
        autenticado &&
        (perfilEhAdministrador(perfil) ||
            perfilEhAdministrador(usuario?.idTipoUsuarioNavigation?.titulo) ||
            perfilEhAdministrador(usuario?.idTipoUsuarioNavigation?.tituloTipoUsuario));

    return (
        <header className="header">
            <div className="header-conteudo">
                <Link to="/home" className="header-logo">
                    <img src={logoEvent} alt="Logo Event+" />
                </Link>

                <nav className="header-nav" aria-label="Navegação principal">
                    <NavLink to="/home">Home</NavLink>
                    <NavLink to="/eventos">Eventos</NavLink>
                    {ehAdmin && (
                        <>
                            <NavLink to="/tipo-eventos">Tipo de Eventos</NavLink>
                            <NavLink to="/tipo-usuarios">Usuários</NavLink>
                            <NavLink to="/CadastroEvento">Cadastro de Eventos</NavLink>
                        </>
                    )}
                    <a href="/home#contato">Contatos</a>
                </nav>

                <div className="header-acoes">
                    {autenticado ? (
                        <>
                            <span className="header-usuario-nome">
                                {usuario?.nome || (ehAdmin ? "Administrador" : "Usuário")}
                            </span>
                            <Link
                                onClick={logout}
                                to="/login"
                                className="header-botao-login header-botao-sair"
                            >
                                Sair
                            </Link>
                        </>
                    ) : (
                        <Link to="/login" className="header-botao-login">
                            Entrar
                        </Link>
                    )}
                </div>
            </div>
        </header>
    );
}

export default Header;
