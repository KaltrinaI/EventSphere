// src/components/Tickets/SellTicketForm.js
import React, { useState } from 'react';
import { TextField, Button, Paper, Typography } from '@mui/material';
import { sellTicket } from '../../services/ticketService';

const SellTicketForm = () => {
    const [ticketId, setTicketId] = useState('');
    const [quantity, setQuantity] = useState('');
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const handleSell = async () => {
        if (quantity <= 0) {
            setError('Quantity must be greater than zero');
            return;
        }

        try {
            await sellTicket(ticketId, quantity);
            setSuccess('Tickets sold successfully');
            setError('');
        } catch (error) {
            setError('Error selling tickets');
            setSuccess('');
        }
    };

    return (
        <Paper style={{ padding: 16, marginBottom: 16 }}>
            <Typography variant="h6" gutterBottom>
                Sell Ticket
            </Typography>
            <TextField
                label="Ticket ID"
                value={ticketId}
                onChange={(e) => setTicketId(e.target.value)}
                fullWidth
                margin="normal"
            />
            <TextField
                label="Quantity"
                value={quantity}
                onChange={(e) => setQuantity(e.target.value)}
                fullWidth
                margin="normal"
            />
            <Button onClick={handleSell} variant="contained" color="primary">
                Sell
            </Button>
            {error && <Typography color="error">{error}</Typography>}
            {success && <Typography color="success">{success}</Typography>}
        </Paper>
    );
};

export default SellTicketForm;
