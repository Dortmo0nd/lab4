import axios from 'axios';

const api = axios.create({
    baseURL: 'http://localhost:5215/api', // URL вашого WebAPI
});

export const getPlaces = () => api.get('/places').then(res => res.data);
export const addPlace = (place) => api.post('/places', place).then(res => res.data);
export const deletePlace = (id) => api.delete(`/places/${id}`);
export const getReviews = () => api.get('/reviews').then(res => res.data);
export const addReview = (review) => api.post('/reviews', review).then(res => res.data);