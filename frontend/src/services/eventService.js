// src/services/eventService.js
import axios from 'axios';
import { authHeader } from './authService'


const API_URL = 'https://localhost:7135/api/event';

export const getAllEvents = async () => {
    try{
        const response = await axios.get(API_URL, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
};

export const getEventById = async (id) => {
    try{
        const response = await axios.get(`${API_URL}/eventById/${id}`, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
};

export const getEventsByOrganizerId = async (organizerId) => {
    try{
        const response = await axios.get(`${API_URL}/eventByOrganizer/${organizerId}`, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error)); 
    }
};

export const addEvent = async (event) => {
    try{
        const response = await axios.post(API_URL, event, { headers: authHeader() });
        return response.data;
    }catch (error) {
        alert(getErrorMessage(error));
    }
};

export const updateEvent = async (eventId, event) => {
    try{
        const response = await axios.put(`${API_URL}/${eventId}`, event, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
};

export const deleteEvent = async (id) => {
    try{
        const response = await axios.delete(`${API_URL}/${id}`, { headers: authHeader() });
        return response.data;
    } catch (error) {
        alert(getErrorMessage(error));
    }
};

export const getUpcomingEventsSortedByPopularity = async () => {
    try{
        const response = await axios.get(`${API_URL}/upcoming/popularity`, { headers: authHeader() });
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
