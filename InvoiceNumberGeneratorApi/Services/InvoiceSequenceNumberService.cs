namespace InvoiceNumberGeneratorApi.Services;

public class InvoiceSequenceNumberService : ISequenceNumberService
{
    private int _sequenceNumber = 1;

    public int GetAndIncrementSequenceNumber()
    {
        return _sequenceNumber++;
    }

    public int GetSequenceNumber()
    {
        return _sequenceNumber;
    }
}