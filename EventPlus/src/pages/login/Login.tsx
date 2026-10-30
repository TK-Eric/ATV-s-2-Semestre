import { useEffect, useState, type FormEvent } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/Auth";
import logoEvent from "../../assets/logo-event.svg";
import Header from "../../components/Header/Header";
import Footer from "../../components/Footer/footer";
import Botao from "../../components/Botao/Botao";
import { getApiErrorMessage } from "../../services/api";
import "./Login.css";

function Login() {
    const navigate = useNavigate();
    const location = useLocation();
    const { login, autenticado } = useAuth();

    const [email, setEmail] = useState("");
    const [senha, setSenha] = useState("");
    const [erro, setErro] = useState("");
    const [enviando, setEnviando] = useState(false);

    const destino =
        (location.state as { from?: string } | null)?.from || "/home";

    useEffect(() => {
        if (autenticado) navigate(destino, { replace: true });
    }, [autenticado, destino, navigate]);

    async function entrar(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();
        setErro("");
        setEnviando(true);

        try {
            await login({ email: email.trim(), senha });
            navigate(destino, { replace: true });
        } catch (error) {
            setErro(getApiErrorMessage(error));
        } finally {
            setEnviando(false);
        }
    }

    return (
        <div className="pagina-login">
            <Header />

            <main className="login-container">
                <section className="login-card">
                    <img src={logoEvent} alt="Logo Event+" className="login-logo" />
                    <h1 className="login-titulo">Acesse sua conta</h1>

                    <form className="login-form" onSubmit={entrar} noValidate>
                        <div className="login-campo">
                            <label htmlFor="login-email">Email</label>
                            <input
                                id="login-email"
                                className="campo"
                                type="email"
                                value={email}
                                onChange={(event) => setEmail(event.target.value)}
                                placeholder="Digite seu email..."
                                autoComplete="email"
                                required
                            />
                        </div>

                        <div className="login-campo">
                            <label htmlFor="login-senha">Senha</label>
                            <input
                                id="login-senha"
                                className="campo"
                                type="password"
                                value={senha}
                                onChange={(event) => setSenha(event.target.value)}
                                placeholder="Digite sua senha..."
                                autoComplete="current-password"
                                minLength={8}
                                required
                            />
                        </div>

                        {erro && (
                            <p className="login-erro" role="alert">
                                {erro}
                            </p>
                        )}

                        <Botao
                            className="login-botao"
                            texto={enviando ? "Entrando..." : "Entrar"}
                            type="submit"
                            disabled={enviando}
                        />
                    </form>
                </section>
            </main>

            <Footer />
        </div>
    );
}

export default Login;
