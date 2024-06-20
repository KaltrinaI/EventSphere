// src/App.js
import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes, NavLink } from 'react-router-dom';
import { AppBar, Toolbar, Typography, Button, Container, Paper, Box } from '@mui/material';
import AttendeeList from './components/Attendees/AttendeeList';
import AttendeeForm from './components/Attendees/AttendeeForm';
import SearchAttendeeById from './components/Attendees/SearchAttendeeById';
import SearchAttendeeByEventId from './components/Attendees/SearchAttendeeByEventId';
import OrganizerList from './components/Organizers/OrganizerList';
import OrganizerForm from './components/Organizers/OrganizerForm';
import SearchOrganizerById from './components/Organizers/SearchOrganizerById';
import TicketList from './components/Tickets/TicketList';
import TicketForm from './components/Tickets/TicketForm';
import SearchTicketById from './components/Tickets/SearchTicketById';
import SearchTicketsByEventId from './components/Tickets/SearchTicketsByEventId';
import SellTicketForm from './components/Tickets/SellTicketForm';
import RefundTicketForm from './components/Tickets/RefundTicketForm';
import EventList from './components/Events/EventList';
import EventForm from './components/Events/EventForm';
import UpcomingEvents from './components/Events/UpcomingEvents'; 
import SearchEventById from './components/Events/SearchEventById';
import SearchEventsByOrganizerId from './components/Events/SearchEventsByOrganizerId';
import LoginForm from './components/Auth/LoginForm';
import RegisterForm from './components/Auth/RegisterForm';
import { getToken, getUsername, logout } from './services/authService';

