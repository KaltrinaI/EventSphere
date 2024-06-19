// src/components/Organizers/OrganizerList.js
import React, { useEffect, useState } from 'react';
import { getAllOrganizers, deleteOrganizer } from '../../services/organizerService';
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Button } from '@mui/material';

const OrganizerList = ({ organizers: initialOrganizers, onEdit }) => {
    const [organizers, setOrganizers] = useState(initialOrganizers || []);

    useEffect(() => {
        if (!initialOrganizers) {
            fetchOrganizers();
        }
    }, [initialOrganizers]);

    const fetchOrganizers = async () => {
        const data = await getAllOrganizers();
        setOrganizers(data);
    };

    const handleDelete = async (id) => {
        const confirmDelete = window.confirm("Are you sure you want to delete this organizer?");
        if (confirmDelete) {
            try {
                await deleteOrganizer(id);
                if (!initialOrganizers) {
                    fetchOrganizers();
                } else {
                    setOrganizers(organizers.filter(org => org.organizerId !== id));
                }
            } catch (error) {
                console.error('Error deleting organizer:', error);
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
                        <TableCell>Phone</TableCell>
                        <TableCell>Actions</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {organizers.map((organizer) => (
                        <TableRow key={organizer.organizerId}>
                            <TableCell>{organizer.organizerId}</TableCell>
                            <TableCell>{organizer.name}</TableCell>
                            <TableCell>{organizer.phone}</TableCell>
                            <TableCell>
                                <Button onClick={() => onEdit(organizer)}>Edit</Button>
                                <Button onClick={() => handleDelete(organizer.organizerId)}>Delete</Button>
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
};

export default OrganizerList;
