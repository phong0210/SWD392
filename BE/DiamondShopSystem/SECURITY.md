# Security Configuration Guide

## Overview
This document outlines the security features implemented in the DiamondShopSystem API.

## Security Features

### 1. Rate Limiting
- **Authentication Endpoints**: Limited to 5 requests per 15-minute window per IP address
- **Password Reset**: Limited to 3 requests per hour per IP address
- **Implementation**: Custom middleware using in-memory storage

### 2. Authentication Logging
- All login attempts (successful and failed)
- Password reset requests and completions
- Google OAuth login attempts
- Suspicious activity detection
- IP address tracking for security monitoring

### 3. JWT Token Security
- Password reset tokens are marked with `ResetPasswordOnly` claim
- Tokens have configurable expiration times
- Secure token validation with proper error handling

### 4. CORS Configuration
- Configurable allowed origins
- Secure CORS policy with credentials support
- Environment-specific origin lists

## Configuration

### Environment Variables
Add these to your `.env` file:

```env
# JWT Configuration
JWT_KEY=your_super_secret_jwt_key_here_make_it_long_and_complex
JWT_ISSUER=your_jwt_issuer
JWT_AUDIENCE=your_jwt_audience
JWT_TOKEN_VALIDITY_IN_MINUTES=30

# OTP Configuration
OTP_SECRET_SALT=your_otp_secret_salt_here

# VNPay Configuration (if using)
VNPAY_TMN_CODE=your_tmn_code
VNPAY_HASH_SECRET=your_hash_secret
VNPAY_URL=https://sandbox.vnpayment.vn/paymentv2/vpcpay.html
VNPAY_API_URL=https://sandbox.vnpayment.vn/merchant_webapi/api/transaction
```

### appsettings.json Configuration
```json
{
  "Security": {
    "MaxLoginAttemptsPerHour": 5,
    "MaxPasswordResetRequestsPerHour": 3,
    "PasswordResetTokenExpiryMinutes": 15,
    "OTPExpiryMinutes": 10,
    "RequireEmailVerification": true,
    "LogSuspiciousActivity": true,
    "EnableRateLimiting": true,
    "AllowedOrigins": [
      "http://localhost:3000",
      "http://localhost:4200",
      "https://yourdomain.com"
    ]
  }
}
```

## Security Best Practices

### 1. Password Security
- Passwords are hashed using BCrypt
- Minimum 8 characters with complexity requirements
- Password reset tokens expire after 15 minutes

### 2. API Security
- Rate limiting prevents brute force attacks
- Comprehensive logging for security monitoring
- Input validation using FluentValidation
- Secure error handling (no sensitive data in error messages)

### 3. Token Security
- JWT tokens have short expiration times
- Password reset tokens are single-use and time-limited
- Secure token validation with proper error handling

### 4. Monitoring
- All authentication events are logged
- Failed attempts are tracked with IP addresses
- Suspicious activity is flagged and logged

## Monitoring and Alerts

### Log Levels
- **Information**: Successful authentication events
- **Warning**: Failed authentication attempts
- **Error**: Suspicious activity and security violations

### Key Metrics to Monitor
- Failed login attempts per IP
- Password reset request frequency
- Token validation failures
- Rate limit violations

## Incident Response

### Rate Limit Violations
- Requests are blocked with 429 status code
- Retry-After header indicates when to retry
- All violations are logged for analysis

### Suspicious Activity
- Multiple failed login attempts
- Rapid password reset requests
- Invalid token usage
- Unusual IP addresses

## Future Enhancements

1. **Redis-based Rate Limiting**: Replace in-memory storage with Redis for distributed deployments
2. **Email-based Alerts**: Send alerts for suspicious activity
3. **IP Geolocation**: Track and log user locations
4. **Device Fingerprinting**: Enhanced client identification
5. **Two-Factor Authentication**: Add 2FA support
6. **Account Lockout**: Temporary account lockouts after multiple failures 