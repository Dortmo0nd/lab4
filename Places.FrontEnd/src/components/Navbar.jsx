// src/components/Navbar.jsx
import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export default function Navbar() {
    const { user, logout } = useAuth();

    return (
        <nav className="bg-blue-600 text-white p-4">
            <div className="container mx-auto flex justify-between items-center">
                <Link to="/" className="text-xl font-bold">Places App</Link>
                <div className="space-x-4">
                    <Link to="/" className="hover:underline">Home</Link>
                    {user ? (
                        <>
                            {user.role === 'Admin' && (
                                <Link to="/admin" className="hover:underline">Admin Dashboard</Link>
                            )}
                            <button onClick={logout} className="hover:underline">Logout</button>
                        </>
                    ) : (
                        <>
                            <Link to="/login" className="hover:underline">Login</Link>
                            <Link to="/register" className="hover:underline">Register</Link>
                        </>
                    )}
                </div>
            </div>
        </nav>
    );
}