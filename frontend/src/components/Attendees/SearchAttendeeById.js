// src/components/SearchAttendeeById.js
import React, { useState } from 'react';
import { TextField, Button, Paper, Typography } from '@mui/material';
import { getAttendeeById } from '../../services/attendeeService';

const SearchAttendeeById = ({ onResult }) => {
    const [id, setId] = useState('');
    const [error, setError] = useState('');

    const handleSearch = async () => {
        try {
            const result = await getAttendeeById(id);
            onResult(result);
            setError('');
        } catch (error) {
            setError('Attendee not found');
        }
    };

    return (
        <Paper style={{ padding: 16, marginBottom: 16 }}>
            <Typography variant="h6" gutterBottom>
                Search Attendee by ID
            </Typography>
            <TextField
                label="Attendee ID"
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

export default SearchAttendeeById;
