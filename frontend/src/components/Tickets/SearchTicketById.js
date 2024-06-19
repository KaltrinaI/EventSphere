// src/components/Tickets/SearchTicketById.js
import React, { useState } from 'react';
import { TextField, Button, Paper, Typography } from '@mui/material';
import { getTicketById } from '../../services/ticketService';

const SearchTicketById = ({ onResult }) => {
    const [id, setId] = useState('');
    const [error, setError] = useState('');

    const handleSearch = async () => {
        try {
            const result = await getTicketById(id);
            onResult(result);
            setError('');
        } catch (error) {
            setError('Ticket not found');
        }
    };

    return (
        <Paper style={{ padding: 16, marginBottom: 16 }}>
            <Typography variant="h6" gutterBottom>
                Search Ticket by ID
            </Typography>
            <TextField
                label="Ticket ID"
                value={id}
                onChange={(e) => setId(e.target.value)}
                fullWidth
                margin="normal"
            />
            <Button onClick={handleSearch} variant="contained" color="primary">
                Search
            </Button>
            {error && <Typography color="error">{error}</Typography>}
        </Paper>
    );
};

export default SearchTicketById;
