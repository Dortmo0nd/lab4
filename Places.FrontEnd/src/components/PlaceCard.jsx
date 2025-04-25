export default function PlaceCard({ place, onDelete }) {
    return (
        <div className="bg-white p-4 rounded-lg shadow-md">
            <h3 className="text-lg font-bold">{place.name}</h3>
            <p>{place.description || 'No description'}</p>
            <p>Latitude: {place.latitude}</p>
            <p>Longitude: {place.longitude}</p>
            <button onClick={() => onDelete(place.id)} className="bg-red-500 text-white p-2 rounded mt-2 hover:bg-red-600">Delete</button>
        </div>
    );
}