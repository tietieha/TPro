namespace SDK.Purchase.NativeBuyResult
{
    public class NativeBuyResultParserInvalid: NativeBuyResultParser
    {
        public override NativeBuyResult Parse(string result)
        {
            return NativeBuyResult.Fail("Invalid NativeBuyResultParser", null);
        }
    }
}