namespace Util
{
    public struct Either<TL, TR>
    {
        public readonly TL Left;
        public readonly TR Right;
        public readonly bool IsLeft;

        public Either(TL left)
        {
            this.Left = left;
            this.Right = default(TR);
            this.IsLeft = true;
        }

        public Either(TR right)
        {
            this.Left = default(TL);
            this.Right = right;
            this.IsLeft = false;
        }
    }
}