using System;

namespace Outbracket.Globalization
{
    public static class ValidationErrors
    {
        public static readonly Tuple<string, string> UsernameOrEmailIsInvalid = 
            new("UsernameOrEmailIsInvalid", "user name or email is invalid");

        public static readonly Tuple<string, string> UserEmailExist = 
            new("UserEmailExist", "user with this email already exists");
        
        public static readonly Tuple<string, string> UsernameExist = 
            new("UsernameExist", "user with this username already exists");
        
        public static readonly Tuple<string, string> ConfirmationEmailTokenInvalid = 
            new("ConfirmationEmailTokenInvalid", "invalid confirmation email token");
        
        public static readonly Tuple<string, string> UserDoesntExist = 
            new("UserDoesntExist", "user with this email doesn't exist");

        public static readonly Tuple<string, string> RefreshTokenIsInvalid = 
            new("RefreshTokenIsInvalid", "token is invalid");
        
        public static readonly Tuple<string, string> EmailIsNotConfirmed = 
            new("EmailIsNotConfirmed", "email is not confirmed");
        
        public static readonly Tuple<string, string> MaximumLength = 
            new("MaximumLength", "the length of field must be {0} characters or fewer");
    }

    public static class Messages
    {
        public static readonly Tuple<string, string> EmailConfirmationSubject =
            new("EmailConfirmationSubject", "Email confirmation");
        
        public static readonly Tuple<string, string> PasswordResetSubject =
            new("PasswordResetSubject", "Password reset");
        
        public static readonly Tuple<string, string> ConfirmationEmailIsNotSent = 
            new("ConfirmationEmailIsNotSent", "confirmation email is not sent");
        
        public static readonly Tuple<string, string> PasswordResetIsNotSent =
            new("PasswordResetIsNotSent", "password reset email is not sent");
        
        public static readonly Tuple<string, string> UserNotFound =
            new("UserNotFound", "user not found");
        
        public static readonly Tuple<string, string> OperationIsNotPermitted = 
            new("OperationIsNotPermitted", "operation is not permitted");
        
        public static readonly Tuple<string, string> UnhandledException = 
            new("UnhandledException", "unhandled exception");
    }
}
