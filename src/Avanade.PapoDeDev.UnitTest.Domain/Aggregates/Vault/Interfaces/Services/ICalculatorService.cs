namespace Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Vault.Interfaces.Services
{
    public interface ICalculatorService
    {
        decimal Divide(int v1, int v2);

        int Multiply(int[] values);

        int Sum(int[] values);
    }
}
