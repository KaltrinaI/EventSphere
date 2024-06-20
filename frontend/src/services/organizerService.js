// src/services/organizerService.js
import axios from 'axios';
import { authHeader } from './authService'


const API_URL = 'https://localhost:7135/api/organizer';

export const getAllOrganizers = async () => {
    try{
        const response = await axios.get(API_URL, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
};

export const getOrganizerById = async (id) => {
    try {
        const response = await axios.get(`${API_URL}/${id}`, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
};

export const createOrganizer = async (organizer) => {
    try {
        const response = await axios.post(API_URL, organizer, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
};

export const updateOrganizer = async (id, organizer) => {
    try{
        const response = await axios.put(`${API_URL}/${id}`, organizer, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
};

export const deleteOrganizer = async (id) => {
    try{
        const response = await axios.delete(`${API_URL}/${id}`, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
};

function getErrorMessage(error) {
    if (error.response) {
        // Server responded with a status other than 2xx
        return `Response error: ${error.response.data}\nStatus code: ${error.response.status}`;
    } else if (error.request) {
        // Request was made but no response was received
        return `Request error: ${error.request}`;
    } else {
        // Something happened in setting up the request
        return `Error: ${error.message}`;
    }
}
