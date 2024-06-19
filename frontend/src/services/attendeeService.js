// src/services/attendeeService.js
import axios from 'axios';

const API_URL = 'https://localhost:7135/api/attendee';

export const getAllAttendees = async () => {
    const response = await axios.get(API_URL);
    return response.data;
};

export const getAttendeeById = async (id) => {
    const response = await axios.get(`${API_URL}/${id}`);
    return response.data;
};

export const addAttendee = async (attendee) => {
    const response = await axios.post(API_URL, attendee,);
    return response.data;
};

export const updateAttendee = async (id, attendee) => {
    const response = await axios.put(`${API_URL}/${id}`, attendee);
    return response.data;
};

export const deleteAttendee = async (id) => {
    const response = await axios.delete(`${API_URL}/${id}`);
    return response.data;
};

export const getAttendeesByEvent = async (eventId) => {
    const response = await axios.get(`${API_URL}/attendeesByEvent/${eventId}`);
    return response.data;
};
