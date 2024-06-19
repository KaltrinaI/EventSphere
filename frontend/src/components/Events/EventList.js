// src/components/Events/EventList.js
import React, { useEffect, useState } from 'react';
import { getAllEvents, deleteEvent } from '../../services/eventService';
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Button } from '@mui/material';

const EventList = ({ events: initialEvents, onEdit }) => {
    const [events, setEvents] = useState(initialEvents || []);

    useEffect(() => {
        if (!initialEvents) {
            fetchEvents();
        }
    }, [initialEvents]);

    const fetchEvents = async () => {
        const data = await getAllEvents();
        setEvents(data);
    };

    const handleDelete = async (id) => {
        const confirmDelete = window.confirm("Are you sure you want to delete this event?");
        if (confirmDelete) {
            try {
                await deleteEvent(id);
                if (!initialEvents) {
                    fetchEvents();
                } else {
                    setEvents(events.filter(event => event.eventId !== id));
                }
            } catch (error) {
                console.error('Error deleting event:', error);
            }
        }
    };

    return (
        <TableContainer component={Paper}>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell>ID</TableCell>
                        <TableCell>Name</TableCell>
                        <TableCell>Description</TableCell>
                        <TableCell>Start Date</TableCell>
                        <TableCell>End Date</TableCell>
                        <TableCell>Location</TableCell>
                        <TableCell>Capacity</TableCell>
                        <TableCell>Organizer ID</TableCell>
                        <TableCell>Actions</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {events.map((event) => (
                        <TableRow key={event.eventId}>
                            <TableCell>{event.eventId}</TableCell>
                            <TableCell>{event.name}</TableCell>
                            <TableCell>{event.description}</TableCell>
                            <TableCell>{new Date(event.startDate).toLocaleString()}</TableCell>
                            <TableCell>{new Date(event.endDate).toLocaleString()}</TableCell>
                            <TableCell>{event.location}</TableCell>
                            <TableCell>{event.capacity}</TableCell>
                            <TableCell>{event.organizerId}</TableCell>
                            <TableCell>
                                <Button onClick={() => onEdit(event)}>Edit</Button>
                                <Button onClick={() => handleDelete(event.eventId)}>Delete</Button>
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
};

export default EventList;