function App() {
    const [selectedAttendee, setSelectedAttendee] = useState(null);
    const [searchResult, setSearchResult] = useState(null);
    const [attendeesByEvent, setAttendeesByEvent] = useState([]);

    const [selectedOrganizer, setSelectedOrganizer] = useState(null);
    const [searchOrganizerResult, setSearchOrganizerResult] = useState(null);

    const [selectedTicket, setSelectedTicket] = useState(null);
    const [searchTicketResult, setSearchTicketResult] = useState(null);
    const [ticketsByEvent, setTicketsByEvent] = useState([]);
    const [currentEventId, setCurrentEventId] = useState(null);

    const [selectedEvent, setSelectedEvent] = useState(null);
    const [searchEventResult, setSearchEventResult] = useState(null);
    const [eventsByOrganizer, setEventsByOrganizer] = useState([]);

    const [isLoggedIn, setIsLoggedIn] = useState(false);

    useEffect(() => {
        const token = getToken();
        if (token) {
            setIsLoggedIn(true);
        }
    }, []);

    const handleEditAttendee = (attendee) => {
        setSelectedAttendee(attendee);
    };

    const handleSaveAttendee = () => {
        setSelectedAttendee(null);
        setSearchResult(null); // Clear search result after saving
    };

    const handleSearchByIdResult = (result) => {
        setSearchResult(result);
    };

    const handleSearchByEventResult = (result) => {
        setAttendeesByEvent(result);
    };

    const handleEditOrganizer = (organizer) => {
        setSelectedOrganizer(organizer);
    };

    const handleSaveOrganizer = () => {
        setSelectedOrganizer(null);
        setSearchOrganizerResult(null);
    };

    const handleSearchOrganizerByIdResult = (result) => {
        setSearchOrganizerResult(result);
    };

    const handleEditTicket = (ticket) => {
        setSelectedTicket(ticket);
    };

    const handleSaveTicket = () => {
        setSelectedTicket(null);
        setSearchTicketResult(null);
    };

    const handleSearchTicketByIdResult = (result) => {
        setSearchTicketResult(result);
    };

    const handleSearchTicketsByEventResult = (result, eventId) => {
        setTicketsByEvent(result);
        setCurrentEventId(eventId);
    };

    const handleEditEvent = (event) => {
        setSelectedEvent(event);
    };

    const handleSaveEvent = () => {
        setSelectedEvent(null);
        setSearchEventResult(null);
    };

    const handleSearchEventByIdResult = (result) => {
        setSearchEventResult(result);
    };

    const handleSearchEventsByOrganizerIdResult = (result) => {
        setEventsByOrganizer(result);
    };

    return (
        <Router>
            <AppBar position="static" color="primary">
                <Toolbar>
                    <Typography variant="h6" style={{ flexGrow: 1 }}>
                        EventSphere
                    </Typography>
                    {isLoggedIn && <>
                    <Button 
                        color="inherit"
                        component={NavLink}
                        to="/attendees"
                        activeClassName="active"
                        style={({ isActive }) => ({
                            color: isActive ? '#ff69b4' : 'inherit',
                        })}
                    >
                        Attendees
                    </Button>
                    <Button 
                        color="inherit"
                        component={NavLink}
                        to="/organizers"
                        activeClassName="active"
                        style={({ isActive }) => ({
                            color: isActive ? '#ff69b4' : 'inherit',
                        })}
                    >
                        Organizers
                    </Button>
                    <Button 
                        color="inherit"
                        component={NavLink}
                        to="/tickets"
                        activeClassName="active"
                        style={({ isActive }) => ({
                            color: isActive ? '#ff69b4' : 'inherit',
                        })}
                    >
                        Tickets
                    </Button>
                    <Button 
                        color="inherit"
                        component={NavLink}
                        to="/events"
                        activeClassName="active"
                        style={({ isActive }) => ({
                            color: isActive ? '#ff69b4' : 'inherit',
                        })}
                    >
                        Events
                    </Button>
                    </> }
                    <Box sx={{ flexGrow: 1 }} />
                    {isLoggedIn ? <>

                        <Typography>
                            {getUsername()}
                        </Typography>
                        <Button onClick={()=>{logout();setIsLoggedIn(false)}}
                        color="inherit"
                        component={NavLink}
                        to="/login"
                        activeClassName="active"
                        style={({ isActive }) => ({
                            color: isActive ? '#ff69b4' : 'inherit',
                        })}
                    >
                        logout
                    </Button>
                    
                    </>:<>    
                    <Button
                        color="inherit"
                        component={NavLink}
                        to="/login"
                        activeClassName="active"
                        style={({ isActive }) => ({
                            color: isActive ? '#ff69b4' : 'inherit',
                        })}
                    >
                        Login
                    </Button>
                    <Button 
                        color="inherit"
                        component={NavLink}
                        to="/register"
                        activeClassName="active"
                        style={({ isActive }) => ({
                            color: isActive ? '#ff69b4' : 'inherit',
                        })}
                    >
                        Register
                    </Button>
                </>}
                </Toolbar>
            </AppBar>
            <Container>
                <Paper style={{ padding: 16, backgroundColor: 'rgba(255, 255, 255, 0.8)', marginTop: 16 }}>
                    <Routes>
                        <Route path="/attendees" element={
                            <>
                                <AttendeeForm selectedAttendee={selectedAttendee} onSave={handleSaveAttendee} />
                                <SearchAttendeeById onResult={handleSearchByIdResult} />
                                <SearchAttendeeByEventId onResult={handleSearchByEventResult} />
                                {searchResult && (
                                    <AttendeeList
                                        attendees={[searchResult]}
                                        onEdit={handleEditAttendee}
                                    />
                                )}
                                {attendeesByEvent.length > 0 && (
                                    <AttendeeList
                                        attendees={attendeesByEvent}
                                        onEdit={handleEditAttendee}
                                    />
                                )}
                                {!searchResult && attendeesByEvent.length === 0 && (
                                    <AttendeeList onEdit={handleEditAttendee} />
                                )}
                            </>
                        } />
                        <Route path="/organizers" element={
                            <>
                                <OrganizerForm selectedOrganizer={selectedOrganizer} onSave={handleSaveOrganizer} />
                                <SearchOrganizerById onResult={handleSearchOrganizerByIdResult} />
                                {searchOrganizerResult && (
                                    <OrganizerList
                                        organizers={[searchOrganizerResult]}
                                        onEdit={handleEditOrganizer}
                                    />
                                )}
                                {!searchOrganizerResult && (
                                    <OrganizerList onEdit={handleEditOrganizer} />
                                )}
                            </>
                        } />
                        <Route path="/tickets" element={
                            <>
                                <TicketForm selectedTicket={selectedTicket} onSave={handleSaveTicket} />
                                <SearchTicketById onResult={handleSearchTicketByIdResult} />
                                <SearchTicketsByEventId onResult={handleSearchTicketsByEventResult} />
                                <SellTicketForm />
                                <RefundTicketForm />
                                {searchTicketResult && (
                                    <TicketList
                                        tickets={[searchTicketResult]}
                                        onEdit={handleEditTicket}
                                    />
                                )}
                                {ticketsByEvent.length > 0 && (
                                    <TicketList
                                        tickets={ticketsByEvent}
                                        eventId={currentEventId}
                                        onEdit={handleEditTicket}
                                    />
                                )}
                                {!searchTicketResult && ticketsByEvent.length === 0 && (
                                    <TicketList onEdit={handleEditTicket} />
                                )}
                            </>
                        } />
                        <Route path="/events" element={
                            <>
                                <EventForm selectedEvent={selectedEvent} onSave={handleSaveEvent} />
                                <SearchEventById onResult={handleSearchEventByIdResult} />
                                <SearchEventsByOrganizerId onResult={handleSearchEventsByOrganizerIdResult} />
                                {searchEventResult && (
                                    <EventList
                                        events={[searchEventResult]}
                                        onEdit={handleEditEvent}
                                    />
                                )}
                                {eventsByOrganizer.length > 0 && (
                                    <EventList
                                        events={eventsByOrganizer}
                                        onEdit={handleEditEvent}
                                    />
                                )}
                                {!searchEventResult && eventsByOrganizer.length === 0 && (
                                    <EventList onEdit={handleEditEvent} />
                                )}
                            </>
                        } />
                        <Route path="/login" element={<LoginForm onLoginSucceed = {()=>setIsLoggedIn(true)} />} />
                        <Route path="/register" element={<RegisterForm />} />
                    </Routes>
                </Paper>
            </Container>
        </Router>
    );
}

export default App;