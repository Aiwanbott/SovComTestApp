using SovComTestApp.Interfaces;
using SovComTestApp.Models;
using Microsoft.EntityFrameworkCore;
using SovComTestApp.Services;
using RussianTransliteration;
using System.Text.RegularExpressions;
using SovComTestApp.Data;

namespace SovComTestApp.Services;

public class InvitationService : IInvitationService
{
    private readonly IValidators _validators;
    private readonly InvitationDbContext _context;
    private readonly DbSet<Invitation> _dbset;

    public InvitationService(IValidators validators, InvitationDbContext context)
    {
        _validators = validators;
        _context = context;
        _dbset = context.Set<Invitation>();
    }

    public ResponseObject SendInvite(Invites invites)
    {
        IList<string>? phoneNumbers = invites.PhoneNumbers;
        string? message = invites.Message;

        
        if (!_validators.ValidatePhoneNumbers(phoneNumbers))
        {
            return new ResponseObject(
                "400 BAD_REQUEST PHONE_NUMBERS_INVALID",
                "One or several phone numbers do not match with international format");
        }

        if (phoneNumbers == null || phoneNumbers.Count == 0)
        {
            return new ResponseObject(
                "401 BAD_REQUEST PHONE_NUMBERS_EMPTY",
                "Phone numbers are missing");
        }

        if (phoneNumbers.Count > 16)
        {
            return new ResponseObject(
                "402 BAD_REQUEST PHONE_NUMBERS_INVALID",
                "Too much phone numbers, should be less or equal to 16 per request"
                );
        }

        if (CountRequests(invites.ApiId) > 128
            || (CountRequests(invites.ApiId) + phoneNumbers.Count) > 128)
        {
            return new ResponseObject(
                "403 BAD_REQUEST PHONE_NUMBERS_INVALID",
                "Too much phone numbers, should be less or equal to 128 per day");
        }

        if (!_validators.ValidateDuplicates(phoneNumbers))
        {
            return new ResponseObject(
                "404 BAD_REQUEST PHONE_NUMBERS_INVALID",
                "Duplicate numbers detected");
        }

        if (string.IsNullOrEmpty(message))
        {
            return new ResponseObject(
                "405 BAD_REQUEST MESSAGE_EMPTY",
                "Invite message is missing");
        }

        if (!_validators.ValidateCyrillic(message) && !_validators.ValidateGSM(message))
        {
            return new ResponseObject(
                "406 BAD_REQUEST MESSAGE_INVALID",
                "Invite message should contain only characters in 7-bit GSM encoding or Cyrillic letters as well");
        }

        if (!_validators.ValidateLength(message))
        {
            return new ResponseObject(
                "407 BAD_REQUEST MESSAGE_INVALID",
                "Invite message too long, should be less or equal to 128 characters of 7-bit GSM charset");
        }

        var invitations = new List<Invitation>();
        message = PrepareMessage(message);

        foreach (var phoneNumber in phoneNumbers)
        {
            
            if (IsSentToday(phoneNumber, invites.ApiId))
            {
                return new ResponseObject(
                    "408 BAD_REQUEST PHONE_NUMBERS_INVALID",
                    "One of phone numbers is used today"
                    );
            }

            var invitation = new Invitation
            {
                CreatedDate = DateTime.Now.Date,
                PhoneNumber = phoneNumber,
                ApiId = invites.ApiId,
                Message = message
            };
            _context.Invitations.Add(invitation);
        }

              
        //_smsSender.Send(invitations);

        
        _dbset.AddRange(invitations);
        _context.SaveChanges();

        return new ResponseObject("200", "ok");
    }

    private static string PrepareMessage(string message)
    {
        if (!Regex.IsMatch(message, @"S""[\p{IsCyrillic}\p{P}\p{N}\s]*"""))
        {
            return RussianTransliterator.GetTransliteration(message);
        }

        return message;
    }

        /// <summary>
        /// Метод для подсчёта запросов по apiId
        /// </summary>
        public int CountRequests(int apiId)
        {
            
            return _dbset.Where(e => e.ApiId == apiId && e.CreatedDate == DateTime.Now.Date).Count();
        }

        /// <summary>
        /// Метод для проверки, было ли сегодня отправлено сообщение на этот номер
        /// </summary>
        public bool IsSentToday(string phoneNumber, int apiId)
        {
            return _dbset
                .Where(e => e.ApiId == apiId && e.CreatedDate == DateTime.Now.Date)
                .Any(e => e.PhoneNumber == phoneNumber);


        }
    }

   







