// src/components/Organizers/OrganizerForm.js
import React, { useState, useEffect } from 'react';
import { TextField, Button, Paper } from '@mui/material';
import { createOrganizer, updateOrganizer } from '../../services/organizerService';

const OrganizerForm = ({ selectedOrganizer, onSave }) => {
    const [organizer, setOrganizer] = useState({
        name: '',
        phone: '',
    });

    useEffect(() => {
        if (selectedOrganizer) {
            setOrganizer(selectedOrganizer);
        }
    }, [selectedOrganizer]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setOrganizer((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (organizer.organizerId) {
            await updateOrganizer(organizer.organizerId, organizer);
        } else {
            await createOrganizer(organizer);
        }
        onSave();
        setOrganizer({ name: '', phone: '' });
    };

    return (
        <Paper style={{ padding: 16 }}>
            <form onSubmit={handleSubmit}>
                <TextField
                    label="Name"
                    name="name"
                    value={organizer.name}
                    onChange={handleChange}
                    fullWidth
                    margin="normal"
                />
                <TextField
                    label="Phone"
                    name="phone"
                    value={organizer.phone}
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

export default OrganizerForm;
