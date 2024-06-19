// src/services/eventService.js
import axios from 'axios';


const API_URL = 'https://localhost:7135/api/event';

export const getAllEvents = async () => {
    const response = await axios.get(API_URL);
    return response.data;
};

export const getEventById = async (id) => {
    const response = await axios.get(`${API_URL}/eventById/${id}`);
    return response.data;
};

export const getEventsByOrganizerId = async (organizerId) => {
    const response = await axios.get(`${API_URL}/eventByOrganizer/${organizerId}`);
    return response.data;
};

export const addEvent = async (event) => {
    const response = await axios.post(API_URL, event);
    return response.data;
};

export const updateEvent = async (eventId, event) => {
    const response = await axios.put(`${API_URL}/${eventId}`, event);
    return response.data;
};

export const deleteEvent = async (id) => {
    const response = await axios.delete(`${API_URL}/${id}`);
    return response.data;
};

export const getUpcomingEventsSortedByPopularity = async () => {
    const response = await axios.get(`${API_URL}/upcoming/popularity`);
    return response.data;
};
