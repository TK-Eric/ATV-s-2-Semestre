import type { ReactElement } from "react";
import { Navigate, Route, Routes, useLocation } from "react-router-dom";
import Home from "../pages/Home/Home";
import Eventos from "../pages/Eventos/Eventos";
import Login from "../pages/login/Login";
import DetalhesEvento from "../components/DetalhesEvento/DetalhesEvento";
import CadastroEvento from "../pages/CadastroEventos/CadastroEventos";
import TipoEventos from "../pages/TipoEventos/TipoEventos";
import TipoUsuarios from "../pages/TipoUsuarios/TipoUsuarios";
import { useAuth, perfilEhAdministrador } from "../context/Auth";

function RotaAdministrador({ children }: { children: ReactElement }) {
    const { autenticado, usuario, perfil, carregandoSessao } = useAuth();
    const location = useLocation();

    if (carregandoSessao) {
        return null;
    }

    if (!autenticado) {
        return <Navigate to="/login" state={{ from: location.pathname }} replace />;
    }

    const ehAdmin =
        perfilEhAdministrador(perfil) ||
        perfilEhAdministrador(usuario?.idTipoUsuarioNavigation?.titulo) ||
        perfilEhAdministrador(usuario?.idTipoUsuarioNavigation?.tituloTipoUsuario);

    if (!ehAdmin) {
        return <Navigate to="/home" replace />;
    }

    return children;
}

function AppRoutes() {
    return (
        <Routes>
            <Route path="/" element={<Navigate to="/home" replace />} />
            <Route path="/home" element={<Home />} />
            <Route path="/Home" element={<Navigate to="/home" replace />} />
            <Route path="/eventos" element={<Eventos />} />
            <Route path="/Eventos" element={<Navigate to="/eventos" replace />} />
            <Route path="/eventos/:id" element={<DetalhesEvento />} />
            <Route path="/Eventos/:id" element={<DetalhesEvento />} />

            {/* Rotas restritas ao Administrador */}
            <Route
                path="/CadastroEvento"
                element={
                    <RotaAdministrador>
                        <CadastroEvento />
                    </RotaAdministrador>
                }
            />
            <Route
                path="/cadastro-eventos"
                element={
                    <RotaAdministrador>
                        <CadastroEvento />
                    </RotaAdministrador>
                }
            />
            <Route
                path="/tipo-eventos"
                element={
                    <RotaAdministrador>
                        <TipoEventos />
                    </RotaAdministrador>
                }
            />
            <Route
                path="/TipoEventos"
                element={
                    <RotaAdministrador>
                        <TipoEventos />
                    </RotaAdministrador>
                }
            />
            <Route
                path="/tipo-evento"
                element={
                    <RotaAdministrador>
                        <TipoEventos />
                    </RotaAdministrador>
                }
            />
            <Route
                path="/TipoEvento"
                element={
                    <RotaAdministrador>
                        <TipoEventos />
                    </RotaAdministrador>
                }
            />
            <Route
                path="/tipo-usuarios"
                element={
                    <RotaAdministrador>
                        <TipoUsuarios />
                    </RotaAdministrador>
                }
            />
            <Route
                path="/TipoUsuarios"
                element={
                    <RotaAdministrador>
                        <TipoUsuarios />
                    </RotaAdministrador>
                }
            />
            <Route
                path="/tipo-usuario"
                element={
                    <RotaAdministrador>
                        <TipoUsuarios />
                    </RotaAdministrador>
                }
            />
            <Route
                path="/TipoUsuario"
                element={
                    <RotaAdministrador>
                        <TipoUsuarios />
                    </RotaAdministrador>
                }
            />
            <Route
                path="/usuarios"
                element={
                    <RotaAdministrador>
                        <TipoUsuarios />
                    </RotaAdministrador>
                }
            />

            <Route path="/login" element={<Login />} />
            <Route path="/Login" element={<Navigate to="/login" replace />} />
            <Route path="*" element={<Navigate to="/home" replace />} />
        </Routes>
    );
}

export default AppRoutes;
