import React, { useState } from 'react';
import { TextField, Button, Paper, Typography } from '@mui/material';
import { refundTicket } from '../../services/ticketService';

const RefundTicketForm = () => {
    const [ticketId, setTicketId] = useState('');
    const [quantity, setQuantity] = useState('');
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const handleRefund = async () => {
        if (quantity <= 0) {
            setError('Quantity must be greater than zero');
            return;
        }

        try {
            await refundTicket(ticketId, quantity);
            setSuccess('Tickets refunded successfully');
            setError('');
        } catch (error) {
            setError('Error refunding tickets');
            setSuccess('');
        }
    };

    return (
        <Paper style={{ padding: 16, marginBottom: 16 }}>
            <Typography variant="h6" gutterBottom>
                Refund Ticket
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
            <Button onClick={handleRefund} variant="contained" color="primary">
                Refund
            </Button>
            {error && <Typography color="error">{error}</Typography>}
            {success && <Typography color="success">{success}</Typography>}
        </Paper>
    );
};

export default RefundTicketForm;
