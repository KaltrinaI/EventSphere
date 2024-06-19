// src/components/SearchAttendeeByEventId.js
import React, { useState } from 'react';
import { TextField, Button, Paper, Typography } from '@mui/material';
import { getAttendeesByEvent } from '../../services/attendeeService';

const SearchAttendeeByEventId = ({ onResult }) => {
    const [eventId, setEventId] = useState('');
    const [error, setError] = useState('');

    const handleSearch = async () => {
        try {
            const result = await getAttendeesByEvent(eventId);
            onResult(result);
            setError('');
        } catch (error) {
            setError('No attendees found for this event');
        }
    };

    return (
        <Paper style={{ padding: 16, marginBottom: 16 }}>
            <Typography variant="h6" gutterBottom>
                Search Attendees by Event ID
            </Typography>
            <TextField
                label="Event ID"
                value={eventId}
                onChange={(e) => setEventId(e.target.value)}
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

export default SearchAttendeeByEventId;
