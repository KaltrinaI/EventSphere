// src/components/Events/UpcomingEvents.js
import React, { useEffect, useState } from 'react';
import { getUpcomingEventsSortedByPopularity } from '../../services/eventService';
import { Container, Typography, List, ListItem, ListItemText } from '@mui/material';

const UpcomingEvents = () => {
    const [events, setEvents] = useState([]);

    useEffect(() => {
        const fetchEvents = async () => {
            try {
                const data = await getUpcomingEventsSortedByPopularity();
                setEvents(data);
            } catch (error) {
                console.error('Error fetching upcoming events:', error);
            }
        };

        fetchEvents();
    }, []);

    return (
        <Container>
            <Typography variant="h4" gutterBottom>
                Upcoming Events Sorted by Popularity
            </Typography>
            <List>
                {events.map(event => (
                    <ListItem key={event.eventId}>
                        <ListItemText
                            primary={event.name}
                            secondary={`Starts: ${new Date(event.startDate).toLocaleDateString()} - Ends: ${new Date(event.endDate).toLocaleDateString()}`}
                        />
                    </ListItem>
                ))}
            </List>
        </Container>
    );
};

export default UpcomingEvents;
