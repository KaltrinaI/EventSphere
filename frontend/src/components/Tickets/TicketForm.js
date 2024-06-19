// src/components/Tickets/TicketForm.js
import React, { useState, useEffect } from 'react';
import { TextField, Button, Paper } from '@mui/material';
import { addTicket, updateTicket } from '../../services/ticketService';

const TicketForm = ({ selectedTicket, onSave }) => {
    const [ticket, setTicket] = useState({
        eventId: '',
        price: '',
        ticketType: '',
        quantityAvailable: '',
    });

    useEffect(() => {
        if (selectedTicket) {
            setTicket(selectedTicket);
        }
    }, [selectedTicket]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setTicket((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (ticket.ticketId) {
            await updateTicket(ticket.ticketId, ticket);
        } else {
            await addTicket(ticket);
        }
        onSave();
        setTicket({ eventId: '', price: '', ticketType: '', quantityAvailable: '' });
    };

    return (
        <Paper style={{ padding: 16 }}>
            <form onSubmit={handleSubmit}>
                <TextField
                    label="Event ID"
                    name="eventId"
                    value={ticket.eventId}
                    onChange={handleChange}
                    fullWidth
                    margin="normal"
                />
                <TextField
                    label="Price"
                    name="price"
                    value={ticket.price}
                    onChange={handleChange}
                    fullWidth
                    margin="normal"
                />
                <TextField
                    label="Ticket Type"
                    name="ticketType"
                    value={ticket.ticketType}
                    onChange={handleChange}
                    fullWidth
                    margin="normal"
                />
                <TextField
                    label="Quantity Available"
                    name="quantityAvailable"
                    value={ticket.quantityAvailable}
                    onChange={handleChange}
                    fullWidth
                    margin="normal"
                />
                <Button type="submit" variant="contained" color="primary" style={{ marginTop: 16 }}>
                    Save
                </Button>
            </form>
        </Paper>
    );
};

export default TicketForm;
