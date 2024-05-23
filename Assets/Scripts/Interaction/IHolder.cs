public interface IHolder
{
    public void Grab(Holdable holdable);
    public Holdable Drop();
    public void ExchangeWith(IHolder previousHolder);
}