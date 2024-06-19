// src/services/organizerService.js
import axios from 'axios';

const API_URL = 'https://localhost:7135/api/organizer';

export const getAllOrganizers = async () => {
    const response = await axios.get(API_URL);
    return response.data;
};

export const getOrganizerById = async (id) => {
    const response = await axios.get(`${API_URL}/${id}`);
    return response.data;
};

export const createOrganizer = async (organizer) => {
    const response = await axios.post(API_URL, organizer);
    return response.data;
};

export const updateOrganizer = async (id, organizer) => {
    const response = await axios.put(`${API_URL}/${id}`, organizer);
    return response.data;
};

export const deleteOrganizer = async (id) => {
    const response = await axios.delete(`${API_URL}/${id}`);
    return response.data;
};
