namespace InvoiceNumberGeneratorApi.Services;

public interface ISequenceNumberService
{
    int GetSequenceNumber();
    int GetAndIncrementSequenceNumber();
}