namespace InvoiceNumberGeneratorApi.Services;

public class InvoiceSequenceNumberService : ISequenceNumberService
{
    private int _sequenceNumber;

    public int GetAndIncrementSequenceNumber()
    {
        return _sequenceNumber++;
    }

    public int GetSequenceNumber()
    {
        return _sequenceNumber;
    }
}