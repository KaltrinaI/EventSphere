// src/components/Tickets/TicketList.js
import React, { useEffect, useState } from 'react';
import { getTicketsByEventId, deleteTicket } from '../../services/ticketService';
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Button } from '@mui/material';

const TicketList = ({ tickets: initialTickets, eventId, onEdit }) => {
    const [tickets, setTickets] = useState(initialTickets || []);

    useEffect(() => {
        if (!initialTickets && eventId) {
            fetchTickets();
        }
    }, [initialTickets, eventId]);

    const fetchTickets = async () => {
        const data = await getTicketsByEventId(eventId);
        setTickets(data);
    };

    const handleDelete = async (id) => {
        const confirmDelete = window.confirm("Are you sure you want to delete this ticket?");
        if (confirmDelete) {
            try {
                await deleteTicket(id);
                if (!initialTickets) {
                    fetchTickets();
                } else {
                    setTickets(tickets.filter(ticket => ticket.ticketId !== id));
                }
            } catch (error) {
                console.error('Error deleting ticket:', error);
            }
        }
    };

    return (
        <TableContainer component={Paper}>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell>ID</TableCell>
                        <TableCell>Event ID</TableCell>
                        <TableCell>Price</TableCell>
                        <TableCell>Type</TableCell>
                        <TableCell>Quantity Available</TableCell>
                        <TableCell>Actions</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {tickets.map((ticket) => (
                        <TableRow key={ticket.ticketId}>
                            <TableCell>{ticket.ticketId}</TableCell>
                            <TableCell>{ticket.eventId}</TableCell>
                            <TableCell>{ticket.price}</TableCell>
                            <TableCell>{ticket.ticketType}</TableCell>
                            <TableCell>{ticket.quantityAvailable}</TableCell>
                            <TableCell>
                                <Button onClick={() => onEdit(ticket)}>Edit</Button>
                                <Button onClick={() => handleDelete(ticket.ticketId)}>Delete</Button>
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
};

export default TicketList;
