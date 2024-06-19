// src/components/Events/EventForm.js
import React, { useState, useEffect } from 'react';
import { TextField, Button, Paper } from '@mui/material';
import { addEvent, updateEvent } from '../../services/eventService';

const EventForm = ({ selectedEvent, onSave }) => {
    const [event, setEvent] = useState({
        name: '',
        description: '',
        startDate: '',
        endDate: '',
        location: '',
        capacity: '',
        organizerId: '',
    });

    useEffect(() => {
        if (selectedEvent) {
            setEvent({
                ...selectedEvent,
                startDate: new Date(selectedEvent.startDate).toISOString().substring(0, 16),
                endDate: new Date(selectedEvent.endDate).toISOString().substring(0, 16),
            });
        }
    }, [selectedEvent]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setEvent((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const eventToSave = {
            ...event,
            startDate: new Date(event.startDate).toISOString(),
            endDate: new Date(event.endDate).toISOString(),
        };

        if (event.eventId) {
            await updateEvent(event.eventId, eventToSave);
        } else {
            await addEvent(eventToSave);
        }
        onSave();
        setEvent({
            name: '',
            description: '',
            startDate: '',
            endDate: '',
            location: '',
            capacity: '',
            organizerId: '',
        });
    };

    return (
        <Paper style={{ padding: 16 }}>
            <form onSubmit={handleSubmit}>
                <TextField
                    label="Name"
                    name="name"
                    value={event.name}
                    onChange={handleChange}
                    fullWidth
                    margin="normal"
                />
                <TextField
                    label="Description"
                    name="description"
                    value={event.description}
                    onChange={handleChange}
                    fullWidth
                    margin="normal"
                />
                <TextField
                    label="Start Date"
                    name="startDate"
                    type="datetime-local"
                    value={event.startDate}
                    onChange={handleChange}
                    fullWidth
                    margin="normal"
                    InputLabelProps={{
                        shrink: true,
                    }}
                />
                <TextField
                    label="End Date"
                    name="endDate"
                    type="datetime-local"
                    value={event.endDate}
                    onChange={handleChange}
                    fullWidth
                    margin="normal"
                    InputLabelProps={{
                        shrink: true,
                    }}
                />
                <TextField
                    label="Location"
                    name="location"
                    value={event.location}
                    onChange={handleChange}
                    fullWidth
                    margin="normal"
                />
                <TextField
                    label="Capacity"
                    name="capacity"
                    value={event.capacity}
                    onChange={handleChange}
                    fullWidth
                    margin="normal"
                />
                <TextField
                    label="Organizer ID"
                    name="organizerId"
                    value={event.organizerId}
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

export default EventForm;
