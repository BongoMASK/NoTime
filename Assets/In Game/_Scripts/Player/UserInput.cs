public class UserInput {
    public int x, y;
    public bool jumping;
    public bool pressed;

    public bool isMoving => x > 0 || y > 0;

    public bool IsPressed() {
        return x == 0 && y == 0;
    }

    public void ResetInput() {
        x = 0;
        y = 0;
    }
}