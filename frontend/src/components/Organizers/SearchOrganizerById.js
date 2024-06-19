// src/components/Organizers/SearchOrganizerById.js
import React, { useState } from 'react';
import { TextField, Button, Paper, Typography } from '@mui/material';
import { getOrganizerById } from '../../services/organizerService';

const SearchOrganizerById = ({ onResult }) => {
    const [id, setId] = useState('');
    const [error, setError] = useState('');

    const handleSearch = async () => {
        try {
            const result = await getOrganizerById(id);
            onResult(result);
            setError('');
        } catch (error) {
            setError('Organizer not found');
        }
    };

    return (
        <Paper style={{ padding: 16, marginBottom: 16 }}>
            <Typography variant="h6" gutterBottom>
                Search Organizer by ID
            </Typography>
            <TextField
                label="Organizer ID"
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

export default SearchOrganizerById;
