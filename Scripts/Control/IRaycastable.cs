namespace RPG.Control
{
    public interface IRaycastable
    {
        bool HandleRayCast(PlayerController controller);
        CursorType GetCursorType();


    }
        

}