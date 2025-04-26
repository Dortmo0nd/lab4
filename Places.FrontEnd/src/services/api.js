import axios from 'axios';

const API_URL = 'http://localhost:5215/api'; // Адреса вашого API з launchSettings.json

export const getPlaces = () => axios.get(`${API_URL}/Places`);
export const getPlaceById = (id) => axios.get(`${API_URL}/Places/${id}`);
export const createPlace = (place) => axios.post(`${API_URL}/Places`, place);
export const updatePlace = (id, place) => axios.put(`${API_URL}/Places/${id}`, place);
export const deletePlace = (id) => axios.delete(`${API_URL}/Places/${id}`);