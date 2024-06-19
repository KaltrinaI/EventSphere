// src/components/Events/SearchEventById.js
import React, { useState } from 'react';
import { TextField, Button, Paper, Typography } from '@mui/material';
import { getEventById } from '../../services/eventService';

const SearchEventById = ({ onResult }) => {
    const [id, setId] = useState('');
    const [error, setError] = useState('');

    const handleSearch = async () => {
        try {
            const result = await getEventById(id);
            onResult(result);
            setError('');
        } catch (error) {
            setError('Event not found');
        }
    };

    return (
        <Paper style={{ padding: 16, marginBottom: 16 }}>
            <Typography variant="h6" gutterBottom>
                Search Event by ID
            </Typography>
            <TextField
                label="Event ID"
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

export default SearchEventById;
