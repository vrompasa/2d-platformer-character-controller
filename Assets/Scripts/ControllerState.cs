public class ControllerState
{
    public bool IsCollidingRight { get; set; }
    public bool IsCollidingLeft { get; set; }
    public bool IsCollidingAbove { get; set; }
    public bool IsCollidingBelow { get; set; }
    public bool DropThroughPlatform { get; set; }
    public bool HasCollisions { get { return IsCollidingLeft || IsCollidingRight || IsCollidingAbove || IsCollidingBelow; } }

    public void Reset()
    {
        IsCollidingRight =
        IsCollidingLeft =
        IsCollidingAbove =
        IsCollidingBelow = false;
    }

    public override string ToString()
    {
        return string.Format(
            "(controller: r:{0} l:{1} a:{2} b:{3})",
            IsCollidingRight,
            IsCollidingLeft,
            IsCollidingAbove,
            IsCollidingBelow
        );
    }

}
