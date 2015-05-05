namespace Azyobuzi.OwinRazor
{
    public class RawString
    {
        public RawString(string value)
        {
            this.Value = value;
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            return this.Value;
        }
    }
}
