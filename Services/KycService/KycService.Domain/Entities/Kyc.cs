using Finvia.Shared.Domain;
using KycService.Domain.Enums;
using KycService.Domain.Events;

namespace KycService.Domain.Entities;

public class Kyc : Entity, IAggregateRoot
{
    public Guid UserId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string NationalId { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string CountryCode { get; private set; }
    public KycStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? VerifiedAt { get; private set; }

    private Kyc()
    {
    }

    private Kyc(Guid userId, string firstName, string lastName, string nationalId, DateTime dob, string countryCode)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        NationalId = nationalId;
        DateOfBirth = dob;
        CountryCode = countryCode;
        Status = KycStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public static Kyc Create(Guid userId, string firstName, string lastName, string nationalId, DateTime dob,
        string countryCode)
    {
        return new Kyc(userId, firstName, lastName, nationalId, dob, countryCode);
    }

    public void Approve()
    {
        Status = KycStatus.Approved;
        VerifiedAt = DateTime.UtcNow;
        AddDomainEvent(new KycApprovedEvent(UserId));
    }

    public void Reject()
    {
        Status = KycStatus.Rejected;
        AddDomainEvent(new KycRejectedEvent(UserId));
    }

    public void Fail()
    {
        Status = KycStatus.Failed;
        AddDomainEvent(new KycFailedEvent(UserId));
    }
}