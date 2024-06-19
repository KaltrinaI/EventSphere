// src/services/authService.js
import axios from 'axios';
import jwtDecode from 'jwt-decode';

const API_URL = 'https://localhost:7135/api/auth';

export const login = async (email, password) => {
    const response = await axios.post(`${API_URL}/login`, { email, password });
    localStorage.setItem('token', response.data.token);
    return response.data;
};

export const register = async (user) => {
    const response = await axios.post(`${API_URL}/register`, user);
    return response.data;
};

export const getToken = () => localStorage.getItem('token');

export const getUserRole = () => {
    const token = getToken();
    if (!token) return null;
    const decoded = jwtDecode(token);
    return decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
};

export const getUsername = () => {
    const token = getToken();
    if (!token) return null;
    const decoded = jwtDecode(token);
    return decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
};
