// src/services/ticketService.js
import axios from 'axios';

const API_URL = 'https://localhost:7135/api/ticket';

export const getTicketById = async (id) => {
    const response = await axios.get(`${API_URL}/${id}`);
    return response.data;
};

export const getTicketsByEventId = async (eventId) => {
    const response = await axios.get(`${API_URL}/${eventId}`);
    return response.data;
};

export const addTicket = async (ticket) => {
    const response = await axios.post(API_URL, ticket);
    return response.data;
};

export const updateTicket = async (id, ticket) => {
    const response = await axios.put(`${API_URL}/${id}`, ticket);
    return response.data;
};

export const deleteTicket = async (id) => {
    const response = await axios.delete(`${API_URL}/${id}`);
    return response.data;
};

export const checkTicketAvailability = async (eventId) => {
    const response = await axios.get(`${API_URL}/${eventId}/available`);
    return response.data;
};

export const sellTicket = async (id, quantity) => {
    const response = await axios.put(`${API_URL}/${id},${quantity}/sell`);
    return response.data;
};

export const refundTicket = async (id, quantity) => {
    const response = await axios.put(`${API_URL}/${id},${quantity}/refund`);
    return response.data;
};
