import React, { useState } from 'react';
import { TextField, Button, Paper, Typography } from '@mui/material';
import { getTicketsByEventId } from '../../services/ticketService';

const SearchTicketsByEventId = ({ onResult }) => {
    const [eventId, setEventId] = useState('');
    const [error, setError] = useState('');

    const handleSearch = async () => {
        try {
            const result = await getTicketsByEventId(eventId);
            onResult(result);
            setError('');
        } catch (error) {
            setError('Tickets not found for this event');
        }
    };

    return (
        <Paper style={{ padding: 16, marginBottom: 16 }}>
            <Typography variant="h6" gutterBottom>
                Search Tickets by Event ID
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

export default SearchTicketsByEventId;
