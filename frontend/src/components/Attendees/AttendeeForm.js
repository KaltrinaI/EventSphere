// src/components/Attendees/AttendeeForm.js
import React, { useState, useEffect } from 'react';
import { TextField, Button, Paper } from '@mui/material';
import { addAttendee, updateAttendee } from '../../services/attendeeService';

const AttendeeForm = ({ selectedAttendee, onSave }) => {
    const [attendee, setAttendee] = useState({
        name: '',
        email: '',
        phone: '',
        eventId: '',
    });

    useEffect(() => {
        if (selectedAttendee) {
            setAttendee(selectedAttendee);
        }
    }, [selectedAttendee]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setAttendee((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (attendee.attendeeId) {
            await updateAttendee(attendee.attendeeId, attendee);
        } else {
            await addAttendee(attendee);
        }
        onSave();
        setAttendee({ name: '', email: '', phone: '', eventId: '' });
    };

    return (
        <Paper style={{ padding: 16 }}>
            <form onSubmit={handleSubmit}>
                <TextField
                    label="Name"
                    name="name"
                    value={attendee.name}
                    onChange={handleChange}
                    fullWidth
                    margin="normal"
                />
                <TextField
                    label="Email"
                    name="email"
                    value={attendee.email}
                    onChange={handleChange}
                    fullWidth
                    margin="normal"
                />
                <TextField
                    label="Phone"
                    name="phone"
                    value={attendee.phone}
                    onChange={handleChange}
                    fullWidth
                    margin="normal"
                />
                <TextField
                    label="Event ID"
                    name="eventId"
                    value={attendee.eventId}
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

export default AttendeeForm;
