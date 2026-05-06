# Event Management API Documentation

## Overview
REST API for managing events, bookings, payments, and feedback built with .NET 9, Entity Framework Core, and JWT authentication.

## Technology Stack
- Framework: .NET 9.0
- Database: SQL Server + Entity Framework Core 9.0.15
- Authentication: JWT Bearer Token
- Validation: FluentValidation 12.1.1
- Documentation: Swagger/OpenAPI (Swashbuckle.AspNetCore 10.1.7)
- CORS: Enabled for all origins

## Architecture
- **API Layer:** ASP.NET Core Controllers
- **BLL Layer:** Services, DTOs, Interfaces
- **DAL Layer:** Entity Framework Repositories
- **CrossCutting:** Shared Entities & Enums

---

# Authentication

## JWT Configuration
- Secret: Minimum 32 characters
- Issuer: EventManagement
- Audience: EventManagementAudience
- Expiry: 60 minutes
- Validation: Issuer, Audience, Signature

## Flow
1. POST /api/auth/register → Get token
2. POST /api/auth/login → Get token
3. Use: Authorization: Bearer {token}

---

# API ENDPOINTS

## Auth Endpoints

### POST /api/auth/register
**Description:** Create new user account  
**Auth:** Not Required  
**Request:**
```json
{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "SecurePassword123!",
  "confirmPassword": "SecurePassword123!",
  "role": 3
}
```
**Response (201):**
```json
{
  "userId": 1,
  "name": "John Doe",
  "email": "john@example.com",
  "role": "Attendee",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### POST /api/auth/login
**Description:** Login and get JWT token  
**Auth:** Not Required  
**Request:**
```json
{
  "email": "john@example.com",
  "password": "SecurePassword123!"
}
```
**Response (200):**
```json
{
  "userId": 1,
  "name": "John Doe",
  "email": "john@example.com",
  "role": "Attendee",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

---

## Event Endpoints

### POST /api/event
**Description:** Create new event  
**Auth:** Required | **Authorization:** Organizer/Admin  
**Request:**
```json
{
  "title": "Tech Conference 2025",
  "description": "Annual technology conference",
  "eventDate": "2025-06-15",
  "startTime": "09:00:00",
  "endTime": "17:00:00",
  "categoryId": 1,
  "locationId": 2
}
```
**Response (201):**
```json
{
  "id": 5,
  "title": "Tech Conference 2025",
  "description": "Annual technology conference",
  "eventDate": "2025-06-15T00:00:00",
  "startTime": "09:00:00",
  "endTime": "17:00:00",
  "status": "Draft",
  "organizerName": "Jane Smith",
  "category": "Technology",
  "location": "Convention Center, New York"
}
```

### GET /api/event/{id}
**Description:** Get event details  
**Auth:** Not Required  
**Response (200):**
```json
{
  "id": 5,
  "title": "Tech Conference 2025",
  "description": "Annual technology conference",
  "eventDate": "2025-06-15T00:00:00",
  "startTime": "09:00:00",
  "endTime": "17:00:00",
  "status": "Published",
  "organizerName": "Jane Smith",
  "category": "Technology",
  "location": "Convention Center, New York"
}
```

### GET /api/event/published
**Description:** Get all published events (public)  
**Auth:** Not Required  
**Response (200):**
```json
[
  {
    "id": 1,
    "title": "Summer Festival",
    "description": "Annual summer festival",
    "eventDate": "2025-07-20T00:00:00",
    "startTime": "18:00:00",
    "endTime": "23:00:00",
    "status": "Published",
    "organizerName": "Event Co.",
    "category": "Music",
    "location": "Central Park, New York"
  }
]
```

### GET /api/event/organizer
**Description:** Get organizer's events (draft + published)  
**Auth:** Required | **Authorization:** Organizer/Admin  
**Response (200):** Array of EventDto

### PUT /api/event/{id}
**Description:** Update event (draft only)  
**Auth:** Required | **Authorization:** Event creator  
**Request:** Same as POST /api/event  
**Response (200):** Updated EventDto

### DELETE /api/event/{id}
**Description:** Delete event (draft only)  
**Auth:** Required | **Authorization:** Event creator  
**Response (204):** No Content

### POST /api/event/{id}/publish
**Description:** Publish draft event  
**Auth:** Required | **Authorization:** Event creator  
**Response (204):** No Content

### POST /api/event/{id}/cancel
**Description:** Cancel published event  
**Auth:** Required | **Authorization:** Event creator  
**Response (204):** No Content

---

## Booking Endpoints

### POST /api/booking
**Description:** Create booking  
**Auth:** Required  
**Request:**
```json
{
  "eventId": 5,
  "ticketId": 10,
  "quantity": 2
}
```
**Response (201):**
```json
{
  "id": 8,
  "eventId": 5,
  "eventTitle": "Tech Conference 2025",
  "quantity": 2,
  "status": "Confirmed",
  "bookingDate": "2025-05-06T14:30:00",
  "totalPrice": 299.98
}
```

### GET /api/booking/{id}
**Description:** Get booking details  
**Auth:** Required | **Authorization:** Booking creator  
**Response (200):** BookingDto

### GET /api/booking/me
**Description:** Get user's bookings  
**Auth:** Required  
**Response (200):** Array of BookingDto

### DELETE /api/booking/{id}
**Description:** Cancel booking  
**Auth:** Required | **Authorization:** Booking creator  
**Response (204):** No Content

---

## Payment Endpoints

### POST /api/payment
**Description:** Create payment  
**Auth:** Required  
**Request:**
```json
{
  "bookingId": 8,
  "amount": 299.98,
  "paymentMethod": "CreditCard"
}
```
**Response (201):**
```json
{
  "id": 3,
  "bookingId": 8,
  "amount": 299.98,
  "paymentMethod": "CreditCard",
  "status": "Pending",
  "paidAt": null
}
```

### GET /api/payment/booking/{bookingId}
**Description:** Get payment by booking  
**Auth:** Required | **Authorization:** Booking creator  
**Response (200):** PaymentDto

### PUT /api/payment/booking/{bookingId}/status
**Description:** Update payment status  
**Auth:** Required | **Authorization:** Booking creator  
**Request:** 
```json
"Completed"
```
**Valid Status Values:** Pending, Completed, Failed, Refunded  
**Response (200):** Updated PaymentDto

---

## Feedback Endpoints

### POST /api/feedback
**Description:** Submit feedback (requires confirmed booking)  
**Auth:** Required  
**Request:**
```json
{
  "eventId": 5,
  "rating": 5,
  "comment": "Excellent conference with great speakers!"
}
```
**Response (201):**
```json
{
  "id": 12,
  "userId": 1,
  "userName": "John Doe",
  "eventId": 5,
  "rating": 5,
  "comment": "Excellent conference!",
  "createdAt": "2025-05-06T15:45:00"
}
```

### GET /api/feedback/event/{eventId}
**Description:** Get all feedback for event  
**Auth:** Not Required  
**Response (200):** Array of FeedbackDto

---

# Data Models

## User Roles
- Admin (1): Full system access
- Organizer (2): Create and manage events
- Attendee (3): Book events, submit feedback (default)

## Event Statuses
- Draft: Event in preparation
- Published: Live and visible
- Cancelled: Event cancelled
- Completed: Event occurred

## Booking Statuses
- Pending: Awaiting payment
- Confirmed: Payment received
- Cancelled: Booking cancelled

## Payment Statuses
- Pending: Awaiting processing
- Completed: Successfully processed
- Failed: Processing failed
- Refunded: Payment refunded

---

# Error Handling

## Response Format
```json
{
  "message": "Error description",
  "statusCode": 400
}
```

## HTTP Status Codes
- 200: OK
- 201: Created
- 204: No Content
- 400: Bad Request
- 401: Unauthorized (invalid/missing token)
- 403: Forbidden (insufficient permissions)
- 404: Not Found
- 500: Internal Server Error

---

# Setup & Configuration

## Prerequisites
- .NET 9.0 SDK
- SQL Server 2019+
- Visual Studio 2022 or VS Code

## Configuration (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=EventManagement;User Id=sa;Password=YOUR_PASSWORD;"
  },
  "Jwt": {
    "Secret": "your-super-secret-key-minimum-32-characters",
    "Issuer": "EventManagement",
    "Audience": "EventManagementAudience",
    "ExpiryMinutes": 60
  }
}
```

## Running
1. `dotnet restore`
2. `dotnet ef database update`
3. `dotnet run`
4. Access: `https://localhost:5001`

