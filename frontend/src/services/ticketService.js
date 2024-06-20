import axios from 'axios';
import { authHeader } from './authService'


const API_URL = 'https://localhost:7135/api/ticket';

export const getTicketById = async (id) => {
    try{
        const response = await axios.get(`${API_URL}/${id}`, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
};

export const getTicketsByEventId = async (eventId) => {
    try {
        const response = await axios.get(`${API_URL}/ticketsByEvent/${eventId}`, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
};

export const addTicket = async (ticket) => {
    try{
        const response = await axios.post(API_URL, ticket, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
};

export const updateTicket = async (id, ticket) => {
    try{
        const response = await axios.put(`${API_URL}/${id}`, ticket, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
};

export const deleteTicket = async (id) => {
    try{
        const response = await axios.delete(`${API_URL}/${id}`, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
};

export const checkTicketAvailability = async (eventId) => {
    try {
        const response = await axios.get(`${API_URL}/available/${eventId}`, { headers: authHeader() });
        return response.data;
    }  catch (error) {
        alert(getErrorMessage(error));
    }
};

export const sellTicket = async (id, quantity) => {
    try {
        const response = await axios.patch(`${API_URL}/sell/${id},${quantity}`, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
};

export const refundTicket = async (id, quantity) => {
    try{
        const response = await axios.patch(`${API_URL}/refund/${id},${quantity}`, { headers: authHeader() });
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
