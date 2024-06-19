// src/components/Events/SearchEventsByOrganizerId.js
import React, { useState } from 'react';
import { TextField, Button, Paper, Typography } from '@mui/material';
import { getEventsByOrganizerId } from '../../services/eventService';

const SearchEventsByOrganizerId = ({ onResult }) => {
    const [organizerId, setOrganizerId] = useState('');
    const [error, setError] = useState('');

    const handleSearch = async () => {
        try {
            const result = await getEventsByOrganizerId(organizerId);
            onResult(result);
            setError('');
        } catch (error) {
            setError('Events not found for this organizer');
        }
    };

    return (
        <Paper style={{ padding: 16, marginBottom: 16 }}>
            <Typography variant="h6" gutterBottom>
                Search Events by Organizer ID
            </Typography>
            <TextField
                label="Organizer ID"
                value={organizerId}
                onChange={(e) => setOrganizerId(e.target.value)}
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

export default SearchEventsByOrganizerId;
