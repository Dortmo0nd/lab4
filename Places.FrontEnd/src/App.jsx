import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import PlaceDetails from './pages/PlaceDetails';
import Login from './pages/Login';
import Register from './pages/Register'; // Додаємо сторінку реєстрації
import AdminDashboard from './pages/AdminDashboard';

export default function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/place/:id" element={<PlaceDetails />} />
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} /> {/* Новий маршрут */}
                <Route path="/admin" element={<AdminDashboard />} />
            </Routes>
        </BrowserRouter>
    );
}