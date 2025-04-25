import React from 'react';
import { Link } from 'react-router-dom';

export default function Home() {
    return (
        <div className="min-h-screen bg-gray-100 flex flex-col items-center justify-center">
            <h1 className="text-4xl font-bold mb-4">Ласкаво просимо до Places App</h1>
            <p className="text-lg mb-8">Досліджуйте та діліться своїми улюбленими місцями!</p>
            <div className="space-x-4">
                <Link to="/login" className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600">
                    Увійти
                </Link>
                <Link to="/register" className="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600">
                    Зареєструватися
                </Link>
            </div>
        </div>
    );
}