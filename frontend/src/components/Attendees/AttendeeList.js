// src/components/AttendeeList.js
import React, { useEffect, useState } from 'react';
import { getAllAttendees, deleteAttendee } from '../../services/attendeeService';
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Button } from '@mui/material';

const AttendeeList = ({ attendees: initialAttendees, onEdit }) => {
    const [attendees, setAttendees] = useState(initialAttendees || []);

    useEffect(() => {
        if (!initialAttendees) {
            fetchAttendees();
        }
    }, [initialAttendees]);

    const fetchAttendees = async () => {
        const data = await getAllAttendees();
        setAttendees(data);
    };

    const handleDelete = async (id) => {
        const confirmDelete = window.confirm("Are you sure you want to delete this organizer?");
        if(confirmDelete){
            try{
                await deleteAttendee(id);
        if (!initialAttendees) {
            fetchAttendees();
        } else {
            setAttendees(attendees.filter(att => att.attendeeId !== id));
        }
            }catch (error) {
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
                        <TableCell>Email</TableCell>
                        <TableCell>Phone</TableCell>
                        <TableCell>Event ID</TableCell>
                        <TableCell>Actions</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {attendees.map((attendee) => (
                        <TableRow key={attendee.attendeeId}>
                            <TableCell>{attendee.attendeeId}</TableCell>
                            <TableCell>{attendee.name}</TableCell>
                            <TableCell>{attendee.email}</TableCell>
                            <TableCell>{attendee.phone}</TableCell>
                            <TableCell>{attendee.eventId}</TableCell>
                            <TableCell>
                                <Button onClick={() => onEdit(attendee)}>Edit</Button>
                                <Button onClick={() => handleDelete(attendee.attendeeId)}>Delete</Button>
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
};

export default AttendeeList;
