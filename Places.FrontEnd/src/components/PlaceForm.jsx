import { useState } from 'react';
import { addPlace } from '../services/api';

export default function PlaceForm({ onPlaceAdded }) {
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [latitude, setLatitude] = useState('');
    const [longitude, setLongitude] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!name || !latitude || !longitude) {
            alert('Please fill in all required fields');
            return;
        }
        try {
            await addPlace({ name, description, latitude: parseFloat(latitude), longitude: parseFloat(longitude) });
            setName(''); setDescription(''); setLatitude(''); setLongitude('');
            onPlaceAdded();
        } catch (error) {
            alert('Failed to add place');
        }
    };

    return (
        <section className="mb-8 bg-white p-6 rounded-lg shadow-md">
            <h2 className="text-xl font-semibold mb-4">Add a New Place</h2>
            <form onSubmit={handleSubmit} className="grid grid-cols-1 gap-4">
                <input type="text" value={name} onChange={(e) => setName(e.target.value)} placeholder="Name" className="border p-2 rounded" />
                <textarea value={description} onChange={(e) => setDescription(e.target.value)} placeholder="Description" className="border p-2 rounded" rows="4" />
                <input type="number" value={latitude} onChange={(e) => setLatitude(e.target.value)} placeholder="Latitude" className="border p-2 rounded" step="any" />
                <input type="number" value={longitude} onChange={(e) => setLongitude(e.target.value)} placeholder="Longitude" className="border p-2 rounded" step="any" />
                <button type="submit" className="bg-blue-500 text-white p-2 rounded hover:bg-blue-600">Add Place</button>
            </form>
        </section>
    );
}