public class Key : Holdable
{
    private Door door;

    public void LinkTo(Door door) => this.door = door;
}