---

# Frontend Integration Guide

## Token Management
```javascript
// Store token in sessionStorage
sessionStorage.setItem('token', response.token);

// Include in requests
fetch('https://localhost:5001/api/booking/me', {
  headers: {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  }
});

// Handle 401 responses
if (response.status === 401) {
  window.location.href = '/login'; // Redirect to login
}
```

## Error Handling
```javascript
try {
  const response = await fetch('https://localhost:5001/api/event', {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(eventData)
  });

  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.message);
  }
  return await response.json();
} catch (error) {
  console.error('API Error:', error.message);
}
```

## Request Validation
- Validate data before sending
- Check authorization levels
- Verify token validity before requests

## Caching Strategy
- Cache public data (published events)
- Don't cache user-specific data
- Implement cache invalidation

## Environment Configuration
```javascript
// .env.development
REACT_APP_API_URL=http://localhost:5001

// .env.production
REACT_APP_API_URL=https://api.yourdomain.com

// Usage
const API_URL = process.env.REACT_APP_API_URL;
```

---

# Example Workflow: Complete Booking

```
1. Register/Login
   POST /api/auth/register or POST /api/auth/login
   ↓ (Get token)

2. Browse Events
   GET /api/event/published
   ↓ (Select event)

3. Create Booking
   POST /api/booking
   ↓ (Get booking ID)

4. Process Payment
   POST /api/payment
   ↓ (Complete payment)

5. Update Payment Status
   PUT /api/payment/booking/{id}/status
   ↓ (Confirm payment)

6. Submit Feedback (after event)
   POST /api/feedback
   ↓ (Complete workflow)
```

---

**API Version:** 1.0  
**Last Updated:** May 2025  
**Status:** Production Ready  
**Framework:** .NET 9.0  
**Database:** SQL Server  
