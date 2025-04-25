import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

export default function Register() {
    const [fullName, setFullName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState(null);
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!fullName || !email || !password) {
            setError('Будь ласка, заповніть усі поля');
            return;
        }

        try {
            await axios.post('http://localhost:5069/api/users', {
                full_name: fullName,
                email: email,
                password: password,
                role: 0 // Visitor role
            });
            setFullName('');
            setEmail('');
            setPassword('');
            setError(null);
            navigate('/login');
        } catch (err) {
            setError('Не вдалося зареєструватися. Спробуйте ще раз.');
        }
    };

    return (
        <div className="min-h-screen bg-gray-100 flex items-center justify-center">
            <div className="bg-white p-8 rounded-lg shadow-md w-full max-w-md">
                <h2 className="text-2xl font-semibold mb-6 text-center">Реєстрація</h2>
                {error && <p className="text-red-500 mb-4 text-center">{error}</p>}
                <div className="space-y-4">
                    <div>
                        <label className="block text-sm font-medium mb-1">Повне ім'я</label>
                        <input
                            type="text"
                            value={fullName}
                            onChange={(e) => setFullName(e.target.value)}
                            placeholder="Введіть ваше повне ім'я"
                            className="w-full border p-2 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
                            required
                        />
                    </div>
                    <div>
                        <label className="block text-sm font-medium mb-1">Електронна пошта</label>
                        <input
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            placeholder="Введіть вашу електронну пошту"
                            className="w-full border p-2 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
                            required
                        />
                    </div>
                    <div>
                        <label className="block text-sm font-medium mb-1">Пароль</label>
                        <input
                            type="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            placeholder="Введіть ваш пароль"
                            className="w-full border p-2 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
                            required
                        />
                    </div>
                    <button
                        onClick={handleSubmit}
                        className="w-full bg-green-500 text-white p-2 rounded hover:bg-green-600"
                    >
                        Зареєструватися
                    </button>
                </div>
                <p className="mt-4 text-center">
                    Вже маєте акаунт?{' '}
                    <Link to="/login" className="text-blue-500 hover:underline">
                        Увійти
                    </Link>
                </p>
            </div>
        </div>
    );
}