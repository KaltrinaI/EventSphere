// src/services/attendeeService.js
import axios from 'axios';
import { authHeader } from './authService'

const API_URL = 'https://localhost:7135/api/attendee';

export const getAllAttendees = async () => {
    try{
        const response = await axios.get(API_URL, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
};

export const getAttendeeById = async (id) => {
    try{
        const response = await axios.get(`${API_URL}/${id}`, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
};

export const addAttendee = async (attendee) => {
    try{
        const response = await axios.post(API_URL, attendee, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
};

export const updateAttendee = async (id, attendee) => {
    try{
        const response = await axios.put(`${API_URL}/${id}`, attendee , { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
};

export const deleteAttendee = async (id) => {
    try{
        const response = await axios.delete(`${API_URL}/${id}`, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
    
};

export const getAttendeesByEvent = async (eventId) => {
    try{
        const response = await axios.get(`${API_URL}/attendeesByEvent/${eventId}`, { headers: authHeader() });
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
