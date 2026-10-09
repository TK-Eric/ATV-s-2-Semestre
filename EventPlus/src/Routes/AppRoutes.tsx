import { Navigate, Route, Routes } from "react-router-dom";
import Home from "../pages/Home/Home"
import Eventos from "../pages/Eventos/Eventos";

function AppRoutes() {
    return (
        <Routes>
            <Route path="/Home" element={<Home/>}/>
            <Route path="/Eventos" element={<Eventos/>}/>
            <Route path="*" element={<Navigate to="/Home" replace/>}/>
        </Routes>
    )
}

export default AppRoutes;