namespace Audited.Formula
{
    public interface Formula : Amount
    {
        Amount Calculate();
    }
}